using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CCPromptLauncher
{
    /// <summary>提示词编辑器窗口：与主窗口共用现代化视觉样式。</summary>
    public class PromptEditorForm : Form
    {
        private readonly string _promptDir;
        private readonly ListBox _list = new ListBox();
        private readonly TextBox _txtName = new TextBox();
        private readonly TextBox _txtBody = new TextBox();
        private readonly Label _status = new Label();
        private List<PromptInfo> _items = new List<PromptInfo>();

        public PromptEditorForm(string promptDir)
        {
            _promptDir = promptDir;
            Text = "提示词编辑器（自建 / 编辑）";
            Font = new Font("Microsoft YaHei UI", 9.5F);
            BackColor = UiTheme.WindowBack;
            ForeColor = UiTheme.Text;
            ClientSize = new Size(840, 620);
            MinimumSize = Size;
            StartPosition = FormStartPosition.CenterParent;
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

            // ===== 左：文件列表 =====
            var grpL = new ModernGroupBox { Text = "提示词文件", Location = new Point(12, 12), Size = new Size(240, 470) };
            _list.Location = new Point(10, 24); _list.Size = new Size(220, 358);
            _list.IntegralHeight = false;
            _list.BorderStyle = BorderStyle.FixedSingle;
            _list.BackColor = UiTheme.Surface;
            _list.ForeColor = UiTheme.Text;
            _list.SelectedIndexChanged += delegate { LoadSelected(); };
            var btnNew = new RoundedButton { Text = "新建", Location = new Point(10, 390), Size = new Size(100, 32) };
            btnNew.Click += delegate { NewPrompt(); };
            var btnDel = new RoundedButton { Text = "删除", Location = new Point(120, 390), Size = new Size(100, 32), Tone = ButtonTone.Danger };
            btnDel.Click += delegate { DeleteSelected(); };
            var btnRef = new RoundedButton { Text = "刷新列表", Location = new Point(10, 430), Size = new Size(210, 30) };
            btnRef.Click += delegate { RefreshList(); };
            grpL.Controls.AddRange(new Control[] { _list, btnNew, btnDel, btnRef });

            // ===== 右：编辑区 =====
            var grpR = new ModernGroupBox { Text = "编辑内容", Location = new Point(260, 12), Size = new Size(568, 470) };
            var lblName = new Label { Text = "文件名（不含 .md，如 07-my-prompt）", Location = new Point(10, 24), Size = new Size(320, 20) };
            _txtName.Location = new Point(10, 48); _txtName.Size = new Size(330, 26);
            _txtName.BorderStyle = BorderStyle.FixedSingle;
            var btnSave = new RoundedButton { Text = "保存", Location = new Point(448, 46), Size = new Size(110, 30), Tone = ButtonTone.Primary };
            btnSave.Click += delegate { SaveCurrent(); };
            _txtBody.Multiline = true;
            _txtBody.ScrollBars = ScrollBars.Both;
            _txtBody.AcceptsTab = true;
            _txtBody.Location = new Point(10, 82); _txtBody.Size = new Size(548, 356);
            _txtBody.Font = new Font("Consolas", 10F);
            _txtBody.BorderStyle = BorderStyle.FixedSingle;
            _txtBody.BackColor = UiTheme.SurfaceSoft;
            _txtBody.ForeColor = UiTheme.Text;
            _status.Text = "";
            _status.Location = new Point(10, 444); _status.Size = new Size(548, 20);
            _status.ForeColor = UiTheme.TextSoft;
            grpR.Controls.AddRange(new Control[] { lblName, _txtName, btnSave, _txtBody, _status });

            Controls.AddRange(new Control[] { grpL, grpR });

            RefreshList();
        }

        private void RefreshList()
        {
            _items = PromptLib.ListPrompts();
            _list.Items.Clear();
            foreach (var p in _items)
                _list.Items.Add(p.Id + "  |  " + p.Title);
            SetStatus("共 " + _items.Count + " 个提示词文件。");
        }

        private void LoadSelected()
        {
            if (_list.SelectedIndex < 0 || _list.SelectedIndex >= _items.Count) return;
            var p = _items[_list.SelectedIndex];
            _txtName.Text = p.Id;
            _txtBody.Text = File.ReadAllText(p.FilePath, Encoding.UTF8);
            SetStatus("已载入: " + p.Id + ".md");
        }

        private void NewPrompt()
        {
            string id = PromptLib.NextPromptId() + "new-prompt";
            _txtName.Text = id;
            _txtBody.Text = "# 新提示词标题\r\n\r\n（在这里编写提示词内容……保存后自动出现在主界面列表）\r\n";
            _list.ClearSelected();
            SetStatus("新建: 文件名 " + id + ".md，改好内容后点「保存」。");
            _txtName.Focus();
            _txtName.SelectAll();
        }

        private void SaveCurrent()
        {
            try
            {
                string name = _txtName.Text.Trim();
                string file = PromptLib.SavePromptFile(name, _txtBody.Text);
                string id = Path.GetFileNameWithoutExtension(file);
                RefreshList();
                for (int i = 0; i < _items.Count; i++)
                {
                    if (string.Equals(_items[i].Id, id, StringComparison.OrdinalIgnoreCase)) { _list.SelectedIndex = i; break; }
                }
                SetStatus("已保存: " + Path.GetFileName(file));
                MessageBox.Show(this, "已保存: " + Path.GetFileName(file) + "\n\n关闭本窗口后，主界面列表会自动刷新。", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteSelected()
        {
            if (_list.SelectedIndex < 0 || _list.SelectedIndex >= _items.Count) return;
            var p = _items[_list.SelectedIndex];
            if (MessageBox.Show(this, "确定删除「" + p.Id + ".md」？\n该操作不可撤销。", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                PromptLib.DeletePromptFile(p.Id);
                RefreshList();
                _txtName.Clear();
                _txtBody.Clear();
                SetStatus("已删除: " + p.Id + ".md");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetStatus(string msg) { _status.Text = msg; }
    }
}
