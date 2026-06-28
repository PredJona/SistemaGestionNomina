using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FontAwesome.Sharp;
using MaterialSkin;

namespace SistemaGestionNomina.Helpers
{
    public static class ControlStyleHelper
    {
        private const int PanelRadius = 12;
        private const int ButtonRadius = 8;
        private static bool materialThemeApplied;

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
                else if (control is DateTimePicker)
                {
                    StyleDatePicker((DateTimePicker)control);
                }
                else if (control is CheckBox)
                {
                    StyleCheckBox((CheckBox)control);
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
            grid.BackgroundColor = ThemeHelper.CardDeep;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.GridColor = ThemeHelper.BorderSoft;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = ThemeHelper.CardAlt;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = ThemeHelper.Text;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = ThemeHelper.Hover;
            grid.ColumnHeadersDefaultCellStyle.Font = ThemeHelper.BodyFont(9F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 40;
            grid.DefaultCellStyle.BackColor = ThemeHelper.CardDeep;
            grid.DefaultCellStyle.ForeColor = ThemeHelper.Text;
            grid.DefaultCellStyle.SelectionBackColor = ThemeHelper.Hover;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Font = ThemeHelper.BodyFont(9.25F, FontStyle.Regular);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(20, 23, 34);
            grid.AlternatingRowsDefaultCellStyle.ForeColor = ThemeHelper.Text;
            grid.RowTemplate.Height = 36;
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
            button.BackColor = primary ? ThemeHelper.AccentAlt : ThemeHelper.CardDeep;
            button.ForeColor = ThemeHelper.Text;
            button.Font = ThemeHelper.BodyFont(9F, FontStyle.Bold);
            button.UseVisualStyleBackColor = false;
            button.MouseEnter -= Button_MouseEnter;
            button.MouseLeave -= Button_MouseLeave;
            button.MouseEnter += Button_MouseEnter;
            button.MouseLeave += Button_MouseLeave;
            button.Resize -= Button_Resize;
            button.Resize += Button_Resize;
            button.Tag = primary ? "primary" : "secondary";
            ApplyRoundedRegion(button, ButtonRadius);
        }

        private static void StyleIconButton(IconButton button, bool active)
        {
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = active ? ThemeHelper.AccentSoft : ThemeHelper.Sidebar;
            button.ForeColor = active ? ThemeHelper.Text : ThemeHelper.TextMuted;
            button.IconColor = active ? ThemeHelper.Accent : ThemeHelper.TextMuted;
            button.Font = ThemeHelper.BodyFont(10F, FontStyle.Bold);
            button.UseVisualStyleBackColor = false;
            button.MouseEnter -= IconButton_MouseEnter;
            button.MouseLeave -= IconButton_MouseLeave;
            button.MouseEnter += IconButton_MouseEnter;
            button.MouseLeave += IconButton_MouseLeave;
            button.Tag = active ? "active" : "inactive";
            ApplyRoundedRegion(button, ButtonRadius);
        }

        private static void StyleTextBox(TextBox textBox)
        {
            textBox.BackColor = ThemeHelper.Input;
            textBox.ForeColor = ThemeHelper.Text;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = ThemeHelper.BodyFont(9.5F, FontStyle.Regular);
        }

        private static void StyleComboBox(ComboBox comboBox)
        {
            comboBox.BackColor = ThemeHelper.Input;
            comboBox.ForeColor = ThemeHelper.Text;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = ThemeHelper.BodyFont(9.5F, FontStyle.Regular);
        }

        private static void StyleNumeric(NumericUpDown numeric)
        {
            numeric.BackColor = ThemeHelper.Input;
            numeric.ForeColor = ThemeHelper.Text;
            numeric.BorderStyle = BorderStyle.FixedSingle;
            numeric.Font = ThemeHelper.BodyFont(9.5F, FontStyle.Regular);
        }

        private static void StyleDatePicker(DateTimePicker picker)
        {
            picker.CalendarForeColor = ThemeHelper.Text;
            picker.CalendarMonthBackground = ThemeHelper.Input;
            picker.CalendarTitleBackColor = ThemeHelper.CardAlt;
            picker.CalendarTitleForeColor = ThemeHelper.Text;
            picker.CalendarTrailingForeColor = ThemeHelper.TextMuted;
            picker.Font = ThemeHelper.BodyFont(9.5F, FontStyle.Regular);
        }

        private static void StyleCheckBox(CheckBox checkBox)
        {
            checkBox.BackColor = Color.Transparent;
            checkBox.ForeColor = ThemeHelper.TextMuted;
            checkBox.Font = ThemeHelper.BodyFont(9F, FontStyle.Regular);
            checkBox.Cursor = Cursors.Hand;
            checkBox.UseVisualStyleBackColor = false;
        }

        private static void StylePanel(Panel panel)
        {
            if (IsLayoutPanel(panel))
            {
                panel.ForeColor = ThemeHelper.Text;
                return;
            }

            if (panel.BackColor == Color.Black || panel.BackColor == ThemeHelper.Card || panel.BackColor == ThemeHelper.CardDeep)
            {
                panel.BackColor = ThemeHelper.Card;
            }
            else if (panel.BackColor == ThemeHelper.CardAlt)
            {
                panel.BackColor = ThemeHelper.CardAlt;
            }

            panel.ForeColor = ThemeHelper.Text;
            panel.Paint -= Panel_Paint;
            panel.Resize -= Panel_Resize;
            panel.Paint += Panel_Paint;
            panel.Resize += Panel_Resize;
            ApplyRoundedRegion(panel, PanelRadius);
        }

        private static void StyleLabel(Label label)
        {
            string name = label.Name.ToLowerInvariant();
            if (label.ForeColor == SystemColors.ControlText)
            {
                label.ForeColor = ThemeHelper.Text;
            }

            if (name.Contains("ingreso"))
            {
                label.ForeColor = ThemeHelper.Success;
            }
            else if (name.Contains("deduccion") || name.Contains("deducciones") || name.Contains("alerta"))
            {
                label.ForeColor = ThemeHelper.Error;
            }
            else if (name.Contains("totalneto") || name.Contains("neto"))
            {
                label.ForeColor = ThemeHelper.Accent;
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
                   name.Contains("imprimir") ||
                   name.Contains("email");
        }

        private static void Button_MouseEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = ThemeHelper.Hover;
            button.ForeColor = Color.White;
        }

        private static void Button_MouseLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            bool primary = Convert.ToString(button.Tag) == "primary";
            button.BackColor = primary ? ThemeHelper.AccentAlt : ThemeHelper.CardDeep;
            button.ForeColor = ThemeHelper.Text;
        }

        private static void IconButton_MouseEnter(object sender, EventArgs e)
        {
            IconButton button = (IconButton)sender;
            button.BackColor = ThemeHelper.AccentSoft;
            button.ForeColor = ThemeHelper.Text;
            button.IconColor = ThemeHelper.Accent;
        }

        private static void IconButton_MouseLeave(object sender, EventArgs e)
        {
            IconButton button = (IconButton)sender;
            bool active = Convert.ToString(button.Tag) == "active";
            button.BackColor = active ? ThemeHelper.AccentSoft : ThemeHelper.Sidebar;
            button.ForeColor = active ? ThemeHelper.Text : ThemeHelper.TextMuted;
            button.IconColor = active ? ThemeHelper.Accent : ThemeHelper.TextMuted;
        }

        private static void Panel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            if (panel.Width < 2 || panel.Height < 2)
            {
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = CreateRoundPath(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), PanelRadius))
            using (Pen pen = new Pen(ThemeHelper.Border, 1))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private static void Panel_Resize(object sender, EventArgs e)
        {
            ApplyRoundedRegion((Control)sender, PanelRadius);
        }

        private static void Button_Resize(object sender, EventArgs e)
        {
            ApplyRoundedRegion((Control)sender, ButtonRadius);
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0)
            {
                return;
            }

            using (GraphicsPath path = CreateRoundPath(new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                control.Region = new Region(path);
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

        private static bool IsLayoutPanel(Panel panel)
        {
            string name = panel.Name.ToLowerInvariant();
            return name == "panelsidebar" || name == "panelcontent" || name == "paneltopbar";
        }

        private static void TryApplyMaterialTheme()
        {
            if (materialThemeApplied)
            {
                return;
            }

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
                materialThemeApplied = true;
            }
            catch
            {
                // MaterialSkin is optional here; standard WinForms styling keeps the designer stable.
            }
        }
    }
}
