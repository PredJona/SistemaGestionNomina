using System;
using System.Data.SQLite;
using System.IO;

namespace SistemaGestionNomina.Data
{
    public static class SQLiteConnectionFactory
    {
        public static string DatabasePath
        {
            get { return Path.Combine(GetApplicationFolder(), "nomina.db"); }
        }

        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + DatabasePath + ";Version=3;Foreign Keys=True;");
            connection.Open();
            return connection;
        }

        private static string GetApplicationFolder()
        {
            string location = typeof(SQLiteConnectionFactory).Assembly.Location;
            if (!string.IsNullOrWhiteSpace(location))
            {
                return Path.GetDirectoryName(location);
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
