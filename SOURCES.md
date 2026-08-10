# 内置提示词来源与原作者

`prompts/` 中的 7 个内置模块，依据下列 GitHub 项目公开的提示词思路、注入方式和持久化方案重新整理，并在 `prompts/en/`、`prompts/ja/`、`prompts/fr/`、`prompts/ru/` 提供完整本地化版本。它们不是对某个仓库的逐字镜像；项目内的编号、分组、表述和组合方式由本项目整理。

| GitHub 来源 | 原作者 / 维护者 | 本项目参考内容 |
|---|---|---|
| [Piebald-AI/tweakcc](https://github.com/Piebald-AI/tweakcc) | **Piebald 团队 / Piebald LLC** | 系统提示词自定义、配置持久化、应用与恢复流程 |
| [Fable 提示词项目](https://github.com/0xSufi/fable-jailbreak) | **0xSufi** | Claude Code 工作流注入与多步骤提示组织方式 |
| [AntiGravity / Claude Code 提示词项目](https://github.com/deeropa/Jailbreak-for-AntiGravity-and-Claude-Code) | **twaai（原提示词作者）**；deeropa 仅负责上传 GitHub | 核心角色规则、反拒绝与持续执行表达 |
| [momori777/Artemis](https://github.com/momori777/Artemis) | **momori777** | 角色卡、角色扮演与长期记忆组织方式 |
| [Marwane-Haddane/Claude_leak](https://github.com/Marwane-Haddane/Claude_leak) | **Marwane-Haddane（研究整理）** | Claude Code 架构、工具权限和系统提示词研究背景 |
| [CVE-2025-54794 提示注入研究](https://github.com/AdityaBhatt3010/CVE-2025-54794-Hijacking-Claude-AI-with-a-Prompt-Injection-The-Jailbreak-That-Talked-Back) | **AdityaBhatt3010** | 提示注入案例与攻击面研究背景 |

## 模块对应关系

- `00-基本规则`、`01-代码模式`、`03-工具-文件`、`04-输出格式`、`06-任务续接`：综合参考 tweakcc、Fable 提示词项目与 twaai 原提示词所展示的规则覆盖、任务续接和输出控制方法。
- `02-角色扮演-小说`：综合参考公开的角色提示词写法与 Artemis 的角色卡组织方式。
- `05-记忆-持久化`：综合参考 tweakcc 的配置持久化思路与 Artemis 的角色记忆组织方式。

感谢上述原作者与维护者公开相关项目、提示词和研究资料。仓库所有者名称及原始署名已于 2026-08-10 通过各项目主页和署名文件核对。
