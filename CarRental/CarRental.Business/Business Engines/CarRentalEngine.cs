using CarRental.Business.Common;
using CarRental.Business.Entities;
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using CarRental.Data.Contracts.Repository_Interfaces;
using Core.Common.Exceptions;

namespace CarRental.Business.Business_Engines
{
    [Export(typeof(ICarRentalEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CarRentalEngine : ICarRentalEngine
    {
        [ImportingConstructor]
        public CarRentalEngine(IDataRepositoryFactory dataRepositoryFactory)
        {

        }

        IDataRepositoryFactory _DataRepositoryFactory;

        public bool IsCarCurrentlyRented(int carId)
        {
            var rented = false;

            var rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            var currentRental = rentalRepository.GetCurrentRentalByCar(carId);
            if (currentRental != null)
            {
                rented = true;
            }

            return rented;
        }

        public bool IsCarCurrentlyRented(int carId, int accountId)
        {
            var rented = false;

            var rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            var currentRental = rentalRepository.GetCurrentRentalByCar(carId);
            if (currentRental != null && currentRental.AccountId == accountId)
            {
                rented = true;
            }

            return rented;
        }

        public bool IsCarAvailableForRental(int carId, DateTime pickupDate, DateTime returnDate,
            IEnumerable<Rental> rentedCars, IEnumerable<Reservation> reservedCars)
        {
            var available = true;

            var reservation = reservedCars.Where(item => item.CarId == carId).FirstOrDefault();

            if (reservation != null && (
                (pickupDate >= reservation.RentalDate && pickupDate <= reservation.ReturnDate) ||
                (returnDate >= reservation.RentalDate && returnDate <= reservation.ReturnDate)))
            {
                available = false;
            }

            if (available)
            {
                var rental = rentedCars.Where(item => item.CarId == carId).FirstOrDefault();
                if (rental != null && (pickupDate <= rental.DateDue))
                {
                    available = false;
                }
            }

            return available;
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            if (rentalDate > DateTime.Now)
            {
                throw new UnableToRentForDateException(string.Format("Cannot rent for date {0}", rentalDate));
            }

            var accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            var rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            var carIsRented = IsCarCurrentlyRented(carId);

            var account = accountRepository.GetByLogin(loginEmail);
            if (account == null)
            {
                throw new NotFoundException(string.Format("no account found for login email {0}", loginEmail));
            }

            var rental = new Rental()
            {
                AccountId = account.AccountId,
                CarId = carId,
                DateRented = rentalDate,
                DateDue = dateDueBack
            };

            var savedEntity = rentalRepository.Add(rental);

            return savedEntity;
        }
    }
}
