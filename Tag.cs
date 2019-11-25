// Decompiled with JetBrains decompiler
// Type: MigracionInventarios.Tag
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

using System.Collections.Generic;

namespace MigracionInventarios
{
    public class Tag
    {
        public bool insertarAlmacen = false;

        public string PART_ID { get; set; }

        public string LOCATION_ID { get; set; }

        public double QTY { get; set; }

        public string PIECE_TRACKED { get; set; }

        public string TRACE_PROFILE { get; set; }

        public string TRACE { get; set; }

        public string LENGTH_REQD { get; set; }

        public string WIDTH_REQD { get; set; }

        public string HEIGHT_REQD { get; set; }

        public string DIMENSIONS_UM { get; set; }

        public int TIPOARTICULO { get; set; }

        public double LENGTH { get; set; }

        public double WIDTH { get; set; }

        public double HEIGHT { get; set; }

        public double PIECES { get; set; }

        public double cantidad_bidimensional { get; set; }

        public List<int> errores { get; set; }
    }
}
