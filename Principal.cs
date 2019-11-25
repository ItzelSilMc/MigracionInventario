// Decompiled with JetBrains decompiler
// Type: MigracionInventarios.Principal
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

using C1.C1Excel;
using CTECH.Log;
using Excel;
using MigracionInventarios.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MigracionInventarios
{
    public class Principal : Form
    {
        private string usuario = "";
        private IContainer components = (IContainer)null;
        private string _sConn;
        private Conexion conn;
        private Physical_count inventario;
        private ToolStripButton TSbtnNuevo;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton TSbtnImprimir;
        private ToolStripButton TSbtnExcel;
        private ToolStripButton TSbtnGenerar;
        private ToolStripMenuItem archivoToolStripMenuItem;
        private ToolStripMenuItem ayudaToolStripMenuItem;
        private ToolStripMenuItem acercaDeToolStripMenuItem;
        private ToolStripButton tsbSalir;
        private ToolStripMenuItem nuevoToolStripMenuItem;
        private ToolStripMenuItem cargarDatosToolStripMenuItem;
        private ToolStripMenuItem generarReporteToolStripMenuItem;
        private ToolStripMenuItem exportarAExcelToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
        private SaveFileDialog saveFileDialog1;
        private C1XLBook c1XLBook1;
        private OpenFileDialog dialogFile;
        private MenuStrip miniToolStrip;
        private SplitContainer splitContainer1;
        private ToolStrip toolStrip;
        private ToolStripButton CargarInventario;
        private DateTimePicker fechaInventario;
        private Button catErrores;
        private Label lblAlmacen;
        private Label label2;
        private Button btnEnviar;
        private Button btnValidar;
        private Label label1;
        private ComboBox cmbInventario;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem archivoToolStripMenuItem1;
        private ToolStripMenuItem salirToolStripMenuItem1;
        private ToolStripMenuItem ayudaToolStripMenuItem1;
        private ToolStripMenuItem acercaDeToolStripMenuItem1;
        private DataGridView GridResultado;
        private Panel panel1;
        private ToolStripButton CargarExcel;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton Salir;
        private Label label3;
        private DataGridViewTextBoxColumn QTY;
        private DataGridViewTextBoxColumn PART_ID;
        private DataGridViewTextBoxColumn LOCATION_ID;
        private DataGridViewTextBoxColumn TRACE_PROFILE;
        private DataGridViewTextBoxColumn PIECE_TRACKED;
        private DataGridViewTextBoxColumn TRACE;
        private DataGridViewTextBoxColumn LARGO;
        private DataGridViewTextBoxColumn ANCHO;
        private DataGridViewTextBoxColumn ALTO;
        private DataGridViewTextBoxColumn NO_PIEZAS;
        private DataGridViewTextBoxColumn ERROR;
        private ToolStripButton Clean2;
        private Button errores;
        private Button Todos;
        private ToolStripButton btnLote;
        private CheckBox asignarAlmacen;

        public Principal(string servidor, string pUsuario, string password, string database)
        {
            this.usuario = pUsuario;
            this.InitializeComponent();
            this.conn = new Conexion(servidor, database, pUsuario, password);
            this.cargarInventarios();
            this.fechaInventario.Value = DateTime.Now.Date;
        }

        private void cargarInventarios()
        {
            this.cmbInventario.Items.Clear();
            this.cmbInventario.Items.Add((object)new ComboboxItem()
            {
                Text = "",
                Value = (object)""
            });
            foreach (DataRow row in (InternalDataCollectionBase)this.conn.obtenertabla("SELECT ID from dbo.PHYSICAL_COUNT where STATUS = 'A'  ").Rows)
                this.cmbInventario.Items.Add((object)new ComboboxItem()
                {
                    Text = row["ID"].ToString(),
                    Value = (object)row["ID"].ToString()
                });
        }

        private void cmbInventario_SelectedIndexChanged(object sender, EventArgs e)
        {
            string mensaje = "";
            this.limpiar();
            if (this.cargarInformacion(out mensaje))
                return;
            int num = (int)MessageBox.Show(mensaje);
            this.limpiar();
        }

        private bool cargarInformacion(out string mensaje)
        {
            mensaje = (string)null;
            this.inventario = (Physical_count)null;
            if (!string.IsNullOrEmpty(this.cmbInventario.Text))
            {
                this.inventario = new Physical_count(this.cmbInventario.Text, this.conn);
                this.CargarExcel.Enabled = true;
                if (this.inventario.encabezado.numRegistrosExcel > 0)
                    this.btnValidar.Enabled = true;
                this.lblAlmacen.Text = this.inventario.encabezado.WAREHOUSE_ID;
            }
            else
                this.limpiar();
            return true;
        }

        public void limpiar()
        {
            this.btnValidar.Enabled = false;
            this.btnEnviar.Enabled = false;
            this.CargarExcel.Enabled = false;
            this.errores.Enabled = false;
            this.Todos.Enabled = false;
            this.GridResultado.Rows.Clear();
        }

        public void limpiar_form()
        {
            this.limpiar();
            this.lblAlmacen.Text = "-";
            this.cmbInventario.Items.Clear();
        }

        private void TSbtnExcel_Click(object sender, EventArgs e)
        {
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                SingletonLogger.Instance.Error(nameof(tsbSalir_Click) + ex.Message);
            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int num = (int)new About.About(Application.ProductName, Application.ProductVersion, "Diciembre 2015").ShowDialog();
            }
            catch (Exception ex)
            {
                SingletonLogger.Instance.Error(nameof(acercaDeToolStripMenuItem_Click) + ex.Message);
            }
        }

        private void bwExportalExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            this.exportarExcel();
        }

        private void bwExportalExcel_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void exportarExcel()
        {
        }

        public void ExportarExcelDataTable(DataTable pDtTblDatosKardex, string RutaExcel, bool bDetalle, bool bVerCosto)
        {
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
        }

        private void cargarInformacionExcel()
        {
            this.conn.obtenertabla(string.Format("DELETE FROM VMX_MIGRACION_INVENTARIO WHERE PHYS_COUNT_ID = '{0}'", (object)this.inventario.encabezado.ID));
            try
            {
                using (FileStream fileStream = File.Open(this.dialogFile.FileName, FileMode.Open, FileAccess.Read))
                {
                    
                    IExcelDataReader excelDataReader = (IExcelDataReader)null;
                    if (Path.GetExtension(this.dialogFile.FileName) == ".xls")
                        excelDataReader = ExcelReaderFactory.CreateBinaryReader((Stream)fileStream);
                    if (Path.GetExtension(this.dialogFile.FileName) == ".xlsx")
                        excelDataReader = ExcelReaderFactory.CreateOpenXmlReader((Stream)fileStream);
                    if (this.insertarInformacionDT(excelDataReader.AsDataSet().Tables[0]))
                    {
                        this.btnValidar.Enabled = true;
                    }
                    else
                    {
                        int num = (int)MessageBox.Show("La información no se pudo insertar, debido a que el archivo Excel contiene errores.", "Migración de Inventario - Error");
                    }
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message.ToString());
            }
        }

        private bool insertarInformacionDT(DataTable dt)
        {
            int num1 = 0;
            this.GridResultado.Rows.Clear();
            bool flag = true;
            try
            {
                this.conn.crearTransaccion();
                foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                {
                    ++num1;
                    if (num1 > 1)
                    {
                        List<int> error;
                        string mensaje;
                        if (!this.correrValidacionesUpload(row, out error, out mensaje))
                            flag = false;
                        int index = this.GridResultado.Rows.Add(row[0], row[1], row[2], (object)"", (object)"", row[3], row[4], row[5], row[6], row[7]);
                        this.GridResultado.Rows[index].Cells["ERROR"].Value = (object)mensaje;
                        if (error.Count > 0)
                        {
                            if (error.Contains(1))
                                this.GridResultado.Rows[index].Cells[0].Style.BackColor = Color.Red;
                            if (error.Contains(2))
                                this.GridResultado.Rows[index].Cells[1].Style.BackColor = Color.Red;
                            if (error.Contains(3))
                                this.GridResultado.Rows[index].Cells[2].Style.BackColor = Color.Red;
                            if (error.Contains(7))
                                this.GridResultado.Rows[index].Cells[6].Style.BackColor = Color.Red;
                            if (error.Contains(8))
                                this.GridResultado.Rows[index].Cells[7].Style.BackColor = Color.Red;
                            if (error.Contains(9))
                                this.GridResultado.Rows[index].Cells[8].Style.BackColor = Color.Red;
                            if (error.Contains(10))
                                this.GridResultado.Rows[index].Cells[9].Style.BackColor = Color.Red;
                        }
                        if (flag)
                            this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO VMX_MIGRACION_INVENTARIO (PHYS_COUNT_ID, PART_ID, QTY, LOCATION_ID, TRACE, LENGTH, WIDTH, HEIGHT, PIECES) VALUES('{0}', '{1}', {2}, '{3}', '{4}',{5},{6},{7},{8})", (object)this.inventario.encabezado.ID, row[1], row[0], row[2], row[3], string.IsNullOrEmpty(row[4].ToString()) ? (object)"0" : row[4], string.IsNullOrEmpty(row[5].ToString()) ? (object)"0" : row[5], string.IsNullOrEmpty(row[6].ToString()) ? (object)"0" : row[6], string.IsNullOrEmpty(row[7].ToString()) ? (object)"0" : row[7]));
                    }
                }
                if (!flag)
                {
                    this.conn.destruirTransaccion();
                    return flag;
                }
            }
            catch (Exception ex)
            {
                int num2 = (int)MessageBox.Show("Ocurrió un error al insertar la carga : " + ex.Message, "Migración de Inventario - Error");
                this.conn.destruirTransaccion();
                flag = false;
            }
            this.conn.commitTransaccion();
            this.GridResultado.Refresh();
            return flag;
        }

        private bool correrValidacionesUpload(DataRow row, out List<int> error, out string mensaje)
        {
            error = new List<int>();
            mensaje = string.Empty;
            bool flag = false;
            double result1;
            if (!double.TryParse(row[0].ToString(), out result1) && string.IsNullOrEmpty(row[0].ToString()))
            {
                error.Add(1);
                mensaje += " Cantidad no NULL ";
                flag = true;
            }
            if (result1 <= 0.0)
            {
                error.Add(1);
                mensaje += " Cantidad > 0 ";
                flag = true;
            }
            if (string.IsNullOrEmpty(row[1].ToString()))
            {
                error.Add(2);
                mensaje += " Parte no NULL ";
                flag = true;
            }
            if (string.IsNullOrEmpty(row[2].ToString()))
            {
                error.Add(3);
                mensaje += " Ubicación no NULL ";
                flag = true;
            }
            double result2;
            if (!double.TryParse(row[4].ToString(), out result2) && result2 < 0.0)
            {
                error.Add(7);
                mensaje += " Largo > 0 ";
                flag = true;
            }
            double result3;
            if (!double.TryParse(row[5].ToString(), out result3) && result3 < 0.0)
            {
                error.Add(8);
                mensaje += " Ancho > 0 ";
                flag = true;
            }
            double result4;
            if (!double.TryParse(row[6].ToString(), out result4) && result4 < 0.0)
            {
                error.Add(9);
                mensaje += " Alto > 0 ";
                flag = true;
            }
            double result5;
            if (!double.TryParse(row[7].ToString(), out result5) && result5 < 0.0)
            {
                error.Add(10);
                mensaje += " Piezas > 0 ";
                flag = true;
            }
            return !flag;
        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            if (this.validar(this.asignarAlmacen.Checked))
            {
                this.btnEnviar.Enabled = true;
            }
            else
            {
                this.btnEnviar.Enabled = false;
                this.errores.Enabled = true;
                this.Todos.Enabled = true;
            }
        }

        private bool validar(bool asignarAlmacen)
        {
            this.inventario.cargarTag(this.conn.obtenertabla(string.Format("SELECT * FROM VMX_MIGRACION_INVENTARIO WHERE PHYS_COUNT_ID = '{0}'", (object)this.inventario.encabezado.ID)), asignarAlmacen);
            this.GridResultado.Rows.Clear();
            bool flag = true;
            foreach (Tag tag in this.inventario.tags)
            {
                string str = string.Empty;
                if (tag.errores.Count > 0)
                {
                    tag.errores.Sort();
                    str = string.Join<int>(",", (IEnumerable<int>)tag.errores);
                    flag = false;
                }
                int index = this.GridResultado.Rows.Add((object)tag.QTY, (object)tag.PART_ID, (object)tag.LOCATION_ID, (object)tag.TRACE_PROFILE, (object)tag.PIECE_TRACKED, (object)tag.TRACE, (object)tag.LENGTH, (object)tag.WIDTH, (object)tag.HEIGHT, (object)tag.PIECES);
                this.GridResultado.Rows[index].Cells["ERROR"].Value = (object)str;
                if (tag.errores.Contains(1))
                    this.GridResultado.Rows[index].Cells[1].Style.BackColor = Color.Red;
            }
            this.GridResultado.Refresh();
            return flag;
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (this.validar(this.asignarAlmacen.Checked))
            {
                if (!this.insertarInventario(this.inventario))
                    return;
                int num = (int)MessageBox.Show("El inventario se migro a Visual de manera exitosa. Es necesario cerrar este inventario de manera manual en Visual. NOTA: Deshabilitar workflows antes de cerrar el inventario.", "Migración de Inventario");
                this.limpiar();
            }
            else
            {
                int num1 = (int)MessageBox.Show("Existen errores en el inventario.", "Migración de Inventario - Error");
            }
        }

        private bool insertarInventario(Physical_count inv)
        {
            if (string.IsNullOrEmpty(this.fechaInventario.Text))
            {
                int num = (int)MessageBox.Show("La fecha del inventario es obligatoria", "Migración de Inventario - Error");
                return false;
            }
            try
            {
                this.conn.obtenertabla(string.Format("DELETE FROM PHYS_TRACE_COUNT WHERE PHYS_COUNT_ID = '{0}'", (object)this.inventario.encabezado.ID));
                this.conn.obtenertabla(string.Format("DELETE FROM PHYS_INV_PIECES WHERE PHYS_COUNT_ID = '{0}'", (object)this.inventario.encabezado.ID));
                this.conn.obtenertabla(string.Format("DELETE FROM PHYS_COUNT_TAG WHERE PHYS_COUNT_ID = '{0}'", (object)this.inventario.encabezado.ID));
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("No se pudo hacer el borrado del inventario en la tabla PHYS_COUNT_ID: " + ex.Message, "Migración de Inventario - Error");
                return false;
            }
            this.conn.crearTransaccion();
            int num1 = 0;
            try
            {
                this.insertarAlmacenes();
                foreach (Tag tag in this.inventario.tags.Where(r => r.TIPOARTICULO == 1).ToList())
                {
                    ++num1;
                    this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO PHYS_COUNT_TAG(PART_ID, LOCATION_ID, COUNT_QTY, COUNT_USER_ID, COUNT_DATE, TRACE_ID, STATUS_EFF_DATE, STATUS, PHYS_COUNT_ID, TAG_NO, GROUP_NO, WAREHOUSE_ID)\r\n                                        VALUES('{0}','{1}',{2},'{3}','{4}',NULL,'{4}','A','{5}','{6}','{7}','{8}')", (object)tag.PART_ID, (object)tag.LOCATION_ID, (object)tag.QTY, (object)this.usuario, (object)string.Format("{0:yyyyMMdd}", (object)this.fechaInventario.Value), (object)this.inventario.encabezado.ID, (object)num1, (object)1, (object)this.inventario.encabezado.WAREHOUSE_ID));
                }
                foreach (Tag tag1 in this.inventario.tags.Where((r => r.TIPOARTICULO == 2)).GroupBy(r => new
                {
                    PART_ID = r.PART_ID,
                    LOCATION_ID = r.LOCATION_ID
                }).Select(r => r.First()).ToList())
        {
                    Tag t = tag1;
                    ++num1;
                    int num2 = this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID)
                           return r.LOCATION_ID == t.LOCATION_ID;
                       return false;
                   })).Count<Tag>();
                    double num3 = this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID)
                           return r.LOCATION_ID == t.LOCATION_ID;
                       return false;
                   })).Sum<Tag>((Func<Tag, double>)(r => r.QTY));
                    this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO PHYS_COUNT_TAG(PART_ID, LOCATION_ID, COUNT_QTY, COUNT_USER_ID, COUNT_DATE, STATUS_EFF_DATE, RECOUNT_QTY, RECOUNT_USER_ID, STATUS, PHYS_COUNT_ID, TAG_NO, GROUP_NO, WAREHOUSE_ID, TRACE_ID)\r\n                                        VALUES('{0}','{1}',{2},'{3}','{4}','{4}',NULL,NULL,'A','{5}','{6}','{7}','{8}','{9}')", (object)t.PART_ID, (object)t.LOCATION_ID, (object)num3, (object)this.usuario, (object)string.Format("{0:yyyyMMdd}", (object)this.fechaInventario.Value), (object)this.inventario.encabezado.ID, (object)num1, (object)1, (object)this.inventario.encabezado.WAREHOUSE_ID, (object)t.TRACE));
                    int num4 = 0;
                    foreach (Tag tag2 in this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID)
                           return r.LOCATION_ID == t.LOCATION_ID;
                       return false;
                   })).ToList<Tag>())
                    {
                        ++num4;
                        this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO PHYS_TRACE_COUNT(PHYS_COUNT_ID, TAG_NO, SUB_TAG_NO, TRACE_ID, SUB_TAG_TOTAL_NO, SUB_COUNT_QTY, SUB_COUNT_USER, SUB_COUNT_DATE)\r\nVALUES ('{0}', {1}, {2}, '{3}', {4}, {5}, '{6}','{7}')", (object)this.inventario.encabezado.ID, (object)num1, (object)num4, (object)tag2.TRACE, (object)num2, (object)tag2.QTY, (object)this.usuario, (object)string.Format("{0:yyyyMMdd}", (object)this.fechaInventario.Value)));
                    }
                }
                
                foreach (Tag tag1 in this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r => r.TIPOARTICULO == 3)).GroupBy(r => new
                {
                    PART_ID = r.PART_ID,
                    LOCATION_ID = r.LOCATION_ID
                }).Select (r => r.First<Tag>()).ToList<Tag>())
        {
                    Tag t = tag1;
                    ++num1;
                    this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID)
                           return r.LOCATION_ID == t.LOCATION_ID;
                       return false;
                   })).Count<Tag>();
                    double num2 = this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID)
                           return r.LOCATION_ID == t.LOCATION_ID;
                       return false;
                   })).Sum<Tag>((Func<Tag, double>)(r => r.cantidad_bidimensional));
                    this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO PHYS_COUNT_TAG(PART_ID, LOCATION_ID, COUNT_QTY, COUNT_USER_ID, COUNT_DATE, STATUS_EFF_DATE, RECOUNT_QTY, RECOUNT_USER_ID, STATUS, PHYS_COUNT_ID, TAG_NO, GROUP_NO, WAREHOUSE_ID, TRACE_ID)\r\n                                        VALUES('{0}','{1}',{2},'{3}','{4}','{4}',NULL,NULL,'A','{5}','{6}','{7}','{8}',NULL)", (object)t.PART_ID, (object)t.LOCATION_ID, (object)num2, (object)this.usuario, (object)string.Format("{0:yyyyMMdd}", (object)this.fechaInventario.Value), (object)this.inventario.encabezado.ID, (object)num1, (object)1, (object)this.inventario.encabezado.WAREHOUSE_ID));
                    foreach (Tag tag2 in this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID)
                           return r.LOCATION_ID == t.LOCATION_ID;
                       return false;
                   })).ToList<Tag>())
                        this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO PHYS_INV_PIECES(PHYS_COUNT_ID, TAG_NO, PART_ID, QTY, LENGTH, WIDTH, HEIGHT, WAREHOUSE_ID, LOCATION_ID, DIMENSIONS_UM, STATUS, SUB_TAG_NO)\r\nVALUES ('{0}', {1}, '{2}', {3}, {4}, {5}, {6}, '{7}', '{8}', '{9}', 'A', 0)", (object)this.inventario.encabezado.ID, (object)num1, (object)tag2.PART_ID, (object)tag2.PIECES, (object)tag2.LENGTH, (object)tag2.WIDTH, (object)tag2.HEIGHT, (object)this.inventario.encabezado.WAREHOUSE_ID, (object)tag2.LOCATION_ID, (object)tag2.DIMENSIONS_UM));
                }
                foreach (Tag tag1 in this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r => r.TIPOARTICULO == 4)).GroupBy(r => new
                {
                    PART_ID = r.PART_ID,
                    LOCATION_ID = r.LOCATION_ID
                }).Select (r => r.First<Tag>()).ToList<Tag>())
                {
                    Tag t = tag1;
                    ++num1;
                    int num2 = this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID)
                           return r.LOCATION_ID == t.LOCATION_ID;
                       return false;
                   })).Count<Tag>();
                    double num3 = this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID)
                           return r.LOCATION_ID == t.LOCATION_ID;
                       return false;
                   })).Sum<Tag>((Func<Tag, double>)(r => r.cantidad_bidimensional));
                    this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO PHYS_COUNT_TAG(PART_ID, LOCATION_ID, COUNT_QTY, COUNT_USER_ID, COUNT_DATE, STATUS_EFF_DATE, RECOUNT_QTY, RECOUNT_USER_ID, STATUS, PHYS_COUNT_ID, TAG_NO, GROUP_NO, WAREHOUSE_ID, TRACE_ID)\r\n                                        VALUES('{0}','{1}',{2},'{3}','{4}','{4}',NULL,NULL,'A','{5}','{6}','{7}','{8}','{9}')", (object)t.PART_ID, (object)t.LOCATION_ID, (object)num3, (object)this.usuario, (object)string.Format("{0:yyyyMMdd}", (object)this.fechaInventario.Value), (object)this.inventario.encabezado.ID, (object)num1, (object)1, (object)this.inventario.encabezado.WAREHOUSE_ID, (object)t.TRACE));
                    int num4 = 0;
                    foreach (Tag tag2 in this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID)
                           return r.LOCATION_ID == t.LOCATION_ID;
                       return false;
                   })).GroupBy(r => new
                   {
                       PART_ID = r.PART_ID,
                       LOCATION_ID = r.LOCATION_ID,
                       TRACE = r.TRACE
                   }).Select(r => r.First<Tag>()).ToList<Tag>())
                    {
                        Tag r2 = tag2;
                        foreach (Tag tag3 in this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                       {
                           if (r.PART_ID == r2.PART_ID && r.LOCATION_ID == r2.LOCATION_ID)
                               return r.TRACE == r2.TRACE;
                           return false;
                       })).GroupBy(r => new
                       {
                           PART_ID = r2.PART_ID,
                           LOCATION_ID = r2.LOCATION_ID
                       }).Select (r => r.First<Tag>()).ToList<Tag>())
            {
                            Tag r3 = tag3;
                            ++num4;
                            double num5 = this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                           {
                               if (r.PART_ID == t.PART_ID && r.LOCATION_ID == t.LOCATION_ID)
                                   return r.TRACE == r3.TRACE;
                               return false;
                           })).Sum<Tag>((Func<Tag, double>)(r => r.cantidad_bidimensional));
                            this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO PHYS_TRACE_COUNT(PHYS_COUNT_ID, TAG_NO, SUB_TAG_NO, TRACE_ID, SUB_TAG_TOTAL_NO, SUB_COUNT_QTY, SUB_COUNT_USER, SUB_COUNT_DATE)\r\nVALUES ('{0}', {1}, {2}, '{3}', {4}, {5}, '{6}', '{7}')", (object)this.inventario.encabezado.ID, (object)num1, (object)num4, (object)r3.TRACE, (object)num2, (object)num5, (object)this.usuario, (object)string.Format("{0:yyyyMMdd}", (object)this.fechaInventario.Value)));
                            foreach (Tag tag4 in this.inventario.tags.Where<Tag>((Func<Tag, bool>)(r =>
                           {
                               if (r.PART_ID == t.PART_ID && r.LOCATION_ID == t.LOCATION_ID)
                                   return r.TRACE == r3.TRACE;
                               return false;
                           })).ToList<Tag>())
                                this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO PHYS_INV_PIECES(PHYS_COUNT_ID, TAG_NO, PART_ID, QTY, LENGTH, WIDTH, HEIGHT, WAREHOUSE_ID, LOCATION_ID, DIMENSIONS_UM, STATUS, SUB_TAG_NO)\r\nVALUES ('{0}', {1}, '{2}', {3}, {4}, {5}, {6}, '{7}', '{8}', '{9}', 'A', {10})", (object)this.inventario.encabezado.ID, (object)num1, (object)tag4.PART_ID, (object)tag4.PIECES, (object)tag4.LENGTH, (object)tag4.WIDTH, (object)tag4.HEIGHT, (object)this.inventario.encabezado.WAREHOUSE_ID, (object)tag4.LOCATION_ID, (object)tag4.DIMENSIONS_UM, (object)num4));
                        }
                    }
                }
                this.conn.commitTransaccion();
                return true;
            }
            catch (Exception ex)
            {
                this.conn.destruirTransaccion();
                int num2 = (int)MessageBox.Show("Existe un problema al procesar el inventario: " + ex.Message, "Migración de Inventario - Error");
                return false;
            }
        }

        private void insertarAlmacenes()
        {
            foreach (Tag tag in this.inventario.tags)
            {
                if (this.asignarAlmacen.Checked && tag.insertarAlmacen)
                {
                    if (this.conn.obtenertablaTrans(string.Format("SELECT * FROM PART_WAREHOUSE WHERE WAREHOUSE_ID = '{0}' AND PART_ID = '{1}'", (object)this.inventario.encabezado.WAREHOUSE_ID, (object)tag.PART_ID)).Rows.Count == 0)
                        this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO PART_WAREHOUSE(WAREHOUSE_ID, PART_ID, AUTO_CREATE, ORDER_POLICY, PLANNING_LEADTIME, PLANNER_USER_ID, BUYER_USER_ID, SAFETY_STOCK_QTY,\r\n                                MINIMUM_ORDER_QTY, MAXIMUM_ORDER_QTY, MULTIPLE_ORDER_QTY, DAYS_OF_SUPPLY, FIXED_ORDER_QTY, ORDER_POINT, ANNUAL_USAGE_QTY, DEMAND_FENCE_1, DEMAND_FENCE_2)\r\n                                VALUES ('{0}', '{1}', 'N', 'N', 0, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 )", (object)this.inventario.encabezado.WAREHOUSE_ID, (object)tag.PART_ID));
                    if (this.conn.obtenertablaTrans(string.Format("SELECT * FROM PART_LOCATION WHERE PART_ID = '{0}' AND WAREHOUSE_ID = '{1}' AND LOCATION_ID = '{2}'", (object)tag.PART_ID, (object)this.inventario.encabezado.WAREHOUSE_ID, (object)tag.LOCATION_ID)).Rows.Count == 0)
                        this.conn.ejecutarConsultaDMLTransaccion(string.Format("INSERT INTO PART_LOCATION ( WAREHOUSE_ID, LOCATION_ID, PART_ID, HOLD_REASON_ID, FROM_HOLD_REASON_ID, QTY, DESCRIPTION, STATUS, LOCKED, TRANSIT, DEF_BACKFLUSH_LOC, AUTO_ISSUE_LOC, DEF_INSPECT_LOC, DC_CLASS_ID, ORDER_POINT, ORDER_UP_TO_QTY )\r\n                                          VALUES ( '{0}', '{1}', '{2}', NULL, LTRIM ( '' ), 0, NULL, 'A', 'N', 'N', 'N', 'N', 'N', NULL, NULL, NULL )", (object)this.inventario.encabezado.WAREHOUSE_ID, (object)tag.LOCATION_ID, (object)tag.PART_ID));
                }
            }
        }

        private void catErrores_Click(object sender, EventArgs e)
        {
            new CatalogoErrores().Show();
        }

        private void Principal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void CargarInventario_Click(object sender, EventArgs e)
        {
            this.cargarInventarios();
        }

        private void CargarExcel_Click(object sender, EventArgs e)
        {
            if (this.inventario.encabezado.numRegistrosExcel > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Ya existen registros cargados para ese inventario, si continua serán borrados", "Confirmación", MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.No || dialogResult == DialogResult.Cancel)
                    return;
            }
            this.dialogFile = new OpenFileDialog();
            this.dialogFile.Filter = "Excel Files|*.xls;*.xlsx";
            if (this.dialogFile.ShowDialog() == DialogResult.OK)
                this.cargarInformacionExcel();
            int num = (int)MessageBox.Show("Verifica que esten deshabilitados los workflows antes de cerrar el inventario.", "Migración de Inventario");
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Clean2_Click(object sender, EventArgs e)
        {
            this.limpiar_form();
        }

        private void errores_Click(object sender, EventArgs e)
        {
            if (this.GridResultado.Rows.Count <= 0)
                return;
            this.GridResultado.CommitEdit(DataGridViewDataErrorContexts.Commit);
            foreach (DataGridViewRow row in (IEnumerable)this.GridResultado.Rows)
            {
                if (!row.IsNewRow)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(row.Cells["ERROR"].Value)))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void Todos_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewBand row in (IEnumerable)this.GridResultado.Rows)
                row.Visible = true;
        }

        private void btnLote_Click(object sender, EventArgs e)
        {
            new MigracionPorLote(this.conn).Show();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Principal));
            this.archivoToolStripMenuItem = new ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new ToolStripMenuItem();
            this.acercaDeToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.saveFileDialog1 = new SaveFileDialog();
            this.nuevoToolStripMenuItem = new ToolStripMenuItem();
            this.cargarDatosToolStripMenuItem = new ToolStripMenuItem();
            this.generarReporteToolStripMenuItem = new ToolStripMenuItem();
            this.exportarAExcelToolStripMenuItem = new ToolStripMenuItem();
            this.salirToolStripMenuItem = new ToolStripMenuItem();
            this.tsbSalir = new ToolStripButton();
            this.TSbtnNuevo = new ToolStripButton();
            this.TSbtnGenerar = new ToolStripButton();
            this.TSbtnImprimir = new ToolStripButton();
            this.TSbtnExcel = new ToolStripButton();
            this.c1XLBook1 = new C1XLBook();
            this.dialogFile = new OpenFileDialog();
            this.miniToolStrip = new MenuStrip();
            this.splitContainer1 = new SplitContainer();
            this.asignarAlmacen = new CheckBox();
            this.Todos = new Button();
            this.errores = new Button();
            this.label3 = new Label();
            this.toolStrip = new ToolStrip();
            this.CargarInventario = new ToolStripButton();
            this.CargarExcel = new ToolStripButton();
            this.Clean2 = new ToolStripButton();
            this.btnLote = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.Salir = new ToolStripButton();
            this.fechaInventario = new DateTimePicker();
            this.catErrores = new Button();
            this.lblAlmacen = new Label();
            this.label2 = new Label();
            this.btnEnviar = new Button();
            this.btnValidar = new Button();
            this.label1 = new Label();
            this.cmbInventario = new ComboBox();
            this.menuStrip1 = new MenuStrip();
            this.archivoToolStripMenuItem1 = new ToolStripMenuItem();
            this.salirToolStripMenuItem1 = new ToolStripMenuItem();
            this.ayudaToolStripMenuItem1 = new ToolStripMenuItem();
            this.acercaDeToolStripMenuItem1 = new ToolStripMenuItem();
            this.GridResultado = new DataGridView();
            this.QTY = new DataGridViewTextBoxColumn();
            this.PART_ID = new DataGridViewTextBoxColumn();
            this.LOCATION_ID = new DataGridViewTextBoxColumn();
            this.TRACE_PROFILE = new DataGridViewTextBoxColumn();
            this.PIECE_TRACKED = new DataGridViewTextBoxColumn();
            this.TRACE = new DataGridViewTextBoxColumn();
            this.LARGO = new DataGridViewTextBoxColumn();
            this.ANCHO = new DataGridViewTextBoxColumn();
            this.ALTO = new DataGridViewTextBoxColumn();
            this.NO_PIEZAS = new DataGridViewTextBoxColumn();
            this.ERROR = new DataGridViewTextBoxColumn();
            this.panel1 = new Panel();
            this.splitContainer1.BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((ISupportInitialize)this.GridResultado).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new Size(53, 20);
            this.ayudaToolStripMenuItem.Text = "Ayuda";
            this.acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            this.acercaDeToolStripMenuItem.Size = new Size(152, 22);
            this.acercaDeToolStripMenuItem.Text = "Acerca de...";
            this.acercaDeToolStripMenuItem.Click += new EventHandler(this.acercaDeToolStripMenuItem_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(149, 6);
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.Size = new Size(159, 22);
            this.nuevoToolStripMenuItem.Text = "Nuevo";
            this.cargarDatosToolStripMenuItem.Name = "cargarDatosToolStripMenuItem";
            this.cargarDatosToolStripMenuItem.Size = new Size(159, 22);
            this.cargarDatosToolStripMenuItem.Text = "Cargar Datos";
            this.generarReporteToolStripMenuItem.Name = "generarReporteToolStripMenuItem";
            this.generarReporteToolStripMenuItem.Size = new Size(159, 22);
            this.generarReporteToolStripMenuItem.Text = "Generar Reporte";
            this.exportarAExcelToolStripMenuItem.Name = "exportarAExcelToolStripMenuItem";
            this.exportarAExcelToolStripMenuItem.Size = new Size(159, 22);
            this.exportarAExcelToolStripMenuItem.Text = "Exportar a Excel";
            this.exportarAExcelToolStripMenuItem.Click += new EventHandler(this.TSbtnExcel_Click);
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new Size(159, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new EventHandler(this.tsbSalir_Click);
            this.tsbSalir.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbSalir.ImageTransparentColor = Color.Magenta;
            this.tsbSalir.Name = "tsbSalir";
            this.tsbSalir.Size = new Size(23, 20);
            this.tsbSalir.Text = "Salir";
            this.tsbSalir.Click += new EventHandler(this.tsbSalir_Click);
            this.TSbtnNuevo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.TSbtnNuevo.Image = (Image)componentResourceManager.GetObject("TSbtnNuevo.Image");
            this.TSbtnNuevo.ImageTransparentColor = Color.Magenta;
            this.TSbtnNuevo.Name = "TSbtnNuevo";
            this.TSbtnNuevo.Size = new Size(23, 20);
            this.TSbtnNuevo.Text = "Nuevo";
            this.TSbtnGenerar.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.TSbtnGenerar.Image = (Image)componentResourceManager.GetObject("TSbtnGenerar.Image");
            this.TSbtnGenerar.ImageTransparentColor = Color.Magenta;
            this.TSbtnGenerar.Name = "TSbtnGenerar";
            this.TSbtnGenerar.Size = new Size(23, 20);
            this.TSbtnGenerar.Text = "Cargar Datos";
            this.TSbtnImprimir.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.TSbtnImprimir.Image = (Image)componentResourceManager.GetObject("TSbtnImprimir.Image");
            this.TSbtnImprimir.ImageTransparentColor = Color.Magenta;
            this.TSbtnImprimir.Name = "TSbtnImprimir";
            this.TSbtnImprimir.Size = new Size(23, 20);
            this.TSbtnImprimir.Text = "Generar Reporte";
            this.TSbtnExcel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.TSbtnExcel.Image = (Image)componentResourceManager.GetObject("TSbtnExcel.Image");
            this.TSbtnExcel.ImageTransparentColor = Color.Magenta;
            this.TSbtnExcel.Name = "TSbtnExcel";
            this.TSbtnExcel.Size = new Size(23, 20);
            this.TSbtnExcel.Text = "Exportar a Excel";
            this.TSbtnExcel.Click += new EventHandler(this.TSbtnExcel_Click);
            this.dialogFile.FileName = "Dialog";
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.BackgroundImage = (Image)Resources.bgdVisual;
            this.miniToolStrip.Dock = DockStyle.None;
            this.miniToolStrip.Location = new Point(120, 2);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Padding = new Padding(7, 2, 0, 2);
            this.miniToolStrip.Size = new Size(1100, 24);
            this.miniToolStrip.TabIndex = 26;
            this.splitContainer1.BackColor = Color.White;
            this.splitContainer1.FixedPanel = FixedPanel.Panel1;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.BackColor = Color.Transparent;
            this.splitContainer1.Panel1.Controls.Add((Control)this.asignarAlmacen);
            this.splitContainer1.Panel1.Controls.Add((Control)this.Todos);
            this.splitContainer1.Panel1.Controls.Add((Control)this.errores);
            this.splitContainer1.Panel1.Controls.Add((Control)this.label3);
            this.splitContainer1.Panel1.Controls.Add((Control)this.toolStrip);
            this.splitContainer1.Panel1.Controls.Add((Control)this.fechaInventario);
            this.splitContainer1.Panel1.Controls.Add((Control)this.catErrores);
            this.splitContainer1.Panel1.Controls.Add((Control)this.lblAlmacen);
            this.splitContainer1.Panel1.Controls.Add((Control)this.label2);
            this.splitContainer1.Panel1.Controls.Add((Control)this.btnEnviar);
            this.splitContainer1.Panel1.Controls.Add((Control)this.btnValidar);
            this.splitContainer1.Panel1.Controls.Add((Control)this.label1);
            this.splitContainer1.Panel1.Controls.Add((Control)this.cmbInventario);
            this.splitContainer1.Panel1.Controls.Add((Control)this.menuStrip1);
            this.splitContainer1.Panel2.AllowDrop = true;
            this.splitContainer1.Panel2.BackColor = Color.White;
            this.splitContainer1.Panel2.Controls.Add((Control)this.GridResultado);
            this.splitContainer1.Panel2.Cursor = Cursors.IBeam;
            this.splitContainer1.Size = new Size(995, 606);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            this.asignarAlmacen.AutoSize = true;
            this.asignarAlmacen.Location = new Point(830, 112);
            this.asignarAlmacen.Name = "asignarAlmacen";
            this.asignarAlmacen.Size = new Size(137, 34);
            this.asignarAlmacen.TabIndex = 38;
            this.asignarAlmacen.Text = "Asignar productos al\r\nalmacen y locación";
            this.asignarAlmacen.UseVisualStyleBackColor = true;
            this.Todos.Enabled = false;
            this.Todos.ImageAlign = ContentAlignment.MiddleRight;
            this.Todos.Location = new Point(640, 116);
            this.Todos.Name = "Todos";
            this.Todos.Size = new Size(93, 25);
            this.Todos.TabIndex = 37;
            this.Todos.Text = "Mostrar todos";
            this.Todos.UseVisualStyleBackColor = true;
            this.Todos.Click += new EventHandler(this.Todos_Click);
            this.errores.Enabled = false;
            this.errores.ImageAlign = ContentAlignment.MiddleRight;
            this.errores.Location = new Point(526, 116);
            this.errores.Name = "errores";
            this.errores.Size = new Size(93, 25);
            this.errores.TabIndex = 36;
            this.errores.Text = "Filtrar errores";
            this.errores.UseVisualStyleBackColor = true;
            this.errores.Click += new EventHandler(this.errores_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(360, 77);
            this.label3.Name = "label3";
            this.label3.Size = new Size(44, 15);
            this.label3.TabIndex = 35;
            this.label3.Text = "Fecha:";
            this.toolStrip.AutoSize = false;
            this.toolStrip.BackgroundImage = (Image)Resources.background_image;
            this.toolStrip.GripMargin = new Padding(5);
            this.toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new ToolStripItem[6]
            {
        (ToolStripItem) this.CargarInventario,
        (ToolStripItem) this.CargarExcel,
        (ToolStripItem) this.Clean2,
        (ToolStripItem) this.btnLote,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.Salir
            });
            this.toolStrip.Location = new Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new Padding(8, 0, 1, 0);
            this.toolStrip.RenderMode = ToolStripRenderMode.Professional;
            this.toolStrip.Size = new Size(995, 33);
            this.toolStrip.TabIndex = 24;
            this.toolStrip.Text = "toolStrip1";
            this.CargarInventario.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.CargarInventario.Image = (Image)Resources.MNUREFRSH;
            this.CargarInventario.ImageTransparentColor = Color.Magenta;
            this.CargarInventario.Name = "CargarInventario";
            this.CargarInventario.Size = new Size(23, 30);
            this.CargarInventario.Text = "Cargar Inventarios";
            this.CargarInventario.Click += new EventHandler(this.CargarInventario_Click);
            this.CargarExcel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.CargarExcel.Enabled = false;
            this.CargarExcel.Image = (Image)Resources.MNUEXCEL;
            this.CargarExcel.ImageTransparentColor = Color.Magenta;
            this.CargarExcel.Name = "CargarExcel";
            this.CargarExcel.Size = new Size(23, 30);
            this.CargarExcel.Text = "Cargar Excel";
            this.CargarExcel.Click += new EventHandler(this.CargarExcel_Click);
            this.Clean2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.Clean2.Image = (Image)Resources.New;
            this.Clean2.ImageTransparentColor = Color.Magenta;
            this.Clean2.Name = "Clean2";
            this.Clean2.Size = new Size(23, 30);
            this.Clean2.Text = "Limpiar ";
            this.Clean2.Click += new EventHandler(this.Clean2_Click);
            this.btnLote.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.btnLote.Image = (Image)Resources.list;
            this.btnLote.ImageTransparentColor = Color.Magenta;
            this.btnLote.Name = "btnLote";
            this.btnLote.Size = new Size(23, 30);
            this.btnLote.ToolTipText = "Migración por Lotes";
            this.btnLote.Click += new EventHandler(this.btnLote_Click);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 33);
            this.Salir.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.Salir.Image = (Image)Resources.MNUEXIT;
            this.Salir.ImageTransparentColor = Color.Magenta;
            this.Salir.Name = "Salir";
            this.Salir.Size = new Size(23, 30);
            this.Salir.Text = "Salir";
            this.Salir.Click += new EventHandler(this.Salir_Click);
            this.fechaInventario.Format = DateTimePickerFormat.Short;
            this.fechaInventario.Location = new Point(407, 74);
            this.fechaInventario.Name = "fechaInventario";
            this.fechaInventario.Size = new Size(95, 21);
            this.fechaInventario.TabIndex = 34;
            this.fechaInventario.Value = new DateTime(2015, 12, 21, 0, 0, 0, 0);
            this.catErrores.Location = new Point(830, 74);
            this.catErrores.Name = "catErrores";
            this.catErrores.Size = new Size(136, 24);
            this.catErrores.TabIndex = 33;
            this.catErrores.Text = "Catálogo de Errores";
            this.catErrores.UseVisualStyleBackColor = true;
            this.catErrores.Click += new EventHandler(this.catErrores_Click);
            this.lblAlmacen.AutoSize = true;
            this.lblAlmacen.Location = new Point(589, 77);
            this.lblAlmacen.Name = "lblAlmacen";
            this.lblAlmacen.Size = new Size(11, 15);
            this.lblAlmacen.TabIndex = 32;
            this.lblAlmacen.Text = "-";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(527, 77);
            this.label2.Name = "label2";
            this.label2.Size = new Size(58, 15);
            this.label2.TabIndex = 31;
            this.label2.Text = "Almacén:";
            this.btnEnviar.Enabled = false;
            this.btnEnviar.ImageAlign = ContentAlignment.MiddleRight;
            this.btnEnviar.Location = new Point(229, 116);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new Size(135, 25);
            this.btnEnviar.TabIndex = 30;
            this.btnEnviar.Text = "Procesar inventario";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new EventHandler(this.btnEnviar_Click);
            this.btnValidar.Enabled = false;
            this.btnValidar.Location = new Point(79, 116);
            this.btnValidar.Name = "btnValidar";
            this.btnValidar.Size = new Size(130, 25);
            this.btnValidar.TabIndex = 29;
            this.btnValidar.Text = "Validar información";
            this.btnValidar.UseVisualStyleBackColor = true;
            this.btnValidar.Click += new EventHandler(this.btnValidar_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 76);
            this.label1.Name = "label1";
            this.label1.Size = new Size(63, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Inventario:";
            this.cmbInventario.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbInventario.FormattingEnabled = true;
            this.cmbInventario.Location = new Point(79, 73);
            this.cmbInventario.Name = "cmbInventario";
            this.cmbInventario.Size = new Size(249, 23);
            this.cmbInventario.TabIndex = 27;
            this.cmbInventario.SelectedIndexChanged += new EventHandler(this.cmbInventario_SelectedIndexChanged);
            this.menuStrip1.Items.AddRange(new ToolStripItem[2]
            {
        (ToolStripItem) this.archivoToolStripMenuItem1,
        (ToolStripItem) this.ayudaToolStripMenuItem1
            });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new Size(995, 24);
            this.menuStrip1.TabIndex = 26;
            this.menuStrip1.Text = "menuStrip1";
            this.archivoToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) this.salirToolStripMenuItem1
            });
            this.archivoToolStripMenuItem1.Name = "archivoToolStripMenuItem1";
            this.archivoToolStripMenuItem1.Size = new Size(60, 20);
            this.archivoToolStripMenuItem1.Text = "Archivo";
            this.salirToolStripMenuItem1.Image = (Image)componentResourceManager.GetObject("salirToolStripMenuItem1.Image");
            this.salirToolStripMenuItem1.Name = "salirToolStripMenuItem1";
            this.salirToolStripMenuItem1.Size = new Size(96, 22);
            this.salirToolStripMenuItem1.Text = "Salir";
            this.salirToolStripMenuItem1.Click += new EventHandler(this.tsbSalir_Click);
            this.ayudaToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) this.acercaDeToolStripMenuItem1
            });
            this.ayudaToolStripMenuItem1.Name = "ayudaToolStripMenuItem1";
            this.ayudaToolStripMenuItem1.Size = new Size(53, 20);
            this.ayudaToolStripMenuItem1.Text = "Ayuda";
            this.acercaDeToolStripMenuItem1.Image = (Image)componentResourceManager.GetObject("acercaDeToolStripMenuItem1.Image");
            this.acercaDeToolStripMenuItem1.Name = "acercaDeToolStripMenuItem1";
            this.acercaDeToolStripMenuItem1.Size = new Size(135, 22);
            this.acercaDeToolStripMenuItem1.Text = "Acerca de...";
            this.acercaDeToolStripMenuItem1.Click += new EventHandler(this.acercaDeToolStripMenuItem_Click);
            this.GridResultado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridResultado.Columns.AddRange((DataGridViewColumn)this.QTY, (DataGridViewColumn)this.PART_ID, (DataGridViewColumn)this.LOCATION_ID, (DataGridViewColumn)this.TRACE_PROFILE, (DataGridViewColumn)this.PIECE_TRACKED, (DataGridViewColumn)this.TRACE, (DataGridViewColumn)this.LARGO, (DataGridViewColumn)this.ANCHO, (DataGridViewColumn)this.ALTO, (DataGridViewColumn)this.NO_PIEZAS, (DataGridViewColumn)this.ERROR);
            this.GridResultado.Location = new Point(8, 3);
            this.GridResultado.Name = "GridResultado";
            this.GridResultado.ReadOnly = true;
            this.GridResultado.Size = new Size(975, 417);
            this.GridResultado.TabIndex = 0;
            this.QTY.HeaderText = "CANTIDAD";
            this.QTY.Name = "QTY";
            this.QTY.ReadOnly = true;
            this.QTY.Width = 70;
            this.PART_ID.HeaderText = "ID PRODUCTO";
            this.PART_ID.Name = "PART_ID";
            this.PART_ID.ReadOnly = true;
            this.PART_ID.Width = 115;
            this.LOCATION_ID.HeaderText = "ALMACEN";
            this.LOCATION_ID.Name = "LOCATION_ID";
            this.LOCATION_ID.ReadOnly = true;
            this.LOCATION_ID.Width = 110;
            this.TRACE_PROFILE.HeaderText = "EST. LOTE";
            this.TRACE_PROFILE.Name = "TRACE_PROFILE";
            this.TRACE_PROFILE.ReadOnly = true;
            this.PIECE_TRACKED.HeaderText = "DIMENSIONES";
            this.PIECE_TRACKED.Name = "PIECE_TRACKED";
            this.PIECE_TRACKED.ReadOnly = true;
            this.TRACE.HeaderText = "LOTE";
            this.TRACE.Name = "TRACE";
            this.TRACE.ReadOnly = true;
            this.TRACE.Width = 80;
            this.LARGO.HeaderText = "LARGO";
            this.LARGO.Name = "LARGO";
            this.LARGO.ReadOnly = true;
            this.LARGO.Width = 60;
            this.ANCHO.HeaderText = "ANCHO";
            this.ANCHO.Name = "ANCHO";
            this.ANCHO.ReadOnly = true;
            this.ANCHO.Width = 60;
            this.ALTO.HeaderText = "ALTO";
            this.ALTO.Name = "ALTO";
            this.ALTO.ReadOnly = true;
            this.ALTO.Width = 60;
            this.NO_PIEZAS.HeaderText = "PIEZAS";
            this.NO_PIEZAS.Name = "NO_PIEZAS";
            this.NO_PIEZAS.ReadOnly = true;
            this.NO_PIEZAS.Width = 70;
            this.ERROR.HeaderText = "ERROR";
            this.ERROR.Name = "ERROR";
            this.ERROR.ReadOnly = true;
            this.ERROR.Width = 90;
            this.panel1.Controls.Add((Control)this.splitContainer1);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(996, 608);
            this.panel1.TabIndex = 0;
            this.AutoScaleDimensions = new SizeF(7f, 15f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackgroundImage = (Image)Resources.bgdVisual;
            this.ClientSize = new Size(996, 608);
            this.Controls.Add((Control)this.panel1);
            this.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.MinimumSize = new Size(1004, 639);
            this.Name = nameof(Principal);
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Migración de Inventario";
            this.FormClosed += new FormClosedEventHandler(this.Principal_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((ISupportInitialize)this.GridResultado).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        public enum upload
        {
            QTY = 1,
            PART_ID = 2,
            LOCATION_ID = 3,
            LARGO = 7,
            ANCHO = 8,
            ALTO = 9,
            PIEZAS = 10, // 0x0000000A
        }
    }
}
