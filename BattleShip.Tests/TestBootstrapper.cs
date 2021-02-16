using Autofac;
using BattleShip.Api.DependencyInjection;
using BattleShip.Api.Models;
using BattleShip.Api.Services;
using NUnit.Framework;

namespace BattleShip.Tests
{
    [SetUpFixture]
    public class TestBootstrapper
    {
        /// <summary>
        /// This method is called first as the test project runs and it initializes all the services, this is only called once as the project starts.
        /// </summary>
        [OneTimeSetUp]
        public void Bootstrap()
        {
            // Register dependencies
            var builder = new ContainerBuilder();

            new ApiBootstrapper().Bootstrap(builder, "N/A", "UnitTests");
            var container = builder.Build();
            ServiceLocatorContainer.SetCurrent(new ServiceLocator(container));
        }
    }
}