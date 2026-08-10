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
        private readonly ModernGroupBox _grpFiles = new ModernGroupBox();
        private readonly ModernGroupBox _grpContent = new ModernGroupBox();
        private List<PromptInfo> _items = new List<PromptInfo>();

        public PromptEditorForm(string promptDir)
        {
            _promptDir = promptDir;
            Text = AppText.T("EditorTitle");
            Font = new Font(AppText.UiFontName, 9.5F);
            BackColor = UiTheme.WindowBack;
            ForeColor = UiTheme.Text;
            ClientSize = new Size(840, 620);
            MinimumSize = Size;
            StartPosition = FormStartPosition.CenterParent;
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

            // ===== 左：文件列表 =====
            _grpFiles.Text = AppText.T("PromptFiles"); _grpFiles.Location = new Point(12, 12); _grpFiles.Size = new Size(240, 470);
            _list.Location = new Point(10, 24); _list.Size = new Size(220, 358);
            _list.IntegralHeight = false;
            _list.HorizontalScrollbar = true;
            _list.BorderStyle = BorderStyle.FixedSingle;
            _list.BackColor = UiTheme.Surface;
            _list.ForeColor = UiTheme.Text;
            _list.SelectedIndexChanged += delegate { LoadSelected(); };
            var btnNew = new RoundedButton { Name = "btnNew", Text = AppText.T("New"), Location = new Point(10, 390), Size = new Size(100, 32) };
            btnNew.Click += delegate { NewPrompt(); };
            var btnDel = new RoundedButton { Name = "btnDelete", Text = AppText.T("Delete"), Location = new Point(120, 390), Size = new Size(100, 32), Tone = ButtonTone.Danger };
            btnDel.Click += delegate { DeleteSelected(); };
            var btnRef = new RoundedButton { Name = "btnRefresh", Text = AppText.T("RefreshList"), Location = new Point(10, 430), Size = new Size(210, 30) };
            btnRef.Click += delegate { RefreshList(); };
            _grpFiles.Controls.AddRange(new Control[] { _list, btnNew, btnDel, btnRef });

            // ===== 右：编辑区 =====
            _grpContent.Text = AppText.T("EditContent"); _grpContent.Location = new Point(260, 12); _grpContent.Size = new Size(568, 470);
            var lblName = new Label { Name = "lblName", Text = AppText.T("FileNameHint"), Location = new Point(10, 24), Size = new Size(400, 20) };
            _txtName.Location = new Point(10, 48); _txtName.Size = new Size(330, 26);
            _txtName.BorderStyle = BorderStyle.FixedSingle;
            var btnSave = new RoundedButton { Name = "btnSave", Text = AppText.T("Save"), Location = new Point(448, 46), Size = new Size(110, 30), Tone = ButtonTone.Primary };
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
            _grpContent.Controls.AddRange(new Control[] { lblName, _txtName, btnSave, _txtBody, _status });

            Controls.AddRange(new Control[] { _grpFiles, _grpContent });
            Resize += delegate { LayoutControls(); };
            LayoutControls();

            RefreshList();
        }

        private void RefreshList()
        {
            _items = PromptLib.ListPrompts();
            _list.Items.Clear();
            foreach (var p in _items)
                _list.Items.Add(p.Id);
            SetStatus(AppText.T("PromptFileCount", _items.Count));
        }

        private void LoadSelected()
        {
            if (_list.SelectedIndex < 0 || _list.SelectedIndex >= _items.Count) return;
            var p = _items[_list.SelectedIndex];
            _txtName.Text = p.Id;
            _txtBody.Text = File.ReadAllText(p.FilePath, Encoding.UTF8);
            SetStatus(AppText.T("LoadedFile", p.Id));
        }

        private void NewPrompt()
        {
            string id = PromptLib.NextPromptId() + "new-prompt";
            _txtName.Text = id;
            _txtBody.Text = AppText.T("NewPromptBody");
            _list.ClearSelected();
            SetStatus(AppText.T("NewFileStatus", id));
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
                SetStatus(AppText.T("SavedFile", Path.GetFileName(file)));
                MessageBox.Show(this, AppText.T("SavedMessage", Path.GetFileName(file)), AppText.T("SaveSuccess"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, AppText.T("SaveFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteSelected()
        {
            if (_list.SelectedIndex < 0 || _list.SelectedIndex >= _items.Count) return;
            var p = _items[_list.SelectedIndex];
            if (MessageBox.Show(this, AppText.T("DeleteConfirmBody", p.Id), AppText.T("DeleteConfirmTitle"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                PromptLib.DeletePromptFile(p.Id);
                RefreshList();
                _txtName.Clear();
                _txtBody.Clear();
                SetStatus(AppText.T("DeletedFile", p.Id));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, AppText.T("DeleteFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetStatus(string msg) { _status.Text = msg; }

        private void LayoutControls()
        {
            if (ClientSize.Width < 100 || ClientSize.Height < 100) return;
            const int margin = 12;
            const int gap = 8;
            int extraWidth = Math.Max(0, ClientSize.Width - 840);
            int leftWidth = 240 + extraWidth / 4;
            int groupHeight = ClientSize.Height - margin * 2;
            int rightWidth = ClientSize.Width - margin * 2 - gap - leftWidth;
            _grpFiles.SetBounds(margin, margin, leftWidth, groupHeight);
            _grpContent.SetBounds(margin + leftWidth + gap, margin, rightWidth, groupHeight);

            int fileRowY = _grpFiles.Height - 80;
            int fileInnerWidth = _grpFiles.Width - 20;
            int halfWidth = (fileInnerWidth - 10) / 2;
            _list.SetBounds(10, 24, fileInnerWidth, Math.Max(80, fileRowY - 32));
            Control btnNew = _grpFiles.Controls["btnNew"];
            Control btnDelete = _grpFiles.Controls["btnDelete"];
            Control btnRefresh = _grpFiles.Controls["btnRefresh"];
            btnNew.SetBounds(10, fileRowY, halfWidth, 32);
            btnDelete.SetBounds(20 + halfWidth, fileRowY, fileInnerWidth - halfWidth - 10, 32);
            btnRefresh.SetBounds(10, fileRowY + 40, fileInnerWidth, 30);

            Control lblName = _grpContent.Controls["lblName"];
            Control btnSave = _grpContent.Controls["btnSave"];
            lblName.SetBounds(10, 24, _grpContent.Width - 20, 20);
            btnSave.SetBounds(_grpContent.Width - 120, 46, 110, 30);
            _txtName.SetBounds(10, 48, Math.Max(120, btnSave.Left - 20), 26);
            int statusY = _grpContent.Height - 26;
            _txtBody.SetBounds(10, 82, _grpContent.Width - 20, Math.Max(80, statusY - 88));
            _status.SetBounds(10, statusY, _grpContent.Width - 20, 20);
        }
    }
}
