// Decompiled with JetBrains decompiler
// Type: MigracionInventarios.MigracionPorLote
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

using CTECH.Configuracion.Business;
using CTECH.Configuracion.Entities;
using CTECH.Directorios;
using Excel;
using MigracionInventarios.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MigracionInventarios
{
    public class MigracionPorLote : Form
    {
        private LogErrores archivoErrores = (LogErrores)null;
        private int indice_property_fecha = -1;
        private int indice_property_pedimento = -1;
        private IContainer components = (IContainer)null;
        private Conexion _cnn;
        private MigracionPorLote.encabezadoTrace encabezado;
        private List<MigracionPorLote.registroTrace> registros;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem archivoToolStripMenuItem;
        private ToolStripMenuItem nuevoToolStripMenuItem;
        private ToolStripMenuItem salirToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton bNuevo;
        private ToolStripButton bExcel;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton bSalir;
        private ToolStripMenuItem excelToolStripMenuItem;
        private ToolStripButton bValidar;
        private ToolStripButton bSubir;
        private ToolStripMenuItem verificarArchivoToolStripMenuItem;
        private ToolStripMenuItem subirArchivoToolStripMenuItem;
        private StatusStrip statusStrip1;
        private SplitContainer splitContainer1;
        private TextBox txtArchivo;
        private Label label3;
        private DateTimePicker dtFecha;
        private Label label2;
        private ComboBox cbSite;
        private Label label1;
        private DataGridView dataGridView1;
        private ToolStripButton bCatErrores;
        private DataGridView dgvResultado;
        private ComboBox cbFechaPedimento;
        private Label label4;
        private DataGridViewTextBoxColumn PARTID;
        private DataGridViewTextBoxColumn TRACEID;
        private DataGridViewTextBoxColumn APPROPERTY_1;
        private DataGridViewTextBoxColumn APPROPERTY_2;
        private DataGridViewTextBoxColumn APPROPERTY_3;
        private DataGridViewTextBoxColumn APPROPERTY_4;
        private DataGridViewTextBoxColumn APPROPERTY_5;
        private DataGridViewTextBoxColumn ERROR;
        private ComboBox cbUDF_Pedimento;
        private Label label5;
        private ComboBox cbPedimento;
        private Label label6;

        public MigracionPorLote()
        {
            this.InitializeComponent();
        }

        public MigracionPorLote(Conexion c)
        {
            this.InitializeComponent();
            this._cnn = c;
            this.catalogoSites();
            this.catalogoApproperty();
            this.catalogoUDF_Pedimento();
        }

        private void catalogoSites()
        {
            try
            {
                this.cbSite.Items.Clear();
                DataTable dataTable = this._cnn.obtenertabla(this.obtenerQuery("MigracionPorLote_Sites"));
                if (dataTable == null)
                    return;
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                    this.cbSite.Items.Add((object)row["ID"].ToString());
            }
            catch (Exception ex)
            {
                this.archivoErrores = new LogErrores();
                this.archivoErrores.escribir(nameof(MigracionPorLote), " private void catalogoSites()", ex.Message);
            }
        }

        private void catalogoApproperty()
        {
            this.cbFechaPedimento.Items.Clear();
            this.cbPedimento.Items.Clear();
            string[] strArray = new string[5]
            {
        "approperty_1",
        "approperty_2",
        "approperty_3",
        "approperty_4",
        "approperty_5"
            };
            this.cbFechaPedimento.Items.AddRange((object[])strArray);
            this.cbPedimento.Items.AddRange((object[])strArray);
        }

        private void catalogoUDF_Pedimento()
        {
            try
            {
                this.cbUDF_Pedimento.Items.Clear();
                DataTable dataTable = this._cnn.obtenertabla(this.obtenerQuery("MigracionPorLote_UDF"));
                if (dataTable == null)
                    return;
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                    this.cbUDF_Pedimento.Items.Add((object)row["ID"].ToString());
            }
            catch (Exception ex)
            {
                this.archivoErrores = new LogErrores();
                this.archivoErrores.escribir(nameof(MigracionPorLote), " private void catalogoUDF_Pedimento()", ex.Message);
            }
        }

        private void nuevo()
        {
            this.txtArchivo.Text = "";
            this.cbSite.Text = "";
            this.cbUDF_Pedimento.Text = "";
            this.encabezado = (MigracionPorLote.encabezadoTrace)null;
            this.registros = (List<MigracionPorLote.registroTrace>)null;
            this.dgvResultado.Rows.Clear();
            this.indice_property_fecha = -1;
            this.indice_property_pedimento = -1;
        }

        private void seleccionExcel()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.txtArchivo.Text = openFileDialog.FileName;
            this.bSubir.Enabled = false;
            this.subirArchivoToolStripMenuItem.Enabled = false;
            this.indice_property_fecha = -1;
            this.indice_property_pedimento = -1;
            if (this.registros != null)
                this.registros = (List<MigracionPorLote.registroTrace>)null;
            if (this.encabezado != null)
                this.registros = (List<MigracionPorLote.registroTrace>)null;
            this.dgvResultado.Rows.Clear();
        }

        private string obtenerQuery(string id)
        {
            string empty = string.Empty;
            string str = CustomConfigurationManager.Instance.Get(SectionType.sql, id);
            if (string.IsNullOrEmpty(str))
                throw new NullReferenceException("Identificador de consulta SQL no encontrado: " + id);
            return str;
        }

        private DataTable abrirExcel()
        {
            try
            {
                using (FileStream fileStream = File.Open(this.txtArchivo.Text, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelDataReader = (IExcelDataReader)null;
                    if (Path.GetExtension(this.txtArchivo.Text) == ".xls")
                        excelDataReader = ExcelReaderFactory.CreateBinaryReader((Stream)fileStream);
                    if (Path.GetExtension(this.txtArchivo.Text) == ".xlsx")
                        excelDataReader = ExcelReaderFactory.CreateOpenXmlReader((Stream)fileStream);
                    return excelDataReader.AsDataSet().Tables[0];
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message.ToString());
                return (DataTable)null;
            }
        }

        private void validar()
        {
            this.encabezado = new MigracionPorLote.encabezadoTrace();
            this.encabezado.Site_Id = this.cbSite.Text;
            this.encabezado.Fecha = this.dtFecha.Value.Date;
            this.dgvResultado.Rows.Clear();
            if (!this.validarSiteId(this.encabezado) || !this.validarUDF() || !this.validarExcel())
                return;
            this.bSubir.Enabled = true;
            this.subirArchivoToolStripMenuItem.Enabled = true;
        }

        private bool validarSiteId(MigracionPorLote.encabezadoTrace e)
        {
            if (e.Site_Id != "")
                return true;
            int num = (int)MessageBox.Show("Debe seleccionar un site", "Migración por Lotes", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
        }

        private bool validarUDF()
        {
            if (!(this.cbUDF_Pedimento.Text != "") || this.cbPedimento.Text != "" && this.cbFechaPedimento.Text != "")
                return true;
            int num = (int)MessageBox.Show("Al seleccionar un UDF de pedimento también debe elegir los campos de pedimento y fecha de pedimento", "Migración por Lotes", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
        }

        private bool validarExcel()
        {
            bool flag = true;
            Cursor.Current = Cursors.WaitCursor;
            DataTable dataTable = this.abrirExcel();
            this.registros = new List<MigracionPorLote.registroTrace>();
            this.registros.Clear();
            if (dataTable != null)
            {
                int num1 = 0;
                foreach (DataRow row1 in (InternalDataCollectionBase)dataTable.Rows)
                {
                    if ((uint)num1 > 0U)
                    {
                        MigracionPorLote.registroTrace registroTrace = new MigracionPorLote.registroTrace();
                        registroTrace.asignaCantidadesDefault();
                        registroTrace.approperties = new List<string>();
                        registroTrace.partId = row1[this.encabezado.columna_1].ToString();
                        registroTrace.traceId = row1[this.encabezado.columna_2].ToString();
                        for (int index = 5; index < dataTable.Columns.Count; ++index)
                        {
                            string str = row1[dataTable.Columns[index].ColumnName] != null ? row1[dataTable.Columns[index].ColumnName].ToString() : "";
                            registroTrace.approperties.Add(str);
                        }
                        registroTrace.error = this.validarPartId(registroTrace.partId);
                        if (registroTrace.error == 0)
                        {
                            registroTrace.numberId = registroTrace.partId;
                            int num2 = 1;
                            DataRow row2 = this._cnn.obtenertabla(string.Format("SELECT APROPERTY_LABEL_1, APROPERTY_1_REQD, APROPERTY_LABEL_2, APROPERTY_2_REQD, APROPERTY_LABEL_3, APROPERTY_3_REQD,APROPERTY_LABEL_4, APROPERTY_4_REQD,APROPERTY_LABEL_5, APROPERTY_5_REQD FROM TRACE_PROFILE WHERE PART_ID = '{0}' AND SITE_ID = '{1}'", (object)registroTrace.numberId, (object)this.encabezado.Site_Id)).Rows[0];
                            foreach (string approperty in registroTrace.approperties)
                            {
                                if (!string.IsNullOrEmpty(row2[string.Format("APROPERTY_LABEL_{0}", (object)num2)].ToString()) && row2[string.Format("APROPERTY_{0}_REQD", (object)num2)].ToString() == "Y" && (string.IsNullOrEmpty(approperty) && string.IsNullOrWhiteSpace(approperty)))
                                {
                                    registroTrace.error = 21;
                                    break;
                                }
                                if (!string.IsNullOrEmpty(approperty) && !string.IsNullOrWhiteSpace(approperty) && (string.IsNullOrEmpty(row2[string.Format("APROPERTY_LABEL_{0}", (object)num2)].ToString()) && row2[string.Format("APROPERTY_{0}_REQD", (object)num2)].ToString() == "N"))
                                {
                                    registroTrace.error = 22;
                                    break;
                                }
                                ++num2;
                            }
                            if (registroTrace.error == 0)
                            {
                                if (this.cbUDF_Pedimento.Text != "")
                                {
                                    if (this.validarUDF_Pedimento(registroTrace.partId, this.cbUDF_Pedimento.Text))
                                    {
                                        this.indice_property_pedimento = (int)Convert.ToInt16(this.cbPedimento.Text.Substring(this.cbPedimento.Text.Length - 1, 1)) - 1;
                                        registroTrace.error = this.validarApproperty(registroTrace.approperties[this.indice_property_pedimento]) ? 0 : 20;
                                        if (registroTrace.error == 0)
                                        {
                                            this.indice_property_fecha = (int)Convert.ToInt16(this.cbFechaPedimento.Text.Substring(this.cbFechaPedimento.Text.Length - 1, 1)) - 1;
                                            if (!string.IsNullOrEmpty(registroTrace.approperties[this.indice_property_fecha]))
                                            {
                                                DateTime d;
                                                registroTrace.error = this.validarFecha(registroTrace.approperties[this.indice_property_fecha], out d) ? 0 : 19;
                                                if (registroTrace.error == 0)
                                                    registroTrace.approperties[this.indice_property_fecha] = d.Date.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.indice_property_fecha = -1;
                                        this.indice_property_pedimento = -1;
                                    }
                                }
                                int num3 = 0;
                                foreach (string approperty in registroTrace.approperties)
                                {
                                    if (registroTrace.error == 0 && num3 != this.indice_property_fecha && num3 != this.indice_property_pedimento)
                                        registroTrace.error = this.validarApproperty(approperty) ? 0 : 18;
                                    ++num3;
                                }
                            }
                        }
                        this.registros.Add(registroTrace);
                        if ((uint)registroTrace.error > 0U)
                            flag = false;
                    }
                    else
                    {
                        string str = "" + (row1[0].ToString() == this.encabezado.columna_1 ? "" : "\nColumna 1: " + this.encabezado.columna_1) + (row1[4].ToString() == this.encabezado.columna_2 ? "" : "\nColumna 5: " + this.encabezado.columna_2);
                        if (str != "")
                        {
                            int num2 = (int)MessageBox.Show("Debe cambiar los nombres de las columnas:" + str, "Migración por Lote", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            break;
                        }
                        for (int index = 0; index < dataTable.Columns.Count; ++index)
                        {
                            if (row1[index] != null && row1[index].ToString() != "")
                                dataTable.Columns[index].ColumnName = row1[index].ToString();
                        }
                    }
                    ++num1;
                }
            }
            if (this.registros.Count > 0)
            {
                foreach (MigracionPorLote.registroTrace registro in this.registros)
                {
                    DataGridViewRow dataGridViewRow = (DataGridViewRow)this.dgvResultado.Rows[0].Clone();
                    dataGridViewRow.Cells[0].Value = (object)registro.partId;
                    dataGridViewRow.Cells[1].Value = (object)registro.traceId;
                    dataGridViewRow.Cells[7].Value = (object)registro.error;
                    int index = 2;
                    foreach (string approperty in registro.approperties)
                    {
                        dataGridViewRow.Cells[index].Value = (object)approperty;
                        ++index;
                    }
                    this.dgvResultado.Rows.Add(dataGridViewRow);
                }
            }
            Cursor.Current = Cursors.Default;
            return flag;
        }

        private int validarPartId(string partId)
        {
            int num = 0;
            try
            {
                DataTable dataTable = this._cnn.obtenertabla(string.Format(this.obtenerQuery("MigracionPorLote_Part"), (object)this.encabezado.Site_Id, (object)partId));
                num = dataTable == null || dataTable.Rows.Count <= 0 ? 1 : (dataTable.Rows[0]["TRACE"].ToString() != "" ? 0 : 17);
            }
            catch (Exception ex)
            {
                this.archivoErrores = new LogErrores();
                this.archivoErrores.escribir(nameof(MigracionPorLote), " private void catalogoSites()", ex.Message);
            }
            return num;
        }

        private bool validarUDF_Pedimento(string partId, string udf)
        {
            bool flag = false;
            try
            {
                DataTable dataTable = this._cnn.obtenertabla(string.Format(this.obtenerQuery("MigracionPorLote_UDF_PartId"), (object)udf, (object)partId));
                if (dataTable != null && dataTable.Rows.Count > 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                this.archivoErrores = new LogErrores();
                this.archivoErrores.escribir(nameof(MigracionPorLote), " private void catalogoUDF_Pedimento()", ex.Message);
            }
            return flag;
        }

        private bool validarFecha(string fecha, out DateTime d)
        {
            d = new DateTime();
            if (fecha == null || !(fecha != ""))
                return false;
            try
            {
                d = Convert.ToDateTime(fecha);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool validarApproperty(string a)
        {
            return a != null && a.Length <= 80;
        }

        private bool validarPedimento(string p)
        {
            return this.validarApproperty(p) && p != "";
        }

        private void guardarExcel()
        {
            if (this.subirExcel())
            {
                int num1 = (int)MessageBox.Show("El archivo fue procesado con éxito", "Migración por Lote", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                int num2 = (int)MessageBox.Show("El archivo no fue procesado", "Migración por Lote", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private bool subirExcel()
        {
            Cursor.Current = Cursors.WaitCursor;
            bool flag = false;
            string empty = string.Empty;
            this._cnn.crearTransaccion();
            foreach (MigracionPorLote.registroTrace registro in this.registros)
            {
                try
                {
                    object[] objArray1 = new object[17];
                    objArray1[0] = (object)registro.partId;
                    objArray1[1] = (object)registro.traceId;
                    objArray1[2] = (object)registro.partId;
                    objArray1[3] = (object)registro.OUT_QTY;
                    objArray1[4] = (object)registro.IN_QTY;
                    objArray1[5] = (object)registro.REPORTED_QTY;
                    objArray1[6] = (object)registro.ASSIGNED_QTY;
                    object[] objArray2 = objArray1;
                    int index = 7;
                    DateTime dateTime = this.encabezado.Fecha;
                    dateTime = dateTime.Date;
                    string str = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    objArray2[index] = (object)str;
                    objArray1[8] = (object)registro.DISP_IN_QTY;
                    objArray1[9] = (object)registro.DISP_OUT_QTY;
                    objArray1[10] = (object)registro.UNAVAILABLE_QTY;
                    objArray1[11] = (object)"NULL";
                    int num1 = 0;
                    foreach (string approperty in registro.approperties)
                    {
                        int num2 = string.IsNullOrEmpty(approperty) ? 1 : (string.IsNullOrWhiteSpace(approperty) ? 1 : 0);
                        objArray1[num1 + 12] = num2 == 0 ? (this.indice_property_fecha != num1 ? (object)string.Format("'{0}'", (object)approperty) : (object)string.Format("'{0:dd/MM/yyyy}'", (object)Convert.ToDateTime(approperty))) : (object)"NULL";
                        ++num1;
                    }
                    if (num1 < 5)
                    {
                        for (; num1 < 5; ++num1)
                            objArray1[num1 + 12] = (object)"";
                    }
                    this._cnn.ejecutarConsultaDMLTransaccion(string.Format(this.obtenerQuery("MigracionPorLote_Trace"), objArray1));
                    flag = true;
                }
                catch (Exception ex)
                {
                    this.archivoErrores = new LogErrores();
                    this.archivoErrores.escribir(nameof(MigracionPorLote), " subirExcel()", ex.Message);
                    flag = false;
                    Cursor.Current = Cursors.Default;
                    int num = (int)MessageBox.Show("Ocurrió un error al intentar subir el archivo." + (!ex.Message.StartsWith("Message: Violation of PRIMARY KEY") ? "\n" + ex.Message : "\nSe intentó subir el Part_Id: " + registro.partId + " con un número de serie ya existente: " + registro.traceId), "Migración por Lotes", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    break;
                }
            }
            if (flag)
                this._cnn.commitTransaccion();
            else
                this._cnn.destruirTransaccion();
            Cursor.Current = Cursors.Default;
            return flag;
        }

        private void bExcel_Click(object sender, EventArgs e)
        {
            this.seleccionExcel();
        }

        private void bCatErrores_Click(object sender, EventArgs e)
        {
            new CatalogoErrores().Show();
        }

        private void bNuevo_Click(object sender, EventArgs e)
        {
            this.nuevo();
        }

        private void bValidar_Click(object sender, EventArgs e)
        {
            this.validar();
        }

        private void bSubir_Click(object sender, EventArgs e)
        {
            this.guardarExcel();
        }

        private void bSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void verificarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.validar();
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.seleccionExcel();
        }

        private void subirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.guardarExcel();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.nuevo();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MigracionPorLote_Load(object sender, EventArgs e)
        {
            this.dtFecha.Value = DateTime.Now;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MigracionPorLote));
            this.menuStrip1 = new MenuStrip();
            this.archivoToolStripMenuItem = new ToolStripMenuItem();
            this.nuevoToolStripMenuItem = new ToolStripMenuItem();
            this.excelToolStripMenuItem = new ToolStripMenuItem();
            this.verificarArchivoToolStripMenuItem = new ToolStripMenuItem();
            this.subirArchivoToolStripMenuItem = new ToolStripMenuItem();
            this.salirToolStripMenuItem = new ToolStripMenuItem();
            this.statusStrip1 = new StatusStrip();
            this.splitContainer1 = new SplitContainer();
            this.cbPedimento = new ComboBox();
            this.label6 = new Label();
            this.cbUDF_Pedimento = new ComboBox();
            this.label5 = new Label();
            this.cbFechaPedimento = new ComboBox();
            this.label4 = new Label();
            this.txtArchivo = new TextBox();
            this.label3 = new Label();
            this.dtFecha = new DateTimePicker();
            this.label2 = new Label();
            this.cbSite = new ComboBox();
            this.label1 = new Label();
            this.dgvResultado = new DataGridView();
            this.PARTID = new DataGridViewTextBoxColumn();
            this.TRACEID = new DataGridViewTextBoxColumn();
            this.APPROPERTY_1 = new DataGridViewTextBoxColumn();
            this.APPROPERTY_2 = new DataGridViewTextBoxColumn();
            this.APPROPERTY_3 = new DataGridViewTextBoxColumn();
            this.APPROPERTY_4 = new DataGridViewTextBoxColumn();
            this.APPROPERTY_5 = new DataGridViewTextBoxColumn();
            this.ERROR = new DataGridViewTextBoxColumn();
            this.dataGridView1 = new DataGridView();
            this.toolStrip1 = new ToolStrip();
            this.bNuevo = new ToolStripButton();
            this.bExcel = new ToolStripButton();
            this.bValidar = new ToolStripButton();
            this.bSubir = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.bCatErrores = new ToolStripButton();
            this.bSalir = new ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((ISupportInitialize)this.dgvResultado).BeginInit();
            ((ISupportInitialize)this.dataGridView1).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            this.menuStrip1.Items.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) this.archivoToolStripMenuItem
            });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(902, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
            {
        (ToolStripItem) this.nuevoToolStripMenuItem,
        (ToolStripItem) this.excelToolStripMenuItem,
        (ToolStripItem) this.verificarArchivoToolStripMenuItem,
        (ToolStripItem) this.subirArchivoToolStripMenuItem,
        (ToolStripItem) this.salirToolStripMenuItem
            });
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            this.nuevoToolStripMenuItem.Image = (Image)Resources.New;
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.Size = new Size(160, 22);
            this.nuevoToolStripMenuItem.Text = "Nuevo";
            this.nuevoToolStripMenuItem.Click += new EventHandler(this.nuevoToolStripMenuItem_Click);
            this.excelToolStripMenuItem.Image = (Image)Resources.MNUEXCEL;
            this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
            this.excelToolStripMenuItem.Size = new Size(160, 22);
            this.excelToolStripMenuItem.Text = "Excel";
            this.excelToolStripMenuItem.Click += new EventHandler(this.excelToolStripMenuItem_Click);
            this.verificarArchivoToolStripMenuItem.Image = (Image)Resources.check_black;
            this.verificarArchivoToolStripMenuItem.Name = "verificarArchivoToolStripMenuItem";
            this.verificarArchivoToolStripMenuItem.Size = new Size(160, 22);
            this.verificarArchivoToolStripMenuItem.Text = "Verificar Archivo";
            this.verificarArchivoToolStripMenuItem.Click += new EventHandler(this.verificarArchivoToolStripMenuItem_Click);
            this.subirArchivoToolStripMenuItem.Enabled = false;
            this.subirArchivoToolStripMenuItem.Image = (Image)Resources.up_arrow;
            this.subirArchivoToolStripMenuItem.Name = "subirArchivoToolStripMenuItem";
            this.subirArchivoToolStripMenuItem.Size = new Size(160, 22);
            this.subirArchivoToolStripMenuItem.Text = "Subir Archivo";
            this.subirArchivoToolStripMenuItem.Click += new EventHandler(this.subirArchivoToolStripMenuItem_Click);
            this.salirToolStripMenuItem.Image = (Image)Resources.MNUEXIT;
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new Size(160, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new EventHandler(this.salirToolStripMenuItem_Click);
            this.statusStrip1.Location = new Point(0, 408);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(902, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.FixedPanel = FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.Controls.Add((Control)this.cbPedimento);
            this.splitContainer1.Panel1.Controls.Add((Control)this.label6);
            this.splitContainer1.Panel1.Controls.Add((Control)this.cbUDF_Pedimento);
            this.splitContainer1.Panel1.Controls.Add((Control)this.label5);
            this.splitContainer1.Panel1.Controls.Add((Control)this.cbFechaPedimento);
            this.splitContainer1.Panel1.Controls.Add((Control)this.label4);
            this.splitContainer1.Panel1.Controls.Add((Control)this.txtArchivo);
            this.splitContainer1.Panel1.Controls.Add((Control)this.label3);
            this.splitContainer1.Panel1.Controls.Add((Control)this.dtFecha);
            this.splitContainer1.Panel1.Controls.Add((Control)this.label2);
            this.splitContainer1.Panel1.Controls.Add((Control)this.cbSite);
            this.splitContainer1.Panel1.Controls.Add((Control)this.label1);
            this.splitContainer1.Panel2.Controls.Add((Control)this.dgvResultado);
            this.splitContainer1.Panel2.Controls.Add((Control)this.dataGridView1);
            this.splitContainer1.Size = new Size(902, 359);
            this.splitContainer1.SplitterDistance = 85;
            this.splitContainer1.TabIndex = 3;
            this.cbPedimento.BackColor = Color.White;
            this.cbPedimento.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbPedimento.FormattingEnabled = true;
            this.cbPedimento.Location = new Point(705, 46);
            this.cbPedimento.Name = "cbPedimento";
            this.cbPedimento.Size = new Size(133, 21);
            this.cbPedimento.TabIndex = 45;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(588, 49);
            this.label6.Name = "label6";
            this.label6.Size = new Size(111, 13);
            this.label6.TabIndex = 44;
            this.label6.Text = "Campo de Pedimento:";
            this.cbUDF_Pedimento.BackColor = Color.White;
            this.cbUDF_Pedimento.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbUDF_Pedimento.FormattingEnabled = true;
            this.cbUDF_Pedimento.Location = new Point(125, 46);
            this.cbUDF_Pedimento.Name = "cbUDF_Pedimento";
            this.cbUDF_Pedimento.Size = new Size(133, 21);
            this.cbUDF_Pedimento.TabIndex = 43;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(16, 49);
            this.label5.Name = "label5";
            this.label5.Size = new Size(100, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "UDF de Pedimento:";
            this.cbFechaPedimento.BackColor = Color.White;
            this.cbFechaPedimento.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbFechaPedimento.FormattingEnabled = true;
            this.cbFechaPedimento.Location = new Point(446, 46);
            this.cbFechaPedimento.Name = "cbFechaPedimento";
            this.cbFechaPedimento.Size = new Size(133, 21);
            this.cbFechaPedimento.TabIndex = 39;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(281, 49);
            this.label4.Name = "label4";
            this.label4.Size = new Size(159, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Campo de Fecha de Pedimento:";
            this.txtArchivo.BackColor = Color.White;
            this.txtArchivo.Enabled = false;
            this.txtArchivo.Location = new Point(68, 14);
            this.txtArchivo.MaxLength = 1000;
            this.txtArchivo.Name = "txtArchivo";
            this.txtArchivo.Size = new Size(332, 20);
            this.txtArchivo.TabIndex = 37;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(16, 17);
            this.label3.Name = "label3";
            this.label3.Size = new Size(46, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Archivo:";
            this.dtFecha.CalendarMonthBackground = Color.White;
            this.dtFecha.Format = DateTimePickerFormat.Short;
            this.dtFecha.Location = new Point(705, 14);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Size = new Size(133, 20);
            this.dtFecha.TabIndex = 35;
            this.dtFecha.Value = new DateTime(2016, 5, 9, 0, 0, 0, 0);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(659, 17);
            this.label2.Name = "label2";
            this.label2.Size = new Size(40, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fecha:";
            this.cbSite.BackColor = Color.White;
            this.cbSite.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbSite.FormattingEnabled = true;
            this.cbSite.Location = new Point(446, 13);
            this.cbSite.Name = "cbSite";
            this.cbSite.Size = new Size(133, 21);
            this.cbSite.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(412, 17);
            this.label1.Name = "label1";
            this.label1.Size = new Size(28, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Site:";
            this.label1.TextAlign = ContentAlignment.MiddleRight;
            this.dgvResultado.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResultado.Columns.AddRange((DataGridViewColumn)this.PARTID, (DataGridViewColumn)this.TRACEID, (DataGridViewColumn)this.APPROPERTY_1, (DataGridViewColumn)this.APPROPERTY_2, (DataGridViewColumn)this.APPROPERTY_3, (DataGridViewColumn)this.APPROPERTY_4, (DataGridViewColumn)this.APPROPERTY_5, (DataGridViewColumn)this.ERROR);
            this.dgvResultado.Dock = DockStyle.Fill;
            this.dgvResultado.Location = new Point(0, 0);
            this.dgvResultado.Name = "dgvResultado";
            this.dgvResultado.Size = new Size(902, 270);
            this.dgvResultado.TabIndex = 1;
            this.PARTID.HeaderText = "NO. PARTE";
            this.PARTID.Name = "PARTID";
            this.TRACEID.HeaderText = "NO. SERIE";
            this.TRACEID.Name = "TRACEID";
            this.TRACEID.ReadOnly = true;
            this.APPROPERTY_1.HeaderText = "APPROPERTY_1";
            this.APPROPERTY_1.Name = "APPROPERTY_1";
            this.APPROPERTY_1.ReadOnly = true;
            this.APPROPERTY_2.HeaderText = "APPROPERTY_2";
            this.APPROPERTY_2.Name = "APPROPERTY_2";
            this.APPROPERTY_2.ReadOnly = true;
            this.APPROPERTY_3.HeaderText = "APPROPERTY_3";
            this.APPROPERTY_3.Name = "APPROPERTY_3";
            this.APPROPERTY_3.ReadOnly = true;
            this.APPROPERTY_4.HeaderText = "APPROPERTY_4";
            this.APPROPERTY_4.Name = "APPROPERTY_4";
            this.APPROPERTY_4.ReadOnly = true;
            this.APPROPERTY_5.HeaderText = "APPROPERTY_5";
            this.APPROPERTY_5.Name = "APPROPERTY_5";
            this.APPROPERTY_5.ReadOnly = true;
            this.ERROR.HeaderText = "ERROR";
            this.ERROR.Name = "ERROR";
            this.ERROR.ReadOnly = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new Size(902, 270);
            this.dataGridView1.TabIndex = 0;
            this.toolStrip1.BackgroundImage = (Image)Resources.background_image;
            this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new ToolStripItem[7]
            {
        (ToolStripItem) this.bNuevo,
        (ToolStripItem) this.bExcel,
        (ToolStripItem) this.bValidar,
        (ToolStripItem) this.bSubir,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.bCatErrores,
        (ToolStripItem) this.bSalir
            });
            this.toolStrip1.Location = new Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(902, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.bNuevo.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bNuevo.Image = (Image)Resources.New;
            this.bNuevo.ImageTransparentColor = Color.Magenta;
            this.bNuevo.Name = "bNuevo";
            this.bNuevo.Size = new Size(23, 22);
            this.bNuevo.Text = "Nuevo";
            this.bNuevo.Click += new EventHandler(this.bNuevo_Click);
            this.bExcel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bExcel.Image = (Image)Resources.excel;
            this.bExcel.ImageTransparentColor = Color.Magenta;
            this.bExcel.Name = "bExcel";
            this.bExcel.Size = new Size(23, 22);
            this.bExcel.Text = "Excel";
            this.bExcel.Click += new EventHandler(this.bExcel_Click);
            this.bValidar.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bValidar.Image = (Image)Resources.White_check_svg;
            this.bValidar.ImageTransparentColor = Color.Magenta;
            this.bValidar.Name = "bValidar";
            this.bValidar.Size = new Size(23, 22);
            this.bValidar.Text = "Verificar Archivo";
            this.bValidar.Click += new EventHandler(this.bValidar_Click);
            this.bSubir.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bSubir.Enabled = false;
            this.bSubir.Image = (Image)Resources.load;
            this.bSubir.ImageTransparentColor = Color.Magenta;
            this.bSubir.Name = "bSubir";
            this.bSubir.Size = new Size(23, 22);
            this.bSubir.Text = "Subir Archivo";
            this.bSubir.Click += new EventHandler(this.bSubir_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 25);
            this.bCatErrores.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bCatErrores.Image = (Image)Resources.generar;
            this.bCatErrores.ImageTransparentColor = Color.Magenta;
            this.bCatErrores.Name = "bCatErrores";
            this.bCatErrores.Size = new Size(23, 22);
            this.bCatErrores.Text = "Catálogo de Errores";
            this.bCatErrores.Click += new EventHandler(this.bCatErrores_Click);
            this.bSalir.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bSalir.Image = (Image)Resources.MNUEXIT;
            this.bSalir.ImageTransparentColor = Color.Magenta;
            this.bSalir.Name = "bSalir";
            this.bSalir.Size = new Size(23, 22);
            this.bSalir.Text = "Salir";
            this.bSalir.TextImageRelation = TextImageRelation.Overlay;
            this.bSalir.Click += new EventHandler(this.bSalir_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(902, 430);
            this.Controls.Add((Control)this.splitContainer1);
            this.Controls.Add((Control)this.statusStrip1);
            this.Controls.Add((Control)this.toolStrip1);
            this.Controls.Add((Control)this.menuStrip1);
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.MainMenuStrip = this.menuStrip1;
            this.Name = nameof(MigracionPorLote);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Migración por Lote (TRACE)";
            this.Load += new EventHandler(this.MigracionPorLote_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((ISupportInitialize)this.dgvResultado).EndInit();
            ((ISupportInitialize)this.dataGridView1).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public class encabezadoTrace
        {
            public string Site_Id { get; set; }

            public DateTime Fecha { get; set; }

            public string columna_1 { get; set; }

            public string columna_2 { get; set; }

            public encabezadoTrace()
            {
                this.columna_1 = "No. Parte";
                this.columna_2 = "Numero de Serie";
            }
        }

        public class registroTrace
        {
            public string partId { get; set; }

            public string traceId { get; set; }

            public string numberId { get; set; }

            public List<string> approperties { get; set; }

            public int error { get; set; }

            public int OUT_QTY { get; set; }

            public int IN_QTY { get; set; }

            public int REPORTED_QTY { get; set; }

            public int ASSIGNED_QTY { get; set; }

            public int DISP_IN_QTY { get; set; }

            public int DISP_OUT_QTY { get; set; }

            public int UNAVAILABLE_QTY { get; set; }

            public void asignaCantidadesDefault()
            {
                this.OUT_QTY = 0;
                this.IN_QTY = 0;
                this.REPORTED_QTY = 0;
                this.ASSIGNED_QTY = 0;
                this.DISP_IN_QTY = 0;
                this.DISP_OUT_QTY = 0;
                this.UNAVAILABLE_QTY = 0;
            }
        }
    }
}
