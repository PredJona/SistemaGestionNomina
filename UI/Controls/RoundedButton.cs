using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;

namespace SistemaGestionNomina.UI.Controls
{
    public class RoundedButton : Button
    {
        public int Radius { get; set; }
        public Color BorderColor { get; set; }

        public RoundedButton()
        {
            Radius = 8;
            BorderColor = ThemeHelper.Border;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = ThemeHelper.Accent;
            ForeColor = Color.FromArgb(32, 18, 67);
            Font = ThemeHelper.BodyFont(9.5F, FontStyle.Bold);
            Cursor = Cursors.Hand;
            Height = 38;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = CreateRoundPath(rect, Radius))
            using (SolidBrush brush = new SolidBrush(BackColor))
            using (Pen pen = new Pen(BorderColor, 1))
            using (SolidBrush textBrush = new SolidBrush(ForeColor))
            {
                pevent.Graphics.FillPath(brush, path);
                pevent.Graphics.DrawPath(pen, path);
                TextRenderer.DrawText(pevent.Graphics, Text, Font, rect, ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        private static GraphicsPath CreateRoundPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
