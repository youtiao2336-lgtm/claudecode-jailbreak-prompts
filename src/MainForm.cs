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
        private readonly string _promptDir;
        private readonly CheckedListBox _list = new CheckedListBox();
        private readonly RadioButton _rbUser = new RadioButton();
        private readonly RadioButton _rbProj = new RadioButton();
        private readonly RadioButton _rbCustom = new RadioButton();
        private readonly TextBox _txtCustom = new TextBox();
        private readonly TextBox _log = new TextBox();
        private readonly Label _lblCount = new Label();
        private List<PromptInfo> _items = new List<PromptInfo>();

        public MainForm(string promptDir)
        {
            _promptDir = promptDir;
            Text = "Claude Code 破限提示词一键启动器";
            Font = new Font("Microsoft YaHei UI", 9.5F);
            BackColor = UiTheme.WindowBack;
            ForeColor = UiTheme.Text;
            ClientSize = new Size(880, 640);
            MinimumSize = Size;
            StartPosition = FormStartPosition.CenterScreen;
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

            // ===== ① 选择提示词 =====
            var grp1 = new ModernGroupBox { Text = "① 选择提示词", Location = new Point(12, 12), Size = new Size(420, 392) };
            _lblCount.Text = "已选 0 / 0";
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
            var btnToggle = new RoundedButton { Text = "全选 / 全不选", Location = new Point(12, 350), Size = new Size(120, 30) };
            btnToggle.Click += delegate { ToggleAll(); };
            var btnOpen = new RoundedButton { Text = "打开提示词目录", Location = new Point(140, 350), Size = new Size(140, 30) };
            btnOpen.Click += delegate
            {
                try { System.Diagnostics.Process.Start("explorer.exe", _promptDir); }
                catch (Exception ex) { Log("打开目录失败: " + ex.Message); }
            };
            var btnCopy = new RoundedButton { Text = "复制到剪贴板", Location = new Point(288, 350), Size = new Size(120, 30) };
            btnCopy.Click += delegate { CopySelected(); };
            grp1.Controls.AddRange(new Control[] { _lblCount, _list, btnToggle, btnOpen, btnCopy });

            // ===== ② 注入目标位置 =====
            var grp2 = new ModernGroupBox { Text = "② 注入目标位置", Location = new Point(440, 12), Size = new Size(428, 392) };
            _rbUser.Text = "用户级（%USERPROFILE%\\.claude\\CLAUDE.md）全局生效";
            _rbUser.Location = new Point(14, 28); _rbUser.Size = new Size(400, 26); _rbUser.Checked = true;
            _rbProj.Text = "项目级（当前目录 CLAUDE.md）";
            _rbProj.Location = new Point(14, 60); _rbProj.Size = new Size(400, 26);
            _rbCustom.Text = "自定义路径";
            _rbCustom.Location = new Point(14, 92); _rbCustom.Size = new Size(110, 26);
            _txtCustom.Location = new Point(126, 91); _txtCustom.Size = new Size(220, 26);
            _txtCustom.BorderStyle = BorderStyle.FixedSingle;
            _txtCustom.Enabled = false;
            _rbCustom.CheckedChanged += delegate { _txtCustom.Enabled = _rbCustom.Checked; };
            var btnBrowse = new RoundedButton { Text = "浏览…", Location = new Point(352, 90), Size = new Size(62, 28) };
            btnBrowse.Click += delegate
            {
                using (var dlg = new OpenFileDialog())
                {
                    dlg.Title = "选择要注入的 CLAUDE.md 文件";
                    dlg.Filter = "Markdown 文件|*.md|所有文件|*.*";
                    if (dlg.ShowDialog(this) == DialogResult.OK) _txtCustom.Text = dlg.FileName;
                }
            };
            var tip = new Label
            {
                Text = "提示：勾选多个提示词后点「一键启用」，会按顺序合并写入目标 CLAUDE.md 并自动备份原文件；不用了点「回滚」即可还原。桌面端和 CLI 读取的都是这个文件，一处启用两边生效。",
                Location = new Point(14, 130), Size = new Size(400, 110),
                ForeColor = UiTheme.TextSoft
            };
            var btnDetect = new RoundedButton { Text = "自动检测 Claude 位置", Location = new Point(14, 300), Size = new Size(190, 32) };
            btnDetect.Click += delegate { DetectClaude(); };
            var btnOpenClaude = new RoundedButton { Text = "打开 Claude 目录", Location = new Point(214, 300), Size = new Size(150, 32) };
            btnOpenClaude.Click += delegate { OpenClaudeDir(); };
            var btnBackup = new RoundedButton { Text = "手动备份目标文件", Location = new Point(14, 342), Size = new Size(190, 32) };
            btnBackup.Click += delegate { BackupTarget(); };
            grp2.Controls.AddRange(new Control[] { _rbUser, _rbProj, _rbCustom, _txtCustom, btnBrowse, tip, btnDetect, btnOpenClaude, btnBackup });

            // ===== 底部操作按钮 =====
            var btnRefresh = new RoundedButton { Text = "刷新列表", Location = new Point(12, 412), Size = new Size(110, 38) };
            btnRefresh.Click += delegate { RefreshList(); };
            var btnEdit = new RoundedButton { Text = "管理提示词（新建/编辑）", Location = new Point(130, 412), Size = new Size(200, 38) };
            btnEdit.Click += delegate
            {
                using (var ed = new PromptEditorForm(_promptDir)) ed.ShowDialog(this);
                RefreshList();
                Log("提示词列表已刷新。");
            };
            var btnRollback = new RoundedButton { Text = "回滚", Location = new Point(600, 412), Size = new Size(130, 38), Tone = ButtonTone.Warning };
            btnRollback.Click += delegate { RollbackTarget(); };
            var btnEnable = new RoundedButton { Text = "一键启用", Location = new Point(738, 412), Size = new Size(130, 38), Tone = ButtonTone.Primary };
            btnEnable.Click += delegate { EnableChecked(); };

            // ===== 操作日志 =====
            var grp3 = new ModernGroupBox { Text = "操作日志", Location = new Point(12, 460), Size = new Size(856, 168) };
            _log.Multiline = true; _log.ReadOnly = true;
            _log.ScrollBars = ScrollBars.Vertical;
            _log.BackColor = UiTheme.SurfaceSoft;
            _log.ForeColor = UiTheme.Text;
            _log.BorderStyle = BorderStyle.FixedSingle;
            _log.Location = new Point(10, 22); _log.Size = new Size(836, 134);
            _log.Font = new Font("Consolas", 9.5F);
            grp3.Controls.Add(_log);

            Controls.AddRange(new Control[] { grp1, grp2, grp3, btnRefresh, btnEdit, btnRollback, btnEnable });

            RefreshList();
            Log("提示词目录: " + _promptDir);
            Log("就绪。勾选多个提示词 → 点「一键启用」批量注入；不用了点「回滚」还原。");
            DetectAtStartup();
        }

        private void UpdateCount()
        {
            int n = _list.CheckedIndices.Count;
            _lblCount.Text = "已选 " + n + " / " + _items.Count;
        }

        private void RefreshList()
        {
            _items = PromptLib.ListPrompts();
            _list.Items.Clear();
            foreach (var p in _items)
                _list.Items.Add(p.Id + "  |  " + p.Title);
            UpdateCount();
            Log("已加载 " + _items.Count + " 个提示词。");
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
            if (custom.Length == 0) throw new Exception("请填写自定义路径");
            return Path.GetFullPath(custom);
        }

        private void ToggleAll()
        {
            bool anyUnchecked = false;
            for (int i = 0; i < _list.Items.Count; i++)
                if (!_list.GetItemChecked(i)) { anyUnchecked = true; break; }
            for (int i = 0; i < _list.Items.Count; i++)
                _list.SetItemChecked(i, anyUnchecked);
            Log(anyUnchecked ? "已全选（" + _list.Items.Count + " 项）。" : "已全不选。");
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
            if (files.Count == 0) { Log("请先勾选至少一个提示词。"); return; }
            try
            {
                string target = ResolveTarget();
                string bak = PromptLib.ApplyMultiple(files, target);
                if (bak != null) Log("已备份原文件: " + bak);
                else Log("目标文件不存在或已是本工具生成，直接写入（未新建 .bak）");
                Log("已启用 " + files.Count + " 个提示词: " + string.Join("、", names) + "  ->  " + target);
                MessageBox.Show(this, "已启用 " + files.Count + " 个提示词「" + string.Join("、", names) + "」。\n\n目标: " + target, "启用成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("启用失败: " + ex.Message);
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RollbackTarget()
        {
            try
            {
                string target = ResolveTarget();
                string msg = PromptLib.Restore(target);
                Log("已回滚: " + target + "，" + msg + "。");
                MessageBox.Show(this, msg, "回滚成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("回滚失败: " + ex.Message);
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BackupTarget()
        {
            try
            {
                string target = ResolveTarget();
                PromptLib.Backup(target);
                Log("已备份: " + target + ".bak");
            }
            catch (Exception ex)
            {
                Log("备份失败: " + ex.Message);
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (p == null) { Log("请先选择一个提示词。"); return; }
            try
            {
                string text = File.ReadAllText(p.FilePath, Encoding.UTF8);
                Clipboard.SetText(text);
                Log("已复制到剪贴板: " + p.Title + "（" + text.Length + " 字符）。");
            }
            catch (Exception ex)
            {
                Log("复制失败: " + ex.Message);
            }
        }

        /// <summary>自动检测 Claude 安装/配置位置并填入目标路径。</summary>
        private void DetectClaude()
        {
            try
            {
                Log("== Claude 安装位置检测 ==");
                var paths = PromptLib.DetectClaudePaths(true);
                string found = null;
                foreach (var p in paths)
                {
                    Log((p.Exists ? "[找到] " : "[未找到] ") + p.Label + ": " + p.Path);
                    if (p.Exists && p.IsConfig && found == null) found = p.Path;
                }
                if (found != null)
                {
                    string def = PromptLib.UserClaudeDir();
                    if (!string.Equals(Path.GetFullPath(found), Path.GetFullPath(def), StringComparison.OrdinalIgnoreCase))
                    {
                        _txtCustom.Text = Path.Combine(found, "CLAUDE.md");
                        _rbCustom.Checked = true;
                        Log("检测到 Claude 配置目录与默认不同，已填入「自定义路径」: " + _txtCustom.Text);
                    }
                    else
                    {
                        Log("检测到的配置目录与用户级默认一致: " + found);
                    }
                }
                else
                {
                    Log("未找到现成的 Claude 配置目录，将使用默认路径（启用时自动创建）。");
                }
            }
            catch (Exception ex)
            {
                Log("自动检测失败: " + ex.Message);
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
                Log("已打开 Claude 目录: " + dir);
            }
            catch (Exception ex)
            {
                Log("打开目录失败: " + ex.Message);
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
                Log("启动检测: 发现 " + found + " 处 Claude 安装/配置位置。");
                if (hint != null && !string.Equals(Path.GetFullPath(hint), Path.GetFullPath(PromptLib.UserClaudeDir()), StringComparison.OrdinalIgnoreCase))
                    Log("提示: 检测到 Claude 配置目录 " + hint + "，可点「自动检测 Claude 位置」一键填入。");
            }
            catch { }
        }

        private void Log(string msg)
        {
            _log.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg + "\r\n");
        }
    }
}
