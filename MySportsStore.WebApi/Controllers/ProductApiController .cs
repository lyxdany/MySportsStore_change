using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MySportsStore.IBLL;
using MySportsStore.Model;
using Ninject;

namespace MySportsStore.WebApi.Controllers
{
    public class ProductApiController : ApiController
    {
        [Inject]
        public IProductService ProductService { get; set; }

        public ProductApiController()
        {
            this.DisposableObjects = new List<IDisposable>();
            this.AddDisposableObject(ProductService);
        }

        // GET api/productapi
        public IEnumerable<Product> Get()
        {
            return ProductService.LoadEntities(p => true).AsEnumerable();
        }

        // GET api/productapi/5
        public Product Get(int id)
        {
            return ProductService.LoadEntities(p => p.Id == id).FirstOrDefault();
        }

        // POST api/productapi
        public void Post(Product product)
        {
            var dbProduct = ProductService.LoadEntities(p => p.Id == product.Id).FirstOrDefault();
            ProductService.UpdateEntity(dbProduct);
        }

        // PUT api/productapi/5
        public void Put(Product product)
        {
            ProductService.AddEntity(product);
        }

        // DELETE api/productapi/5
        public void Delete(int id)
        {
            var product = ProductService.LoadEntities(p => p.Id == id).FirstOrDefault();
            ProductService.DeleteEntity(product);
        }

        #region 手动垃圾回收逻辑
        protected IList<IDisposable> DisposableObjects { get; private set; }

        protected void AddDisposableObject(object obj)
        {
            IDisposable disposable = obj as IDisposable;
            if (disposable != null)
            {
                this.DisposableObjects.Add(disposable);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (IDisposable obj in this.DisposableObjects)
                {
                    if (null != obj)
                    {
                        obj.Dispose();
                    }
                }
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}