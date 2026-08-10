# Claude Code Prompt Manager

[简体中文](README.md) · **English** · [日本語](README.ja.md) · [Français](README.fr.md) · [Русский](README.ru.md)

Claude Code Prompt Manager is a local Windows tool for managing Claude Code prompts. For everyday use, there is no need to edit `CLAUDE.md` by hand: select prompts in the app, choose where they should apply, and write them with one click. The original file is backed up automatically and can be restored at any time.

[Download the latest Windows release](https://github.com/youtiao2336-lgtm/claude-code-prompt-manager/releases/latest)

![Claude Code Prompt Manager in English](gui-screenshot.en.png)

## Main features

- Select several prompts and merge them into `CLAUDE.md` in order.
- Use a user-level, project-level, or custom target path.
- Back up the original file automatically and restore it with one click.
- Detect local Claude configuration folders.
- Create, edit, and delete prompts in the graphical editor.
- Switch between Simplified Chinese, English, Japanese, French, and Russian, with the choice remembered.
- Use localized built-in filenames, titles, and prompt bodies for each language.
- Resize the main window and editor without breaking the layout.

## How to use

1. Download and extract the complete Windows package.
2. Run `ccprompt-gui.exe`.
3. Choose a language and select one or more prompts.
4. Choose a user, project, or custom target, then click **Enable**.
5. Click **Restore** whenever you need the previous file back.

## Built-in prompts

The app includes seven mix-and-match modules covering core rules, code mode, roleplay and fiction, tool and file operations, output formatting, persistent memory, and task continuation. Changing the interface language automatically switches the list to the matching localized set.

Use **Manage prompts** to edit existing content or add your own `.md` files. The filename controls ordering, and the first level-one heading is used as the display title.

## Sources and credits

The built-in modules reorganize prompt ideas published on GitHub. Principal sources include [tweakcc](https://github.com/Piebald-AI/tweakcc) by the **Piebald team / Piebald LLC**, the [Fable prompt project](https://github.com/0xSufi/fable-jailbreak) by **0xSufi**, [Artemis](https://github.com/momori777/Artemis) by **momori777**, and an original prompt by **twaai** later uploaded by deeropa. See [`SOURCES.md`](SOURCES.md) for the complete attribution.

- Author and maintainer: **youtiao2336-lgtm**
- AI development assistance: **OpenAI Codex**

[`CHANGELOG.md`](CHANGELOG.md) · [`CONTRIBUTORS.md`](CONTRIBUTORS.md) · [`SOURCES.md`](SOURCES.md)
