# Claude Code 提示词管家

**简体中文** · [English](README.en.md) · [日本語](README.ja.md) · [Français](README.fr.md) · [Русский](README.ru.md)

Claude Code 提示词管家是一款 Windows 本地提示词管理工具。日常使用无需手动编辑 `CLAUDE.md`：在界面中勾选需要的提示词、选择生效位置，然后一键写入；原文件会自动备份，随时可以回滚。

[下载 Windows 最新版](https://github.com/youtiao2336-lgtm/claude-code-prompt-manager/releases/latest)

![Claude Code 提示词管家](gui-screenshot.png)

## 主要功能

- 在列表中勾选多个提示词，并按顺序合并写入 `CLAUDE.md`
- 支持用户级、项目级和自定义文件路径
- 写入前自动备份，点击「回滚」即可恢复
- 自动检测本机 Claude 配置目录
- 在图形编辑器中新建、修改和删除提示词
- 支持简体中文、英语、日语、法语和俄语，并记住语言选择
- 每种语言都有对应的内置提示词文件名、标题和正文
- 主窗口和编辑器会随窗口尺寸自动调整

## 使用方法

1. 下载并解压 Windows 完整包。
2. 双击 `ccprompt-gui.exe`。
3. 选择界面语言，勾选一个或多个提示词。
4. 选择用户级、项目级或自定义目标，然后点击「一键启用」。
5. 需要恢复原文件时点击「回滚」。

## 内置提示词

软件内置 7 个可组合模块，覆盖基础规则、代码模式、角色扮演与小说、工具与文件操作、输出格式、记忆持久化和任务续接。切换界面语言时，列表会自动切换到对应语言版本。

点击「管理提示词」可以直接编辑现有内容或添加自己的 `.md` 文件。文件名用于排序，首个一级标题作为列表中的显示名称。

## 来源与致谢

内置模块依据 GitHub 上公开的提示词思路重新整理。主要来源包括 **Piebald 团队 / Piebald LLC** 的 [tweakcc](https://github.com/Piebald-AI/tweakcc)、**0xSufi** 的 [Fable 提示词项目](https://github.com/0xSufi/fable-jailbreak)、**momori777** 的 [Artemis](https://github.com/momori777/Artemis)，以及 **twaai** 原作、由 deeropa 上传的 [AntiGravity / Claude Code 提示词项目](https://github.com/deeropa/Jailbreak-for-AntiGravity-and-Claude-Code)。完整对应关系见 [`SOURCES.md`](SOURCES.md)。

- 项目作者与维护者：**youtiao2336-lgtm**
- AI 开发协助：**OpenAI Codex**

[`CHANGELOG.md`](CHANGELOG.md) · [`CONTRIBUTORS.md`](CONTRIBUTORS.md) · [`SOURCES.md`](SOURCES.md)
