﻿using Framework.Core.Extensions;
using Framework.Core.Models;
using Framework.Core.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Framework.Core.DbContextCore.CodeFirst
{
    public class SqlServerDbContext : BaseDbContext
    {
        public SqlServerDbContext(IOptions<DbContextOption> option) : base(option)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_option.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public override void BulkInsert<T>(IList<T> entities, string destinationTableName = null)
        {
            if (entities == null || !entities.Any()) return;
            if (string.IsNullOrEmpty(destinationTableName))
            {
                var mappingTableName = typeof(T).GetCustomAttribute<TableAttribute>()?.Name;
                destinationTableName = string.IsNullOrEmpty(mappingTableName) ? typeof(T).Name : mappingTableName;
            }
            SqlBulkInsert<T>(entities, destinationTableName);
        }

        private void SqlBulkInsert<T>(IList<T> entities, string destinationTableName = null) where T : BaseModel
        {
            using (var dt = entities.ToDataTable())
            {
                dt.TableName = destinationTableName;
                using (var conn = Database.GetDbConnection() as SqlConnection ?? new SqlConnection(_option.ConnectionString))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran)
                            {
                                BatchSize = entities.Count,
                                DestinationTableName = dt.TableName,
                            };
                            GenerateColumnMappings<T>(bulk.ColumnMappings);
                            bulk.WriteToServerAsync(dt);
                            tran.Commit();
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void GenerateColumnMappings<T>(SqlBulkCopyColumnMappingCollection mappings)
            where T : BaseModel
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes<KeyAttribute>().Any())
                {
                    mappings.Add(new SqlBulkCopyColumnMapping(property.Name, typeof(T).Name + property.Name));
                }
                else
                {
                    mappings.Add(new SqlBulkCopyColumnMapping(property.Name, property.Name));
                }
            }
        }

    }
}
