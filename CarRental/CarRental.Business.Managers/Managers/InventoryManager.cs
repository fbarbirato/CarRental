using CarRental.Business.Common;
using CarRental.Business.Contracts;
using CarRental.Business.Entities;
using CarRental.Common;
using CarRental.Data.Contracts.Repository_Interfaces;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, 
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public class InventoryManager : ManagerBase, IInventoryService
    {
        public InventoryManager()
        {
            
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public InventoryManager(IBusinessEngineFactory businessEngineFactory)
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;
       
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car GetCar(int carId)
        {
            return ExecuteFaultHandledOperation(() =>
            {

                var carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

                var carEntity = carRepository.Get(carId);

                if (carEntity == null)
                {
                    var ex = new NotFoundException(string.Format("Car with ID of {0} is not in the database.", carId));
                    throw new FaultException<NotFoundException>(ex, ex.Message); // wcf compatible
                }

                return carEntity;

            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car[] GetAllCars()
        {
            return ExecuteFaultHandledOperation(() =>
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
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public Car UpdateCar(Car car)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

                Car updatedEntity = null;

                if (car.CarId == 0)
                {
                    updatedEntity = carRepository.Add(car);
                }
                else
                {
                    updatedEntity = carRepository.Update(car);
                }

                return updatedEntity;

            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        public void DeleteCar(int carId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

                carRepository.Remove(carId);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CarRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CarRentalUser)]
        public Car[] GetAvailableCars(DateTime pickupDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();
                var rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                var reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                var carRentalEngine = _BusinessEngineFactory.GetBusinessEngine<ICarRentalEngine>();

                var allCars = carRepository.Get();
                var rentedCars = rentalRepository.GetCurrentlyRentedCars();
                var reservedCars = reservationRepository.Get();

                var availableCars = new List<Car>();

                foreach (var car in allCars)
                {
                    if (carRentalEngine.IsCarAvailableForRental(car.CarId, pickupDate, returnDate, rentedCars, reservedCars))
                    {
                        availableCars.Add(car);
                    }
                }

                return availableCars.ToArray();
            });
        }
    }
}
