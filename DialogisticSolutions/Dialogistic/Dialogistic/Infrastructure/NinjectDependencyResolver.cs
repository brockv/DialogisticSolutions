﻿using Dialogistic.Abstract;
using Dialogistic.Concrete;
using Dialogistic.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Dialogistic.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
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
            //kernel.Bind<IRepository>().To<Repository>();
            kernel.Bind<IDialogisticRepository<Constituent>>().To<DialogisticRepository<Constituent>>();
            kernel.Bind<IDialogisticRepository<ProposedConstituentsChanges>>().To<DialogisticRepository<ProposedConstituentsChanges>>();
            kernel.Bind<IDialogisticRepository<CallAssignment>>().To<DialogisticRepository<CallAssignment>>();
        }
    }
}