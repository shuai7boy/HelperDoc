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
        private static string strConn = ConfigurationManager.ConnectionStrings["sql"].     
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
       //Tansnsiction
       /// <summary>
       ///用于事务处理，执行一条sql语句，并返回主键（select @@IDENTITY）
       /// </summary>
       /// <param name="connection">SqlConnection对象</param>
       /// <param name="trans">SqlTransaction事务</param>
       /// <param name="SQLString">计算查询结果语句</param>
       /// <returns>查询结果（object）</returns>
       public static object GetSingle(SqlConnection connection, SqlTransaction trans, string SQLString, params SqlParameter[] cmdParms)
       {
           using (SqlCommand cmd = new SqlCommand())
           {
               try
               {
                   PrepareCommand(cmd, connection, trans, SQLString, cmdParms);
                   object obj = cmd.ExecuteScalar();
                   cmd.Parameters.Clear();
                   if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                   {
                       return null;
                   }
                   else
                   {
                       return obj;
                   }
               }
               catch (System.Data.SqlClient.SqlException e)
               {
                   //trans.Rollback();
                   throw e;
               }
           }
       }
       private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
       {
           if (conn.State != ConnectionState.Open)
               conn.Open();
           cmd.Connection = conn;
           cmd.CommandText = cmdText;
           if (trans != null)
               cmd.Transaction = trans;
           cmd.CommandType = CommandType.Text;//cmdType;
           if (cmdParms != null)
           {


               foreach (SqlParameter parameter in cmdParms)
               {
                   if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                       (parameter.Value == null))
                   {
                       parameter.Value = DBNull.Value;
                   }
                   cmd.Parameters.Add(parameter);
               }
           }
       }
    }
}
