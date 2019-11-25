// Decompiled with JetBrains decompiler
// Type: MigracionInventarios.Physical_count
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MigracionInventarios
{
    internal class Physical_count
    {
        public InventarioFisico encabezado;
        public List<Tag> tags;
        private Conexion conn;

        public Physical_count(string id_inventario, Conexion conn)
        {
            this.conn = conn;
            this.cargarEncabezado(id_inventario);
        }

        private void cargarEncabezado(string id_inventario)
        {
            this.encabezado = new InventarioFisico();
            this.encabezado.ID = id_inventario;
            this.encabezado.numTags = this.cargarNumeroTags(id_inventario);
            this.encabezado.numRegistrosExcel = this.cargarNumeroRegistrosExcel(id_inventario);
            this.encabezado.LOCATIONS = new List<string>();
            this.cargarAlmacen(this.encabezado.ID);
        }

        public void cargarAlmacen(string id_inventario)
        {
            foreach (DataRow row in (InternalDataCollectionBase)this.conn.obtenertabla(string.Format("SELECT WAREHOUSE_ID, LOCATION_ID FROM PHYS_COUNT_LOC WHERE PHYS_COUNT_ID = '{0}'", (object)id_inventario)).Rows)
            {
                this.encabezado.WAREHOUSE_ID = row["WAREHOUSE_ID"].ToString();
                this.encabezado.LOCATIONS.Add(row["LOCATION_ID"].ToString());
            }
            this.encabezado.SITE_ID = this.conn.obtenertabla(string.Format("SELECT SITE_ID FROM PHYSICAL_COUNT WHERE ID = '{0}'", (object)id_inventario)).Rows[0]["SITE_ID"].ToString();
        }

        public string tipoAlmacen(string warehouse, string location)
        {
            return this.conn.returnValor(string.Format("select TYPE FROM LOCATION WHERE WAREHOUSE_ID = '{0}' AND ID = '{1}'", (object)warehouse, (object)location));
        }

        public void cargarTag(DataTable dt, bool asignarAlmacen)
        {
            this.tags = new List<Tag>();
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                Tag t = new Tag();
                t.errores = new List<int>();
                t.QTY = Convert.ToDouble(row["QTY"].ToString());
                t.PART_ID = row["PART_ID"].ToString();
                t.LOCATION_ID = row["LOCATION_ID"].ToString();
                t.TRACE = row["TRACE"].ToString();
                double result1;
                double.TryParse(row["HEIGHT"].ToString(), out result1);
                t.HEIGHT = result1;
                double result2;
                double.TryParse(row["LENGTH"].ToString(), out result2);
                t.LENGTH = result2;
                double result3;
                double.TryParse(row["WIDTH"].ToString(), out result3);
                t.WIDTH = result3;
                double result4;
                double.TryParse(row["PIECES"].ToString(), out result4);
                t.PIECES = result4;
                if (!this.encabezado.LOCATIONS.Contains(t.LOCATION_ID))
                {
                    t.errores.Add(2);
                }
                else
                {
                    string str = this.tipoAlmacen(this.encabezado.WAREHOUSE_ID, t.LOCATION_ID);
                    if (str != "F" && str != "R")
                        t.errores.Add(15);
                }
                string length = string.Empty;
                string width = string.Empty;
                string height = string.Empty;
                string dimensions = string.Empty;
                t.PIECE_TRACKED = this.obtenerPieceTracked(t.PART_ID, out length, out width, out height, out dimensions);
                if (string.IsNullOrEmpty(t.PIECE_TRACKED))
                {
                    t.errores.Add(1);
                }
                else
                {
                    t.HEIGHT_REQD = height;
                    t.LENGTH_REQD = length;
                    t.WIDTH_REQD = width;
                    t.DIMENSIONS_UM = dimensions;
                }
                t.TRACE_PROFILE = this.exist_trace_profile(t.PART_ID, this.encabezado.SITE_ID);
                bool flag = this.validarExistenciaPartLocation(t.PART_ID, this.encabezado.WAREHOUSE_ID, t.LOCATION_ID);
                if (!asignarAlmacen && !flag)
                    t.errores.Add(3);
                else if (asignarAlmacen && !flag)
                    t.insertarAlmacen = true;
                if (t.TRACE_PROFILE == "N" && t.PIECE_TRACKED == "N")
                    t.TIPOARTICULO = 1;
                else if (t.TRACE_PROFILE == "Y" && t.PIECE_TRACKED == "N")
                    t.TIPOARTICULO = 2;
                else if (t.TRACE_PROFILE == "N" && t.PIECE_TRACKED == "Y")
                    t.TIPOARTICULO = 3;
                else if (t.TRACE_PROFILE == "Y" && t.PIECE_TRACKED == "Y")
                    t.TIPOARTICULO = 4;
                if (t.TRACE_PROFILE == "Y")
                {
                    if (string.IsNullOrEmpty(t.TRACE) || string.IsNullOrWhiteSpace(t.TRACE))
                        t.errores.Add(4);
                    else if (!this.buscarTrace(t.PART_ID, t.TRACE, this.encabezado.SITE_ID))
                        t.errores.Add(5);
                }
                if (t.PIECE_TRACKED == "Y")
                {
                    if (t.PIECES <= 0.0)
                        t.errores.Add(13);
                    if (t.HEIGHT_REQD == "N")
                    {
                        if (t.HEIGHT > 0.0)
                            t.errores.Add(6);
                    }
                    else if (t.HEIGHT <= 0.0)
                        t.errores.Add(10);
                    if (t.WIDTH_REQD == "N")
                    {
                        if (t.WIDTH > 0.0)
                            t.errores.Add(7);
                    }
                    else if (t.WIDTH <= 0.0)
                        t.errores.Add(11);
                    if (t.LENGTH_REQD == "N")
                    {
                        if (t.LENGTH > 0.0)
                            t.errores.Add(8);
                    }
                    else if (t.LENGTH <= 0.0)
                        t.errores.Add(12);
                    double num1 = result3 <= 0.0 ? 1.0 : result3;
                    double num2 = result2 <= 0.0 ? 1.0 : result2;
                    double num3 = result1 <= 0.0 ? 1.0 : result1;
                    t.cantidad_bidimensional = Convert.ToDouble(Decimal.Ceiling(Convert.ToDecimal(num1 * num2 * num3) * new Decimal(10000)) / new Decimal(10000) * (Decimal)t.PIECES);
                    if (t.QTY != t.cantidad_bidimensional)
                        t.errores.Add(14);
                }
                else
                {
                    if (t.HEIGHT > 0.0)
                        t.errores.Add(6);
                    if (t.WIDTH > 0.0)
                        t.errores.Add(7);
                    if (t.LENGTH > 0.0)
                        t.errores.Add(8);
                }
                if (t.TIPOARTICULO == 1)
                {
                    if (this.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID)
                           return r.LOCATION_ID == t.LOCATION_ID;
                       return false;
                   })).Count<Tag>() > 0)
                        t.errores.Add(9);
                }
                else if (t.TIPOARTICULO == 2)
                {
                    if (this.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID && r.LOCATION_ID == t.LOCATION_ID)
                           return r.TRACE == t.TRACE;
                       return false;
                   })).Count<Tag>() > 0)
                        t.errores.Add(9);
                }
                else if (t.TIPOARTICULO == 3)
                {
                    if (this.tags.Where<Tag>((Func<Tag, bool>)(r =>
                   {
                       if (r.PART_ID == t.PART_ID && r.LOCATION_ID == t.LOCATION_ID && (r.LENGTH == t.LENGTH && r.WIDTH == t.WIDTH))
                           return r.HEIGHT == t.HEIGHT;
                       return false;
                   })).Count<Tag>() > 0)
                        t.errores.Add(9);
                }
                else if (t.TIPOARTICULO == 4 && this.tags.Where<Tag>((Func<Tag, bool>)(r =>
               {
                   if (r.PART_ID == t.PART_ID && r.LOCATION_ID == t.LOCATION_ID && (r.TRACE == t.TRACE && r.LENGTH == t.LENGTH) && r.WIDTH == t.WIDTH)
                       return r.HEIGHT == t.HEIGHT;
                   return false;
               })).Count<Tag>() > 0)
                    t.errores.Add(9);
                this.tags.Add(t);
            }
            //this.encabezado.numProductos = this.tags.GroupBy(r => new
            //{
            //    PART_ID = r.PART_ID,
            //    LOCATION_ID = r.LOCATION_ID
            //}).Count<Tag>()
                //.Count<IGrouping<\u003C\u003Ef__AnonymousType0<string, string>, Tag>>()
        }

    public bool buscarTrace(string PART_ID, string TRACE, string SITE_ID)
        {
            return this.conn.obtenertabla(string.Format("SELECT * from TRACE where PART_ID = '{0}' AND ID = '{1}'", (object)PART_ID, (object)TRACE)).Rows.Count > 0;
        }

        public bool validarExistenciaPartLocation(string PART_ID, string WAREHOUSE_ID, string LOCATION_ID)
        {
            return this.conn.obtenertabla(string.Format("SELECT * FROM PART_LOCATION WHERE PART_ID = '{0}' AND WAREHOUSE_ID = '{1}' AND LOCATION_ID = '{2}'", (object)PART_ID, (object)WAREHOUSE_ID, (object)LOCATION_ID)).Rows.Count > 0;
        }

        public string exist_trace_profile(string part_id, string SITE_ID)
        {
            return this.conn.obtenertabla(string.Format("SELECT * FROM TRACE_PROFILE WHERE PART_ID = '{0}' AND SITE_ID = '{1}'", (object)part_id, (object)SITE_ID)).Rows.Count > 0 ? "Y" : "N";
        }

        public string obtenerPieceTracked(string PART_ID, out string length, out string width, out string height, out string dimensions)
        {
            string query = string.Format("SELECT PIECE_TRACKED, LENGTH_REQD, WIDTH_REQD, HEIGHT_REQD, DIMENSIONS_UM FROM PART WHERE ID = '{0}'", (object)PART_ID);
            dimensions = string.Empty;
            length = (string)null;
            width = (string)null;
            height = (string)null;
            string str = (string)null;
            DataTable dataTable = this.conn.obtenertabla(query);
            if (dataTable.Rows.Count <= 0)
                return str;
            if (dataTable.Rows[0]["PIECE_TRACKED"].ToString() == "Y")
            {
                length = Convert.ToString(dataTable.Rows[0]["LENGTH_REQD"]);
                width = Convert.ToString(dataTable.Rows[0]["WIDTH_REQD"]);
                height = Convert.ToString(dataTable.Rows[0]["HEIGHT_REQD"]);
                dimensions = Convert.ToString(dataTable.Rows[0]["DIMENSIONS_UM"]);
            }
            return dataTable.Rows[0]["PIECE_TRACKED"].ToString();
        }

        public int cargarNumeroTags(string id_inventario)
        {
            return Convert.ToInt32(this.conn.returnValor(string.Format("SELECT COUNT(*) AS CONTADOR FROM PHYS_COUNT_TAG where PHYS_COUNT_ID = '{0}' AND GROUP_NO = 1", (object)id_inventario)));
        }

        public int cargarNumeroRegistrosExcel(string id_inventario)
        {
            return Convert.ToInt32(this.conn.returnValor(string.Format("SELECT COUNT(*) FROM (SELECT COUNT(*) AS CONTADOR FROM VMX_MIGRACION_INVENTARIO where PHYS_COUNT_ID = '{0}' GROUP BY PART_ID, LOCATION_ID) AS T", (object)id_inventario)));
        }

        public enum tiposDeArticulos
        {
            articulo_sin_lote = 1,
            articulo_con_lote = 2,
            dimensional_sin_lote = 3,
            dimensional_con_lote = 4,
        }

        public enum error
        {
            NOEXISTE = 1,
            NOEXISTE_LOCATION = 2,
            NOEXISTE_ARTICULO_LOCATION = 3,
            LOTE_OBLIGATORIO = 4,
            NOEXISTELOTE = 5,
            HEIGHT_NO_NECESARIO = 6,
            WIDTH_NO_NECESARIO = 7,
            LENGTH_NO_NECESARIO = 8,
            REPETIDO = 9,
            HEIGHT_REQUERIDO = 10, // 0x0000000A
            WIDTH_REQUERIDO = 11, // 0x0000000B
            LENGTH_REQUERIDO = 12, // 0x0000000C
            PIECES_REQUERIDO = 13, // 0x0000000D
            CANTIDADES_DIFERENTES = 14, // 0x0000000E
            LOCATION_TIPO_INVALIDO = 15, // 0x0000000F
            TRACE_LOCATION_PART_REPETIDO = 16, // 0x00000010
        }
    }
}
