using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.MyManagers;
using CachingSolutionsSamples.MyCacheImplementations;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
		[TestMethod]
		public void MemoryCache()
		{
			var categoryManager = new CategoriesManager(new CategoriesMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void RedisCache()
		{
			var categoryManager = new CategoriesManager(new CategoriesRedisCache("localhost"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

        [TestMethod]
        public void ProductsMemoryCacheTest()
        {
            var productsManager = new ProductsManager(new ProductsMemoryCache());

            var data = productsManager.GetProducts();

            Thread.Sleep(1000);
            var data2 = productsManager.GetProducts();
            Thread.Sleep(21000);
            var data3 = productsManager.GetProducts();
        }

        [TestMethod]
        public void ShippersMemoryCacheTest()
        {
            var shippersManager = new ShippersManager(new ShippersMemoryCache());

            var data = shippersManager.GetShippers();

            Thread.Sleep(1000);
            shippersManager.Add("Test");
            Thread.Sleep(5000);
            var data2 = shippersManager.GetShippers();
            Thread.Sleep(1000);
            var data3 = shippersManager.GetShippers();
        }
    }
}
