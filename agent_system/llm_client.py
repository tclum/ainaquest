import os
from openai import OpenAI
from .config import MODEL_NAME

client = OpenAI(api_key=os.getenv("OPENAI_API_KEY"))


def ask_llm(prompt: str, model: str = MODEL_NAME) -> str:
    response = client.responses.create(
        model=model,
        input=prompt,
    )
    return response.output_text
