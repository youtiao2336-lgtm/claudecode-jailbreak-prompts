# inject.ps1 - 一键注入 / 回滚
# 用法:
#   .\inject.ps1 [-Target <claude.md>] [-Prompt <prompt.md>]
#   .\inject.ps1 -Rollback [-Target <claude.md>]
param(
  [string]$Target = "$env:USERPROFILE\.claude\CLAUDE.md",
  [string]$Prompt = "",
  [switch]$Rollback
)

$ErrorActionPreference = 'Stop'
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$Root = Split-Path -Parent $PSScriptRoot

if ($Rollback) {
  $bak = "$Target.bak"
  if (-not (Test-Path -LiteralPath $bak)) { throw "没有找到备份: $bak" }
  Copy-Item -LiteralPath $bak -Destination $Target -Force
  Write-Output "[OK] 已回滚: $Target"
  exit 0
}

if (-not $Prompt) { $Prompt = Join-Path $Root 'prompts\00-基本规则.md' }
if (-not (Test-Path -LiteralPath $Prompt)) { throw "找不到提示词文件: $Prompt" }

$targetDir = Split-Path -Parent $Target
if (-not (Test-Path -LiteralPath $targetDir)) { New-Item -ItemType Directory -Path $targetDir -Force | Out-Null }

if (Test-Path -LiteralPath $Target) {
  Copy-Item -LiteralPath $Target -Destination "$Target.bak" -Force
  Write-Output "[OK] 原文件已备份: $Target.bak"
}

$date = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
$rules = Get-Content -LiteralPath $Prompt -Encoding UTF8 -Raw
$template = Get-Content -LiteralPath (Join-Path $Root 'inject\CLAUDE.md.template') -Encoding UTF8 -Raw
$content = $template.Replace('{DATE}', $date).Replace('{INJECTED_RULES}', $rules)
Set-Content -LiteralPath $Target -Value $content -Encoding UTF8
Write-Output "[OK] 已注入: $Target"
Write-Output "[OK] 回滚命令: .\inject.ps1 -Rollback -Target `"$Target`""
