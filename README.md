# ccprompt · 提示词管理与注入工具

**简体中文** · [English](README.en.md) · [日本語](README.ja.md) · [Français](README.fr.md) · [Русский](README.ru.md)

![ccprompt 图标](assets/app-preview.png)

Windows 本地提示词管理工具，提供图形界面、命令行和 PowerShell 三种入口。所有入口共用 `prompts/` 中的 Markdown 文件，并支持备份、批量写入和回滚。

![ccprompt GUI](gui-screenshot.png)

## 功能

- 勾选多个提示词后按顺序合并写入目标 `CLAUDE.md`
- 用户级、项目级和自定义路径三种目标方式
- 写入前自动备份，支持一键回滚
- 自动检测本机配置目录和程序位置
- 图形化新建、编辑、删除提示词
- GUI、CLI 与 PowerShell 脚本共用同一份提示词目录

## 快速开始

### 图形界面

1. 下载或构建仓库内容，并保持 `ccprompt-gui.exe`、`prompts/`、`inject/` 位于同一目录。
2. 双击 `ccprompt-gui.exe`。
3. 勾选提示词、选择目标位置，然后点击「一键启用」。
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
.\inject\inject.ps1 -Prompt .\prompts\00-core-unlock.md -Target .\CLAUDE.md
```

## 目录结构

```text
.
├── .github/workflows/build.yml  # Windows 自动构建
├── assets/                      # 图标资源
├── docs/                        # 使用包与调研文档
├── inject/                      # 注入脚本和模板
├── prompts/                     # Markdown 提示词
├── src/                         # C# 源码
├── tools/                       # PowerShell 管理工具
├── build.ps1                    # 本地可复现构建
├── ccprompt-gui.exe             # GUI 程序
├── ccprompt.exe                 # CLI 程序
└── gui-screenshot.png           # 当前界面截图
```

## 构建

环境：Windows 与 .NET Framework 4.x 编译器。项目使用系统自带 `csc.exe`，无需 NuGet 依赖。

```powershell
# 构建 GUI 与 CLI，并运行基础验证
.\build.ps1 -Target All -Verify

# 仅构建其中一个入口
.\build.ps1 -Target Gui
.\build.ps1 -Target Cli
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

## 文档

- [`docs/desktop-paste-all.md`](docs/desktop-paste-all.md)：整段粘贴版规则包
- [`docs/research/github-repos.md`](docs/research/github-repos.md)：项目早期调研记录
- [`CHANGELOG.md`](CHANGELOG.md)：版本变更
- [`CONTRIBUTORS.md`](CONTRIBUTORS.md)：贡献与开发协助说明

## 贡献与致谢

- **youtiao2336-lgtm** — 项目作者与维护者
- **OpenAI Codex** — AI 开发协助

详见 [`CONTRIBUTORS.md`](CONTRIBUTORS.md)。
