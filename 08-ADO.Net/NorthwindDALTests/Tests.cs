using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindDAL.Repositories;
using NorthwindDAL.Entities;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Configuration;

namespace NorthwindDALTests
{
    [TestClass]
    public class Tests
    {
        string connectionString;
        string provider;
        OrderRepository repo;

        public Tests()
        {
            var item = ConfigurationManager.ConnectionStrings["NorthwindConnection"];
            connectionString = item.ConnectionString;
            provider = item.ProviderName;
            repo = new OrderRepository(provider, connectionString);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var list = repo.GetAll();

            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void GetOrderInfoListTest()
        {
            var order = new Order();
            order.Id = 10248;

            var orderInfoList = repo.GetOrderInfoList(order);

            Assert.IsTrue(orderInfoList.Count == 3);
            Assert.AreEqual(11, orderInfoList.First().ProductId);
            Assert.AreEqual(42, orderInfoList.Skip(1).First().ProductId);
            Assert.AreEqual(72, orderInfoList.Skip(2).First().ProductId);
        }

        [TestMethod]
        public void AddTest()
        {
            var order = new Order();
            var date = DateTime.Now;
            order.OrderDate = date;

            repo.Add(order);

            var connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "select count(*) from Orders where OrderDate = @date";
            command.CommandType = CommandType.Text;

            var dateParam = command.CreateParameter();
            dateParam.ParameterName = "@date";
            dateParam.Value = date;

            command.Parameters.Add(dateParam);

            var count = command.ExecuteScalar();

            command.Dispose();
            connection.Close();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void UpdateDoesNotWorkWithNotNewOrderTest()
        {
            // Фиксируем время
            var date = DateTime.Now;

            // Достаем заказ, который невозможно изменить и пытаемся его изменить
            var all = repo.GetAll();
            var fg = all.First(_ => _.OrderDate != null);
            fg.RequiredDate = date;
            var notUpdatedOrder = repo.Update(fg);

            var connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "select RequiredDate from Orders where OrderID = @id";
            command.CommandType = CommandType.Text;

            var id = command.CreateParameter();
            id.ParameterName = "@id";
            id.Value = fg.Id;

            command.Parameters.Add(id);

            var trueDate = command.ExecuteScalar();

            command.Dispose();

            // Проверяем, что заказ не изменился
            Assert.AreNotEqual(trueDate, notUpdatedOrder.RequiredDate);
        }

        [TestMethod]
        public void UpdateWorksCorrectlyWithNewOrderTest()
        {
            // Фиксируем время
            var date = DateTime.Now;

            var connection = new SqlConnection(connectionString);
            connection.Open();

            // Создаем новый заказ, со статусом 'Новый' и получаем его ID из базы
            var order = new Order();
            order.RequiredDate = date;
            repo.Add(order);

            var command2 = connection.CreateCommand();
            command2.CommandText = "select OrderID from Orders where RequiredDate = @date1";
            command2.CommandType = CommandType.Text;

            var date1 = command2.CreateParameter();
            date1.ParameterName = "@date1";
            date1.Value = date;

            command2.Parameters.Add(date1);

            var orderId = command2.ExecuteScalar();

            command2.Dispose();

            // Пытаемся изменить 2 поля, одно изменяемое, второе нет
            var date2 = DateTime.Now;
            order.RequiredDate = date2;
            order.ShippedDate = date2;

            var updatedOrder = repo.Update(order);

            var command3 = connection.CreateCommand();
            command3.CommandText = "select RequiredDate from Orders where OrderID = @id2";
            command3.CommandType = CommandType.Text;

            var id2 = command3.CreateParameter();
            id2.ParameterName = "@id2";
            id2.Value = orderId;

            var id3 = command3.CreateParameter();
            id3.ParameterName = "@id3";
            id3.Value = orderId;

            var command4 = connection.CreateCommand();
            command4.CommandText = "select ShippedDate from Orders where OrderID = @id3";
            command4.CommandType = CommandType.Text;

            command3.Parameters.Add(id2);
            command4.Parameters.Add(id3);

            var changedRequiredDate = command3.ExecuteScalar();
            var notChangedShippedDate = command4.ExecuteScalar();

            command3.Dispose();
            connection.Close();

            // Проверяем, что одно поле изменилось, а второе нет
            Assert.AreNotEqual(date, changedRequiredDate);
            Assert.AreEqual(DBNull.Value, notChangedShippedDate);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var order = new Order();
            var date = DateTime.Now;
            order.OrderDate = date;

            repo.Add(order);

            var connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "select OrderID from Orders where OrderDate = @date";
            command.CommandType = CommandType.Text;

            var dateParam = command.CreateParameter();
            dateParam.ParameterName = "@date";
            dateParam.Value = date;

            command.Parameters.Add(dateParam);

            var orderId = command.ExecuteScalar();

            command.Dispose();
            Assert.AreNotEqual(null, orderId);
            Assert.AreNotEqual(DBNull.Value, orderId);
            Assert.AreNotEqual(0, orderId);

            order.Id = (int)orderId;
            bool result = repo.Delete(order);

            Assert.IsTrue(result);

            var command2 = connection.CreateCommand();
            command2.CommandText = "select count(*) from Orders where OrderID = @id";
            command2.CommandType = CommandType.Text;

            var id = command2.CreateParameter();
            id.ParameterName = "@id";
            id.Value = orderId;

            command2.Parameters.Add(id);

            var count = command2.ExecuteScalar();

            command2.Dispose();
            connection.Close();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void DeleteDoesNotWorkWithDoneOrderTest()
        {
            var doneOrder = repo.GetAll().First(_ => _.ShippedDate != null);

            bool result = repo.Delete(doneOrder);

            Assert.IsFalse(result);

            var theSameOrder = repo.GetAll().First(_ => _.ShippedDate != null);

            Assert.AreEqual(doneOrder.Id, theSameOrder.Id);
        }

        [TestMethod]
        public void SetOrderDateTest()
        {
            var order = repo.GetAll().First();
            var date = DateTime.Now;
            order.OrderDate = date;

            var resultOrder = repo.SetOrderDate(order);

            Assert.AreEqual(order.OrderDate.Value.ToLongDateString(), resultOrder.OrderDate.Value.ToLongDateString());
            Assert.AreEqual(order.OrderDate.Value.ToLongTimeString(), resultOrder.OrderDate.Value.ToLongTimeString());
        }

        [TestMethod]
        public void SetShippedDateTest()
        {
            var order = repo.GetAll().First();
            var date = DateTime.Now;
            order.ShippedDate = date;

            var resultOrder = repo.SetShippedDate(order);

            Assert.AreEqual(order.ShippedDate.Value.ToLongDateString(), resultOrder.ShippedDate.Value.ToLongDateString());
            Assert.AreEqual(order.ShippedDate.Value.ToLongTimeString(), resultOrder.ShippedDate.Value.ToLongTimeString());
        }

        [TestMethod]
        public void GetStatisticTest()
        {
            var order = repo.GetAll().First();

            var statistic = repo.GetStatistic(order.CustomerID);

            Assert.AreNotEqual(statistic, null);

            Dictionary<string, int> statistic2 = new Dictionary<string, int>();

            var connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = 
                "select ProductName, Total=SUM(Quantity) from Products P, [Order Details] OD, Orders O, Customers C " +
                "WHERE C.CustomerID = @id AND C.CustomerID = O.CustomerID AND O.OrderID = OD.OrderID AND OD.ProductID = P.ProductID GROUP BY ProductName";
            command.CommandType = CommandType.Text;

            var id = command.CreateParameter();
            id.ParameterName = "@id";
            id.Value = order.CustomerID;

            command.Parameters.Add(id);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var name = reader[0] == DBNull.Value ? null : (string)reader[0];
                    var amount = reader.GetInt32(1);

                    statistic2.Add(name, amount);
                }
            }

            command.Dispose();
            connection.Close();

            Assert.AreEqual(statistic.First(), statistic2.First());
            Assert.AreEqual(statistic.Last().Value, statistic2.Last().Value);
        }

        [TestMethod]
        public void GetCustOrdersDetailTest()
        {
            var order = repo.GetAll().First();

            var custOrdersDetail = repo.GetCustOrdersDetail(order.Id);

            Assert.AreNotEqual(custOrdersDetail, null);

            CustOrdersDetail custOrdersDetail2 = new CustOrdersDetail();

            var connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT ProductName, UnitPrice = ROUND(Od.UnitPrice, 2), Quantity, Discount = CONVERT(int, Discount * 100), " +
                "ExtendedPrice = ROUND(CONVERT(money, Quantity * (1 - Discount) * Od.UnitPrice), 2) FROM Products P, [Order Details] Od " +
                "WHERE Od.ProductID = P.ProductID and Od.OrderID = @OrderID";
            command.CommandType = CommandType.Text;

            var id = command.CreateParameter();
            id.ParameterName = "@OrderID";
            id.Value = order.Id;

            command.Parameters.Add(id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    custOrdersDetail2.ProductName = reader.GetString(0);
                    custOrdersDetail2.UnitPrice = reader.GetDecimal(1);
                    custOrdersDetail2.Quantity = reader.GetInt16(2);
                    custOrdersDetail2.Discount = reader.GetInt32(3);
                    custOrdersDetail2.ExtendedPrice = reader.GetDecimal(4);
                }
            }

            command.Dispose();
            connection.Close();

            Assert.AreEqual(custOrdersDetail.ProductName, custOrdersDetail2.ProductName);
            Assert.AreEqual(custOrdersDetail.Quantity, custOrdersDetail2.Quantity);
        }
    }
}
