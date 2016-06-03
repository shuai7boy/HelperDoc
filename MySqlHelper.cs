using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace MySql练习
{
   public static class SqlHelper
    {
       private static string strConn = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
       //ExecuteNonquery
       public static int ExecuteNonquery(string sql, CommandType cmdType, params MySqlParameter[] pms)
       {
           using (MySqlConnection conn = new MySqlConnection(strConn))
           {
               using (MySqlCommand cmd = new MySqlCommand(sql, conn))
               {
                   cmd.CommandType = cmdType;
                   if (pms != null)
                   {
                       cmd.Parameters.AddRange(pms);

                   }
                   conn.Open();
                   return cmd.ExecuteNonQuery() ;
               }
           }
       }
       //ExecuteScalar
       public static object ExecuteScalar(string sql, CommandType cmdType, params MySqlParameter[] pms)
       {
           using (MySqlConnection conn = new MySqlConnection(strConn))
           {

               using (MySqlCommand cmd = new MySqlCommand(sql, conn))
               {
                   cmd.CommandType = cmdType;
                   if (pms != null)
                   {
                       cmd.Parameters.AddRange(pms);
                   }
                   conn.Open();
                   return cmd.ExecuteScalar();
               }
           }
       }
       //DataReader
       public static MySqlDataReader ExecuteReader(string sql, CommandType cmdType, params MySqlParameter[] pms)
       {
           MySqlConnection conn = new MySqlConnection(strConn);
           using (MySqlCommand cmd = new MySqlCommand(sql, conn))
           {
               cmd.CommandType = cmdType;
               if (pms != null)
               {
                   cmd.Parameters.AddRange(pms);
               }
               try
               {
                   if (conn.State == ConnectionState.Closed)
                   {
                       conn.Open();
                   }
                   return cmd.ExecuteReader(CommandBehavior.CloseConnection);
               }
               catch
               {
                   conn.Close();
                   conn.Dispose();
                   throw;
               }
           }
       }
       //DataTable 
       public static DataSet ExecuteDataTable(string sql, CommandType cmdType, params MySqlParameter[] pms)
       {                   
           DataSet dt = new DataSet();
           using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, strConn))
           {
               adapter.SelectCommand.CommandType = cmdType;
               if (pms != null)
               {
                   adapter.SelectCommand.Parameters.AddRange(pms);
               }
               adapter.Fill(dt);
           }
           return dt;
       }

    }
}
