using System.Configuration;

namespace MySportsStore.Common
{
    /// <summary>
    /// 获取数据库连接字符串，最好要加密该字符串、然后解密
    /// </summary>
  public static class DBConnect
    {
      public static string ConnString 
      {
          get 
          {

              string _connectionString = ConfigurationManager.AppSettings["ConnStr"];       
                
                return _connectionString; 
            
          }
      }

    }
}
