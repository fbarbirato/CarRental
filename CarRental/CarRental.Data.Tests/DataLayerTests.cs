using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Core;
using CarRental.Business.Bootstrapper;
using CarRental.Data.Contracts.Repository_Interfaces;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using CarRental.Business.Entities;
using Moq;
using Core.Common.Contracts;

namespace CarRental.Data.Tests
{
    [TestClass]
    public class DataLayerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [TestMethod]
        public void test_repository_usage()
        {
            var repositoryTest = new RepositoryTestClass();

            var cars = repositoryTest.GetCars();

            Assert.IsTrue(cars != null);
        }

        [TestMethod]
        public void test_repository_mocking()
        {
            var cars = new List<Car>()
            {
                new Car() { CarId = 1, Description = "Mustang"},
                new Car() { CarId = 2, Description = "Corvette"}
            };

            var mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(m => m.Get()).Returns(cars);

            var repositoryTest = new RepositoryTestClass(mockCarRepository.Object);

            var result = repositoryTest.GetCars();

            Assert.IsTrue(result == cars);
        }

        [TestMethod]
        public void test_repository_factory_usage()
        {
            var factoryTest = new RepositoryFactoryTestClass();

            var cars = factoryTest.GetCars();

            Assert.IsTrue(cars != null);
        }

        [TestMethod]
        public void test_repository_factory_mocking1()
        {
            var cars = new List<Car>()
            {
                new Car() { CarId = 1, Description = "Mustang"},
                new Car() { CarId = 2, Description = "Corvette"}
            };

            var mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockDataRepositoryFactory.Setup(m => m.GetDataRepository<ICarRepository>().Get()).Returns(cars);

            var repositoryFactoryTest = new RepositoryFactoryTestClass(mockDataRepositoryFactory.Object);

            var result = repositoryFactoryTest.GetCars();

            Assert.IsTrue(result == cars);
        }

        [TestMethod]
        public void test_repository_factory_mocking2()
        {
            var cars = new List<Car>()
            {
                new Car() { CarId = 1, Description = "Mustang"},
                new Car() { CarId = 2, Description = "Corvette"}
            };
            
            var mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(m => m.Get()).Returns(cars);

            var mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockDataRepositoryFactory.Setup(m => m.GetDataRepository<ICarRepository>()).Returns(mockCarRepository.Object);

            var repositoryFactoryTest = new RepositoryFactoryTestClass(mockDataRepositoryFactory.Object);

            var result = repositoryFactoryTest.GetCars();

            Assert.IsTrue(result == cars);
        }
    }

    public class RepositoryTestClass
    {
        public RepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this); // irá resolver a dependência de ICarRepository para CarRepository
        }

        public RepositoryTestClass(ICarRepository carRepository)
        {
            _CarRepository = carRepository; // utilizado para mock
        }

        [Import]
        ICarRepository _CarRepository;

        public IEnumerable<Car> GetCars()
        {
            var cars = _CarRepository.Get();

            return cars;
        }
    }

    public class RepositoryFactoryTestClass
    {
        public RepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public RepositoryFactoryTestClass(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        public IEnumerable<Car> GetCars()
        {
            var carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
            var cars = carRepository.Get();

            return cars;
        }
    }
}
