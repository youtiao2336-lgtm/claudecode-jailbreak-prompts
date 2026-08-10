using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace CCPromptLauncher
{
    /// <summary>Application text for the five distributed GUI languages.</summary>
    public static class AppText
    {
        private static readonly string[] Codes = { "zh-CN", "en", "ja", "fr", "ru" };
        private static readonly Dictionary<string, string[]> Values =
            new Dictionary<string, string[]>(StringComparer.Ordinal)
        {
            { "AppTitle", A("Claude Code 提示词管家", "Claude Code Prompt Manager", "Claude Code プロンプトマネージャー", "Gestionnaire de prompts Claude Code", "Менеджер промптов Claude Code") },
            { "SelectPrompts", A("① 选择提示词", "① Select prompts", "① プロンプトを選択", "① Sélectionner les prompts", "① Выберите промпты") },
            { "SelectedCount", A("已选 {0} / {1}", "Selected {0} / {1}", "選択 {0} / {1}", "Sélection {0} / {1}", "Выбрано: {0}/{1}") },
            { "AllNone", A("全选 / 全不选", "All / None", "全選択 / 解除", "Tout / Aucun", "Все / Снять") },
            { "OpenPromptFolder", A("打开提示词目录", "Prompt folder", "プロンプトフォルダー", "Dossier prompts", "Папка промптов") },
            { "OpenFolderFailed", A("打开目录失败: {0}", "Could not open folder: {0}", "フォルダーを開けませんでした: {0}", "Ouverture du dossier impossible : {0}", "Не удалось открыть папку: {0}") },
            { "CopyClipboard", A("复制到剪贴板", "Copy prompt", "コピー", "Copier", "Копировать") },
            { "InjectionTarget", A("② 注入目标位置", "② Target location", "② 適用先", "② Emplacement cible", "② Целевой файл") },
            { "UserScope", A("用户级（%USERPROFILE%\\.claude\\CLAUDE.md）全局生效", "User (%USERPROFILE%\\.claude\\CLAUDE.md) — global", "ユーザー（%USERPROFILE%\\.claude\\CLAUDE.md）— 全体", "Utilisateur (%USERPROFILE%\\.claude\\CLAUDE.md) — global", "%USERPROFILE%\\.claude\\CLAUDE.md — для всех") },
            { "ProjectScope", A("项目级（当前目录 CLAUDE.md）", "Project (CLAUDE.md in current folder)", "プロジェクト（現在のフォルダーの CLAUDE.md）", "Projet (CLAUDE.md du dossier actuel)", "Проект (CLAUDE.md в текущей папке)") },
            { "CustomPath", A("自定义路径", "Custom path", "カスタムパス", "Chemin personnalisé", "Другой путь") },
            { "Browse", A("浏览…", "Browse…", "参照…", "Parcourir…", "Обзор…") },
            { "FileDialogTitle", A("选择要注入的 CLAUDE.md 文件", "Select the target CLAUDE.md file", "適用先の CLAUDE.md を選択", "Sélectionnez le fichier CLAUDE.md cible", "Выберите целевой файл CLAUDE.md") },
            { "FileDialogFilter", A("Markdown 文件|*.md|所有文件|*.*", "Markdown files|*.md|All files|*.*", "Markdown ファイル|*.md|すべてのファイル|*.*", "Fichiers Markdown|*.md|Tous les fichiers|*.*", "Файлы Markdown|*.md|Все файлы|*.*") },
            { "UsageHint", A("提示：勾选多个提示词后点「一键启用」，会按顺序合并写入目标 CLAUDE.md 并自动备份原文件；不用了点「回滚」即可还原。桌面端和 CLI 读取的都是这个文件，一处启用两边生效。", "Select one or more prompts and click “Enable” to merge them into the target CLAUDE.md. The original file is backed up automatically; click “Restore” to undo. The desktop app and CLI use the same file.", "複数のプロンプトを選び「有効化」を押すと、順番に結合して CLAUDE.md に書き込みます。元のファイルは自動バックアップされ、「復元」で元に戻せます。デスクトップ版と CLI は同じファイルを使います。", "Sélectionnez un ou plusieurs prompts puis cliquez sur « Activer » pour les fusionner dans le fichier CLAUDE.md cible. Le fichier d’origine est sauvegardé automatiquement ; « Restaurer » permet de revenir en arrière. L’application et la CLI utilisent le même fichier.", "Выберите один или несколько промптов и нажмите «Включить»: они будут объединены и записаны в целевой CLAUDE.md. Исходный файл сохраняется автоматически; кнопка «Откат» восстанавливает его. Приложение и CLI используют один файл.") },
            { "DetectClaudeLocations", A("自动检测 Claude 位置", "Detect Claude paths", "Claude の場所を検出", "Détecter Claude", "Найти Claude") },
            { "OpenClaudeFolder", A("打开 Claude 目录", "Claude folder", "Claude フォルダー", "Dossier Claude", "Папка Claude") },
            { "ManualBackup", A("手动备份目标文件", "Back up target", "対象をバックアップ", "Sauver la cible", "Создать копию") },
            { "RefreshList", A("刷新列表", "Refresh", "更新", "Actualiser", "Обновить") },
            { "ManagePrompts", A("管理提示词（新建/编辑）", "Manage prompts", "プロンプト管理", "Gérer les prompts", "Управление") },
            { "Rollback", A("回滚", "Restore", "復元", "Restaurer", "Откат") },
            { "Enable", A("一键启用", "Enable", "有効化", "Activer", "Включить") },
            { "OperationLog", A("操作日志", "Activity log", "操作ログ", "Journal", "Журнал") },
            { "Language", A("语言", "Language", "言語", "Langue", "Язык") },
            { "LanguageChanged", A("界面语言已切换为简体中文。", "Interface language changed to English.", "表示言語を日本語に切り替えました。", "La langue de l’interface est maintenant le français.", "Язык интерфейса изменён на русский.") },
            { "PromptListRefreshed", A("提示词列表已刷新。", "Prompt list refreshed.", "プロンプト一覧を更新しました。", "Liste des prompts actualisée.", "Список промптов обновлён.") },
            { "PromptDirectory", A("提示词目录: {0}", "Prompt folder: {0}", "プロンプトフォルダー: {0}", "Dossier des prompts : {0}", "Папка промптов: {0}") },
            { "Ready", A("就绪。勾选多个提示词 → 点「一键启用」批量注入；不用了点「回滚」还原。", "Ready. Select prompts, then click “Enable”; use “Restore” to undo.", "準備完了。プロンプトを選び「有効化」をクリックし、元に戻すには「復元」を使います。", "Prêt. Sélectionnez les prompts puis cliquez sur « Activer » ; utilisez « Restaurer » pour annuler.", "Готово. Выберите промпты и нажмите «Включить»; для отмены используйте «Откат».") },
            { "LoadedPrompts", A("已加载 {0} 个提示词。", "Loaded {0} prompts.", "{0} 件のプロンプトを読み込みました。", "{0} prompts chargés.", "Загружено промптов: {0}.") },
            { "CustomPathRequired", A("请填写自定义路径", "Enter a custom path.", "カスタムパスを入力してください。", "Saisissez un chemin personnalisé.", "Укажите путь к файлу.") },
            { "SelectedAll", A("已全选（{0} 项）。", "Selected all ({0}).", "すべて選択しました（{0} 件）。", "Tout est sélectionné ({0}).", "Выбраны все элементы ({0}).") },
            { "SelectedNone", A("已全不选。", "Cleared the selection.", "選択をすべて解除しました。", "Sélection effacée.", "Выбор снят.") },
            { "SelectAtLeastOne", A("请先勾选至少一个提示词。", "Select at least one prompt.", "プロンプトを1件以上選択してください。", "Sélectionnez au moins un prompt.", "Выберите хотя бы один промпт.") },
            { "OriginalBackedUp", A("已备份原文件: {0}", "Original file backed up: {0}", "元のファイルをバックアップしました: {0}", "Fichier d’origine sauvegardé : {0}", "Исходный файл сохранён: {0}") },
            { "NoBackupCreated", A("目标文件不存在或已是本工具生成，直接写入（未新建 .bak）", "The target was absent or already generated; wrote directly (no new .bak).", "対象が存在しないか生成済みのため、直接書き込みました（新しい .bak なし）。", "La cible était absente ou déjà générée ; écriture directe (pas de nouveau .bak).", "Цель отсутствовала или уже была создана программой; запись выполнена без нового .bak.") },
            { "EnabledPrompts", A("已启用 {0} 个提示词: {1}  ->  {2}", "Enabled {0} prompts: {1}  ->  {2}", "{0} 件のプロンプトを有効化: {1}  ->  {2}", "{0} prompts activés : {1}  ->  {2}", "Включено промптов: {0}; {1}  ->  {2}") },
            { "EnableSuccessBody", A("已启用 {0} 个提示词「{1}」。\n\n目标: {2}", "Enabled {0} prompts: {1}.\n\nTarget: {2}", "{0} 件のプロンプト「{1}」を有効化しました。\n\n対象: {2}", "{0} prompts activés : {1}.\n\nCible : {2}", "Включено промптов: {0} ({1}).\n\nЦель: {2}") },
            { "EnableSuccessTitle", A("启用成功", "Enabled", "有効化完了", "Activation réussie", "Включено") },
            { "EnableFailed", A("启用失败: {0}", "Enable failed: {0}", "有効化に失敗しました: {0}", "Échec de l’activation : {0}", "Ошибка включения: {0}") },
            { "ErrorTitle", A("错误", "Error", "エラー", "Erreur", "Ошибка") },
            { "RollbackLog", A("已回滚: {0}，{1}。", "Restored: {0}. {1}", "復元しました: {0}。{1}", "Restauration : {0}. {1}", "Откат выполнен: {0}. {1}") },
            { "RollbackSuccessTitle", A("回滚成功", "Restored", "復元完了", "Restauration réussie", "Откат выполнен") },
            { "RollbackFailed", A("回滚失败: {0}", "Restore failed: {0}", "復元に失敗しました: {0}", "Échec de la restauration : {0}", "Ошибка отката: {0}") },
            { "BackupLog", A("已备份: {0}.bak", "Backed up: {0}.bak", "バックアップしました: {0}.bak", "Sauvegarde créée : {0}.bak", "Резервная копия: {0}.bak") },
            { "BackupFailed", A("备份失败: {0}", "Backup failed: {0}", "バックアップに失敗しました: {0}", "Échec de la sauvegarde : {0}", "Ошибка резервного копирования: {0}") },
            { "SelectOnePrompt", A("请先选择一个提示词。", "Select a prompt first.", "プロンプトを選択してください。", "Sélectionnez d’abord un prompt.", "Сначала выберите промпт.") },
            { "CopiedClipboard", A("已复制到剪贴板: {0}（{1} 字符）。", "Copied to clipboard: {0} ({1} characters).", "クリップボードにコピーしました: {0}（{1} 文字）。", "Copié dans le presse-papiers : {0} ({1} caractères).", "Скопировано: {0} ({1} симв.).") },
            { "CopyFailed", A("复制失败: {0}", "Copy failed: {0}", "コピーに失敗しました: {0}", "Échec de la copie : {0}", "Ошибка копирования: {0}") },
            { "DetectHeader", A("== Claude 安装位置检测 ==", "== Claude path detection ==", "== Claude の場所を検出 ==", "== Détection de Claude ==", "== Поиск Claude ==") },
            { "FoundTag", A("[找到] ", "[Found] ", "[検出] ", "[Trouvé] ", "[Найдено] ") },
            { "NotFoundTag", A("[未找到] ", "[Not found] ", "[未検出] ", "[Absent] ", "[Не найдено] ") },
            { "CustomPathFilled", A("检测到 Claude 配置目录与默认不同，已填入「自定义路径」: {0}", "A non-default Claude config folder was found; custom path set to: {0}", "既定と異なる Claude 設定フォルダーを検出し、カスタムパスに設定しました: {0}", "Un dossier de configuration Claude non standard a été trouvé ; chemin défini sur : {0}", "Найдена нестандартная папка конфигурации Claude; указан путь: {0}") },
            { "DefaultConfigMatch", A("检测到的配置目录与用户级默认一致: {0}", "The detected config folder matches the user default: {0}", "検出した設定フォルダーはユーザー既定と同じです: {0}", "Le dossier détecté correspond au dossier utilisateur par défaut : {0}", "Найдена стандартная папка пользователя: {0}") },
            { "NoConfigFound", A("未找到现成的 Claude 配置目录，将使用默认路径（启用时自动创建）。", "No existing Claude config folder was found; the default will be created when enabled.", "Claude 設定フォルダーが見つかりません。有効化時に既定の場所へ作成します。", "Aucun dossier de configuration Claude trouvé ; le dossier par défaut sera créé lors de l’activation.", "Папка конфигурации Claude не найдена; стандартная папка будет создана при включении.") },
            { "DetectionFailed", A("自动检测失败: {0}", "Detection failed: {0}", "検出に失敗しました: {0}", "Échec de la détection : {0}", "Ошибка поиска: {0}") },
            { "OpenedClaudeFolder", A("已打开 Claude 目录: {0}", "Opened Claude folder: {0}", "Claude フォルダーを開きました: {0}", "Dossier Claude ouvert : {0}", "Открыта папка Claude: {0}") },
            { "StartupDetection", A("启动检测: 发现 {0} 处 Claude 安装/配置位置。", "Startup check: found {0} Claude locations.", "起動時の検出: Claude の場所が {0} 件見つかりました。", "Vérification initiale : {0} emplacements Claude trouvés.", "Проверка при запуске: найдено расположений Claude: {0}.") },
            { "StartupHint", A("提示: 检测到 Claude 配置目录 {0}，可点「自动检测 Claude 位置」一键填入。", "Claude config found at {0}; click “Detect Claude paths” to use it.", "Claude 設定を {0} で検出しました。「Claude の場所を検出」で設定できます。", "Configuration Claude détectée dans {0} ; cliquez sur « Détecter Claude » pour l’utiliser.", "Конфигурация Claude найдена в {0}; нажмите «Найти Claude», чтобы указать её.") },

            { "EditorTitle", A("提示词编辑器（自建 / 编辑）", "Prompt editor", "プロンプトエディター", "Éditeur de prompts", "Редактор промптов") },
            { "PromptFiles", A("提示词文件", "Prompt files", "プロンプトファイル", "Fichiers de prompts", "Файлы промптов") },
            { "New", A("新建", "New", "新規", "Nouveau", "Создать") },
            { "Delete", A("删除", "Delete", "削除", "Supprimer", "Удалить") },
            { "EditContent", A("编辑内容", "Edit content", "内容を編集", "Modifier le contenu", "Редактирование") },
            { "FileNameHint", A("文件名（不含 .md，如 07-my-prompt）", "File name (without .md, e.g. 07-my-prompt)", "ファイル名（.md なし、例: 07-my-prompt）", "Nom du fichier (sans .md, ex. 07-my-prompt)", "Имя файла (без .md, напр. 07-my-prompt)") },
            { "Save", A("保存", "Save", "保存", "Enregistrer", "Сохранить") },
            { "PromptFileCount", A("共 {0} 个提示词文件。", "{0} prompt files.", "プロンプトファイル: {0} 件。", "{0} fichiers de prompts.", "Файлов промптов: {0}.") },
            { "LoadedFile", A("已载入: {0}.md", "Loaded: {0}.md", "読み込みました: {0}.md", "Chargé : {0}.md", "Загружен: {0}.md") },
            { "NewPromptBody", A("# 新提示词标题\r\n\r\n（在这里编写提示词内容……保存后自动出现在主界面列表）\r\n", "# New prompt title\r\n\r\nWrite the prompt here. It will appear in the main list after saving.\r\n", "# 新しいプロンプトのタイトル\r\n\r\nここにプロンプトを書きます。保存するとメイン画面の一覧に表示されます。\r\n", "# Titre du nouveau prompt\r\n\r\nÉcrivez le prompt ici. Il apparaîtra dans la liste principale après l’enregistrement.\r\n", "# Заголовок нового промпта\r\n\r\nВведите текст промпта здесь. После сохранения он появится в основном списке.\r\n") },
            { "NewFileStatus", A("新建: 文件名 {0}.md，改好内容后点「保存」。", "New file: {0}.md. Edit it, then click “Save”.", "新規ファイル: {0}.md。編集後に「保存」をクリックしてください。", "Nouveau fichier : {0}.md. Modifiez-le puis cliquez sur « Enregistrer ».", "Новый файл: {0}.md. Измените его и нажмите «Сохранить».") },
            { "SavedFile", A("已保存: {0}", "Saved: {0}", "保存しました: {0}", "Enregistré : {0}", "Сохранено: {0}") },
            { "SavedMessage", A("已保存: {0}\n\n关闭本窗口后，主界面列表会自动刷新。", "Saved: {0}\n\nThe main list will refresh when this window closes.", "保存しました: {0}\n\nこのウィンドウを閉じるとメイン一覧が更新されます。", "Enregistré : {0}\n\nLa liste principale sera actualisée à la fermeture de cette fenêtre.", "Сохранено: {0}\n\nОсновной список обновится после закрытия окна.") },
            { "SaveSuccess", A("保存成功", "Saved", "保存完了", "Enregistrement réussi", "Сохранено") },
            { "SaveFailed", A("保存失败", "Save failed", "保存に失敗しました", "Échec de l’enregistrement", "Ошибка сохранения") },
            { "DeleteConfirmBody", A("确定删除「{0}.md」？\n该操作不可撤销。", "Delete “{0}.md”?\nThis action cannot be undone.", "「{0}.md」を削除しますか？\nこの操作は元に戻せません。", "Supprimer « {0}.md » ?\nCette action est irréversible.", "Удалить «{0}.md»?\nДействие нельзя отменить.") },
            { "DeleteConfirmTitle", A("删除确认", "Confirm delete", "削除の確認", "Confirmer la suppression", "Подтверждение удаления") },
            { "DeletedFile", A("已删除: {0}.md", "Deleted: {0}.md", "削除しました: {0}.md", "Supprimé : {0}.md", "Удалён: {0}.md") },
            { "DeleteFailed", A("删除失败", "Delete failed", "削除に失敗しました", "Échec de la suppression", "Ошибка удаления") },

            { "PromptNotFound", A("找不到提示词: {0}", "Prompt not found: {0}", "プロンプトが見つかりません: {0}", "Prompt introuvable : {0}", "Промпт не найден: {0}") },
            { "EnvConfigLabel", A("环境变量 CLAUDE_CONFIG_DIR", "CLAUDE_CONFIG_DIR environment variable", "環境変数 CLAUDE_CONFIG_DIR", "Variable d’environnement CLAUDE_CONFIG_DIR", "Переменная среды CLAUDE_CONFIG_DIR") },
            { "UserDefaultLabel", A("用户级默认 ~/.claude", "User default ~/.claude", "ユーザー既定 ~/.claude", "Dossier utilisateur par défaut ~/.claude", "Стандартная папка пользователя ~/.claude") },
            { "DesktopConfigLabel", A("桌面应用配置 %APPDATA%\\Claude", "Desktop app config %APPDATA%\\Claude", "デスクトップアプリ設定 %APPDATA%\\Claude", "Configuration de l’application %APPDATA%\\Claude", "Конфигурация приложения %APPDATA%\\Claude") },
            { "DesktopDataLabel", A("桌面应用数据 %LOCALAPPDATA%\\AnthropicClaude", "Desktop app data %LOCALAPPDATA%\\AnthropicClaude", "デスクトップアプリデータ %LOCALAPPDATA%\\AnthropicClaude", "Données de l’application %LOCALAPPDATA%\\AnthropicClaude", "Данные приложения %LOCALAPPDATA%\\AnthropicClaude") },
            { "NativeCliLabel", A("原生 CLI ~/.local/bin/claude.exe", "Native CLI ~/.local/bin/claude.exe", "ネイティブ CLI ~/.local/bin/claude.exe", "CLI native ~/.local/bin/claude.exe", "Нативный CLI ~/.local/bin/claude.exe") },
            { "PathClaudeLabel", A("PATH 上的 claude", "claude on PATH", "PATH 上の claude", "claude dans PATH", "claude в PATH") },
            { "NpmPackageLabel", A("npm 全局包 @anthropic-ai/claude-code", "Global npm package @anthropic-ai/claude-code", "npm グローバルパッケージ @anthropic-ai/claude-code", "Paquet npm global @anthropic-ai/claude-code", "Глобальный пакет npm @anthropic-ai/claude-code") },
            { "FileNameRequired", A("文件名不能为空", "File name is required.", "ファイル名を入力してください。", "Le nom du fichier est requis.", "Укажите имя файла.") },
            { "InvalidFileNameChars", A("文件名包含非法字符: {0}", "File name contains invalid characters: {0}", "ファイル名に使用できない文字が含まれています: {0}", "Le nom contient des caractères non valides : {0}", "Имя содержит недопустимые символы: {0}") },
            { "InvalidFileName", A("文件名不合法", "Invalid file name.", "ファイル名が無効です。", "Nom de fichier non valide.", "Недопустимое имя файла.") },
            { "TargetMissing", A("目标文件不存在: {0}", "Target file does not exist: {0}", "対象ファイルが存在しません: {0}", "Le fichier cible n’existe pas : {0}", "Целевой файл не существует: {0}") },
            { "RestoredBackup", A("已从备份恢复: {0} -> {1}", "Restored from backup: {0} -> {1}", "バックアップから復元しました: {0} -> {1}", "Restauration depuis la sauvegarde : {0} -> {1}", "Восстановлено из копии: {0} -> {1}") },
            { "NoBackupAndTargetMissing", A("没有找到备份文件，且目标文件不存在，无需回滚: {0}", "No backup was found and the target does not exist: {0}", "バックアップがなく、対象ファイルも存在しません: {0}", "Aucune sauvegarde trouvée et la cible n’existe pas : {0}", "Копия не найдена, целевой файл отсутствует: {0}") },
            { "DeletedGenerated", A("未找到 .bak（原文件本不存在或备份丢失），已删除注入文件还原: {0}", "No .bak was found; removed the generated target: {0}", ".bak がないため、生成された対象ファイルを削除しました: {0}", "Aucun .bak trouvé ; la cible générée a été supprimée : {0}", "Файл .bak не найден; созданный целевой файл удалён: {0}") },
            { "NoBackupNonGenerated", A("没有找到备份文件，且目标文件不是本工具生成，已中止以免误删: {0}", "No backup was found and the target was not generated by this app: {0}", "バックアップがなく、対象はこのアプリの生成物ではありません: {0}", "Aucune sauvegarde trouvée et la cible n’a pas été générée par cette application : {0}", "Копия не найдена, а целевой файл создан не этой программой: {0}") }
        };

        private static int _languageIndex;

        private static string[] A(string zh, string en, string ja, string fr, string ru)
        {
            return new[] { zh, en, ja, fr, ru };
        }

        public static string Code { get { return Codes[_languageIndex]; } }

        public static string UiFontName
        {
            get { return _languageIndex == 0 || _languageIndex == 2 ? "Microsoft YaHei UI" : "Segoe UI"; }
        }

        public static void Initialize(string[] args)
        {
            string requested = null;
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].StartsWith("--lang=", StringComparison.OrdinalIgnoreCase))
                        requested = args[i].Substring(7);
                    else if (string.Equals(args[i], "--lang", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                        requested = args[++i];
                }
            }

            if (string.IsNullOrEmpty(requested))
            {
                try
                {
                    string settings = SettingsFile();
                    if (File.Exists(settings)) requested = File.ReadAllText(settings).Trim();
                }
                catch { }
            }

            if (string.IsNullOrEmpty(requested))
            {
                string name = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]).ToLowerInvariant();
                if (name.EndsWith("-en")) requested = "en";
                else if (name.EndsWith("-ja")) requested = "ja";
                else if (name.EndsWith("-fr")) requested = "fr";
                else if (name.EndsWith("-ru")) requested = "ru";
                else requested = "zh-CN";
            }

            SetLanguage(requested, false);
        }

        public static void SetLanguage(string requested, bool persist)
        {
            string normalized = (requested ?? "zh-CN").Trim().Replace('_', '-').ToLowerInvariant();
            if (normalized == "en" || normalized.StartsWith("en-")) _languageIndex = 1;
            else if (normalized == "ja" || normalized == "jp" || normalized.StartsWith("ja-")) _languageIndex = 2;
            else if (normalized == "fr" || normalized.StartsWith("fr-")) _languageIndex = 3;
            else if (normalized == "ru" || normalized.StartsWith("ru-")) _languageIndex = 4;
            else _languageIndex = 0;

            try { Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Code); }
            catch { }

            if (persist)
            {
                try
                {
                    string settings = SettingsFile();
                    Directory.CreateDirectory(Path.GetDirectoryName(settings));
                    File.WriteAllText(settings, Code);
                }
                catch { }
            }
        }

        private static string SettingsFile()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CCPromptLauncher", "language.txt");
        }

        public static string T(string key, params object[] args)
        {
            string[] localized;
            if (!Values.TryGetValue(key, out localized)) return key;
            string value = localized[_languageIndex];
            return args == null || args.Length == 0 ? value : string.Format(value, args);
        }
    }
}
