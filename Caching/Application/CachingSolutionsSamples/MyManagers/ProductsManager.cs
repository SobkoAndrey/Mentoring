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
    public class ProductsManager
    {
        private IProductsCache cache;

        public ProductsManager(IProductsCache cache)
        {
            this.cache = cache;
        }

        public IEnumerable<Product> GetProducts()
        {
            Console.WriteLine("Get Products");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var products = cache.Get(user);

            if (products == null)
            {
                Console.WriteLine("From DB");

                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    products = dbContext.Products.ToList();
                    cache.Set(user, products);
                }
            }
            else
            {
                Console.WriteLine("From cache");
            }

            return products;
        }
    }
}
