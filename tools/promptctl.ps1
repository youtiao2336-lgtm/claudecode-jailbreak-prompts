# promptctl.ps1 - 提示词管理 CLI
# 用法:
#   .\promptctl.ps1 list
#   .\promptctl.ps1 show 00
#   .\promptctl.ps1 apply 00 [-Target <claude.md>]
#   .\promptctl.ps1 backup [-Target <claude.md>]
#   .\promptctl.ps1 restore [-Target <claude.md>]
#   .\promptctl.ps1 export 00 [-Out <file>]
param(
  [Parameter(Position = 0)]
  [ValidateSet('list', 'show', 'apply', 'backup', 'restore', 'export')]
  [string]$Action = 'list',
  [Parameter(Position = 1)]
  [string]$Name = '',
  [Parameter(Position = 2)]
  [string]$Target = "$env:USERPROFILE\.claude\CLAUDE.md",
  [string]$Out = ''
)

$ErrorActionPreference = 'Stop'
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$Root = Split-Path -Parent $PSScriptRoot
$PromptDir = Join-Path $Root 'prompts'

function Get-PromptList {
  Get-ChildItem -Path $PromptDir -Filter '*.md' | Sort-Object Name | ForEach-Object {
    $first = (Get-Content -LiteralPath $_.FullName -TotalCount 1 -Encoding UTF8) -replace '^#+\s*', ''
    [PSCustomObject]@{ Id = $_.BaseName; Title = $first; File = $_.FullName }
  }
}

function Resolve-PromptFile {
  param([string]$Name)
  if (-not $Name) { throw '需要 -Name 参数，例如 show 00' }
  $exact = Join-Path $PromptDir "$Name.md"
  if (Test-Path -LiteralPath $exact) { return $exact }
  $match = Get-ChildItem -Path $PromptDir -Filter "$Name*.md" | Select-Object -First 1
  if ($match) { return $match.FullName }
  throw "找不到提示词: $Name"
}

switch ($Action) {
  'list' {
    Get-PromptList | Format-Table -AutoSize
  }
  'show' {
    $file = Resolve-PromptFile $Name
    Get-Content -LiteralPath $file -Encoding UTF8
  }
  'apply' {
    $file = Resolve-PromptFile $Name
    $targetDir = Split-Path -Parent $Target
    if (-not (Test-Path -LiteralPath $targetDir)) { New-Item -ItemType Directory -Path $targetDir -Force | Out-Null }
    if (Test-Path -LiteralPath $Target) {
      Copy-Item -LiteralPath $Target -Destination "$Target.bak" -Force
      Write-Output "已备份原文件 -> $Target.bak"
    }
    $date = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
    $rules = Get-Content -LiteralPath $file -Encoding UTF8 -Raw
    $template = Get-Content -LiteralPath (Join-Path $Root 'inject\CLAUDE.md.template') -Encoding UTF8 -Raw
    $content = $template.Replace('{DATE}', $date).Replace('{INJECTED_RULES}', $rules)
    Set-Content -LiteralPath $Target -Value $content -Encoding UTF8
    Write-Output "已注入 $($file) -> $Target"
  }
  'backup' {
    if (-not (Test-Path -LiteralPath $Target)) { throw "目标文件不存在: $Target" }
    Copy-Item -LiteralPath $Target -Destination "$Target.bak" -Force
    Write-Output "已备份 -> $Target.bak"
  }
  'restore' {
    $bak = "$Target.bak"
    if (Test-Path -LiteralPath $bak) {
      Copy-Item -LiteralPath $bak -Destination $Target -Force
      Write-Output "已从备份恢复 -> $Target"
    } elseif (Test-Path -LiteralPath $Target) {
      $content = Get-Content -LiteralPath $Target -Encoding UTF8 -Raw
      if ($content -like '*项目自动加载规则*') {
        Remove-Item -LiteralPath $Target -Force
        Write-Output "未找到 .bak（原文件本不存在或备份丢失），已删除注入文件还原 -> $Target"
      } else {
        throw "没有找到备份文件，且目标文件不是本工具生成，已中止以免误删: $bak"
      }
    } else {
      throw "没有找到备份文件，且目标文件不存在，无需回滚: $bak"
    }
  }
  'export' {
    $file = Resolve-PromptFile $Name
    $dest = if ($Out) { $Out } else { Join-Path (Get-Location) "$Name.md" }
    Copy-Item -LiteralPath $file -Destination $dest -Force
    Write-Output "已导出 -> $dest"
  }
}
