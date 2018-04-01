using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqToDB;
using Linq2DbTask.Entities;
using System.Linq;
using System.Collections.Generic;
using System.Data.Linq;
using LinqToDB.Data;

namespace Linq2DbTask
{
    [TestClass]
    public class Task2Test
    {
        // Добавить нового сотрудника, и указать ему список территорий, за которые он несет ответственность. 
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new Northwind())
            {
                LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
                db.BeginTransaction();

                var newEmployee = new Employee();

                newEmployee.FirstName = "Andrey";
                newEmployee.LastName = "Sobko";
                db.Insert(newEmployee);

                var employeeId = db.Employees.First(_ => _.FirstName == "Andrey" && _.LastName == "Sobko").Id;

                var firstTerr = db.Territories.First().Id;
                var secondTerr = db.Territories.Skip(8).First().Id;

                var empTer1 = new EmployeeTerritory();
                empTer1.EmployeeId = employeeId;
                empTer1.TerritoryId = firstTerr;

                var empTer2 = new EmployeeTerritory();
                empTer2.EmployeeId = employeeId;
                empTer2.TerritoryId = secondTerr;

                db.Insert(empTer1);
                db.Insert(empTer2);

                var resultEmp = db.Employees.FirstOrDefault(_ => _.FirstName == "Andrey" && _.LastName == "Sobko");
                Assert.IsNotNull(resultEmp);

                var emplTerr = db.EmployeeTerritories.Where(_ => _.EmployeeId == resultEmp.Id);
                var territories = (from terr in db.Territories.LoadWith(_ => _.EmployeeTerritory)
                                   join emplTer in emplTerr on terr.Id equals emplTer.TerritoryId
                                   where terr.EmployeeTerritory.EmployeeId == resultEmp.Id
                                   select terr).ToList();

                Assert.IsTrue(territories.Count == 2);
                Assert.IsTrue(territories.First().EmployeeTerritory.EmployeeId == resultEmp.Id);
                Assert.IsTrue(territories.Skip(1).First().EmployeeTerritory.EmployeeId == resultEmp.Id);

                db.RollbackTransaction();
            }
        }

        // Перенести продукты из одной категории в другую 
        [TestMethod]
        public void TestMethod2()
        {
            using (var db = new Northwind())
            {
                db.BeginTransaction();

                var countProductsWithCategoryOne = db.Products.Where(_ => _.CategoryId == 1).Count();
                var countProductsWithCategoryTwo = db.Products.Where(_ => _.CategoryId == 2).Count();
                var product = db.Products.First(_ => _.CategoryId == 1);

                var productId = product.Id;
                product.CategoryId = 2;
                db.Update(product);

                var countProductsWithCategoryOneNew = db.Products.Where(_ => _.CategoryId == 1).Count();
                var countProductsWithCategoryTwoNew = db.Products.Where(_ => _.CategoryId == 2).Count();
                var prod = db.Products.First(_ => _.Id == productId);

                Assert.AreEqual(countProductsWithCategoryOne - 1, countProductsWithCategoryOneNew);
                Assert.AreEqual(countProductsWithCategoryTwo + 1, countProductsWithCategoryTwoNew);
                Assert.AreEqual(2, prod.CategoryId);

                db.RollbackTransaction();
            }
        }

        // Добавить список продуктов со своими поставщиками и категориями (массовое занесение), при этом если поставщик или категория с таким названием есть, то использовать их – иначе создать новые. 
        [TestMethod]
        public void TestMethod3()
        {
            using (var db = new Northwind())
            {
                db.BeginTransaction();

                var category1 = db.Categories.First(_ => _.Id == 1);
                var supplier1 = db.Suppliers.First(_ => _.Id == 1);

                var newCategory = new Category();
                newCategory.CategoryName = "TestCategory";
                var newSupplier = new Supplier();
                newSupplier.CompanyName = "TestSupplier";

                var product1 = new Product();
                product1.ProductName = "TestProduct1";
                product1.Category = category1;
                product1.Supplier = supplier1;

                var product2 = new Product();
                product2.ProductName = "TestProduct2";
                product2.Supplier = supplier1;
                product2.Category = newCategory;

                var product3 = new Product();
                product3.ProductName = "TestProduct3";
                product3.Category = category1;
                product3.Supplier = newSupplier;

                var list = new List<Product>();
                list.Add(product1);
                list.Add(product2);
                list.Add(product3);

                foreach (var product in list)
                {

                    var category = db.Categories.FirstOrDefault(_ => _.CategoryName == product.Category.CategoryName);
                    if (category == null)
                        db.Insert(product.Category);

                    category = db.Categories.FirstOrDefault(_ => _.CategoryName == product.Category.CategoryName);

                    var supplier = db.Suppliers.FirstOrDefault(_ => _.CompanyName == product.Supplier.CompanyName);
                    if (supplier == null)
                        db.Insert(product.Supplier);

                    supplier = db.Suppliers.FirstOrDefault(_ => _.CompanyName == product.Supplier.CompanyName);

                    db.Products.Value(_ => _.ProductName, product.ProductName)
                               .Value(_ => _.CategoryId, category.Id)
                               .Value(_ => _.SupplierId, supplier.Id)
                               .Insert();
                }

                var newProduct1 = db.Products.LoadWith(_ => _.Category).LoadWith(_ => _.Supplier).FirstOrDefault(_ => _.ProductName == "TestProduct1");

                Assert.IsNotNull(newProduct1);
                Assert.AreEqual(1, newProduct1.Category.Id);
                Assert.AreEqual(1, newProduct1.Supplier.Id);

                var newProduct2 = db.Products.LoadWith(_ => _.Category).LoadWith(_ => _.Supplier).FirstOrDefault(_ => _.ProductName == "TestProduct2");

                Assert.IsNotNull(newProduct2);
                Assert.AreEqual("TestCategory", newProduct2.Category.CategoryName);
                Assert.AreEqual(1, newProduct2.Supplier.Id);

                var newProduct3 = db.Products.LoadWith(_ => _.Category).LoadWith(_ => _.Supplier).FirstOrDefault(_ => _.ProductName == "TestProduct3");

                Assert.IsNotNull(newProduct3);
                Assert.AreEqual(1, newProduct1.Category.Id);
                Assert.AreEqual("TestSupplier", newProduct3.Supplier.CompanyName);

                db.RollbackTransaction();
            }
        }

        // Замена продукта на аналогичный: во всех еще неисполненных заказах (считать таковыми заказы, у которых ShippedDate = NULL) заменить один продукт на другой.
        [TestMethod]
        public void TestMethod4()
        {
            using (var db = new Northwind())
            {
                db.BeginTransaction();

                var detailsForUpdate = (from details in db.OrdersDetails
                                       join order in db.Orders on details.OrderId equals order.Id
                                       where order.ShippedDate == null
                                       select details).ToList();

                var firstDetails = detailsForUpdate.First();
                var lastDetails = detailsForUpdate.Skip(detailsForUpdate.Count - 1).First();

                foreach (var details in detailsForUpdate)
                {
                    var productToReplace = db.Products.First(_ => _.Id == details.ProductId);
                    var categoryId = productToReplace.CategoryId;
                    var newProduct = db.Products.FirstOrDefault(_ => _.CategoryId == categoryId && _.Id != productToReplace.Id);

                    details.ProductId = newProduct.Id;
                    db.Update(details);
                }

                var first = db.OrdersDetails.First(_ => _.OrderId == firstDetails.OrderId);
                var last = db.OrdersDetails.First(_ => _.OrderId == lastDetails.OrderId);

                Assert.AreNotEqual(firstDetails.ProductId, first.ProductId);
                Assert.AreNotEqual(lastDetails.ProductId, last.ProductId);

                db.RollbackTransaction();
            }
        }
    }
}
