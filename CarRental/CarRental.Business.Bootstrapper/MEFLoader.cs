using CarRental.Business.Business_Engines;
using CarRental.Data.Data_Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            var catalog = new AggregateCatalog();

            // Only needs one to discover all in the assembly
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(CarRentalEngine).Assembly));
            
            var container = new CompositionContainer(catalog);

            return container;
        }
    }
}
