using CarRental.Business.Contracts.Service_Contracts;
using CarRental.Business.Entities;
using CarRental.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Managers.Managers
{
    public class InventoryManager : IInventoryService
    {
        public InventoryManager()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;
        
        public Car GetCar(int carId)
        {
            try
            {
                var carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

                var carEntity = carRepository.Get(carId);

                if (carEntity == null)
                {
                    var ex = new NotFoundException(string.Format("Car with ID of {0} is not in the database.", carId));
                    throw new FaultException<NotFoundException>(ex, ex.Message); // wcf compatible
                }

                return carEntity;
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public Car[] GetAllCars()
        {
            try
            {
                var carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                var rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                var cars = carRepository.Get();
                var rentedCars = rentalRepository.GetCurrentlyRentedCars();

                foreach (var car in cars)
                {
                    var rentedCar = rentedCars.Where(item => item.CarId == car.CarId).FirstOrDefault();
                    car.CurrentlyRented = (rentedCar != null);
                }

                return cars.ToArray();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}
