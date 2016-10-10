using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Core;
using CarRental.Client.Bootstrapper;
using CarRental.Client.Contracts;
using CarRental.Client.Proxies.Service_Proxies;
using Core.Common.Contracts;

namespace CarRental.Client.Proxies.Tests
{
    [TestClass]
    public class ProxyObtainmentTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [TestMethod]
        public void obtain_proxy_from_container_using_service_contract()
        {
            var proxy = ObjectBase.Container.GetExportedValue<IInventoryService>();

            Assert.IsTrue(proxy is InventoryClient);
        }

        [TestMethod]
        public void obtain_proxy_from_service_factory()
        {
            var factory = new ServiceFactory();

            var proxy = factory.CreateClient<IInventoryService>();

            Assert.IsTrue(proxy is InventoryClient);
        }

        [TestMethod]
        public void obtain_service_factory_and_proxy_from_container()
        {
            var factory = ObjectBase.Container.GetExportedValue<IServiceFactory>();

            var proxy = factory.CreateClient<IInventoryService>();

            Assert.IsTrue(proxy is InventoryClient);
        }
    }
}
