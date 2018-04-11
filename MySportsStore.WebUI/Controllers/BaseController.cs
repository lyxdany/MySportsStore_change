using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MySportsStore.WebUI.Controllers
{
    public class BaseController : Controller
    {
        //定义一种释放分配的资源的方法。
        protected IList<IDisposable> DisposableObjects { get; private set; }

        public BaseController()
        {
            this.DisposableObjects = new List<IDisposable>();
        }

        //添加资源
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
    }
}


