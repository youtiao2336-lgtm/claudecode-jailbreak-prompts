[CmdletBinding()]
param(
    [ValidateSet('All', 'Gui', 'Cli')]
    [string]$Target = 'All',
    [switch]$Verify
)

$ErrorActionPreference = 'Stop'
[Console]::OutputEncoding = [Text.UTF8Encoding]::new($false)
$Root = [IO.Path]::GetFullPath($PSScriptRoot)

$candidates = @(
    "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319\csc.exe",
    "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe"
)
$Csc = $candidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
if (-not $Csc) { throw 'The .NET Framework 4.x C# compiler (csc.exe) was not found.' }

function Invoke-Compiler {
    param([string]$Name, [string[]]$CompilerArgs)
    Write-Output "== Building $Name =="
    & $Csc @CompilerArgs
    if ($LASTEXITCODE -ne 0) { throw "$Name build failed with exit code $LASTEXITCODE" }
}

if ($Target -in @('All', 'Gui')) {
    Invoke-Compiler 'ccprompt-gui.exe' @(
        '/nologo', '/codepage:65001', '/target:winexe',
        "/win32icon:$Root\assets\app.ico",
        "/out:$Root\ccprompt-gui.exe",
        '/reference:System.dll', '/reference:System.Drawing.dll', '/reference:System.Windows.Forms.dll',
        "$Root\src\ProgramGUI.cs", "$Root\src\PromptLib.cs", "$Root\src\MainForm.cs",
        "$Root\src\Ui.cs", "$Root\src\PromptEditorForm.cs"
    )
}

if ($Target -in @('All', 'Cli')) {
    Invoke-Compiler 'ccprompt.exe' @(
        '/nologo', '/codepage:65001', '/target:exe',
        "/out:$Root\ccprompt.exe",
        '/reference:System.dll',
        "$Root\src\ProgramCLI.cs", "$Root\src\PromptLib.cs", "$Root\src\Cli.cs"
    )
}

if ($Verify) {
    $promptCount = @(Get-ChildItem -LiteralPath "$Root\prompts" -Filter '*.md' -File).Count
    if ($Target -in @('All', 'Gui')) {
        [Reflection.AssemblyName]::GetAssemblyName("$Root\ccprompt-gui.exe") | Out-Null
        Write-Output "[OK] GUI assembly is valid; prompt files: $promptCount"
    }
    if ($Target -in @('All', 'Cli')) {
        $output = @(& "$Root\ccprompt.exe" list 2>&1)
        $exitCode = $LASTEXITCODE
        $output | ForEach-Object { Write-Output $_ }
        $listedCount = @($output | Where-Object { $_ -match '^\d{2}-' }).Count
        if ($exitCode -ne 0 -or $listedCount -ne $promptCount) {
            throw "CLI verification failed with exit code $exitCode"
        }
        Write-Output '[OK] CLI list verification passed'
    }
}

Write-Output "Build complete: $Target"
