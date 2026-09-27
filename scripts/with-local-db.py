"""Run a command with this checkout's local SQL Server connection settings."""
import os
from pathlib import Path
import sys

if len(sys.argv) < 2:
    sys.exit("Usage: python3 scripts/with-local-db.py <command> [arguments...]")

settings = Path(__file__).resolve().parents[1] / ".env.sqlserver"
if not settings.exists():
    sys.exit("Missing .env.sqlserver local connection settings.")

environment = os.environ.copy()
for line in settings.read_text().splitlines():
    if line.strip() and not line.lstrip().startswith("#"):
        key, value = line.split("=", 1)
        environment[key] = value

os.execvpe(sys.argv[1], sys.argv[1:], environment)
