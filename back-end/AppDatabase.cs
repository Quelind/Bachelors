using MySql.Data.MySqlClient;
using System;

namespace ClinicAPI
{
    public class AppDatabase : IDisposable
    {
        public MySqlConnection Connection { get; }

        public AppDatabase(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}
