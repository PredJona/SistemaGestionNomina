using System.Drawing;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.UI.Controls;

namespace SistemaGestionNomina.UI
{
    public static class UiFactory
    {
        public static Label Title(string text, int x, int y, int width)
        {
            return ThemeHelper.Label(text, x, y, width, 40, 20F, FontStyle.Bold, ThemeHelper.Text);
        }

        public static Label Subtitle(string text, int x, int y, int width)
        {
            return ThemeHelper.Label(text, x, y, width, 28, 10.5F, FontStyle.Regular, ThemeHelper.TextMuted);
        }

        public static RoundedPanel Card(int x, int y, int width, int height)
        {
            RoundedPanel panel = new RoundedPanel();
            panel.Location = new Point(x, y);
            panel.Size = new Size(width, height);
            return panel;
        }

        public static RoundedButton PrimaryButton(string text, int x, int y, int width)
        {
            RoundedButton button = new RoundedButton();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Width = width;
            button.BackColor = ThemeHelper.Accent;
            button.ForeColor = Color.FromArgb(48, 25, 92);
            button.BorderColor = ThemeHelper.Accent;
            return button;
        }

        public static RoundedButton SecondaryButton(string text, int x, int y, int width)
        {
            RoundedButton button = PrimaryButton(text, x, y, width);
            button.BackColor = Color.Black;
            button.ForeColor = ThemeHelper.Text;
            button.BorderColor = ThemeHelper.Border;
            return button;
        }

        public static DateTimePicker DatePicker(string name, int x, int y, int width)
        {
            DateTimePicker picker = new DateTimePicker();
            picker.Name = name;
            picker.Location = new Point(x, y);
            picker.Size = new Size(width, 30);
            picker.Format = DateTimePickerFormat.Short;
            picker.CalendarForeColor = ThemeHelper.Text;
            picker.CalendarMonthBackground = ThemeHelper.CardAlt;
            return picker;
        }

        public static DataGridView Grid(int x, int y, int width, int height)
        {
            DataGridView grid = new DataGridView();
            grid.Location = new Point(x, y);
            grid.Size = new Size(width, height);
            ThemeHelper.ApplyGrid(grid);
            return grid;
        }

        public static void ShowExported(string path)
        {
            MessageBox.Show("Archivo generado correctamente:\n" + path, "Exportación",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
