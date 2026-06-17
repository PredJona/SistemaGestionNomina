using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;

namespace SistemaGestionNomina.UI.Controls
{
    public class RoundedPanel : Panel
    {
        public int Radius { get; set; }
        public Color BorderColor { get; set; }

        public RoundedPanel()
        {
            Radius = 12;
            BorderColor = ThemeHelper.Border;
            BackColor = ThemeHelper.Card;
            DoubleBuffered = true;
            Padding = new Padding(16);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = CreateRoundPath(ClientRectangle, Radius))
            using (Pen pen = new Pen(BorderColor, 1))
            {
                Region = new Region(path);
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static GraphicsPath CreateRoundPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Rectangle rect = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
