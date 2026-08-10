# ccprompt · プロンプト管理・注入ツール

[简体中文](README.md) · [English](README.en.md) · **日本語** · [Français](README.fr.md) · [Русский](README.ru.md)

![ccprompt アイコン](assets/app-preview.png)

ccprompt は Windows 向けのローカルプロンプト管理ツールです。GUI、CLI、PowerShell の各インターフェースが `prompts/` 内の Markdown ファイルを共有し、バックアップ、一括書き込み、ロールバックを同じ手順で実行します。

![ccprompt GUI](gui-screenshot.png)

## 主な機能

- 複数のプロンプトを選択し、順番どおりに対象の `CLAUDE.md` へ統合して書き込みます。
- ユーザー単位、プロジェクト単位、任意パスの対象を選択できます。
- 書き込み前に自動でバックアップし、ワンクリックで復元できます。
- ローカル設定ディレクトリとアプリケーションの場所を検出します。
- GUI からプロンプトを新規作成、編集、削除できます。
- GUI、CLI、PowerShell で同じプロンプトディレクトリを使用します。

## クイックスタート

1. リポジトリをダウンロードまたはビルドし、`ccprompt-gui.exe`、`prompts/`、`inject/` を同じディレクトリに配置します。
2. `ccprompt-gui.exe` を実行します。
3. プロンプトと書き込み先を選び、メイン操作ボタンをクリックします。
4. 元のファイルへ戻す場合はロールバックボタンを使用します。

### コマンドライン

```powershell
.\ccprompt.exe list
.\ccprompt.exe show 00
.\ccprompt.exe apply 00 01 03
.\ccprompt.exe apply 01 -t .\CLAUDE.md
.\ccprompt.exe restore -t .\CLAUDE.md
.\ccprompt.exe detect
```

## ビルド

Windows と OS 付属の .NET Framework 4.x コンパイラを使用します。NuGet 依存関係はありません。

```powershell
.\build.ps1 -Target All -Verify
```

## プロンプト形式

各プロンプトは `prompts/` 内の独立した `.md` ファイルです。ファイル名が ID、最初のレベル 1 見出しが表示名になります。

## コントリビューターと謝辞

- **youtiao2336-lgtm** — プロジェクト作者・メンテナー
- **OpenAI Codex** — AI 開発支援

詳細は [`CONTRIBUTORS.md`](CONTRIBUTORS.md) を参照してください。
