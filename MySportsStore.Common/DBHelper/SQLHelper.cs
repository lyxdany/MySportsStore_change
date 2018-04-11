using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace MySportsStore.Common
{
   public static class SQLHelper
    {
        /// <summary>
       /// 获取配置文件中的，连接字符串
       /// </summary>
      public static readonly string connectionString = DBConnect.ConnString;

       /// <summary>
       /// 执行非查询操作(增、删、改)
       /// </summary>
       /// <param name="commandText">T-SQL语句</param>
       /// <param name="paras">参数</param>
       /// <returns>受影响的行</returns>
       public static int ExecuteNoneQuery(string commandText,params SqlParameter[] paras) 
       {
           using (SqlConnection conn = new SqlConnection(connectionString)) 
           {
               using (SqlCommand cmd = new SqlCommand(commandText, conn)) 
               {
                   if (paras != null && paras.Length != 0) 
                   {
                       cmd.Parameters.AddRange(paras);
                   }
                   if (conn.State == ConnectionState.Closed) 
                   {
                       conn.Open();
                   }
                   return cmd.ExecuteNonQuery();
               }
           }

       }
       /// <summary>
       /// 执行非查询操作(增、删、改)
       /// </summary>
       /// <param name="commandText">T-SQL语句</param>
       /// <param name="type">命令类型，可以为存储过程</param>
       /// <param name="paras">参数</param>
       /// <returns></returns>
       public static int ExecuteNoneQuery(string commandText, CommandType type, params SqlParameter[] paras) 
       {
           using (SqlConnection conn = new SqlConnection(connectionString))
           {
               using (SqlCommand cmd = new SqlCommand(commandText, conn))
               {
                   cmd.CommandType = type;

                   if (paras != null && paras.Length != 0)
                   {
                       cmd.Parameters.AddRange(paras);
                   }
                   if (conn.State == ConnectionState.Closed)
                   {
                       conn.Open();
                   }
                   return cmd.ExecuteNonQuery();
               }
           }
       }

       /// <summary>
       /// 返回一个数据的查询
       /// </summary>
       /// <param name="commandText">T-SQL语句</param>
       /// <param name="paras">参数</param>
       /// <returns>返回第一行第一列数据</returns>
       public static object ExecuteScalar(string commandText, params SqlParameter[] paras) 
       {
           using (SqlConnection conn = new SqlConnection(connectionString))
           {
               using (SqlCommand cmd = new SqlCommand(commandText, conn))
               {
                   if (paras != null && paras.Length != 0)
                   {
                       cmd.Parameters.AddRange(paras);
                   }
                   if (conn.State == ConnectionState.Closed)
                   {
                       conn.Open();
                   }
                   return cmd.ExecuteScalar();
               }
           }
       }
       /// <summary>
       /// 返回一个数据的查询
       /// </summary>
       /// <param name="commandText">T-SQL语句</param>
       /// <param name="type">可以是存储过程</param>
       /// <param name="paras">参数</param>
       /// <returns>返回第一行第一列数据</returns>
       public static object ExecuteScalar(string commandText,CommandType type, params SqlParameter[] paras)
       {
           using (SqlConnection conn = new SqlConnection(connectionString))
           {
               using (SqlCommand cmd = new SqlCommand(commandText, conn))
               {
                   cmd.CommandType = type;

                   if (paras != null && paras.Length != 0)
                   {
                       cmd.Parameters.AddRange(paras);
                   }
                   if (conn.State == ConnectionState.Closed)
                   {
                       conn.Open();
                   }
                   return cmd.ExecuteScalar();
               }
           }
       }

       /// <summary>
       /// 查询获取数据表
       /// </summary>
       /// <param name="commandText"></param>
       /// <param name="paras"></param>
       /// <returns>DataTable 数据表</returns>
       public static DataTable GetDataTable(string commandText, params SqlParameter[] paras) 
       {
           DataTable dt = new DataTable();
           //使用DataAdapter填充数据表
           using (SqlDataAdapter da = new SqlDataAdapter(commandText, connectionString)) 
           {
               if (paras != null && paras.Length != 0)
               {
                   da.SelectCommand.Parameters.AddRange(paras);
               }

               da.Fill(dt);
           }
           return dt;
       }
       /// <summary>
       /// 查询获取数据表
       /// </summary>
       /// <param name="commandText">T-SQL语句</param>
       /// <param name="type">命令类型</param>
       /// <param name="paras">参数</param>
       /// <returns></returns>
       public static DataTable GetDataTable(string commandText,CommandType type, params SqlParameter[] paras)
       {
           DataTable dt = new DataTable();
           //使用DataAdapter填充数据表
           using (SqlDataAdapter da = new SqlDataAdapter(commandText, connectionString))
           {
               da.SelectCommand.CommandType = type;

               if (paras != null && paras.Length != 0)
               {
                   da.SelectCommand.Parameters.AddRange(paras);
               }

               da.Fill(dt);
           }
           return dt;
       }

       /// <summary>
       /// 执行多条SQL语句，实现数据库事务。
       /// </summary>
       /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
       public static void ExecuteSqlTran(Hashtable SQLStringList)
       {
           using (SqlConnection conn = new SqlConnection(connectionString))
           {
               if (conn.State == ConnectionState.Closed)
               {
                   conn.Open();
               }

               using (SqlTransaction trans = conn.BeginTransaction())
               {
                   using (SqlCommand cmd = new SqlCommand())
                   {
                       try
                       {
                           //循环
                           foreach (DictionaryEntry myDE in SQLStringList)
                           {
                               string cmdText = myDE.Key.ToString();
                               SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                               PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                               int val = cmd.ExecuteNonQuery();
                               cmd.Parameters.Clear();
                           }
                           trans.Commit();
                       }
                       catch (Exception ex)
                       {

                           trans.Rollback();
                           throw ex;
                       }
                   }
               }
           }
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="cmd"></param>
       /// <param name="conn"></param>
       /// <param name="trans"></param>
       /// <param name="cmdText"></param>
       /// <param name="cmdParms"></param>
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
