using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Business.Entities;
using CarRental.Data.Contracts.Repository_Interfaces;
using Moq;
using Core.Common.Contracts;
using CarRental.Business.Business_Engines;

namespace CarRental.Business.Tests
{
    [TestClass]
    public class CarRentalEngineTests
    {
        [TestMethod]
        public void IsCarCurrentlyRented_any_account()
        {
            var rental = new Rental()
            {
                CarId = 1
            };

            var mockRentalRepository = new Mock<IRentalRepository>();
            mockRentalRepository.Setup(obj => obj.GetCurrentRentalByCar(1)).Returns(rental);

            var mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory.Setup(obj => obj.GetDataRepository<IRentalRepository>()).Returns(mockRentalRepository.Object);

            var engine = new CarRentalEngine(mockRepositoryFactory.Object);

            bool try1 = engine.IsCarCurrentlyRented(2);
            bool try2 = engine.IsCarCurrentlyRented(1);

            Assert.IsFalse(try1);
            Assert.IsTrue(try2);
        }
    }
}
