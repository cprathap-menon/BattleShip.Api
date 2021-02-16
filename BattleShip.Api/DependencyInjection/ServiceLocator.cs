using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace BattleShip.Api.DependencyInjection
{
    public interface IServiceLocator
    {
        TService Resolve<TService>();
        object GetContainer();
    }
    public class ServiceLocator : IServiceLocator
    {
        private readonly ILifetimeScope _container;

        public ServiceLocator(ILifetimeScope container)
        {
            _container = container;
        }

        public TService Resolve<TService>()
        {
            var service = _container.Resolve<TService>();
            return service;
        }

        public object GetContainer()
        {
            return _container;
        }
    }
}
