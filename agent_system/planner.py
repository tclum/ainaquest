def build_plan_prompt(task_info: dict, file_context: str, memory_context: str) -> str:
    success = "\n".join(f"- {item}" for item in task_info["success_criteria"])
    editable = "\n".join(f"- {item}" for item in task_info["editable_files"])

    return f"""
You are the Planner Agent for a Unity game project.

Project memory:
{memory_context}

Task:
{task_info["title"]}

Success Criteria:
{success}

Editable Files:
{editable}

Relevant File Context:
{file_context}

Produce:
1. Implementation summary
2. Step-by-step plan
3. Risks / possible breakpoints
4. Validation checklist

Keep the plan practical and scoped to only the editable files.
""".strip()
