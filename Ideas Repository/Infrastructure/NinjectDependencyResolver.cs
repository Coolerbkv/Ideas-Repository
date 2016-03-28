using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System.Configuration;
using Ideas_Repository.BusinessLogic;
using Ideas_Repository.Abstract;

namespace Ideas_Repository.Infrastructure
{
  public class NinjectDependencyResolver : IDependencyResolver
  {
    private IKernel kernel;
    public NinjectDependencyResolver()
    {
      kernel = new StandardKernel();
      AddBindings();
    }
    public object GetService(Type serviceType)
    {
      return kernel.TryGet(serviceType);
    }
    public IEnumerable<object> GetServices(Type serviceType)
    {
      return kernel.GetAll(serviceType);
    }
    private void AddBindings()
    {
        kernel.Bind<IIdeasRepositoryBusinessLogic>().To<DataManager>();
    }
  }
}