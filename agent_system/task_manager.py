from pathlib import Path
from .config import TASKS_DIR
from .file_tools import read_text_file


def load_current_task() -> str:
    task_file = Path(TASKS_DIR) / "current_task.txt"
    return read_text_file(task_file)


def parse_task(task_text: str) -> dict:
    task = {
        "title": "",
        "success_criteria": [],
        "editable_files": [],
    }

    current_section = None

    for raw_line in task_text.splitlines():
        line = raw_line.strip()

        if not line:
            continue

        if line.startswith("Task:"):
            task["title"] = line.replace("Task:", "", 1).strip()
            current_section = None
        elif line.startswith("Success Criteria:"):
            current_section = "success_criteria"
        elif line.startswith("Editable Files:"):
            current_section = "editable_files"
        elif line.startswith("-") and current_section:
            task[current_section].append(line[1:].strip())

    return task
