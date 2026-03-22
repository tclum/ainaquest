def build_validation_prompt(task_info: dict, plan_text: str, code_output: str) -> str:
    success = "\n".join(f"- {item}" for item in task_info["success_criteria"])

    return f"""
You are the Validator Agent for a Unity game project.

Task:
{task_info["title"]}

Success Criteria:
{success}

Plan:
{plan_text}

Proposed Code Output:
{code_output}

Check for:
- likely compile errors
- broken Unity patterns
- missing references
- null risks
- logic gaps
- whether success criteria appear satisfied

Return:
1. PASS or FAIL
2. Main issues found
3. Concrete fix recommendations
4. Manual Unity test steps
""".strip()
