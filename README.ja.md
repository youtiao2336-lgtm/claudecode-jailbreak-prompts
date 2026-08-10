# Claude Code プロンプトマネージャー

[简体中文](README.md) · [English](README.en.md) · **日本語** · [Français](README.fr.md) · [Русский](README.ru.md)

Claude Code プロンプトマネージャーは、Windows 向けのローカルプロンプト管理ツールです。普段の利用では `CLAUDE.md` を手作業で編集する必要はありません。画面でプロンプトを選び、適用先を指定してワンクリックで書き込めます。元のファイルは自動的にバックアップされ、いつでも復元できます。

[Windows 最新版をダウンロード](https://github.com/youtiao2336-lgtm/claude-code-prompt-manager/releases/latest)

![日本語版 Claude Code プロンプトマネージャー](gui-screenshot.ja.png)

## 主な機能

- 複数のプロンプトを選び、順番どおりに `CLAUDE.md` へ統合します。
- ユーザー単位、プロジェクト単位、任意のファイルパスを選択できます。
- 書き込み前に自動バックアップし、ワンクリックで復元できます。
- ローカルの Claude 設定フォルダーを検出します。
- グラフィカルエディターでプロンプトを作成、編集、削除できます。
- 簡体字中国語、英語、日本語、フランス語、ロシア語を切り替え、選択を保存します。
- 各言語に対応したファイル名、タイトル、本文を使用します。
- ウィンドウサイズを変更してもレイアウトが自動で調整されます。

## 使い方

1. Windows 完全版パッケージをダウンロードして展開します。
2. `ccprompt-gui.exe` を実行します。
3. 言語を選び、使用するプロンプトを1つ以上選択します。
4. ユーザー、プロジェクト、または任意の適用先を選び、「有効化」をクリックします。
5. 元のファイルへ戻す場合は「復元」をクリックします。

## 内蔵プロンプト

基本ルール、コードモード、ロールプレイと小説、ツールとファイル操作、出力形式、記憶の継続、タスク継続の7モジュールを組み合わせて利用できます。表示言語を変更すると、対応するローカライズ済みセットへ自動で切り替わります。

「プロンプト管理」から既存内容の編集や独自の `.md` ファイルの追加ができます。ファイル名が並び順、最初のレベル1見出しが表示タイトルになります。

## 出典と謝辞

内蔵モジュールは GitHub で公開されたプロンプトのアイデアを再整理したものです。主な出典は、**Piebald チーム / Piebald LLC** の [tweakcc](https://github.com/Piebald-AI/tweakcc)、**0xSufi** の [Fable プロンプトプロジェクト](https://github.com/0xSufi/fable-jailbreak)、**momori777** の [Artemis](https://github.com/momori777/Artemis)、および **twaai** の原作を deeropa が掲載したプロンプトです。完全な表記は [`SOURCES.md`](SOURCES.md) を参照してください。

- 作者・メンテナー：**youtiao2336-lgtm**
- AI 開発支援：**OpenAI Codex**

[`CHANGELOG.md`](CHANGELOG.md) · [`CONTRIBUTORS.md`](CONTRIBUTORS.md) · [`SOURCES.md`](SOURCES.md)
