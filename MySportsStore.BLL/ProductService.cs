using MySportsStore.IBLL;
using MySportsStore.Model;

namespace MySportsStore.BLL
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService():base(){}

        public override bool SetCurrentRepository()
        {
            //若是其他XXXservice对应的就是DbSessionContext.xxxRepository
            this.CurrentRepository = DbSessionContext.ProductRepository;
            this.AddDisposableObject(this.CurrentRepository);
            return true;
        }
    }
}