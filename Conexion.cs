// Decompiled with JetBrains decompiler
// Type: MigracionInventarios.Conexion
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

using CTECH.Acceso_a_Datos;
using System.Data;

namespace MigracionInventarios
{
    public class Conexion
    {
        private Microsoft_SQL_Server conn;

        public Conexion(string server, string database, string usuario, string password)
        {
            this.conn = new Microsoft_SQL_Server(server, database, usuario, password);
        }

        public DataTable obtenertabla(string query)
        {
            this.conn.CrearConexion();
            this.conn.AbrirConexion();
            DataTable dataTable = this.conn.EjecutarConsulta(query, "Table");
            this.conn.CerrarConexion();
            this.conn.DestruirConexion();
            return dataTable;
        }

        public DataTable obtenertablaTrans(string query)
        {
            return this.conn.EjecutarConsulta(query, "Table");
        }

        public string returnValor(string query)
        {
            this.conn.CrearConexion();
            this.conn.AbrirConexion();
            string str = this.conn.executeScalar(query);
            this.conn.CerrarConexion();
            this.conn.DestruirConexion();
            return str;
        }

        public void crearTransaccion()
        {
            this.conn.CrearConexion();
            this.conn.AbrirConexion();
            this.conn.CrearTransaccion();
        }

        public void commitTransaccion()
        {
            this.conn.TransCommit();
            this.conn.CerrarConexion();
            this.conn.DestruirConexion();
        }

        public void ejecutarConsultaDMLTransaccion(string query)
        {
            this.conn.EjecutarDML(query);
        }

        public void destruirTransaccion()
        {
            this.conn.TransRollback();
            this.conn.DestruirTransaccion();
            this.conn.CerrarConexion();
            this.conn.DestruirConexion();
        }

        public void probarConexion()
        {
            this.conn.CrearConexion();
            this.conn.AbrirConexion();
            this.conn.CerrarConexion();
            this.conn.DestruirConexion();
        }
    }
}
