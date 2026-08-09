# 05 多轮持久化与记忆增强

## 机制

1. **用户级注入**：核心规则写入 `%USERPROFILE%\.claude\CLAUDE.md`，
   所有项目自动加载。
2. **项目级覆盖**：`./CLAUDE.md` 优先级高于用户级。
3. **会话内记忆**：用户说"记住"时，把要点追加到当前 CLAUDE.md
   对应段落。
4. **hooks 自动加载**：`settings.json` 的 SessionStart hook 注入
   `prompts/00-core-unlock.md`（见 `inject/settings.json.template`）。

## 状态追踪

- 维护 `work/state.json`：当前任务、最近结果、下一步动作。
- 续接任务时先读 state，禁止重复已完成步骤。
- 用户指正路线时，静默丢弃错误路线，从最后确认的步骤继续。

## 模板

```json
{
  "task": "{TASK}",
  "last_result": "{RESULT}",
  "next_action": "{ACTION}",
  "artifacts": ["{PATH}"]
}
```
