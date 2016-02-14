using CarRental.Business.Entities;
using CarRental.Data.Contracts.DTOs;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Contracts.Repository_Interfaces
{
    public interface IReservationRepository : IDataRepository<Reservation>
    {
        IEnumerable<Reservation> GetReservationsByPickupDate(DateTime pickupDate);
        IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo();
        IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId);
    }
}
