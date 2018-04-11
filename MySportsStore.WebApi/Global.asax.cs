using System.Web.Http;
using MySportsStore.BLL;
using MySportsStore.IBLL;
using MySportsStore.WebApi.Extension;

namespace MySportsStore.WebApi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            

            NinjectDependencyResolver dependencyResolver = new NinjectDependencyResolver();
            dependencyResolver.Register<IProductService, ProductService>();
            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;
        }
    }
}