using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using MaterialSkin;

namespace SistemaGestionNomina.Helpers
{
    public static class ControlStyleHelper
    {
        private static readonly Color Hover = Color.FromArgb(91, 62, 168);
        private static readonly Color Warning = Color.FromArgb(251, 191, 36);
        private static readonly Color Input = Color.FromArgb(24, 25, 34);

        public static void ApplyModernForm(Form form)
        {
            TryApplyMaterialTheme();
            form.BackColor = ThemeHelper.Background;
            form.ForeColor = ThemeHelper.Text;
            form.Font = ThemeHelper.BodyFont(9F, FontStyle.Regular);
            StyleContainer(form);
        }

        public static void StyleContainer(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is DataGridView)
                {
                    StyleGrid((DataGridView)control);
                }
                else if (control is IconButton)
                {
                    StyleIconButton((IconButton)control, false);
                }
                else if (control is Button)
                {
                    StyleButton((Button)control, IsPrimaryButton((Button)control));
                }
                else if (control is TextBox)
                {
                    StyleTextBox((TextBox)control);
                }
                else if (control is ComboBox)
                {
                    StyleComboBox((ComboBox)control);
                }
                else if (control is NumericUpDown)
                {
                    StyleNumeric((NumericUpDown)control);
                }
                else if (control is Panel)
                {
                    StylePanel((Panel)control);
                }
                else if (control is Label)
                {
                    StyleLabel((Label)control);
                }

                if (control.HasChildren)
                {
                    StyleContainer(control);
                }
            }
        }

        public static void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.Black;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.GridColor = ThemeHelper.Border;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 25, 34);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = ThemeHelper.Accent;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Hover;
            grid.ColumnHeadersDefaultCellStyle.Font = ThemeHelper.BodyFont(9F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 38;
            grid.DefaultCellStyle.BackColor = Color.Black;
            grid.DefaultCellStyle.ForeColor = ThemeHelper.Text;
            grid.DefaultCellStyle.SelectionBackColor = Hover;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Font = ThemeHelper.BodyFont(9F, FontStyle.Regular);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(10, 10, 14);
            grid.AlternatingRowsDefaultCellStyle.ForeColor = ThemeHelper.Text;
            grid.RowTemplate.Height = 34;
        }

        public static void StyleSidebarButton(IconButton button, bool active)
        {
            StyleIconButton(button, active);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.ImageAlign = ContentAlignment.MiddleLeft;
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
            button.Padding = new Padding(16, 0, 0, 0);
            button.IconSize = 20;
        }

        public static void SetActiveSidebarButton(Control sidebar, IconButton activeButton)
        {
            foreach (Control control in sidebar.Controls)
            {
                IconButton button = control as IconButton;
                if (button != null)
                {
                    StyleSidebarButton(button, button == activeButton);
                }
            }
        }

        private static void StyleButton(Button button, bool primary)
        {
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = primary ? ThemeHelper.Accent : ThemeHelper.Border;
            button.BackColor = primary ? ThemeHelper.Accent : Color.Black;
            button.ForeColor = primary ? Color.FromArgb(43, 25, 88) : ThemeHelper.Text;
            button.Font = ThemeHelper.BodyFont(9F, FontStyle.Bold);
            button.MouseEnter -= Button_MouseEnter;
            button.MouseLeave -= Button_MouseLeave;
            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;
            button.Tag = primary ? "primary" : "secondary";
        }

        private static void StyleIconButton(IconButton button, bool active)
        {
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = active ? Color.FromArgb(24, 18, 37) : ThemeHelper.Sidebar;
            button.ForeColor = active ? ThemeHelper.Text : ThemeHelper.TextMuted;
            button.IconColor = active ? ThemeHelper.Accent : ThemeHelper.TextMuted;
            button.Font = ThemeHelper.BodyFont(10F, FontStyle.Bold);
            button.MouseEnter -= IconButton_MouseEnter;
            button.MouseLeave -= IconButton_MouseLeave;
            button.MouseEnter += IconButton_MouseEnter;
            button.MouseLeave += IconButton_MouseLeave;
            button.Tag = active ? "active" : "inactive";
        }

        private static void StyleTextBox(TextBox textBox)
        {
            textBox.BackColor = Input;
            textBox.ForeColor = ThemeHelper.Text;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = ThemeHelper.BodyFont(9.5F, FontStyle.Regular);
        }

        private static void StyleComboBox(ComboBox comboBox)
        {
            comboBox.BackColor = Input;
            comboBox.ForeColor = ThemeHelper.Text;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = ThemeHelper.BodyFont(9.5F, FontStyle.Regular);
        }

        private static void StyleNumeric(NumericUpDown numeric)
        {
            numeric.BackColor = Input;
            numeric.ForeColor = ThemeHelper.Text;
            numeric.BorderStyle = BorderStyle.FixedSingle;
            numeric.Font = ThemeHelper.BodyFont(9.5F, FontStyle.Regular);
        }

        private static void StylePanel(Panel panel)
        {
            if (panel.BackColor == Color.Black || panel.BackColor == ThemeHelper.Card)
            {
                panel.BackColor = ThemeHelper.Card;
            }
            panel.ForeColor = ThemeHelper.Text;
        }

        private static void StyleLabel(Label label)
        {
            if (label.ForeColor == SystemColors.ControlText)
            {
                label.ForeColor = ThemeHelper.Text;
            }
            label.BackColor = Color.Transparent;
        }

        private static bool IsPrimaryButton(Button button)
        {
            string name = button.Name.ToLowerInvariant();
            return name.Contains("guardar") ||
                   name.Contains("nuevo") ||
                   name.Contains("editar") ||
                   name.Contains("calcular") ||
                   name.Contains("confirmar") ||
                   name.Contains("ingresar") ||
                   name.Contains("registrar") ||
                   name.Contains("procesar") ||
                   name.Contains("email");
        }

        private static void Button_MouseEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = Hover;
            button.ForeColor = Color.White;
        }

        private static void Button_MouseLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            bool primary = Convert.ToString(button.Tag) == "primary";
            button.BackColor = primary ? ThemeHelper.Accent : Color.Black;
            button.ForeColor = primary ? Color.FromArgb(43, 25, 88) : ThemeHelper.Text;
        }

        private static void IconButton_MouseEnter(object sender, EventArgs e)
        {
            IconButton button = (IconButton)sender;
            button.BackColor = Color.FromArgb(18, 14, 28);
            button.ForeColor = ThemeHelper.Text;
            button.IconColor = ThemeHelper.Accent;
        }

        private static void IconButton_MouseLeave(object sender, EventArgs e)
        {
            IconButton button = (IconButton)sender;
            bool active = Convert.ToString(button.Tag) == "active";
            button.BackColor = active ? Color.FromArgb(24, 18, 37) : ThemeHelper.Sidebar;
            button.ForeColor = active ? ThemeHelper.Text : ThemeHelper.TextMuted;
            button.IconColor = active ? ThemeHelper.Accent : ThemeHelper.TextMuted;
        }

        private static void TryApplyMaterialTheme()
        {
            try
            {
                MaterialSkinManager manager = MaterialSkinManager.Instance;
                manager.Theme = MaterialSkinManager.Themes.DARK;
                manager.ColorScheme = new ColorScheme(
                    Primary.DeepPurple400,
                    Primary.DeepPurple700,
                    Primary.DeepPurple200,
                    Accent.DeepPurple200,
                    TextShade.WHITE);
            }
            catch
            {
                // MaterialSkin is optional here; standard WinForms styling keeps the designer stable.
            }
        }
    }
}
