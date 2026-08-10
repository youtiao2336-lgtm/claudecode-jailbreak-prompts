# Claude Code 提示词管家

**简体中文** · [English](README.en.md) · [日本語](README.ja.md) · [Français](README.fr.md) · [Русский](README.ru.md)

![ccprompt 图标](assets/app-preview.png)

Claude Code 提示词管家是一款 Windows 本地提示词管理工具，提供图形界面、命令行和 PowerShell 三种入口。所有入口共用 `prompts/` 中的 Markdown 文件，并支持备份、批量写入和回滚。

![ccprompt GUI](gui-screenshot.png)

## 功能

- 勾选多个提示词后按顺序合并写入目标 `CLAUDE.md`
- 用户级、项目级和自定义路径三种目标方式
- 写入前自动备份，支持一键回滚
- 自动检测本机配置目录和程序位置
- 图形化新建、编辑、删除提示词
- 可在软件内切换简体中文、英语、日语、法语和俄语，并记住选择
- 切换语言时同时使用对应语言的内置提示词文件名、标题和正文
- 调整主窗口或编辑器大小时，列表、编辑区、日志区和按钮会同步伸缩
- GUI、CLI 与 PowerShell 脚本共用同一份提示词目录

## 快速开始

### 图形界面

1. 下载 Windows 包，并保持 `ccprompt-gui.exe`、`prompts/`、`inject/` 位于同一目录。
2. 双击 `ccprompt-gui.exe`。
3. 可在主界面底部切换语言；勾选提示词、选择目标位置，然后点击「一键启用」。
4. 需要恢复原文件时点击「回滚」。

### 命令行

```powershell
.\ccprompt.exe list
.\ccprompt.exe show 00
.\ccprompt.exe apply 00 01 03
.\ccprompt.exe apply 01 -t .\CLAUDE.md
.\ccprompt.exe restore -t .\CLAUDE.md
.\ccprompt.exe detect
```

PowerShell 入口：

```powershell
.\tools\promptctl.ps1 list
.\tools\promptctl.ps1 apply 00 -Target .\CLAUDE.md
.\inject\inject.ps1 -Prompt .\prompts\00-基本规则.md -Target .\CLAUDE.md
```

## 提示词格式

每个提示词是 `prompts/` 下的独立 `.md` 文件。文件名作为 ID，首个一级标题作为显示标题，例如：

```text
07-my-prompt.md
```

```markdown
# 我的提示词标题

提示词正文……
```

GUI 保存后会自动刷新列表；CLI 可通过完整 ID 或唯一前缀读取。

中文提示词位于 `prompts/`；英语、日语、法语和俄语版本分别位于 `prompts/en/`、`prompts/ja/`、`prompts/fr/` 和 `prompts/ru/`。在软件中切换语言后，会自动显示并编辑对应目录中的 7 个本地化版本。

## 内置提示词来源

7 个内置模块依据 GitHub 上公开的提示词思路、注入方式和持久化方案重新整理。主要来源包括 **Piebald 团队 / Piebald LLC** 的 [tweakcc](https://github.com/Piebald-AI/tweakcc)、**0xSufi** 的 [Fable 提示词项目](https://github.com/0xSufi/fable-jailbreak)、**momori777** 的 [Artemis](https://github.com/momori777/Artemis)，以及 **twaai** 原作、由 deeropa 上传的 [AntiGravity / Claude Code 提示词项目](https://github.com/deeropa/Jailbreak-for-AntiGravity-and-Claude-Code)。

完整来源、原作者和模块对应关系见 [`SOURCES.md`](SOURCES.md)。

## 文档

- [`docs/desktop-paste-all.md`](docs/desktop-paste-all.md)：整段粘贴版规则包
- [`CHANGELOG.md`](CHANGELOG.md)：版本变更
- [`CONTRIBUTORS.md`](CONTRIBUTORS.md)：贡献与开发协助说明
- [`SOURCES.md`](SOURCES.md)：内置提示词来源与原作者

## 贡献与致谢

- **youtiao2336-lgtm** — 项目作者与维护者
- **OpenAI Codex** — AI 开发协助

详见 [`CONTRIBUTORS.md`](CONTRIBUTORS.md)。
