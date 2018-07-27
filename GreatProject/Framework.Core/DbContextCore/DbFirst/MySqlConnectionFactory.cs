using Chloe.Infrastructure;
using MySql.Data.MySqlClient;
using System.Data;

namespace Framework.Core.DbContextCore.DbFirst
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public MySqlConnectionFactory(string connString)
        {
            this._connString = connString;
        }
        public IDbConnection CreateConnection()
        {
            IDbConnection conn = new MySqlConnection(this._connString);
            return conn;
        }
    }
}
