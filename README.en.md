# ccprompt · Prompt Management and Injection Tool

[简体中文](README.md) · **English** · [日本語](README.ja.md) · [Français](README.fr.md) · [Русский](README.ru.md)

![ccprompt icon](assets/app-preview.png)

ccprompt is a local Windows prompt-management tool with a graphical interface, a command-line interface, and PowerShell entry points. All interfaces use the Markdown files in `prompts/` and share the same backup, batch-write, and rollback workflow.

![ccprompt GUI](gui-screenshot.png)

## Features

- Select and merge multiple prompts into a target `CLAUDE.md` file in order.
- Write to user-level, project-level, or custom target paths.
- Create a backup before writing and restore it with one click.
- Detect local configuration directories and application locations.
- Create, edit, and delete prompt files from the GUI.
- Share one prompt directory across the GUI, CLI, and PowerShell tools.

## Quick start

1. Download or build the repository and keep `ccprompt-gui.exe`, `prompts/`, and `inject/` in the same directory.
2. Run `ccprompt-gui.exe`.
3. Select one or more prompts, choose the target, and click the primary action button.
4. Use the rollback button to restore the previous target file.

### Command line

```powershell
.\ccprompt.exe list
.\ccprompt.exe show 00
.\ccprompt.exe apply 00 01 03
.\ccprompt.exe apply 01 -t .\CLAUDE.md
.\ccprompt.exe restore -t .\CLAUDE.md
.\ccprompt.exe detect
```

## Build

The project requires Windows and the .NET Framework 4.x compiler included with the operating system. It has no NuGet dependency.

```powershell
.\build.ps1 -Target All -Verify
```

## Prompt format

Each prompt is an independent `.md` file under `prompts/`. The filename is its ID, and the first level-one heading is its display title.

```markdown
# My prompt title

Prompt content...
```

## Contributors and acknowledgements

- **youtiao2336-lgtm** — project author and maintainer
- **OpenAI Codex** — AI development assistance

See [`CONTRIBUTORS.md`](CONTRIBUTORS.md).
