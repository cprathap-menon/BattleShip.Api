using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Api.DependencyInjection
{
    public static class ServiceLocatorContainer
    {
        public static IServiceLocator CurrentLocator { get; private set; }

        public static void SetCurrent(IServiceLocator locator)
        {
            CurrentLocator = locator;
        }
    }
}
