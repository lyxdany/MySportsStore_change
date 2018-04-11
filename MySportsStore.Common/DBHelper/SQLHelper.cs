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
       /// ��ȡ�����ļ��еģ������ַ���
       /// </summary>
      public static readonly string connectionString = DBConnect.ConnString;

       /// <summary>
       /// ִ�зǲ�ѯ����(����ɾ����)
       /// </summary>
       /// <param name="commandText">T-SQL���</param>
       /// <param name="paras">����</param>
       /// <returns>��Ӱ�����</returns>
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
       /// ִ�зǲ�ѯ����(����ɾ����)
       /// </summary>
       /// <param name="commandText">T-SQL���</param>
       /// <param name="type">�������ͣ�����Ϊ�洢����</param>
       /// <param name="paras">����</param>
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
       /// ����һ�����ݵĲ�ѯ
       /// </summary>
       /// <param name="commandText">T-SQL���</param>
       /// <param name="paras">����</param>
       /// <returns>���ص�һ�е�һ������</returns>
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
       /// ����һ�����ݵĲ�ѯ
       /// </summary>
       /// <param name="commandText">T-SQL���</param>
       /// <param name="type">�����Ǵ洢����</param>
       /// <param name="paras">����</param>
       /// <returns>���ص�һ�е�һ������</returns>
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
       /// ��ѯ��ȡ���ݱ�
       /// </summary>
       /// <param name="commandText"></param>
       /// <param name="paras"></param>
       /// <returns>DataTable ���ݱ�</returns>
       public static DataTable GetDataTable(string commandText, params SqlParameter[] paras) 
       {
           DataTable dt = new DataTable();
           //ʹ��DataAdapter������ݱ�
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
       /// ��ѯ��ȡ���ݱ�
       /// </summary>
       /// <param name="commandText">T-SQL���</param>
       /// <param name="type">��������</param>
       /// <param name="paras">����</param>
       /// <returns></returns>
       public static DataTable GetDataTable(string commandText,CommandType type, params SqlParameter[] paras)
       {
           DataTable dt = new DataTable();
           //ʹ��DataAdapter������ݱ�
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
       /// ִ�ж���SQL��䣬ʵ�����ݿ�����
       /// </summary>
       /// <param name="SQLStringList">SQL���Ĺ�ϣ��keyΪsql��䣬value�Ǹ�����SqlParameter[]��</param>
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
                           //ѭ��
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
