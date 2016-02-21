using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Business.Entities;
using Moq;
using Core.Common.Contracts;
using CarRental.Data.Contracts.Repository_Interfaces;
using System.Security.Principal;
using System.Threading;

namespace CarRental.Business.Managers.Tests
{
    [TestClass]
    public class InventoryManagerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            var principal = new GenericPrincipal(
                new GenericIdentity("Felipe"), new string[] { "CarRentalAdmin" });

            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void UpdateCar_add_new()
        {
            var newCar = new Car();
            var addedCar = new Car() { CarId = 1 };

            var mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(obj => obj.GetDataRepository<ICarRepository>().Add(newCar)).Returns(addedCar);

            var manager = new InventoryManager(mockRepositoryFactory.Object);

            var results = manager.UpdateCar(newCar);
            
            Assert.IsTrue(results == addedCar);
        }

        [TestMethod]
        public void UpdateCar_update_existing()
        {
            Car existingCar = new Car() { CarId = 1 };
            Car updatedCar = new Car() { CarId = 1 };

            var mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(obj => obj.GetDataRepository<ICarRepository>().Update(existingCar)).Returns(updatedCar);

            var manager = new InventoryManager(mockRepositoryFactory.Object);

            var results = manager.UpdateCar(existingCar);

            Assert.IsTrue(results == updatedCar);
        }
    }
}
