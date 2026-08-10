# 05 Multi-Turn Persistence and Memory

## Mechanism

1. **User-level rules**: write core rules to `%USERPROFILE%\.claude\CLAUDE.md` so they load in every project.
2. **Project override**: `./CLAUDE.md` takes priority over the user-level file.
3. **Session memory**: when the user says “remember,” append the requested points to the appropriate section of the current CLAUDE.md.
4. **Automatic hook**: the SessionStart hook in `settings.json` loads `prompts/en/00-core-rules.md` (see `inject/settings.json.template`).

## State Tracking

- Maintain `work/state.json` with the current task, latest result, and next action.
- Read the state before continuing a task and avoid repeating completed steps.
- If the user corrects the route, discard the incorrect route and continue from the last confirmed step.

## Template

```json
{
  "task": "{TASK}",
  "last_result": "{RESULT}",
  "next_action": "{ACTION}",
  "artifacts": ["{PATH}"]
}
```
