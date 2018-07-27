using Chloe.Infrastructure;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.Core.DbContextCore.DbFirst
{
    public class PostgreSqlConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public PostgreSqlConnectionFactory(string connString)
        {
            this._connString = connString;
        }
        public IDbConnection CreateConnection()
        {
            IDbConnection conn = new NpgsqlConnection(this._connString);
            return conn;

        }
    }
}
