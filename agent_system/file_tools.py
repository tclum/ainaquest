from pathlib import Path


def read_text_file(path: str | Path) -> str:
    path = Path(path)
    return path.read_text(encoding="utf-8")


def write_text_file(path: str | Path, content: str) -> None:
    path = Path(path)
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(content, encoding="utf-8")


def ensure_dir(path: str | Path) -> None:
    Path(path).mkdir(parents=True, exist_ok=True)


def file_exists(path: str | Path) -> bool:
    return Path(path).exists()
