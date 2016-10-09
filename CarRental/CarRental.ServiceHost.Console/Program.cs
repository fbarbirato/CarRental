using CarRental.Business.Bootstrapper;
using CarRental.Business.Managers;
using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using SM = System.ServiceModel;

namespace CarRental.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var principal = new GenericPrincipal(new GenericIdentity("fbarb"), new string[] { "CarRentalAdmin" });
            Thread.CurrentPrincipal = principal;

            ObjectBase.Container = MEFLoader.Init();

            Console.WriteLine("Starting up services...");
            Console.WriteLine();

            var hostInventoryManager = new SM.ServiceHost(typeof(InventoryManager));
            var hostRentalManager = new SM.ServiceHost(typeof(RentalManager));
            var hostAccountManager = new SM.ServiceHost(typeof(AccountManager));

            StartService(hostInventoryManager, "InventoryManager");
            StartService(hostRentalManager, "RentalManager");
            StartService(hostAccountManager, "AccountManager");

            var timer = new System.Timers.Timer(10000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
                        
            Console.WriteLine("Reservation monitor started.");

            Console.WriteLine();
            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();

            timer.Stop();

            Console.WriteLine("Reservation monitor stopped.");

            StopService(hostInventoryManager, "InventoryManager");
            StopService(hostRentalManager, "RentalManager");
            StopService(hostAccountManager, "AccountManager");
        }

        private static void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CancelDeadReservations();
        }

        private static void CancelDeadReservations()
        {
            var rentalManager = new RentalManager();

            var reservations = rentalManager.GetDeadReservations();

            if (reservations != null)
            {
                foreach (var reservation in reservations)
                {
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            rentalManager.CancelReservation(reservation.ReservationId);
                            scope.Complete();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("There was an exception then attempting to cancel reservation '{0}'.", reservation.ReservationId);
                        }
                    }
                }
            }
        }

        static void StartService(SM.ServiceHost host, string serviceDescription)
        {
            host.Open();
            Console.WriteLine("Service {0} started.", serviceDescription);

            foreach (var endpoint in host.Description.Endpoints)
            {
                Console.WriteLine(string.Format("Listening on endpoint:"));
                Console.WriteLine(string.Format("Address: {0}", endpoint.Address.Uri));
                Console.WriteLine(string.Format("Binding: {0}", endpoint.Binding.Name));
                Console.WriteLine(string.Format("Contract: {0}", endpoint.Contract.Name));
            }

            Console.WriteLine();
        }

        static void StopService(SM.ServiceHost host, string serviceDescription)
        {
            host.Close();
            Console.WriteLine("Service {0} stopped.", serviceDescription);
        }
    }
}
