using CachingSolutionsSamples.MyInterfaces;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.MyManagers
{
    public class ShippersManager
    {
        private IShippersCache cache;

        public ShippersManager(IShippersCache cache)
        {
            this.cache = cache;
        }

        public IEnumerable<Shipper> GetShippers()
        {
            Console.WriteLine("Get Shippers");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var shippers = cache.Get(user);

            if (shippers == null)
            {
                Console.WriteLine("From DB");

                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    shippers = dbContext.Shippers.ToList();
                    cache.Set(user, shippers);
                }
            }
            else
            {
                Console.WriteLine("From cache");
            }

            return shippers;
        }

        public void Add(string name)
        {
            using (var dbContext = new Northwind())
            {
                dbContext.Shippers.Add(new Shipper() { CompanyName = name });
                dbContext.SaveChanges();
            }
        }
    }
}
