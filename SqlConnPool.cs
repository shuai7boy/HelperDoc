using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace 连接池
{
    /// <summary>     
    /// 连接池帮助类
	///优点：减少了和数据库的频繁链接和端口，提高了效率
	///缺点：一直开着，浪费了性能
	///针对情景:每天大量用户访问的情况可以使用连接池提高运行效率，其它的没必要
    /// </summary>
    public  class DbConn
    {
        private const int MaxPool = 7;//最大连接数
        private const int MinPool = 3;//最小连接数
        private const bool Asyn_Process = true;//设置异步访问数据库
        //在单个连接上得到和管理多个，
        private const bool Mars = true;
        private const int Conn_Timeout = 15;//设置连接等待时间
        private const int Conn_Lifetime = 15;//设置连接的生命周期
        private  string ConnString = "";//链接字符串
        private static SqlConnection SqlDrConn = null;//链接对象
        public  DbConn()//构造函数
        {
            ConnString = GetConnString();
            SqlDrConn = new SqlConnection(ConnString);
        }
        private string GetConnString()
        {
            return "server=localhost;"
            +"integrated security=sspi;"//这里携程sspi相当于true，只不过如果服务器不支持会报错
                + "database=Inferno;"
                +"Max Pool Size="+MaxPool+";"
                +"Min Pool Size="+MinPool+";"
                +"Connect Timeout="+Conn_Timeout+";"
                +"Connection Lifetime="+Conn_Lifetime+";"
                + "Asynchronous Processing=" + Asyn_Process + ";";
        }
        /// <summary>
        /// 获取数据表
        /// DateSet可以比作内存中的一个数据库，DataTable可以比作内存中的一张表
        /// </summary>
        /// <param name="StrSql">Sql语句</param>
        /// <returns></returns>
        public  DataTable GetDataReader(string StrSql,CommandType cmdType,params SqlParameter[] pms )//数据查询
        {
            //当链接处于打开状态时关闭，然后再打开，避免有时候数据不能及时更新
            if(SqlDrConn.State==ConnectionState.Open)
            {
                SqlDrConn.Close();
            }
            try
            {
                using(SqlCommand cmd=new SqlCommand(StrSql,SqlDrConn))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    SqlDrConn.Open();
                    SqlDataReader SqlDr = cmd.ExecuteReader();
                    if (SqlDr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        //读取SqlDataReader里的内容
                        dt.Load(SqlDr);                                 
                      
                        return dt;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string tsMes=ex.Message;
                return null;
            }
            finally
            {
                SqlDrConn.Close();//关闭连接
            }
        }
        //ExecuteNonquery
        public int ExecuteNonquery(string StrSql, CommandType cmdType, params SqlParameter[] pms)
        {
            //当链接处于打开状态时关闭，然后再打开，避免有时候数据不能及时更新
            if (SqlDrConn.State == ConnectionState.Open)
            {
                SqlDrConn.Close();
            }
            try
            {
               
                using (SqlCommand cmd = new SqlCommand(StrSql, SqlDrConn)) 
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    SqlDrConn.Open();
                    return cmd.ExecuteNonQuery();
                }           
             
            }
            catch (Exception ex)
            {
                string tsMes = ex.Message;
                return 0;
            }
            finally
            {
                SqlDrConn.Close();
            }
        }
        //ExecuteScalar
        public object ExecuteScalar(string StrSql, CommandType cmdType, params SqlParameter[] pms)
        {
            //当链接处于打开状态时关闭，然后再打开，避免有时候数据不能及时更新
            if (SqlDrConn.State == ConnectionState.Open)
            { 
                SqlDrConn.Close();
            }
            try
            {

                using (SqlCommand cmd = new SqlCommand(StrSql, SqlDrConn))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    SqlDrConn.Open();
                    return cmd.ExecuteScalar();
                }

            }
            catch (Exception ex)
            {
                string tsMes = ex.Message;
                return 0;
            }
            finally
            {
                SqlDrConn.Close();
            }

        }
        //DataReader
        public SqlDataReader ExecuteReader(string StrSql, CommandType cmdType, params SqlParameter[] pms)
        {
            //当链接处于打开状态时关闭，然后再打开，避免有时候数据不能及时更新
            if (SqlDrConn.State == ConnectionState.Open)
            {
                SqlDrConn.Close();
            }
            try
            {

                using (SqlCommand cmd = new SqlCommand(StrSql, SqlDrConn))
                {
                    cmd.CommandType = cmdType;
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    SqlDrConn.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

            }
            catch (Exception ex)
            {
                string tsMes = ex.Message;
                return null;
            }           
        }       
        //关闭关联的连接池
        public void ClearRelevance(SqlConnection connection)
        {
           SqlConnection.ClearPool(connection);
        }
        //清空所有连接池
        public void ClearAllConPool()
        {
           SqlConnection.ClearAllPools();
        }
    }
}
