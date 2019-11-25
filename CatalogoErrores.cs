// Decompiled with JetBrains decompiler
// Type: MigracionInventarios.CatalogoErrores
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MigracionInventarios
{
    public class CatalogoErrores : Form
    {
        private IContainer components = (IContainer)null;
        private DataGridView GRIDERRORES;
        private DataGridViewTextBoxColumn COD;
        private DataGridViewTextBoxColumn DESCRIPCION;

        public CatalogoErrores()
        {
            this.InitializeComponent();
        }

        private void CatalogoErrores_Load(object sender, EventArgs e)
        {
            this.GRIDERRORES.Rows.Add((object)1, (object)"Producto inexistente");
            this.GRIDERRORES.Rows.Add((object)2, (object)"La localidad ingresada no pertenece a una asignada al inventario físico");
            this.GRIDERRORES.Rows.Add((object)3, (object)"Producto no asignado al almacén o ubicación");
            this.GRIDERRORES.Rows.Add((object)4, (object)"Producto con lote obligatorio");
            this.GRIDERRORES.Rows.Add((object)5, (object)"El lote no pertenece a ese tipo");
            this.GRIDERRORES.Rows.Add((object)6, (object)"Medida 'Alto' invalido, no es necesario");
            this.GRIDERRORES.Rows.Add((object)7, (object)"Medida 'Ancho' invalido, no es necesario");
            this.GRIDERRORES.Rows.Add((object)8, (object)"Medida 'Largo' invalido, no es necesario");
            this.GRIDERRORES.Rows.Add((object)9, (object)"Producto repetido");
            this.GRIDERRORES.Rows.Add((object)10, (object)"Alto requerido");
            this.GRIDERRORES.Rows.Add((object)11, (object)"Ancho requerido");
            this.GRIDERRORES.Rows.Add((object)12, (object)"Largo requerido");
            this.GRIDERRORES.Rows.Add((object)13, (object)"Número de piezas obligatorio");
            this.GRIDERRORES.Rows.Add((object)14, (object)"Inventario físico dimensional: la cantidad y el cálculo volumen/área no coinciden");
            this.GRIDERRORES.Rows.Add((object)15, (object)"El tipo de localidad es incorrecto. Sólo se permite: 'FLOOR' ó 'REGULAR'");
            this.GRIDERRORES.Rows.Add((object)16, (object)"Producto con mismo lote & localidad");
            this.GRIDERRORES.Rows.Add((object)17, (object)"Producto no rastreable");
            this.GRIDERRORES.Rows.Add((object)18, (object)"Una propiedad sobrepasa el máximo de 15 caractéres");
            this.GRIDERRORES.Rows.Add((object)19, (object)"Fercha de pedimento con formato inválido");
            this.GRIDERRORES.Rows.Add((object)20, (object)"Pedimento vacío o inválido");
            this.GRIDERRORES.Rows.Add((object)21, (object)"Un aproperty es requerido");
            this.GRIDERRORES.Rows.Add((object)22, (object)"Un aproperty no es requerido");
        }

        private void GRIDERRORES_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CatalogoErrores));
            this.GRIDERRORES = new DataGridView();
            this.COD = new DataGridViewTextBoxColumn();
            this.DESCRIPCION = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)this.GRIDERRORES).BeginInit();
            this.SuspendLayout();
            this.GRIDERRORES.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GRIDERRORES.Columns.AddRange((DataGridViewColumn)this.COD, (DataGridViewColumn)this.DESCRIPCION);
            this.GRIDERRORES.Location = new Point(1, 0);
            this.GRIDERRORES.Name = "GRIDERRORES";
            this.GRIDERRORES.ReadOnly = true;
            this.GRIDERRORES.Size = new Size(644, 405);
            this.GRIDERRORES.TabIndex = 0;
            this.GRIDERRORES.CellContentClick += new DataGridViewCellEventHandler(this.GRIDERRORES_CellContentClick);
            this.COD.HeaderText = "Código";
            this.COD.Name = "COD";
            this.COD.ReadOnly = true;
            this.DESCRIPCION.HeaderText = "Descripción";
            this.DESCRIPCION.Name = "DESCRIPCION";
            this.DESCRIPCION.ReadOnly = true;
            this.DESCRIPCION.Width = 500;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(644, 406);
            this.Controls.Add((Control)this.GRIDERRORES);
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.Name = nameof(CatalogoErrores);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Catálogo de Errores";
            this.Load += new EventHandler(this.CatalogoErrores_Load);
            ((ISupportInitialize)this.GRIDERRORES).EndInit();
            this.ResumeLayout(false);
        }
    }
}
