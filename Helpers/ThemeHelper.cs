using System.Drawing;
using System.Windows.Forms;

namespace SistemaGestionNomina.Helpers
{
    public static class ThemeHelper
    {
        public static readonly Color Background = Color.FromArgb(13, 14, 20);
        public static readonly Color Sidebar = Color.FromArgb(5, 5, 7);
        public static readonly Color Card = Color.FromArgb(17, 18, 26);
        public static readonly Color CardAlt = Color.FromArgb(24, 25, 34);
        public static readonly Color Border = Color.FromArgb(42, 43, 56);
        public static readonly Color Text = Color.FromArgb(229, 228, 245);
        public static readonly Color TextMuted = Color.FromArgb(170, 170, 185);
        public static readonly Color Accent = Color.FromArgb(167, 139, 250);
        public static readonly Color Success = Color.FromArgb(52, 211, 153);
        public static readonly Color Error = Color.FromArgb(249, 115, 134);

        public static Font TitleFont(float size)
        {
            return new Font("Segoe UI", size, FontStyle.Bold, GraphicsUnit.Point);
        }

        public static Font BodyFont(float size, FontStyle style)
        {
            return new Font("Segoe UI", size, style, GraphicsUnit.Point);
        }

        public static void ApplyForm(Form form)
        {
            form.BackColor = Background;
            form.ForeColor = Text;
            form.Font = BodyFont(10F, FontStyle.Regular);
        }

        public static Label Label(string text, int x, int y, int width, int height, float size, FontStyle style, Color color)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = BodyFont(size, style),
                ForeColor = color,
                BackColor = Color.Transparent
            };
        }

        public static TextBox TextBox(string name, int x, int y, int width)
        {
            TextBox textBox = new TextBox();
            textBox.Name = name;
            textBox.Location = new Point(x, y);
            textBox.Size = new Size(width, 30);
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = CardAlt;
            textBox.ForeColor = Text;
            textBox.Font = BodyFont(10F, FontStyle.Regular);
            return textBox;
        }

        public static ComboBox ComboBox(string name, int x, int y, int width)
        {
            ComboBox combo = new ComboBox();
            combo.Name = name;
            combo.Location = new Point(x, y);
            combo.Size = new Size(width, 30);
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.BackColor = CardAlt;
            combo.ForeColor = Text;
            combo.FlatStyle = FlatStyle.Flat;
            combo.Font = BodyFont(10F, FontStyle.Regular);
            return combo;
        }

        public static void ApplyGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.Black;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.GridColor = Border;
            grid.ColumnHeadersDefaultCellStyle.BackColor = CardAlt;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextMuted;
            grid.ColumnHeadersDefaultCellStyle.Font = BodyFont(9F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 38;
            grid.DefaultCellStyle.BackColor = Color.Black;
            grid.DefaultCellStyle.ForeColor = Text;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(36, 31, 52);
            grid.DefaultCellStyle.SelectionForeColor = Text;
            grid.DefaultCellStyle.Font = BodyFont(9.5F, FontStyle.Regular);
            grid.RowTemplate.Height = 36;
        }
    }
}
