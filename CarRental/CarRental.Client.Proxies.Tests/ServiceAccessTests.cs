using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Client.Proxies.Service_Proxies;

namespace CarRental.Client.Proxies.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void test_inventory_client_connection()
        {
            var proxy = new InventoryClient();

            proxy.Open();
        }

        [TestMethod]
        public void test_account_client_connection()
        {
            var proxy = new AccountClient();

            proxy.Open();
        }

        [TestMethod]
        public void test_rental_client_connection()
        {
            var proxy = new RentalClient();

            proxy.Open();
        }
    }
}
