using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using Task.TestHelpers;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using Task.Surrogates;

namespace Task
{
	[TestClass]
	public class SerializationSolutions
	{
		Northwind dbContext;

		[TestInitialize]
		public void Initialize()
		{
			dbContext = new Northwind();
		}

		[TestMethod]
		public void SerializationCallbacks()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Category>>(new NetDataContractSerializer(), true);
            // Тут не было подключения продуктов, поэтому сериализовались категории без продуктов. Подключил их и теперь все сериализуется и десериализуется нормально, зачем юзать колбэки и что в них делать - непонятно
			var categories = dbContext.Categories.Include("Products").ToList();

			var c = categories.First();

            var result = tester.SerializeAndDeserialize(categories);
		}

		[TestMethod]
		public void ISerializable()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(new NetDataContractSerializer(), true);
            // То же самое, подключил связанные сущности и все заработало, зачем мне ISerializable - непонятно
			var products = dbContext.Products.Include("Supplier").Include("Category").Include("Order_Details").ToList();
            
			var result = tester.SerializeAndDeserialize(products);
		}


		[TestMethod]
		public void ISerializationSurrogate()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(new NetDataContractSerializer(), true);
            // Все то же самое
			var orderDetails = dbContext.Order_Details.Include("Order").Include("Product").ToList();

            var result = tester.SerializeAndDeserialize(orderDetails);
		}

		[TestMethod]
		public void IDataContractSurrogate()
		{
			dbContext.Configuration.ProxyCreationEnabled = true;
			dbContext.Configuration.LazyLoadingEnabled = true;

            // А вот здесь все нормуль, написал суррогат через который прокси классы, сгенеренные lazy loading подменяются на сущности.
            var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(new DataContractSerializer(typeof(IEnumerable<Order>),
                new DataContractSerializerSettings
                {
                    DataContractSurrogate = new OrderSurrogate()
                }), true);

			var orders = dbContext.Orders.ToList();

            var result = tester.SerializeAndDeserialize(orders);
		}
	}
}
