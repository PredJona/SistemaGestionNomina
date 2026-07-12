using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SistemaGestionNomina.Helpers;
using SistemaGestionNomina.Models;
using SistemaGestionNomina.Security;
using SistemaGestionNomina.Services;

namespace SistemaGestionNomina.UI
{
    public partial class FrmAuditoria : Form
    {
        private const int PageSize = 100;
        private readonly AuditTrailService auditTrailService = new AuditTrailService();
        private readonly AuthorizationService authorizationService = new AuthorizationService();
        private int pageIndex;

        public FrmAuditoria()
        {
            InitializeComponent();
            ControlStyleHelper.ApplyModernForm(this);
        }

        private void FrmAuditoria_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            try
            {
                authorizationService.DemandPermission(Permissions.AuditView);
                LoadFilters();
                LoadRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Auditoría", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadFilters()
        {
            cmbModulo.Items.Clear();
            cmbModulo.Items.Add("Todos");
            foreach (string module in auditTrailService.ObtenerModulos()) cmbModulo.Items.Add(module);
            cmbModulo.SelectedIndex = 0;

            cmbAccion.Items.Clear();
            cmbAccion.Items.Add("Todos");
            foreach (string action in auditTrailService.ObtenerAcciones()) cmbAccion.Items.Add(action);
            cmbAccion.SelectedIndex = 0;
        }

        private void LoadRecords()
        {
            AuditQuery query = new AuditQuery
            {
                Usuario = txtUsuario.Text.Trim(),
                Modulo = SelectedFilter(cmbModulo),
                Accion = SelectedFilter(cmbAccion),
                Detalle = txtDetalle.Text.Trim(),
                FechaDesde = dtpDesde.Checked ? dtpDesde.Value.Date : (DateTime?)null,
                FechaHasta = dtpHasta.Checked ? dtpHasta.Value.Date : (DateTime?)null,
                Offset = pageIndex * PageSize,
                Limit = PageSize
            };

            if (query.FechaDesde.HasValue && query.FechaHasta.HasValue &&
                query.FechaDesde.Value > query.FechaHasta.Value)
            {
                throw new ArgumentException("La fecha inicial no puede ser mayor que la fecha final.");
            }

            List<AuditRecord> records = auditTrailService.Buscar(query);
            dgvAuditoria.Rows.Clear();
            foreach (AuditRecord record in records)
            {
                dgvAuditoria.Rows.Add(record.Fecha.ToString("dd/MM/yyyy HH:mm:ss"), record.Usuario,
                    record.Modulo, record.Accion, record.Detalle);
            }

            lblPagina.Text = "Página " + (pageIndex + 1) + " · " + records.Count + " registros";
            btnAnterior.Enabled = pageIndex > 0;
            btnSiguiente.Enabled = records.Count == PageSize;
        }

        private static string SelectedFilter(ComboBox combo)
        {
            return combo.SelectedItem == null || combo.SelectedItem.ToString() == "Todos"
                ? string.Empty : combo.SelectedItem.ToString();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            pageIndex = 0;
            TryLoadRecords();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtUsuario.Clear();
            txtDetalle.Clear();
            cmbModulo.SelectedIndex = 0;
            cmbAccion.SelectedIndex = 0;
            dtpDesde.Checked = false;
            dtpHasta.Checked = false;
            pageIndex = 0;
            TryLoadRecords();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (pageIndex > 0) pageIndex--;
            TryLoadRecords();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            pageIndex++;
            TryLoadRecords();
        }

        private void TryLoadRecords()
        {
            try
            {
                LoadRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Auditoría", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
