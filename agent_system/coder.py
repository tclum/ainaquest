def build_code_prompt(task_info: dict, plan_text: str, file_context: str, memory_context: str) -> str:
    editable = "\n".join(f"- {item}" for item in task_info["editable_files"])

    return f"""
You are the Code Agent for a Unity C# game project.

Project memory:
{memory_context}

Task:
{task_info["title"]}

Plan:
{plan_text}

Editable Files:
{editable}

Current File Context:
{file_context}

Return:
1. A short summary of the code changes
2. Full updated contents for each changed file

Rules:
- Only modify files listed in Editable Files
- Preserve unrelated logic
- Keep Unity API usage valid
- Avoid unnecessary renaming
- Write compile-safe C#
- If a file does not need changes, omit it

Output format exactly:

SUMMARY:
<your summary>

===== FILE: relative/path/to/file.cs =====
<full file contents>

===== FILE: relative/path/to/another_file.cs =====
<full file contents>
""".strip()
