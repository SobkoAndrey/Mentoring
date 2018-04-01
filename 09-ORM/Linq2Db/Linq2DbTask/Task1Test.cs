using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToDB.Data;
using Linq2DbTask.Entities;
using System.Linq;

namespace Linq2DbTask
{
    [TestClass]
    public class Task1Test
    {
        // Список продуктов с категорией и поставщиком
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new Northwind())
            {
                var query = db.Products.Select(_ => new { Product = _, Category = _.Category, Supplier = _.Supplier });
                var list = query.ToList();

                Assert.IsTrue(list.Count > 0);

                var firstProduct = db.Products.First();
                var category = db.Categories.FirstOrDefault(_ => _.Id == firstProduct.CategoryId);
                var supplier = db.Suppliers.FirstOrDefault(_ => _.Id == firstProduct.SupplierId);

                Assert.AreEqual(list.First().Product.Id, firstProduct.Id);
                Assert.AreEqual(list.First().Category.Id, category.Id);
                Assert.AreEqual(list.First().Supplier.Id, supplier.Id);

                var lastProduct = db.Products.OrderByDescending(_ => _.Id).First();
                var category2 = db.Categories.FirstOrDefault(_ => _.Id == lastProduct.CategoryId);
                var supplier2 = db.Suppliers.FirstOrDefault(_ => _.Id == lastProduct.SupplierId);

                Assert.AreEqual(list.Last().Product.Id, lastProduct.Id);
                Assert.AreEqual(list.Last().Category.Id, category2.Id);
                Assert.AreEqual(list.Last().Supplier.Id, supplier2.Id);
            }
        }

        // Список сотрудников с указанием региона, за который они отвечают
        [TestMethod]
        public void TestMethod2()
        {
            using (var db = new Northwind())
            {
                var query = db.Employees.Select(_ => new { Employee = _, Region = _.EmployeeTerritory.First().Territory.Region });
                var list = query.ToList();

                Assert.IsTrue(list.Count > 0);
            }
        }

        // Статистики по регионам: количества сотрудников по регионам
        [TestMethod]
        public void TestMethod3()
        {
            using (var db = new Northwind())
            {
                var query = db.Employees.GroupBy(_ => _.EmployeeTerritory.First().Territory.Region.RegionDescription).Select(_ => new {Region =  _.Key, count = _.Count()});
                var list = query.ToList();

                Assert.IsTrue(list.Count > 0);
            }
        }

        // Списка «сотрудник – с какими грузоперевозчиками работал» (на основе заказов)
        [TestMethod]
        public void TestMethod4()
        {
            using (var db = new Northwind())
            {
                LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

                var query2 = db.Orders.Select(_ => new { employee = _.Employee, shipper = _.Shipper })
                    .GroupBy(_ => _.employee.Id).Select(s => new { emlployee = s.FirstOrDefault().employee, shippers = s.Select(g => g.shipper) });

                var list = query2.ToList();

                Assert.IsTrue(list.Count > 0);
            }
        }
    }
}
