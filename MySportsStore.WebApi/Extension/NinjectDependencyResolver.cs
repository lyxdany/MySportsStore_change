using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Ninject;

namespace MySportsStore.WebApi.Extension
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private List<IDisposable> disposableServices = new List<IDisposable>();
        public IKernel Kernel { get; private set; }

        public NinjectDependencyResolver(NinjectDependencyResolver parent)
        {
            this.Kernel = parent.Kernel;
        }

        public NinjectDependencyResolver()
        {
            this.Kernel = new StandardKernel();
        }

        public void Register<TFrom, TTO>() where TTO : TFrom
        {
            this.Kernel.Bind<TFrom>().To<TTO>();
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyResolver(this);
        }

        public object GetService(System.Type serviceType)
        {
            return this.Kernel.TryGet(serviceType);
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(System.Type serviceType)
        {
            foreach (var service in this.Kernel.GetAll(serviceType))
            {
                this.AddDisposableService(service);
                yield return service;
            }
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in disposableServices)
            {
                disposable.Dispose();
            }
        }

        private void AddDisposableService(object service)
        {
            IDisposable disposable = service as IDisposable;
            if (null != disposable && !disposableServices.Contains(disposable))
            {
                disposableServices.Add(disposable);
            }
        }
    }
}