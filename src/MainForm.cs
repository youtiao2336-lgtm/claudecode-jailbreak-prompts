using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CCPromptLauncher
{
    /// <summary>主窗口：保留原有布局与行为，使用统一的现代化视觉样式。</summary>
    public class MainForm : Form
    {
        private readonly string _promptRoot;
        private readonly CheckedListBox _list = new CheckedListBox();
        private readonly RadioButton _rbUser = new RadioButton();
        private readonly RadioButton _rbProj = new RadioButton();
        private readonly RadioButton _rbCustom = new RadioButton();
        private readonly TextBox _txtCustom = new TextBox();
        private readonly TextBox _log = new TextBox();
        private readonly Label _lblCount = new Label();
        private readonly ModernGroupBox _grpPrompts = new ModernGroupBox();
        private readonly ModernGroupBox _grpTarget = new ModernGroupBox();
        private readonly ModernGroupBox _grpLog = new ModernGroupBox();
        private List<PromptInfo> _items = new List<PromptInfo>();

        public MainForm(string promptDir)
        {
            _promptRoot = promptDir;
            Text = AppText.T("AppTitle");
            Font = new Font(AppText.UiFontName, 9.5F);
            BackColor = UiTheme.WindowBack;
            ForeColor = UiTheme.Text;
            ClientSize = new Size(880, 640);
            MinimumSize = Size;
            StartPosition = FormStartPosition.CenterScreen;
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

            // ===== ① 选择提示词 =====
            _grpPrompts.Text = AppText.T("SelectPrompts"); _grpPrompts.Tag = "SelectPrompts";
            _grpPrompts.Location = new Point(12, 12); _grpPrompts.Size = new Size(420, 392);
            _lblCount.Text = AppText.T("SelectedCount", 0, 0);
            _lblCount.Location = new Point(230, 14); _lblCount.Size = new Size(176, 20);
            _lblCount.TextAlign = ContentAlignment.MiddleRight;
            _lblCount.ForeColor = UiTheme.Primary;
            _lblCount.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            _list.Location = new Point(12, 38); _list.Size = new Size(396, 306);
            _list.IntegralHeight = false; _list.CheckOnClick = true;
            _list.ItemHeight = 26;
            _list.BorderStyle = BorderStyle.FixedSingle;
            _list.BackColor = UiTheme.Surface;
            _list.ForeColor = UiTheme.Text;
            _list.ItemCheck += delegate { UpdateCount(); };
            _list.DoubleClick += delegate { CopySelected(); };
            var btnToggle = new RoundedButton { Name = "btnToggle", Text = AppText.T("AllNone"), Tag = "AllNone", Location = new Point(12, 350), Size = new Size(120, 30) };
            btnToggle.Click += delegate { ToggleAll(); };
            var btnOpen = new RoundedButton { Name = "btnOpenPrompts", Text = AppText.T("OpenPromptFolder"), Tag = "OpenPromptFolder", Location = new Point(140, 350), Size = new Size(140, 30) };
            btnOpen.Click += delegate
            {
                try { System.Diagnostics.Process.Start("explorer.exe", PromptLib.PromptDir); }
                catch (Exception ex) { Log(AppText.T("OpenFolderFailed", ex.Message)); }
            };
            var btnCopy = new RoundedButton { Name = "btnCopy", Text = AppText.T("CopyClipboard"), Tag = "CopyClipboard", Location = new Point(288, 350), Size = new Size(120, 30) };
            btnCopy.Click += delegate { CopySelected(); };
            _grpPrompts.Controls.AddRange(new Control[] { _lblCount, _list, btnToggle, btnOpen, btnCopy });

            // ===== ② 注入目标位置 =====
            _grpTarget.Text = AppText.T("InjectionTarget"); _grpTarget.Tag = "InjectionTarget";
            _grpTarget.Location = new Point(440, 12); _grpTarget.Size = new Size(428, 392);
            _rbUser.Text = AppText.T("UserScope"); _rbUser.Tag = "UserScope";
            _rbUser.Location = new Point(14, 28); _rbUser.Size = new Size(400, 26); _rbUser.Checked = true;
            _rbProj.Text = AppText.T("ProjectScope"); _rbProj.Tag = "ProjectScope";
            _rbProj.Location = new Point(14, 60); _rbProj.Size = new Size(400, 26);
            _rbCustom.Text = AppText.T("CustomPath"); _rbCustom.Tag = "CustomPath";
            _rbCustom.Location = new Point(14, 92); _rbCustom.Size = new Size(110, 26);
            _txtCustom.Location = new Point(126, 91); _txtCustom.Size = new Size(220, 26);
            _txtCustom.BorderStyle = BorderStyle.FixedSingle;
            _txtCustom.Enabled = false;
            _rbCustom.CheckedChanged += delegate { _txtCustom.Enabled = _rbCustom.Checked; };
            var btnBrowse = new RoundedButton { Name = "btnBrowse", Text = AppText.T("Browse"), Tag = "Browse", Location = new Point(352, 90), Size = new Size(62, 28) };
            btnBrowse.Click += delegate
            {
                using (var dlg = new OpenFileDialog())
                {
                    dlg.Title = AppText.T("FileDialogTitle");
                    dlg.Filter = AppText.T("FileDialogFilter");
                    if (dlg.ShowDialog(this) == DialogResult.OK) _txtCustom.Text = dlg.FileName;
                }
            };
            var tip = new Label
            {
                Name = "usageHint",
                Text = AppText.T("UsageHint"), Tag = "UsageHint",
                Location = new Point(14, 130), Size = new Size(400, 110),
                ForeColor = UiTheme.TextSoft
            };
            var btnDetect = new RoundedButton { Name = "btnDetect", Text = AppText.T("DetectClaudeLocations"), Tag = "DetectClaudeLocations", Location = new Point(14, 300), Size = new Size(190, 32) };
            btnDetect.Click += delegate { DetectClaude(); };
            var btnOpenClaude = new RoundedButton { Name = "btnOpenClaude", Text = AppText.T("OpenClaudeFolder"), Tag = "OpenClaudeFolder", Location = new Point(214, 300), Size = new Size(150, 32) };
            btnOpenClaude.Click += delegate { OpenClaudeDir(); };
            var btnBackup = new RoundedButton { Name = "btnBackup", Text = AppText.T("ManualBackup"), Tag = "ManualBackup", Location = new Point(14, 342), Size = new Size(190, 32) };
            btnBackup.Click += delegate { BackupTarget(); };
            _grpTarget.Controls.AddRange(new Control[] { _rbUser, _rbProj, _rbCustom, _txtCustom, btnBrowse, tip, btnDetect, btnOpenClaude, btnBackup });

            // ===== 底部操作按钮 =====
            var btnRefresh = new RoundedButton { Name = "btnRefresh", Text = AppText.T("RefreshList"), Tag = "RefreshList", Location = new Point(12, 412), Size = new Size(110, 38) };
            btnRefresh.Click += delegate { RefreshList(); };
            var btnEdit = new RoundedButton { Name = "btnEdit", Text = AppText.T("ManagePrompts"), Tag = "ManagePrompts", Location = new Point(130, 412), Size = new Size(200, 38) };
            btnEdit.Click += delegate
            {
                using (var ed = new PromptEditorForm(PromptLib.PromptDir)) ed.ShowDialog(this);
                RefreshList();
                Log(AppText.T("PromptListRefreshed"));
            };
            var lblLanguage = new Label { Name = "lblLanguage", Text = AppText.T("Language"), Tag = "Language", Location = new Point(350, 421), Size = new Size(62, 24), TextAlign = ContentAlignment.MiddleRight };
            var cmbLanguage = new ComboBox { Name = "cmbLanguage", Location = new Point(418, 418), Size = new Size(160, 28), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbLanguage.Items.AddRange(new object[] { "简体中文", "English", "日本語", "Français", "Русский" });
            string[] languageCodes = { "zh-CN", "en", "ja", "fr", "ru" };
            int languageIndex = Array.IndexOf(languageCodes, AppText.Code);
            cmbLanguage.SelectedIndex = languageIndex < 0 ? 0 : languageIndex;
            cmbLanguage.SelectionChangeCommitted += delegate
            {
                AppText.SetLanguage(languageCodes[cmbLanguage.SelectedIndex], true);
                PromptLib.PromptRoot = _promptRoot;
                PromptLib.UsePromptLanguage(AppText.Code);
                ApplyLanguage();
                RefreshList();
                Log(AppText.T("LanguageChanged"));
            };
            var btnRollback = new RoundedButton { Name = "btnRollback", Text = AppText.T("Rollback"), Tag = "Rollback", Location = new Point(600, 412), Size = new Size(130, 38), Tone = ButtonTone.Warning };
            btnRollback.Click += delegate { RollbackTarget(); };
            var btnEnable = new RoundedButton { Name = "btnEnable", Text = AppText.T("Enable"), Tag = "Enable", Location = new Point(738, 412), Size = new Size(130, 38), Tone = ButtonTone.Primary };
            btnEnable.Click += delegate { EnableChecked(); };

            // ===== 操作日志 =====
            _grpLog.Text = AppText.T("OperationLog"); _grpLog.Tag = "OperationLog";
            _grpLog.Location = new Point(12, 460); _grpLog.Size = new Size(856, 168);
            _log.Multiline = true; _log.ReadOnly = true;
            _log.ScrollBars = ScrollBars.Vertical;
            _log.BackColor = UiTheme.SurfaceSoft;
            _log.ForeColor = UiTheme.Text;
            _log.BorderStyle = BorderStyle.FixedSingle;
            _log.Location = new Point(10, 22); _log.Size = new Size(836, 134);
            _log.Font = new Font("Consolas", 9.5F);
            _grpLog.Controls.Add(_log);

            Controls.AddRange(new Control[] { _grpPrompts, _grpTarget, _grpLog, btnRefresh, btnEdit, lblLanguage, cmbLanguage, btnRollback, btnEnable });
            Resize += delegate { LayoutControls(); };
            LayoutControls();

            RefreshList();
            Log(AppText.T("PromptDirectory", PromptLib.PromptDir));
            Log(AppText.T("Ready"));
            DetectAtStartup();
        }

        private void UpdateCount()
        {
            int n = _list.CheckedIndices.Count;
            _lblCount.Text = AppText.T("SelectedCount", n, _items.Count);
        }

        private void RefreshList()
        {
            _items = PromptLib.ListPrompts();
            _list.Items.Clear();
            foreach (var p in _items)
                _list.Items.Add(p.Id + "  |  " + p.Title);
            UpdateCount();
            Log(AppText.T("LoadedPrompts", _items.Count));
        }

        private PromptInfo Selected()
        {
            if (_list.SelectedIndex < 0 || _list.SelectedIndex >= _items.Count) return null;
            return _items[_list.SelectedIndex];
        }

        private string ResolveTarget()
        {
            if (_rbUser.Checked) return PromptLib.UserClaudeMd();
            if (_rbProj.Checked) return PromptLib.ProjClaudeMd();
            string custom = _txtCustom.Text.Trim();
            if (custom.Length == 0) throw new Exception(AppText.T("CustomPathRequired"));
            return Path.GetFullPath(custom);
        }

        private void ToggleAll()
        {
            bool anyUnchecked = false;
            for (int i = 0; i < _list.Items.Count; i++)
                if (!_list.GetItemChecked(i)) { anyUnchecked = true; break; }
            for (int i = 0; i < _list.Items.Count; i++)
                _list.SetItemChecked(i, anyUnchecked);
            Log(anyUnchecked ? AppText.T("SelectedAll", _list.Items.Count) : AppText.T("SelectedNone"));
        }

        private void EnableChecked()
        {
            var files = new List<string>();
            var names = new List<string>();
            foreach (int i in _list.CheckedIndices)
            {
                if (i < 0 || i >= _items.Count) continue;
                files.Add(_items[i].FilePath);
                names.Add(_items[i].Id);
            }
            if (files.Count == 0) { Log(AppText.T("SelectAtLeastOne")); return; }
            try
            {
                string target = ResolveTarget();
                string bak = PromptLib.ApplyMultiple(files, target);
                if (bak != null) Log(AppText.T("OriginalBackedUp", bak));
                else Log(AppText.T("NoBackupCreated"));
                Log(AppText.T("EnabledPrompts", files.Count, string.Join(", ", names), target));
                MessageBox.Show(this, AppText.T("EnableSuccessBody", files.Count, string.Join(", ", names), target), AppText.T("EnableSuccessTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log(AppText.T("EnableFailed", ex.Message));
                MessageBox.Show(this, ex.Message, AppText.T("ErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RollbackTarget()
        {
            try
            {
                string target = ResolveTarget();
                string msg = PromptLib.Restore(target);
                Log(AppText.T("RollbackLog", target, msg));
                MessageBox.Show(this, msg, AppText.T("RollbackSuccessTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log(AppText.T("RollbackFailed", ex.Message));
                MessageBox.Show(this, ex.Message, AppText.T("ErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BackupTarget()
        {
            try
            {
                string target = ResolveTarget();
                PromptLib.Backup(target);
                Log(AppText.T("BackupLog", target));
            }
            catch (Exception ex)
            {
                Log(AppText.T("BackupFailed", ex.Message));
                MessageBox.Show(this, ex.Message, AppText.T("ErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopySelected()
        {
            var p = Selected();
            if (p == null)
            {
                foreach (int i in _list.CheckedIndices)
                {
                    if (i >= 0 && i < _items.Count) { p = _items[i]; break; }
                }
            }
            if (p == null) { Log(AppText.T("SelectOnePrompt")); return; }
            try
            {
                string text = File.ReadAllText(p.FilePath, Encoding.UTF8);
                Clipboard.SetText(text);
                Log(AppText.T("CopiedClipboard", p.Title, text.Length));
            }
            catch (Exception ex)
            {
                Log(AppText.T("CopyFailed", ex.Message));
            }
        }

        /// <summary>自动检测 Claude 安装/配置位置并填入目标路径。</summary>
        private void DetectClaude()
        {
            try
            {
                Log(AppText.T("DetectHeader"));
                var paths = PromptLib.DetectClaudePaths(true);
                string found = null;
                foreach (var p in paths)
                {
                    Log((p.Exists ? AppText.T("FoundTag") : AppText.T("NotFoundTag")) + p.Label + ": " + p.Path);
                    if (p.Exists && p.IsConfig && found == null) found = p.Path;
                }
                if (found != null)
                {
                    string def = PromptLib.UserClaudeDir();
                    if (!string.Equals(Path.GetFullPath(found), Path.GetFullPath(def), StringComparison.OrdinalIgnoreCase))
                    {
                        _txtCustom.Text = Path.Combine(found, "CLAUDE.md");
                        _rbCustom.Checked = true;
                        Log(AppText.T("CustomPathFilled", _txtCustom.Text));
                    }
                    else
                    {
                        Log(AppText.T("DefaultConfigMatch", found));
                    }
                }
                else
                {
                    Log(AppText.T("NoConfigFound"));
                }
            }
            catch (Exception ex)
            {
                Log(AppText.T("DetectionFailed", ex.Message));
            }
        }

        /// <summary>打开检测到的 Claude 配置目录。</summary>
        private void OpenClaudeDir()
        {
            try
            {
                string dir = null;
                foreach (var p in PromptLib.DetectClaudePaths(false))
                {
                    if (p.Exists && p.IsConfig) { dir = p.Path; break; }
                }
                if (dir == null)
                {
                    dir = PromptLib.UserClaudeDir();
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                }
                System.Diagnostics.Process.Start("explorer.exe", dir);
                Log(AppText.T("OpenedClaudeFolder", dir));
            }
            catch (Exception ex)
            {
                Log(AppText.T("OpenFolderFailed", ex.Message));
            }
        }

        /// <summary>启动时快速检测，提示异常安装位置。</summary>
        private void DetectAtStartup()
        {
            try
            {
                var paths = PromptLib.DetectClaudePaths(false);
                int found = 0;
                string hint = null;
                foreach (var p in paths)
                {
                    if (p.Exists) { found++; if (p.IsConfig) hint = p.Path; }
                }
                Log(AppText.T("StartupDetection", found));
                if (hint != null && !string.Equals(Path.GetFullPath(hint), Path.GetFullPath(PromptLib.UserClaudeDir()), StringComparison.OrdinalIgnoreCase))
                    Log(AppText.T("StartupHint", hint));
            }
            catch { }
        }

        private void Log(string msg)
        {
            _log.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg + "\r\n");
        }

        private void ApplyLanguage()
        {
            Text = AppText.T("AppTitle");
            Font = new Font(AppText.UiFontName, 9.5F);
            ApplyLanguage(Controls);
            UpdateCount();
            Invalidate(true);
        }

        private static void ApplyLanguage(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                string key = control.Tag as string;
                if (!string.IsNullOrEmpty(key)) control.Text = AppText.T(key);
                if (control.HasChildren) ApplyLanguage(control.Controls);
            }
        }

        private void LayoutControls()
        {
            if (ClientSize.Width < 100 || ClientSize.Height < 100) return;
            const int margin = 12;
            const int gap = 8;
            const int actionHeight = 38;
            int extraWidth = Math.Max(0, ClientSize.Width - 880);
            int extraHeight = Math.Max(0, ClientSize.Height - 640);
            int topHeight = 392 + extraHeight * 55 / 100;
            int actionY = margin + topHeight + gap;
            int logY = actionY + actionHeight + 10;
            int contentWidth = ClientSize.Width - margin * 2;
            int leftWidth = 420 + extraWidth / 2;
            int rightWidth = contentWidth - gap - leftWidth;

            _grpPrompts.SetBounds(margin, margin, leftWidth, topHeight);
            _grpTarget.SetBounds(margin + leftWidth + gap, margin, rightWidth, topHeight);
            _grpLog.SetBounds(margin, logY, contentWidth, Math.Max(80, ClientSize.Height - logY - margin));

            _lblCount.SetBounds(_grpPrompts.Width - 188, 14, 176, 20);
            int promptInnerWidth = _grpPrompts.Width - 24;
            int promptRowY = _grpPrompts.Height - 42;
            _list.SetBounds(12, 38, promptInnerWidth, Math.Max(50, promptRowY - 44));
            int promptExtra = Math.Max(0, promptInnerWidth - 396);
            int toggleWidth = 120 + promptExtra / 3;
            int openWidth = 140 + promptExtra / 3;
            int copyWidth = promptInnerWidth - toggleWidth - openWidth - gap * 2;
            Control btnToggle = _grpPrompts.Controls["btnToggle"];
            Control btnOpenPrompts = _grpPrompts.Controls["btnOpenPrompts"];
            Control btnCopy = _grpPrompts.Controls["btnCopy"];
            btnToggle.SetBounds(12, promptRowY, toggleWidth, 30);
            btnOpenPrompts.SetBounds(12 + toggleWidth + gap, promptRowY, openWidth, 30);
            btnCopy.SetBounds(12 + toggleWidth + gap + openWidth + gap, promptRowY, Math.Max(70, copyWidth), 30);

            int targetInnerWidth = _grpTarget.Width - 28;
            _rbUser.Width = targetInnerWidth;
            _rbProj.Width = targetInnerWidth;
            _rbCustom.Width = 110;
            Control btnBrowse = _grpTarget.Controls["btnBrowse"];
            int browseWidth = 82;
            int browseX = _grpTarget.Width - 14 - browseWidth;
            btnBrowse.SetBounds(browseX, 90, browseWidth, 28);
            _txtCustom.SetBounds(126, 91, Math.Max(80, browseX - 134), 26);
            Control usageHint = _grpTarget.Controls["usageHint"];
            usageHint.SetBounds(14, 130, targetInnerWidth, 110);
            int targetRowY = _grpTarget.Height - 92;
            int targetButtonGap = 10;
            int targetButtonWidth = (targetInnerWidth - targetButtonGap) / 2;
            Control btnDetect = _grpTarget.Controls["btnDetect"];
            Control btnOpenClaude = _grpTarget.Controls["btnOpenClaude"];
            Control btnBackup = _grpTarget.Controls["btnBackup"];
            btnDetect.SetBounds(14, targetRowY, targetButtonWidth, 32);
            btnOpenClaude.SetBounds(14 + targetButtonWidth + targetButtonGap, targetRowY, targetInnerWidth - targetButtonWidth - targetButtonGap, 32);
            btnBackup.SetBounds(14, targetRowY + 42, targetButtonWidth, 32);

            Control btnRefresh = Controls["btnRefresh"];
            Control btnEdit = Controls["btnEdit"];
            Control lblLanguage = Controls["lblLanguage"];
            Control cmbLanguage = Controls["cmbLanguage"];
            Control btnRollback = Controls["btnRollback"];
            Control btnEnable = Controls["btnEnable"];
            btnRefresh.SetBounds(margin, actionY, 110, actionHeight);
            btnEdit.SetBounds(margin + 118, actionY, 200, actionHeight);
            btnEnable.SetBounds(ClientSize.Width - margin - 130, actionY, 130, actionHeight);
            btnRollback.SetBounds(btnEnable.Left - gap - 130, actionY, 130, actionHeight);
            int languageBlockWidth = 230;
            int languageX = btnEdit.Right + Math.Max(0, (btnRollback.Left - btnEdit.Right - languageBlockWidth) / 2);
            lblLanguage.SetBounds(languageX, actionY + 9, 62, 24);
            cmbLanguage.SetBounds(languageX + 68, actionY + 6, 162, 28);

            _log.SetBounds(10, 22, _grpLog.Width - 20, Math.Max(40, _grpLog.Height - 34));
        }
    }
}
