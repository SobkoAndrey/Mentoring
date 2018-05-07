using CachingSolutionsSamples.MyInterfaces;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.MyCacheImplementations
{
    class ShippersMemoryCache : IShippersCache
    {
        ObjectCache cache = MemoryCache.Default;
        string prefix = "Cache_Shippers";

        public IEnumerable<Shipper> Get(string forUser)
        {
            return (IEnumerable<Shipper>)cache.Get(prefix + forUser);
        }

        public void Set(string forUser, IEnumerable<Shipper> shippers)
        {
            var policy = new CacheItemPolicy();
            var item = ConfigurationManager.ConnectionStrings["Northwind"];
            var connectionString = item.ConnectionString;
            //var conString = "Data Source = localhost; Initial Catalog = Northwind; Integrated Security = True";
            SqlDependency.Start(connectionString);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("select ShipperID from dbo.Shippers", conn))
                {
                    command.Notification = null;
                    command.CommandType = System.Data.CommandType.Text;

                    SqlDependency dep = new SqlDependency();

                    dep.AddCommandDependency(command);

                    conn.Open();

                    using (var reader = command.ExecuteReader())
                    {
                    }

                    SqlChangeMonitor monitor = new SqlChangeMonitor(dep);

                    policy.ChangeMonitors.Add(monitor);
                }
            }

            //policy.ChangeMonitors.Add(new SqlChangeMonitor(new SqlDependency(new SqlCommand("select * from Shippers"))));

            cache.Set(prefix + forUser, shippers, policy);
        }
    }
}
