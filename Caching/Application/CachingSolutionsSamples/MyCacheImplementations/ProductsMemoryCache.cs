using CachingSolutionsSamples.MyInterfaces;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.MyCacheImplementations
{
    class ProductsMemoryCache : IProductsCache
    {
        ObjectCache cache = MemoryCache.Default;
        string prefix = "Cache_Products";

        public IEnumerable<Product> Get(string forUser)
        {
            return (IEnumerable<Product>)cache.Get(prefix + forUser);
        }

        public void Set(string forUser, IEnumerable<Product> products)
        {
            cache.Set(prefix + forUser, products, new DateTimeOffset(DateTime.UtcNow.AddSeconds(20)));
        }
    }
}
