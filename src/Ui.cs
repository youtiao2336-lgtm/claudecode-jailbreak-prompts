using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CCPromptLauncher
{
    /// <summary>桌面端统一配色。</summary>
    public static class UiTheme
    {
        public static readonly Color WindowBack = Color.FromArgb(243, 246, 250);
        public static readonly Color Surface = Color.White;
        public static readonly Color SurfaceSoft = Color.FromArgb(248, 250, 252);
        public static readonly Color Border = Color.FromArgb(203, 213, 225);
        public static readonly Color BorderSoft = Color.FromArgb(215, 222, 232);
        public static readonly Color Text = Color.FromArgb(30, 41, 59);
        public static readonly Color TextSoft = Color.FromArgb(100, 116, 139);
        public static readonly Color Primary = Color.FromArgb(13, 148, 136);
        public static readonly Color PrimaryHover = Color.FromArgb(15, 118, 110);
        public static readonly Color PrimaryPressed = Color.FromArgb(17, 94, 89);
        public static readonly Color Warning = Color.FromArgb(217, 119, 6);
        public static readonly Color WarningHover = Color.FromArgb(180, 83, 9);
        public static readonly Color WarningPressed = Color.FromArgb(146, 64, 14);
        public static readonly Color Danger = Color.FromArgb(220, 38, 38);
        public static readonly Color DangerHover = Color.FromArgb(185, 28, 28);
        public static readonly Color DangerPressed = Color.FromArgb(153, 27, 27);
    }

    /// <summary>带圆角描边和现代标题样式的分区卡片。</summary>
    public class ModernGroupBox : GroupBox
    {
        public int Radius = 10;

        public ModernGroupBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = UiTheme.Surface;
            ForeColor = UiTheme.Text;
        }

        private static GraphicsPath MakePath(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d < 2)
            {
                path.AddRectangle(r);
                return path;
            }
            path.AddArc(r.Left, r.Top, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Parent == null ? UiTheme.WindowBack : Parent.BackColor);

            var frame = new Rectangle(0, 8, Math.Max(1, Width - 1), Math.Max(1, Height - 9));
            using (var path = MakePath(frame, Radius))
            using (var fill = new SolidBrush(BackColor))
            using (var border = new Pen(UiTheme.BorderSoft))
            {
                e.Graphics.FillPath(fill, path);
                e.Graphics.DrawPath(border, path);
            }

            Size textSize = TextRenderer.MeasureText(Text, Font);
            var titleBack = new Rectangle(12, 0, textSize.Width + 10, Math.Max(Font.Height + 4, 18));
            using (var brush = new SolidBrush(BackColor)) e.Graphics.FillRectangle(brush, titleBack);
            TextRenderer.DrawText(e.Graphics, Text, Font, new Point(16, 1), ForeColor,
                TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
        }
    }

    public enum ButtonTone
    {
        Neutral,
        Primary,
        Warning,
        Danger
    }

    /// <summary>统一圆角按钮：8px 圆角，支持悬停、按下、禁用和键盘焦点状态。</summary>
    public class RoundedButton : Button
    {
        private bool _hovered;
        private bool _pressed;
        private ButtonTone _tone = ButtonTone.Neutral;

        public int Radius { get; set; }

        public ButtonTone Tone
        {
            get { return _tone; }
            set { _tone = value; Invalidate(); }
        }

        public RoundedButton()
        {
            Radius = 8;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        private static GraphicsPath MakePath(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d < 2)
            {
                path.AddRectangle(r);
                return path;
            }
            path.AddArc(r.Left, r.Top, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ResolveColors(out Color background, out Color border, out Color foreground)
        {
            foreground = Color.White;
            border = Color.Transparent;
            if (_tone == ButtonTone.Primary)
                background = _pressed ? UiTheme.PrimaryPressed : (_hovered ? UiTheme.PrimaryHover : UiTheme.Primary);
            else if (_tone == ButtonTone.Warning)
                background = _pressed ? UiTheme.WarningPressed : (_hovered ? UiTheme.WarningHover : UiTheme.Warning);
            else if (_tone == ButtonTone.Danger)
                background = _pressed ? UiTheme.DangerPressed : (_hovered ? UiTheme.DangerHover : UiTheme.Danger);
            else
            {
                foreground = UiTheme.Text;
                border = UiTheme.Border;
                background = _pressed ? Color.FromArgb(226, 232, 240) :
                    (_hovered ? UiTheme.SurfaceSoft : UiTheme.Surface);
            }

            if (!Enabled)
            {
                background = Color.FromArgb(241, 245, 249);
                border = UiTheme.BorderSoft;
                foreground = Color.FromArgb(148, 163, 184);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hovered = false;
            _pressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left) _pressed = true;
            Invalidate();
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _pressed = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.Clear(Parent == null ? UiTheme.WindowBack : Parent.BackColor);

            Color background, border, foreground;
            ResolveColors(out background, out border, out foreground);
            var rect = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            using (var path = MakePath(rect, Radius))
            using (var fill = new SolidBrush(background))
            {
                pevent.Graphics.FillPath(fill, path);
                if (border.A > 0)
                    using (var pen = new Pen(border)) pevent.Graphics.DrawPath(pen, path);
            }

            var textRect = new Rectangle(5, 1, Math.Max(1, Width - 10), Math.Max(1, Height - 2));
            TextRenderer.DrawText(pevent.Graphics, Text, Font, textRect, foreground,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

            if (Focused && ShowFocusCues)
                ControlPaint.DrawFocusRectangle(pevent.Graphics,
                    new Rectangle(4, 4, Math.Max(1, Width - 9), Math.Max(1, Height - 9)), foreground, background);
        }
    }

}
