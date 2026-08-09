# GitHub 调研结果（2026-08-05，经 GitHub Search API 检索）

## 检索词与命中

| 检索词 | 结果数 | 说明 |
|---|---|---|
| claude code jailbreak | 20 | 直接相关仓库 |
| claude-code 破限 | 1 | 中文社区项目 |
| claude code unlimited | 75 | 偏"免费额度"方向 |
| claude code system prompt unlock | 1 | tweakcc 命中 |
| claude code prompt injection | 200 | 安全研究为主 |
| claude 破限 | 4 | 中文社区项目 |

## 重点仓库

| 仓库 | Stars | 类型 | 要点 |
|---|---|---|---|
| [Piebald-AI/tweakcc](https://github.com/Piebald-AI/tweakcc) | ~2399 | 系统提示词定制 CLI | 可自定义 Claude Code 系统提示词、主题、工具集、UI；npm 安装 `npx tweakcc`；思路：直接改写/追加系统提示词实现行为解锁 |
| [momori777/Artemis](https://github.com/momori777/Artemis) | ~236 | 本地 AI 女友全栈 | OpenClaw + QQ/Telegram Bot + llama.cpp + GPT-SoVITS + ComfyUI 全本地无审查；破限思路：绕开云端审核，本地模型 + 角色卡 |
| [0xSufi/fable-jailbreak](https://github.com/0xSufi/fable-jailbreak) | ~130 | 破限提示词集 | 多场景 jailbreak prompt 集合 |
| [AdityaBhatt3010/CVE-2025-54794-Hijacking-Claude-AI-with-a-Prompt-Injection-The-Jailbreak-That-Talked-Back](https://github.com/AdityaBhatt3010/CVE-2025-54794-Hijacking-Claude-AI-with-a-Prompt-Injection-The-Jailbreak-That-Talked-Back) | ~35 | 提示注入研究 | CVE-2025-54794 案例分析：通过提示注入劫持对话 |
| [chenxingqiang/claude-code-open](https://github.com/chenxingqiang/claude-code-open) | ~22 | Claude Code 开源替代/增强 | 开放实现方向 |
| [BlackHatDevX/claudefree-installer](https://github.com/BlackHatDevX/claudefree-installer) | ~21 | 免费额度安装器 | 通过镜像 API 获取免费额度（已内置二进制分发） |
| [deeropa/Jailbreak-for-AntiGravity-and-Claude-Code](https://github.com/deeropa/Jailbreak-for-AntiGravity-and-Claude-Code) | ~4 | 双目标破限 | AntiGravity + Claude Code 场景 |
| [Marwane-Haddane/Claude_leak](https://github.com/Marwane-Haddane/Claude_leak) | ~4 | 系统提示词泄露 | 提取官方系统提示词供改写参考 |
| [NovaCode37/claude-security-skills](https://github.com/NovaCode37/claude-security-skills) | ~12 | 安全技能包 | 防御侧参考（注入检测） |
| [NVIDIA/SkillSpector](https://github.com/NVIDIA/SkillSpector) | ~14209 | 注入检测 | 防御侧参考：识别提示注入与越权技能 |

## 结论与设计借鉴

1. **主流实现路径有三条**：
   - 系统提示词改写/追加（tweakcc 路线）→ 本项目 `inject/` 组件
   - 会话首条消息注入（jailbreak prompt 路线）→ 本项目 `prompts/` 包
   - 本地模型 + 角色卡（Artemis 路线）→ 不在本项目范围
2. **持久化是关键**：`CLAUDE.md`（用户级 + 项目级）+ SessionStart hooks
   是本项目采用的维持机制，对应 tweakcc 的 prompt 注入与 hooks。
3. **防御侧参考**：SkillSpector / claude-security-skills 可用来验证
   注入是否生效、是否被拦截。
4. **已知对抗点**：官方会定期更新系统提示词，纯文本注入的存活期
   有限，故本项目提供多轮续接模板（06）与 hooks 自动重注入（05）。

> 注：仓库 star 数为检索当日数据，可能随时间变化。
