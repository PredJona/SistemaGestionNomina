namespace SistemaGestionNomina.UI
{
    partial class FrmEmpleados
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelFormulario;
        private System.Windows.Forms.Panel panelFiltros;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.ComboBox cmbFiltroDepartamento;
        private System.Windows.Forms.ComboBox cmbFiltroEstado;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnDesactivar;
        private System.Windows.Forms.Button btnExportarExcel;
        private System.Windows.Forms.Button btnExportarPdf;
        private System.Windows.Forms.Button btnHistorialLaboral;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.TextBox txtCedula;
        private System.Windows.Forms.TextBox txtCargo;
        private System.Windows.Forms.TextBox txtSalario;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.Label lblCedula;
        private System.Windows.Forms.Label lblCargo;
        private System.Windows.Forms.Label lblDepartamento;
        private System.Windows.Forms.Label lblSalario;
        private System.Windows.Forms.ComboBox cmbDepartamento;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.DateTimePicker dtpIngreso;
        private System.Windows.Forms.DataGridView dgvEmpleados;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdEmpleado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCedula;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCargo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDepartamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalarioBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAcciones;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelFormulario = new System.Windows.Forms.Panel();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.txtCedula = new System.Windows.Forms.TextBox();
            this.txtCargo = new System.Windows.Forms.TextBox();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblApellido = new System.Windows.Forms.Label();
            this.lblCedula = new System.Windows.Forms.Label();
            this.lblCargo = new System.Windows.Forms.Label();
            this.lblDepartamento = new System.Windows.Forms.Label();
            this.lblSalario = new System.Windows.Forms.Label();
            this.cmbDepartamento = new System.Windows.Forms.ComboBox();
            this.txtSalario = new System.Windows.Forms.TextBox();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.dtpIngreso = new System.Windows.Forms.DateTimePicker();
            this.panelFiltros = new System.Windows.Forms.Panel();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.cmbFiltroDepartamento = new System.Windows.Forms.ComboBox();
            this.cmbFiltroEstado = new System.Windows.Forms.ComboBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnDesactivar = new System.Windows.Forms.Button();
            this.btnExportarExcel = new System.Windows.Forms.Button();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.btnHistorialLaboral = new System.Windows.Forms.Button();
            this.dgvEmpleados = new System.Windows.Forms.DataGridView();
            this.colIdEmpleado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCedula = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCargo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDepartamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalarioBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAcciones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelFormulario.SuspendLayout();
            this.panelFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpleados)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.lblTitulo.Location = new System.Drawing.Point(34, 24);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(420, 42);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Empleados";
            // 
            // btnHistorialLaboral
            // 
            this.btnHistorialLaboral.BackColor = System.Drawing.Color.FromArgb(17, 18, 26);
            this.btnHistorialLaboral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistorialLaboral.Location = new System.Drawing.Point(824, 28);
            this.btnHistorialLaboral.Name = "btnHistorialLaboral";
            this.btnHistorialLaboral.Size = new System.Drawing.Size(160, 34);
            this.btnHistorialLaboral.TabIndex = 20;
            this.btnHistorialLaboral.Text = "Historial laboral";
            this.btnHistorialLaboral.UseVisualStyleBackColor = false;
            this.btnHistorialLaboral.Click += new System.EventHandler(this.btnHistorialLaboral_Click);
            // 
            // panelFormulario
            // 
            this.panelFormulario.BackColor = System.Drawing.Color.FromArgb(17, 18, 26);
            this.panelFormulario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFormulario.Controls.Add(this.lblCodigo);
            this.panelFormulario.Controls.Add(this.lblNombre);
            this.panelFormulario.Controls.Add(this.lblApellido);
            this.panelFormulario.Controls.Add(this.lblCedula);
            this.panelFormulario.Controls.Add(this.lblCargo);
            this.panelFormulario.Controls.Add(this.lblDepartamento);
            this.panelFormulario.Controls.Add(this.lblSalario);
            this.panelFormulario.Controls.Add(this.txtCodigo);
            this.panelFormulario.Controls.Add(this.txtNombre);
            this.panelFormulario.Controls.Add(this.txtApellido);
            this.panelFormulario.Controls.Add(this.txtCedula);
            this.panelFormulario.Controls.Add(this.txtCargo);
            this.panelFormulario.Controls.Add(this.cmbDepartamento);
            this.panelFormulario.Controls.Add(this.txtSalario);
            this.panelFormulario.Controls.Add(this.cmbEstado);
            this.panelFormulario.Controls.Add(this.dtpIngreso);
            this.panelFormulario.Location = new System.Drawing.Point(36, 86);
            this.panelFormulario.Name = "panelFormulario";
            this.panelFormulario.Size = new System.Drawing.Size(948, 118);
            this.panelFormulario.TabIndex = 1;
            // 
            // txtCodigo
            // 
            this.txtCodigo.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.txtCodigo.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.txtCodigo.Location = new System.Drawing.Point(18, 36);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(100, 23);
            this.txtCodigo.TabIndex = 0;
            this.txtCodigo.Text = "EMP-";
            // 
            // txtNombre
            // 
            this.txtNombre.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.txtNombre.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.txtNombre.Location = new System.Drawing.Point(128, 36);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(120, 23);
            this.txtNombre.TabIndex = 1;
            // 
            // txtApellido
            // 
            this.txtApellido.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.txtApellido.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.txtApellido.Location = new System.Drawing.Point(258, 36);
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(120, 23);
            this.txtApellido.TabIndex = 2;
            // 
            // txtCedula
            // 
            this.txtCedula.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.txtCedula.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.txtCedula.Location = new System.Drawing.Point(388, 36);
            this.txtCedula.Name = "txtCedula";
            this.txtCedula.Size = new System.Drawing.Size(120, 23);
            this.txtCedula.TabIndex = 3;
            // 
            // txtCargo
            // 
            this.txtCargo.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.txtCargo.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.txtCargo.Location = new System.Drawing.Point(518, 36);
            this.txtCargo.Name = "txtCargo";
            this.txtCargo.Size = new System.Drawing.Size(150, 23);
            this.txtCargo.TabIndex = 4;
            // 
            // cmbDepartamento
            // 
            this.cmbDepartamento.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.cmbDepartamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartamento.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.cmbDepartamento.Location = new System.Drawing.Point(678, 36);
            this.cmbDepartamento.Name = "cmbDepartamento";
            this.cmbDepartamento.Size = new System.Drawing.Size(150, 23);
            this.cmbDepartamento.TabIndex = 5;
            // 
            // txtSalario
            // 
            this.txtSalario.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.txtSalario.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.txtSalario.Location = new System.Drawing.Point(18, 84);
            this.txtSalario.Name = "txtSalario";
            this.txtSalario.Size = new System.Drawing.Size(120, 23);
            this.txtSalario.TabIndex = 6;
            // 
            // input labels
            // 
            this.lblCodigo.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblCodigo.Location = new System.Drawing.Point(18, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(100, 18);
            this.lblCodigo.TabIndex = 20;
            this.lblCodigo.Text = "Código";
            this.lblNombre.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblNombre.Location = new System.Drawing.Point(128, 16);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(100, 18);
            this.lblNombre.TabIndex = 21;
            this.lblNombre.Text = "Nombre";
            this.lblApellido.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblApellido.Location = new System.Drawing.Point(258, 16);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(100, 18);
            this.lblApellido.TabIndex = 22;
            this.lblApellido.Text = "Apellido";
            this.lblCedula.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblCedula.Location = new System.Drawing.Point(388, 16);
            this.lblCedula.Name = "lblCedula";
            this.lblCedula.Size = new System.Drawing.Size(100, 18);
            this.lblCedula.TabIndex = 23;
            this.lblCedula.Text = "Cédula";
            this.lblCargo.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblCargo.Location = new System.Drawing.Point(518, 16);
            this.lblCargo.Name = "lblCargo";
            this.lblCargo.Size = new System.Drawing.Size(100, 18);
            this.lblCargo.TabIndex = 24;
            this.lblCargo.Text = "Cargo";
            this.lblDepartamento.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblDepartamento.Location = new System.Drawing.Point(678, 16);
            this.lblDepartamento.Name = "lblDepartamento";
            this.lblDepartamento.Size = new System.Drawing.Size(120, 18);
            this.lblDepartamento.TabIndex = 25;
            this.lblDepartamento.Text = "Departamento";
            this.lblSalario.ForeColor = System.Drawing.Color.FromArgb(170, 170, 185);
            this.lblSalario.Location = new System.Drawing.Point(18, 64);
            this.lblSalario.Name = "lblSalario";
            this.lblSalario.Size = new System.Drawing.Size(100, 18);
            this.lblSalario.TabIndex = 26;
            this.lblSalario.Text = "Salario base";
            // 
            // cmbEstado
            // 
            this.cmbEstado.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.cmbEstado.Items.AddRange(new object[] { "Activo", "Inactivo" });
            this.cmbEstado.Location = new System.Drawing.Point(150, 70);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.SelectedIndex = 0;
            this.cmbEstado.Size = new System.Drawing.Size(120, 23);
            this.cmbEstado.TabIndex = 7;
            // 
            // dtpIngreso
            // 
            this.dtpIngreso.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpIngreso.Location = new System.Drawing.Point(282, 70);
            this.dtpIngreso.Name = "dtpIngreso";
            this.dtpIngreso.Size = new System.Drawing.Size(120, 23);
            this.dtpIngreso.TabIndex = 8;
            // 
            // panelFiltros
            // 
            this.panelFiltros.BackColor = System.Drawing.Color.FromArgb(17, 18, 26);
            this.panelFiltros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFiltros.Controls.Add(this.txtBuscar);
            this.panelFiltros.Controls.Add(this.cmbFiltroDepartamento);
            this.panelFiltros.Controls.Add(this.cmbFiltroEstado);
            this.panelFiltros.Controls.Add(this.btnNuevo);
            this.panelFiltros.Controls.Add(this.btnEditar);
            this.panelFiltros.Controls.Add(this.btnDesactivar);
            this.panelFiltros.Controls.Add(this.btnExportarExcel);
            this.panelFiltros.Controls.Add(this.btnExportarPdf);
            this.panelFiltros.Location = new System.Drawing.Point(36, 224);
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.Size = new System.Drawing.Size(948, 70);
            this.panelFiltros.TabIndex = 2;
            // 
            // txtBuscar
            // 
            this.txtBuscar.BackColor = System.Drawing.Color.FromArgb(24, 25, 34);
            this.txtBuscar.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.txtBuscar.Location = new System.Drawing.Point(18, 24);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(190, 23);
            this.txtBuscar.TabIndex = 0;
            this.txtBuscar.TextChanged += new System.EventHandler(this.FilterChanged);
            // 
            // cmbFiltroDepartamento
            // 
            this.cmbFiltroDepartamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroDepartamento.Location = new System.Drawing.Point(222, 24);
            this.cmbFiltroDepartamento.Name = "cmbFiltroDepartamento";
            this.cmbFiltroDepartamento.Size = new System.Drawing.Size(180, 23);
            this.cmbFiltroDepartamento.TabIndex = 1;
            this.cmbFiltroDepartamento.SelectedIndexChanged += new System.EventHandler(this.FilterChanged);
            // 
            // cmbFiltroEstado
            // 
            this.cmbFiltroEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroEstado.Items.AddRange(new object[] { "Todos", "Activo", "Inactivo" });
            this.cmbFiltroEstado.Location = new System.Drawing.Point(414, 24);
            this.cmbFiltroEstado.Name = "cmbFiltroEstado";
            this.cmbFiltroEstado.SelectedIndex = 0;
            this.cmbFiltroEstado.Size = new System.Drawing.Size(110, 23);
            this.cmbFiltroEstado.TabIndex = 2;
            this.cmbFiltroEstado.SelectedIndexChanged += new System.EventHandler(this.FilterChanged);
            // 
            // btnNuevo
            // 
            this.btnNuevo.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.Location = new System.Drawing.Point(540, 20);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(80, 30);
            this.btnNuevo.TabIndex = 3;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(167, 139, 250);
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Location = new System.Drawing.Point(626, 20);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(80, 30);
            this.btnEditar.TabIndex = 4;
            this.btnEditar.Text = "Guardar";
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnDesactivar
            // 
            this.btnDesactivar.BackColor = System.Drawing.Color.Black;
            this.btnDesactivar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDesactivar.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnDesactivar.Location = new System.Drawing.Point(712, 20);
            this.btnDesactivar.Name = "btnDesactivar";
            this.btnDesactivar.Size = new System.Drawing.Size(90, 30);
            this.btnDesactivar.TabIndex = 5;
            this.btnDesactivar.Text = "Desactivar";
            this.btnDesactivar.UseVisualStyleBackColor = false;
            this.btnDesactivar.Click += new System.EventHandler(this.btnDesactivar_Click);
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.BackColor = System.Drawing.Color.Black;
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnExportarExcel.Location = new System.Drawing.Point(808, 20);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(60, 30);
            this.btnExportarExcel.TabIndex = 6;
            this.btnExportarExcel.Text = "Excel";
            this.btnExportarExcel.UseVisualStyleBackColor = false;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            // 
            // btnExportarPdf
            // 
            this.btnExportarPdf.BackColor = System.Drawing.Color.Black;
            this.btnExportarPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarPdf.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.btnExportarPdf.Location = new System.Drawing.Point(874, 20);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(55, 30);
            this.btnExportarPdf.TabIndex = 7;
            this.btnExportarPdf.Text = "PDF";
            this.btnExportarPdf.UseVisualStyleBackColor = false;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            // 
            // dgvEmpleados
            // 
            this.dgvEmpleados.AllowUserToAddRows = false;
            this.dgvEmpleados.AllowUserToDeleteRows = false;
            this.dgvEmpleados.BackgroundColor = System.Drawing.Color.Black;
            this.dgvEmpleados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmpleados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIdEmpleado,
            this.colCodigo,
            this.colNombre,
            this.colCedula,
            this.colCargo,
            this.colDepartamento,
            this.colSalarioBase,
            this.colEstado,
            this.colAcciones});
            this.dgvEmpleados.Location = new System.Drawing.Point(36, 318);
            this.dgvEmpleados.MultiSelect = false;
            this.dgvEmpleados.Name = "dgvEmpleados";
            this.dgvEmpleados.ReadOnly = true;
            this.dgvEmpleados.RowHeadersVisible = false;
            this.dgvEmpleados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEmpleados.Size = new System.Drawing.Size(948, 360);
            this.dgvEmpleados.TabIndex = 3;
            this.dgvEmpleados.SelectionChanged += new System.EventHandler(this.dgvEmpleados_SelectionChanged);
            // 
            // columns
            // 
            this.colIdEmpleado.HeaderText = "Id";
            this.colIdEmpleado.Name = "IdEmpleado";
            this.colIdEmpleado.ReadOnly = true;
            this.colIdEmpleado.Visible = false;
            this.colCodigo.HeaderText = "Código";
            this.colCodigo.Name = "Codigo";
            this.colCodigo.ReadOnly = true;
            this.colNombre.HeaderText = "Nombre";
            this.colNombre.Name = "Nombre";
            this.colNombre.ReadOnly = true;
            this.colCedula.HeaderText = "Cédula";
            this.colCedula.Name = "Cedula";
            this.colCedula.ReadOnly = true;
            this.colCargo.HeaderText = "Cargo";
            this.colCargo.Name = "Cargo";
            this.colCargo.ReadOnly = true;
            this.colDepartamento.HeaderText = "Departamento";
            this.colDepartamento.Name = "Departamento";
            this.colDepartamento.ReadOnly = true;
            this.colSalarioBase.HeaderText = "Salario Base";
            this.colSalarioBase.Name = "SalarioBase";
            this.colSalarioBase.ReadOnly = true;
            this.colEstado.HeaderText = "Estado";
            this.colEstado.Name = "Estado";
            this.colEstado.ReadOnly = true;
            this.colAcciones.HeaderText = "Acciones";
            this.colAcciones.Name = "Acciones";
            this.colAcciones.ReadOnly = true;
            // 
            // FrmEmpleados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(13, 14, 20);
            this.ClientSize = new System.Drawing.Size(1020, 742);
            this.Controls.Add(this.dgvEmpleados);
            this.Controls.Add(this.panelFiltros);
            this.Controls.Add(this.panelFormulario);
            this.Controls.Add(this.btnHistorialLaboral);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(229, 228, 245);
            this.Name = "FrmEmpleados";
            this.Text = "Empleados";
            this.Load += new System.EventHandler(this.FrmEmpleados_Load);
            this.panelFormulario.ResumeLayout(false);
            this.panelFormulario.PerformLayout();
            this.panelFiltros.ResumeLayout(false);
            this.panelFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpleados)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
