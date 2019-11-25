// Decompiled with JetBrains decompiler
// Type: MigracionInventarios.Program
// Assembly: VKMIGINV, Version=1.0.0.4, Culture=neutral, PublicKeyToken=null
// MVID: 091BB591-6A6D-4391-A9A1-65EF14350A86
// Assembly location: C:\MigracionInventario\VKMIGINV.exe

using CTECH.Log;
using System;
using System.Collections;
using System.Configuration;
using System.Windows.Forms;

namespace MigracionInventarios
{
    internal static class Program
    {
        public static Hashtable Session;

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SingletonLogger.Instance.Severity = (LogSeverity)Enum.Parse(typeof(LogSeverity), ConfigurationManager.AppSettings.Get("LogSeverity"), true);
            SingletonLogger.Instance.Attach((ILog)new ObserverLogToConsole());
            SingletonLogger.Instance.Attach((ILog)new ObserverLogToFile(Application.StartupPath + "\\Log_VKMIGINV.log"));
            try
            {
                Program.Session = new Hashtable();
            }
            catch
            {
            }
            if ((uint)args.Length > 0U)
            {
                string password = args[2].Substring(2, args[1].Length - 2).Trim();
                string pUsuario = args[1].Substring(2, args[1].Length - 2).Trim();
                string database = args[0].Substring(2, args[0].Length - 2).Trim();
                Application.Run((Form)new Principal(args[3], pUsuario, password, database));
            }
            else
                Application.Run((Form)new Login());
        }
    }
}
