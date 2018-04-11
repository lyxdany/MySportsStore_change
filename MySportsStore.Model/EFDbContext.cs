using System.Data.Entity;

namespace MySportsStore.Model
{
    public class EfDbContext : DbContext 
    {
        public EfDbContext()
            : base("conn") 
        {
            // 自定义创建数据库，其中类EfDbInitializer : CreateDatabaseIfNotExists<EfDbContext>
            Database.SetInitializer<EfDbContext>(null);
            //数据库不存在时重新创建数据库
            //Database.SetInitializer(new CreateDatabaseIfNotExists<EfDbContext>());
            //模型更改时重新创建数据库
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EfDbContext>());
            //每次启动应用程序时创建数据库
            //Database.SetInitializer(new DropCreateDatabaseAlways<EfDbContext>());           
        }
         public DbSet<Product> Products { get; set; }
         public DbSet<UserInfo> UserInfoes { get; set; }

    }
}