using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SQLite;
using System.Data;
using System.Data.SqlClient;

namespace Food.DAL
{
   public static class SQLiteHelper
    {
        public static string strConn = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
        //ExecuteNonquery
        public static int ExecuteNonquery(string sql, params SQLiteParameter[] pms)
        {
            using (SQLiteConnection conn = new SQLiteConnection(strConn))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        //ExecuteScalar
        public static object ExecuteScalar(string sql, params SQLiteParameter[] pms)
        {
            using (SQLiteConnection conn = new SQLiteConnection(strConn))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        //ExecuteReader
        public static SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] pms)
        {
            SQLiteConnection conn = new SQLiteConnection(strConn);
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
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
                    return cmd.ExecuteReader();
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
        }
        //ExecuteDataTable
        public static DataTable ExecuteDataTable(string sql, params SQLiteParameter[] pms)
        {
           // SQLiteConnection conn = new SQLiteConnection(strConn);
           // conn.Open();
            DataTable dt = new DataTable();
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, strConn))
            {
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
           // conn.Close();
            return dt;
        }
    }
}
