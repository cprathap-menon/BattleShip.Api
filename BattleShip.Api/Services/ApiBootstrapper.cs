using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using BattleShip.Api.Exceptions;
using BattleShip.Api.Filters;
using BattleShip.Api.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BattleShip.Api.Services
{
    public interface IApiBootstrapper
    {
        void Bootstrap(ContainerBuilder containerBuilder,string containerId, string env);
    }
    public class ApiBootstrapper : IApiBootstrapper
    {
        public void Bootstrap(ContainerBuilder containerBuilder, string containerId, string env)
        {
            var options = new MemoryCacheOptions();
            var cache = new MemoryCache(options);
            containerBuilder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
            containerBuilder.Register((context, parameter) => cache).As<IMemoryCache>().SingleInstance();
            containerBuilder.RegisterType<ExceptionHandler>().As<IExceptionBuilder>().As<IExceptionHandler>().As<IAsyncExceptionFilter>().SingleInstance();
            containerBuilder.RegisterType<BoardService>().As<IBoardService>();
            containerBuilder.RegisterType<GameService>().As<IGameService>();

        }
    }
}
