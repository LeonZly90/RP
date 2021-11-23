using System;
using System.Collections.Generic;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using ResourcePlanning.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using ResourcePlanning.Models;
using Dapper;
using System.Linq;

namespace ResourcePlanning.Services
{
    public static class DataUtil<T>
    {

       

        //public DataTable GetCmicData(string sSQL, string sDataTableName)
        //{

        //    string sConnStr = OracleConnectionManager.GetConnectionString();

        //    System.Data.DataTable dt = new System.Data.DataTable(sDataTableName);


        //    OracleConnection oConn = new OracleConnection(sConnStr);
        //    oConn.Open();

        //    OracleCommand cmd = new OracleCommand();
        //    cmd.Connection = oConn;
        //    cmd.CommandText = sSQL;
        //    cmd.CommandType = System.Data.CommandType.Text;

        //    OracleDataAdapter d = new OracleDataAdapter(cmd);
        //    d.Fill(dt);
        //    cmd.Dispose();
        //    oConn.Close();


        //    return dt;
        //}

        //public void ExecSQL(string sql)
        //{

        //    try
        //    {
        //        string sConnStr = OracleConnectionManager.GetConnectionString();
        //        OracleConnection oConn = new OracleConnection(sConnStr);
        //        oConn.Open();
        //        OracleCommand cmd = new OracleCommand();
        //        cmd.Connection = oConn;
        //        cmd.CommandText = sql;
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.ExecuteNonQuery();
        //        cmd.Dispose();
        //        oConn.Close();

        //    }
        //    catch (SystemException ex)
        //    {
        //        throw;

        //    }
        //}


        public static IEnumerable<T> GetList(T entity, string query)
        {
            IDbConnection connection = OracleConnectionManager.GetConnection();

            var result = connection.Query<T>(query);

            OracleConnectionManager.CloseConnection(connection);

            return result;
        }

        public static T SingleOrDefault(T entity, string query)
        {
            IDbConnection connection = OracleConnectionManager.GetConnection();

            var result = connection.QueryFirstOrDefault<T>(query);

            OracleConnectionManager.CloseConnection(connection);

            return result;
        }

        public static int? ExecuteAction(string query)
        {
            IDbConnection connection = OracleConnectionManager.GetConnection();

            var result = connection.Execute(query);

            OracleConnectionManager.CloseConnection(connection);

            return result;
        }

        





    }
}
