using NorthwindDAL.Interfaces;
using System.Collections.Generic;
using NorthwindDAL.Entities;
using System.Data.Common;
using System.Data;
using System;
using NorthwindDAL.Entities.OrdersLinked;
using System.Configuration;

namespace NorthwindDAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbProviderFactory factory;
        private readonly string connectionString;

        public OrderRepository(string provider, string connectionString)
        {
            factory = DbProviderFactories.GetFactory(provider);
            this.connectionString = connectionString;
        }

        public virtual void Add(Order newOrder)
        {
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = 
                        "insert into Orders (CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, " +
                        "ShipPostalCode, ShipCountry) " +
                        "values (@customerId, @employeeId, @orderDate, @requiredDate, @shippedDate, @shipVia, @freight, @shipName, @shipAddress, @shipCity, @shipRegion, @shipPostalCode, @shipCountry)";

                    command.CommandType = CommandType.Text;

                    AddParameter(command, "@customerId", newOrder.CustomerID);
                    AddParameter(command, "@employeeId", newOrder.EmployeeID);
                    AddParameter(command, "@orderDate", newOrder.OrderDate);
                    AddParameter(command, "@requiredDate", newOrder.RequiredDate);
                    AddParameter(command, "@shippedDate", newOrder.ShippedDate);
                    AddParameter(command, "@shipVia", newOrder.ShipVia);
                    AddParameter(command, "@freight", newOrder.Freight);
                    AddParameter(command, "@shipName", newOrder.ShipName);
                    AddParameter(command, "@shipAddress", newOrder.ShipAddress);
                    AddParameter(command, "@shipCity", newOrder.ShipCity);
                    AddParameter(command, "@shipRegion", newOrder.ShipRegion);
                    AddParameter(command, "@shipPostalCode", newOrder.ShipPostalCode);
                    AddParameter(command, "@shipCountry", newOrder.ShipCountry);

                    command.ExecuteNonQuery();
                }
            }
        }

        public virtual Order Update(Order order)
        {
            if (order.Status != OrderStatuses.New)
                return order;

            Order updatedOrder = null;

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "update Orders set CustomerID = @customerId, EmployeeID = @employeeId, RequiredDate = @requiredDate, ShipVia = @shipVia, Freight = @freight, ShipName = @shipName, ShipAddress = @shipAddress, " +
                        "ShipCity = @shipCity, ShipRegion = @shipRegion, ShipPostalCode = @shipPostalCode, ShipCountry = @shipCountry where OrderID = @id";

                    command.CommandType = CommandType.Text;

                    AddParameter(command, "@id", order.Id);
                    AddParameter(command, "@customerId", order.CustomerID);
                    AddParameter(command, "@employeeId", order.EmployeeID);
                    AddParameter(command, "@requiredDate", order.RequiredDate);
                    AddParameter(command, "@shipVia", order.ShipVia);
                    AddParameter(command, "@freight", order.Freight);
                    AddParameter(command, "@shipName", order.ShipName);
                    AddParameter(command, "@shipAddress", order.ShipAddress);
                    AddParameter(command, "@shipCity", order.ShipCity);
                    AddParameter(command, "@shipRegion", order.ShipRegion);
                    AddParameter(command, "@shipPostalCode", order.ShipPostalCode);
                    AddParameter(command, "@shipCountry", order.ShipCountry);

                    command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    updatedOrder = GetById(order.Id, command);
                }
            }

            return updatedOrder;
        }

        public virtual List<Order> GetAll()
        {
            var orders = new List<Order>();

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, " +
                                          "ShipPostalCode, ShipCountry from Orders";

                    command.CommandType = CommandType.Text;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = MapOrder(reader);
                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        /// <summary>
        /// Здесь хотелось бы конечно ID передать параметром, но задание я понял так что передавать надо сущность. Наверно неправильно понял, но оставлю так, суть та же
        /// ID поставить вместо order не проблема
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public virtual List<ExtendedOrderDetails> GetOrderInfoList(Order order)
        {
            var orderInfoList = new List<ExtendedOrderDetails>();
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select orders.OrderID, details.ProductID, products.ProductName, details.Quantity, details.UnitPrice, details.Discount  " +
                                          "from Orders orders " +
                                          "join [Order Details] details on orders.OrderId = details.OrderID " +
                                          "join Products products on details.ProductID = products.ProductID " +
                                          "where orders.OrderID = @id";

                    command.CommandType = CommandType.Text;

                    var orderId = command.CreateParameter();
                    orderId.ParameterName = "@id";
                    orderId.Value = order.Id;

                    command.Parameters.Add(orderId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var orderInfo = new ExtendedOrderDetails();

                            orderInfo.OrderId = reader.GetInt32(0);
                            orderInfo.ProductId = reader.GetInt32(1);
                            orderInfo.ProductName = reader[2] == DBNull.Value ? null : (string)reader[2];
                            orderInfo.Quantity = reader.GetInt16(3);
                            orderInfo.UnitPrice = reader.GetDecimal(4);
                            orderInfo.Discount = reader.GetFloat(5);

                            orderInfoList.Add(orderInfo);
                        }
                    }
                }
            }

            return orderInfoList;
        }

        public virtual bool Delete(Order order)
        {
            bool result = false;

            if (order.Status == OrderStatuses.Done)
                return result;

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from Orders where OrderID = @id";

                    command.CommandType = CommandType.Text;

                    var orderId = command.CreateParameter();
                    orderId.ParameterName = "@id";
                    orderId.Value = order.Id;
                    command.Parameters.Add(orderId);

                    var count = command.ExecuteNonQuery();

                    if (count == 1)
                        result = true;
                }
            }

            return result;
        }

        public virtual Order SetOrderDate(Order order)
        {
            int counter = 0;

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "update Orders set OrderDate = @orderDate where OrderID = @id";

                    command.CommandType = CommandType.Text;

                    AddParameter(command, "@id", order.Id);
                    AddParameter(command, "@orderDate", order.OrderDate);

                    counter = command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    order = GetById(order.Id, command);
                }
            }

            if (counter == 1)
                return order;
            else
                return null;
        }

        public virtual Order SetShippedDate(Order order)
        {
            int counter = 0;

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "update Orders set ShippedDate = @shippedDate where OrderID = @id";

                    command.CommandType = CommandType.Text;

                    AddParameter(command, "@id", order.Id);
                    AddParameter(command, "@shippedDate", order.ShippedDate);

                    counter = command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    order = GetById(order.Id, command);
                }
            }

            if (counter == 1)
                return order;
            else
                return null;
        }

        public virtual Dictionary<string, int> GetStatistic(string customerId)
        {
            Dictionary<string, int> statistic = new Dictionary<string, int>();
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CustOrderHist";

                    command.CommandType = CommandType.StoredProcedure;

                    AddParameter(command, "@CustomerID", customerId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var name = reader[0] == DBNull.Value ? null : (string)reader[0];
                            var amount = reader.GetInt32(1);

                            statistic.Add(name, amount);
                        }
                    }
                }
            }

            return statistic;
        }

        public virtual CustOrdersDetail GetCustOrdersDetail(int orderId)
        {
            CustOrdersDetail custOrdersDetail = null;

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CustOrdersDetail";

                    command.CommandType = CommandType.StoredProcedure;

                    AddParameter(command, "@OrderID", orderId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            custOrdersDetail = new CustOrdersDetail();
                            custOrdersDetail.ProductName = reader.GetString(0);
                            custOrdersDetail.UnitPrice = reader.GetDecimal(1);
                            custOrdersDetail.Quantity = reader.GetInt16(2);
                            custOrdersDetail.Discount = reader.GetInt32(3);
                            custOrdersDetail.ExtendedPrice = reader.GetDecimal(4);
                        }
                    }
                }
            }

            return custOrdersDetail;
        }

        private Order GetById(int orderId, DbCommand command)
        {
            command.CommandText = "select OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, " +
                                                      "ShipPostalCode, ShipCountry from Orders where OrderID = @id";

            command.CommandType = CommandType.Text;
            AddParameter(command, "@id", orderId);

            var order = new Order();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    order = MapOrder(reader);
            }

            return order;
        }

        private Order MapOrder(DbDataReader reader)
        {
            var order = new Order();

            order.Id = reader.GetInt32(0);
            order.CustomerID = reader[1] == DBNull.Value ? null : (string)reader[1];
            order.EmployeeID = reader[2] == DBNull.Value ? null : (int?)reader[2];
            order.OrderDate = reader[3] == DBNull.Value ? null : (DateTime?)reader[3];
            order.RequiredDate = reader[4] == DBNull.Value ? null : (DateTime?)reader[4];
            order.ShippedDate = reader[5] == DBNull.Value ? null : (DateTime?)reader[5];
            order.ShipVia = reader[6] == DBNull.Value ? null : (int?)reader[6];
            order.Freight = reader[7] == DBNull.Value ? null : (decimal?)reader[7]; ;
            order.ShipName = reader[8] == DBNull.Value ? null : (string)reader[8];
            order.ShipAddress = reader[9] == DBNull.Value ? null : (string)reader[9];
            order.ShipCity = reader[10] == DBNull.Value ? null : (string)reader[10];
            order.ShipRegion = reader[11] == DBNull.Value ? null : (string)reader[11];
            order.ShipPostalCode = reader[12] == DBNull.Value ? null : (string)reader[12];
            order.ShipCountry = reader[13] == DBNull.Value ? null : (string)reader[13];

            return order;
        }

        private object GetValueOrDBNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            else
                return obj;
        }

        private void AddParameter(DbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = GetValueOrDBNull(value);

            command.Parameters.Add(parameter);
        }
    }
}