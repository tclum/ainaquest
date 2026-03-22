from pathlib import Path

UNITY_ROOT = Path(__file__).resolve().parent.parent
AGENT_ROOT = UNITY_ROOT / "agent_system"

PROMPTS_DIR = AGENT_ROOT / "prompts"
MEMORY_DIR = AGENT_ROOT / "memory"
TASKS_DIR = AGENT_ROOT / "tasks"
OUTPUTS_DIR = AGENT_ROOT / "outputs"

MODEL_NAME = "gpt-5"
MAX_FILE_CHARS = 40000
MAX_TOTAL_CONTEXT_CHARS = 120000
