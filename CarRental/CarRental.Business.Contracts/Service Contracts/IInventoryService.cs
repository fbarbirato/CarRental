using CarRental.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Contracts.Service_Contracts
{
    [ServiceContract]
    public interface IInventoryService
    {
        [OperationContract]
        Car GetCar(int carId);

        [OperationContract]
        Car[] GetAllCars();
    }
}
