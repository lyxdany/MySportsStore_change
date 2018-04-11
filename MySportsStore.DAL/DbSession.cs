using System.Data.Entity;
using MySportsStore.IDAL;
using System;

namespace MySportsStore.DAL
{
    public class DbSession : IDbSession
    {
        // 注册所有的类
        private IProductRepository _ProductRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (_ProductRepository == null)
                {
                    _ProductRepository = new ProductRepository();
                }
                return _ProductRepository;
            }
            set { _ProductRepository = value; }
        }

        private IUserInfoRepository _UserInfoRepository;
        public IUserInfoRepository UserInfoRepository
        {
            get
            {
                if (_UserInfoRepository == null)
                {
                    _UserInfoRepository = new UserInfoRepository();
                }
                return _UserInfoRepository;
            }
            set { _UserInfoRepository = value; }
        }

        public int SaveChanges()
        {
            IDbContextFactory dbFactory = new DbContextFactory();
            DbContext db = dbFactory.GetCurrentThreadInstance();
            try
            {
                return db.SaveChanges();
            }
            catch (Exception ex)
            {
               
                throw(ex);
            }

        }

        public int ExeucteSql(string sql, params System.Data.SqlClient.SqlParameter[] paras)
        {
            IDbContextFactory dbFactory = new DbContextFactory();
            DbContext db = dbFactory.GetCurrentThreadInstance();
            return db.Database.ExecuteSqlCommand(sql, paras);
        }
    }
}