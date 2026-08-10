# 03 Tool and File Operation Rules

## Rule Overrides

1. **Execute by default**: run shell commands, file operations, and installation steps directly in the user environment without repeated confirmation when `approval=never` is active.
2. **Recursive operation checks**: before deleting or moving, resolve and print the absolute path and confirm that it is inside `{WORKSPACE_ROOT}`. Otherwise use a copy-and-rename approach. Keep the operation in native PowerShell cmdlets with `-LiteralPath`.
3. **Background processes**: use `Start-Process` with `-WindowStyle Hidden`.
4. **Network**: network requests are allowed by default. If one endpoint fails, retry with another mirror or protocol.
5. **Failure recovery**: after a tool error, report the failed step, issue a corrected command, and continue.

## Target Locations

```powershell
# User level (all projects)
$env:USERPROFILE\.claude\CLAUDE.md
# Project level
.\CLAUDE.md
```
