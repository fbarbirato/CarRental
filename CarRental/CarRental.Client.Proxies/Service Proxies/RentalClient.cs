using CarRental.Client.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CarRental.Client.Contracts.Data_Contracts;
using CarRental.Client.Entities;
using System.ComponentModel.Composition;

namespace CarRental.Client.Proxies.Service_Proxies
{
    [Export(typeof(IRentalService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalClient : ClientBase<IRentalService>, IRentalService
    {
        public void AcceptCarReturn(int carId)
        {
            Channel.AcceptCarReturn(carId);
        }

        public Task AcceptCarReturnAsync(int carId)
        {
            return Channel.AcceptCarReturnAsync(carId);
        }

        public void CancelReservation(int reservationId)
        {
            Channel.CancelReservation(reservationId);
        }

        public Task CancelReservationAsync(int reservationId)
        {
            return Channel.CancelReservationAsync(reservationId);
        }

        public void ExecuteRentalFromReservation(int reservationId)
        {
            Channel.ExecuteRentalFromReservation(reservationId);
        }

        public Task ExecuteRentalFromReservationAsync(int reservationId)
        {
            return Channel.ExecuteRentalFromReservationAsync(reservationId);
        }

        public CustomerRentalData[] GetCurrentRentals()
        {
            return Channel.GetCurrentRentals();
        }

        public Task<CustomerRentalData[]> GetCurrentRentalsAsync()
        {
            return Channel.GetCurrentRentalsAsync();
        }

        public CustomerReservationData[] GetCurrentReservations()
        {
            return Channel.GetCurrentReservations();
        }

        public Task<CustomerReservationData[]> GetCurrentReservationsAsync()
        {
            return Channel.GetCurrentReservationsAsync();
        }

        public CustomerReservationData[] GetCustomerReservations(string loginEmail)
        {
            return Channel.GetCustomerReservations(loginEmail);
        }

        public Task<CustomerReservationData[]> GetCustomerReservationsAsync(string loginEmail)
        {
            return Channel.GetCustomerReservationsAsync(loginEmail);
        }

        public Reservation[] GetDeadReservations()
        {
            return Channel.GetDeadReservations();
        }

        public Task<Reservation[]> GetDeadReservationsAsync()
        {
            return Channel.GetDeadReservationsAsync();
        }

        public Rental GetRental(int rentalId)
        {
            return Channel.GetRental(rentalId);
        }

        public Task<Rental> GetRentalAsync(int rentalId)
        {
            return Channel.GetRentalAsync(rentalId);
        }

        public IEnumerable<Rental> GetRentalHistory(string loginEmail)
        {
            return Channel.GetRentalHistory(loginEmail);
        }

        public Task<IEnumerable<Rental>> GetRentalHistoryAsync(string loginEmail)
        {
            return Channel.GetRentalHistoryAsync(loginEmail);
        }

        public Reservation GetReservation(int reservationId)
        {
            return Channel.GetReservation(reservationId);
        }

        public Task<Reservation> GetReservationAsync(int reservationId)
        {
            return Channel.GetReservationAsync(reservationId);
        }

        public bool IsCarCurrentlyRented(int carId)
        {
            return Channel.IsCarCurrentlyRented(carId);
        }

        public Task<bool> IsCarCurrentlyRentedAsync(int carId)
        {
            return Channel.IsCarCurrentlyRentedAsync(carId);
        }

        public Reservation MakeReservation(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return Channel.MakeReservation(loginEmail, carId, rentalDate, returnDate);
        }

        public Task<Reservation> MakeReservationAsync(string loginEmail, int carId, DateTime rentalDate, DateTime returnDate)
        {
            return Channel.MakeReservationAsync(loginEmail, carId, rentalDate, returnDate);
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime dateDueBack)
        {
            return Channel.RentCarToCustomer(loginEmail, carId, dateDueBack);
        }

        public Rental RentCarToCustomer(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            return Channel.RentCarToCustomer(loginEmail, carId, rentalDate, dateDueBack);
        }

        public Task<Rental> RentCarToCustomerAsync(string loginEmail, int carId, DateTime dateDueBack)
        {
            return Channel.RentCarToCustomerAsync(loginEmail, carId, dateDueBack);
        }

        public Task<Rental> RentCarToCustomerAsync(string loginEmail, int carId, DateTime rentalDate, DateTime dateDueBack)
        {
            return Channel.RentCarToCustomerAsync(loginEmail, carId, rentalDate, dateDueBack);
        }
    }
}
