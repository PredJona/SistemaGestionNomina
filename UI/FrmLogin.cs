using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmLogin : Form
    {
        private readonly AuthService authService = new AuthService();

        public Usuario AuthenticatedUser { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmLogin_Paint(object sender, PaintEventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(
                ClientRectangle,
                Color.FromArgb(13, 14, 20),
                Color.FromArgb(24, 18, 45),
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            using (Pen pen = new Pen(Color.FromArgb(24, 29, 42)))
            {
                for (int x = 0; x < Width; x += 34)
                {
                    e.Graphics.DrawLine(pen, x, 0, x, Height);
                }

                for (int y = 0; y < Height; y += 34)
                {
                    e.Graphics.DrawLine(pen, 0, y, Width, y);
                }
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMensaje.Text = "Ingrese usuario y contraseña.";
                lblMensaje.ForeColor = ThemeHelper.Error;
                return;
            }

            try
            {
                Usuario usuario = authService.Login(txtUsuario.Text.Trim(), txtPassword.Text);
                if (usuario == null)
                {
                    lblMensaje.Text = "Credenciales incorrectas.";
                    lblMensaje.ForeColor = ThemeHelper.Error;
                    return;
                }

                AuthenticatedUser = usuario;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo iniciar sesión.\n\n" + ex.Message,
                    "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
