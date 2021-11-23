using System;
using System.Collections.Generic;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ResourcePlanning.Services
{
    public static class OracleConnectionManager
    {
        private static string OracleConnectionString { get; set; }

        private static void GetConfig()
        {
            var builder = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot config = builder.Build();

            OracleConnectionString = config.GetSection("ConnectionStrings:CMiCConnection").Value;
        }

        public static string GetConnectionString()
        {
            GetConfig();

            return OracleConnectionString;
        }

        

        public static IDbConnection GetConnection()
        {
            GetConfig();

            var conn = new OracleConnection(OracleConnectionString);

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        public static void CloseConnection(IDbConnection conn)
        {
            if (conn.State == ConnectionState.Open || conn.State == ConnectionState.Broken)
            {
                conn.Close();
            }
        }


    }
}
