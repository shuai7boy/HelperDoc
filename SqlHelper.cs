using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace 三层__登录
{
   public static class SqlHelper
    {
       private static string strConn = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
       //ExecuteNonquery
       public static int ExecuteNonquery(string sql, CommandType cmdType, params SqlParameter[] pms)
       {
           using (SqlConnection conn = new SqlConnection(strConn))
           {
               using (SqlCommand cmd = new SqlCommand(sql, conn))
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
       public static object ExecuteScalar(string sql, CommandType cmdType, params SqlParameter[] pms)
       {
           using (SqlConnection conn = new SqlConnection(strConn))
           {
              
               using (SqlCommand cmd = new SqlCommand(sql, conn))
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
       public static SqlDataReader ExecuteReader(string sql, CommandType cmdType, params SqlParameter[] pms)
       {
           SqlConnection conn = new SqlConnection(strConn);
           using (SqlCommand cmd = new SqlCommand(sql, conn))
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
       public static DataSet ExecuteDataTable(string sql, CommandType cmdType, params SqlParameter[] pms)
       {
           DataSet dt = new DataSet();
           using (SqlDataAdapter adapter = new SqlDataAdapter(sql, strConn))
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
