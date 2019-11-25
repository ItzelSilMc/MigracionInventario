// Decompiled with JetBrains decompiler
// Type: Kardex.cExportarExcel
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

using C1.C1Excel;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace Kardex
{
    public class cExportarExcel
    {
        private C1XLBook oBook = (C1XLBook)null;
        private XLSheet sheet = (XLSheet)null;

        private void crearEncabezados(DataTable dtDatos)
        {
            for (int index = 0; index < dtDatos.Columns.Count; ++index)
            {
                this.sheet[2, index].Value = (object)(" " + dtDatos.Columns[index].ColumnName + " ");
                this.sheet[2, index].Style = this.Encabezado();
            }
        }

        private XLStyle Negritas()
        {
            return new XLStyle(this.oBook)
            {
                Font = new Font("Arial", 10f, FontStyle.Bold)
            };
        }

        private XLStyle Encabezado()
        {
            return new XLStyle(this.oBook)
            {
                BackColor = Color.CadetBlue,
                Font = new Font("Arial", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                AlignHorz = XLAlignHorzEnum.Left
            };
        }

        private XLStyle Font()
        {
            return new XLStyle(this.oBook)
            {
                Font = new Font("Arial", 10f)
            };
        }

        public void exportarConsolidado(DataTable dtDatos, string ruta)
        {
            this.oBook = new C1XLBook();
            this.sheet = this.oBook.Sheets[0];
            this.sheet.Name = "Migración de Inventarios";
            this.crearEncabezados(dtDatos);
            string empty = string.Empty;
            for (int index1 = 0; index1 < dtDatos.Rows.Count; ++index1)
            {
                for (int index2 = 0; index2 < dtDatos.Columns.Count; ++index2)
                {
                    string str = dtDatos.Rows[index1][index2].ToString();
                    new XLColumn().Width = 125;
                    XLCell xlCell = this.sheet[index1 + 3, index2];
                    XLStyle xlStyle = new XLStyle(this.oBook);
                    xlStyle.WordWrap = false;
                    if (index2 == 3 || index2 == 4 || (index2 == 5 || index2 == 6) || index2 == 7 || index2 == 8)
                    {
                        xlStyle.Format = "0,0.0";
                        xlStyle.AlignHorz = XLAlignHorzEnum.Right;
                        xlCell.Value = (object)(string.IsNullOrEmpty(str) ? Decimal.Zero : Convert.ToDecimal(str));
                    }
                    else
                    {
                        xlStyle.AlignHorz = XLAlignHorzEnum.Left;
                        xlCell.Value = (object)str;
                    }
                    xlCell.Style = xlStyle;
                }
            }
            this.AutoSizeColumns(this.sheet);
            this.oBook.Save(ruta);
            Process.Start(ruta);
        }

        public void CrearArchivo(DataTable dtDatos, string ruta, bool pVerCosto)
        {
            try
            {
                this.oBook = new C1XLBook();
                this.sheet = this.oBook.Sheets[0];
                this.sheet.Name = "Migración de Inventarios";
                this.crearEncabezados(dtDatos);
                string empty = string.Empty;
                for (int index1 = 0; index1 < dtDatos.Rows.Count; ++index1)
                {
                    for (int index2 = 0; index2 < dtDatos.Columns.Count; ++index2)
                    {
                        if ((index2 != 20 || pVerCosto) && (index2 != 21 || pVerCosto))
                        {
                            string str = dtDatos.Rows[index1][index2].ToString();
                            new XLColumn().Width = 125;
                            XLCell xlCell = this.sheet[index1 + 3, index2];
                            XLStyle xlStyle = new XLStyle(this.oBook);
                            xlStyle.WordWrap = false;
                            if (index2 == 2 || index2 == 3 || (index2 == 4 || index2 == 5) || index2 == 20 || index2 == 21)
                            {
                                xlStyle.Format = "0,0.0";
                                xlStyle.AlignHorz = XLAlignHorzEnum.Right;
                                xlCell.Value = (object)(string.IsNullOrEmpty(str) ? Decimal.Zero : Convert.ToDecimal(str));
                            }
                            else if (index2 == 22 || index2 == 23)
                            {
                                xlStyle.AlignHorz = XLAlignHorzEnum.Right;
                                xlCell.Value = (object)(string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str));
                            }
                            else
                            {
                                xlStyle.AlignHorz = XLAlignHorzEnum.Left;
                                xlCell.Value = (object)str;
                            }
                            xlCell.Style = xlStyle;
                        }
                    }
                }
                this.AutoSizeColumns(this.sheet);
                this.oBook.Save(ruta);
                Process.Start(ruta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AutoSizeColumns(XLSheet sheet)
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                for (int index1 = 0; index1 < sheet.Columns.Count; ++index1)
                {
                    int num = -1;
                    for (int index2 = 0; index2 < sheet.Rows.Count; ++index2)
                    {
                        object obj = sheet[index2, index1].Value;
                        if (obj != null)
                        {
                            string str = obj.ToString();
                            XLStyle style = sheet[index2, index1].Style;
                            if (style != null && style.Format.Length > 0 && obj is IFormattable)
                            {
                                string dotNet = XLStyle.FormatXLToDotNet(style.Format);
                                str = ((IFormattable)obj).ToString(dotNet, (IFormatProvider)CultureInfo.CurrentCulture);
                            }
                            Font font = this.oBook.DefaultFont;
                            if (style != null && style.Font != null)
                                font = style.Font;
                            Size size = Size.Ceiling(graphics.MeasureString(str + "XX", font));
                            if (size.Width > num)
                                num = size.Width;
                        }
                    }
                    if (num > -1)
                        sheet.Columns[index1].Width = C1XLBook.PixelsToTwips((double)num);
                }
            }
        }
    }
}
