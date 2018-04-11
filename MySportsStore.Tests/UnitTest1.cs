using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySportsStore.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace MySportsStore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private static MD5 md5 = MD5.Create();

        
        public void TestMethod1()
        {
            //Md5Hash()
            //SqlParameter[] ps = new SqlParameter[] { };

            //string sql = "select name,Description,price,Category  from Products";

            //DataTable mytable = SQLHelper.GetDataTable(sql, ps);

            ////for (int i = 0; i < mytable.Rows.Count; i++)
            ////{
            ////    Console.WriteLine("{0},{1},{2},{3}", mytable.Rows[i][0], mytable.Rows[i][1], mytable.Rows[i][2], mytable.Rows[i][3]);
            ////}
            //string[] headers = new string[] {
            //    "name",
            //    "Description",
            //    "price",
            //    "Category"
            //   };
            //string filename = "test";
            //return new BaseExcelResult(mytable, headers.ToList(), filename, 1);

            //int i = 6;
            //string s = "输出";
            //Console.WriteLine("{0}还可以这样{1}哦。", i, s);
            //Console.WriteLine("{1}：而且{0}参数的顺序和使用次数都不固定哦。{1}", i, s);
        }

        public static string GetMD5HashString(Encoding encode, string sourceStr)
        {
            StringBuilder sb = new StringBuilder();

            byte[] source = md5.ComputeHash(encode.GetBytes(sourceStr));
            for (int i = 0; i < source.Length; i++)
            {
                sb.Append(source[i].ToString("x2"));
            }

            return sb.ToString();
        }
        [TestMethod]
        public void ButtonRegister_Click()
        {
            string username = "sdfdf";
            string password = "pwd";
            // random salt 
            string salt = Guid.NewGuid().ToString();

            // random salt 
            // you can also use RNGCryptoServiceProvider class            
            //System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider(); 
            //byte[] saltBytes = new byte[36]; 
            //rng.GetBytes(saltBytes); 
            //string salt = Convert.ToBase64String(saltBytes); 
            //string salt = ToHexString(saltBytes); 

            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);

            string hashString = Convert.ToBase64String(hashBytes);

            Console.WriteLine(hashString);
            Console.WriteLine(salt);
            // you can also use ToHexString to convert byte[] to string 
            //string hashString = ToHexString(hashBytes); 

            //连接数据库
            //var db = new TestEntities();
            //usercredential newRecord = usercredential.Createusercredential(username, hashString, salt);
            //db.usercredentials.AddObject(newRecord);
            //db.SaveChanges();
        }


        public void check_pwd()
        {
            string username = "dfd";
            string password = "fdfd";

            //var db = new TestEntities();
            //usercredential record = db.usercredentials.Where(x => string.Compare(x.UserName, username, true) == 0).FirstOrDefault();
            //if (record == default(usercredential))
            //{
            //    throw new ApplicationException("invalid user name and password");
            //}

            //string salt = "dfdfdl";   //record.Salt;
            //byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
            //byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            //string hashString = Convert.ToBase64String(hashBytes);

            //if (hashString == record.PasswordHash)
            //{
            //    // user login successfully 
            //}
            //else
            //{
            //    throw new ApplicationException("invalid user name and password");
            //} 
        }

        
    }
}
