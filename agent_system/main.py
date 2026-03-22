from pathlib import Path

from .config import (
    MEMORY_DIR,
    OUTPUTS_DIR,
    MAX_FILE_CHARS,
    MAX_TOTAL_CONTEXT_CHARS,
)
from .file_tools import read_text_file, write_text_file, ensure_dir, file_exists
from .task_manager import load_current_task, parse_task
from .planner import build_plan_prompt
from .coder import build_code_prompt
from .validator import build_validation_prompt
from .llm_client import ask_llm


def load_memory_context() -> str:
    memory_files = [
        MEMORY_DIR / "project_summary.txt",
        MEMORY_DIR / "architecture_notes.txt",
        MEMORY_DIR / "known_issues.txt",
        MEMORY_DIR / "roadmap.txt",
    ]

    chunks = []
    for path in memory_files:
        if file_exists(path):
            chunks.append(f"===== MEMORY FILE: {path.name} =====\n{read_text_file(path)}")

    if not chunks:
        return "No project memory files yet."

    return "\n\n".join(chunks)


def load_file_context(file_paths: list[str]) -> str:
    chunks = []
    total_chars = 0

    for rel_path in file_paths:
        path = Path(rel_path)

        if not path.exists():
            chunk = f"===== FILE: {rel_path} =====\nERROR: File not found."
        else:
            content = read_text_file(path)
            if len(content) > MAX_FILE_CHARS:
                content = content[:MAX_FILE_CHARS] + "\n\n...TRUNCATED..."
            chunk = f"===== FILE: {rel_path} =====\n{content}"

        if total_chars + len(chunk) > MAX_TOTAL_CONTEXT_CHARS:
            break

        chunks.append(chunk)
        total_chars += len(chunk)

    return "\n\n".join(chunks)


def main() -> None:
    ensure_dir(OUTPUTS_DIR)

    task_text = load_current_task()
    task_info = parse_task(task_text)

    if not task_info["title"]:
        raise ValueError("Task title missing from current_task.txt")

    memory_context = load_memory_context()
    file_context = load_file_context(task_info["editable_files"])

    plan_prompt = build_plan_prompt(task_info, file_context, memory_context)
    plan_text = ask_llm(plan_prompt)
    write_text_file(OUTPUTS_DIR / "plan.txt", plan_text)

    code_prompt = build_code_prompt(task_info, plan_text, file_context, memory_context)
    code_output = ask_llm(code_prompt)
    write_text_file(OUTPUTS_DIR / "code_output.txt", code_output)

    validation_prompt = build_validation_prompt(task_info, plan_text, code_output)
    validation_output = ask_llm(validation_prompt)
    write_text_file(OUTPUTS_DIR / "validation.txt", validation_output)

    print("Agent run complete.")
    print("Outputs written to:")
    print(f"- {OUTPUTS_DIR / 'plan.txt'}")
    print(f"- {OUTPUTS_DIR / 'code_output.txt'}")
    print(f"- {OUTPUTS_DIR / 'validation.txt'}")


if __name__ == "__main__":
    main()
