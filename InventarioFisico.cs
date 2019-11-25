// Decompiled with JetBrains decompiler
// Type: MigracionInventarios.InventarioFisico
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

using System.Collections.Generic;

namespace MigracionInventarios
{
    internal class InventarioFisico
    {
        public string ID { get; set; }

        public int numTags { get; set; }

        public int numRegistrosExcel { get; set; }

        public string WAREHOUSE_ID { get; set; }

        public List<string> LOCATIONS { get; set; }

        public int numProductos { get; set; }

        public string SITE_ID { get; set; }
    }
}
