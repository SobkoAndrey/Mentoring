using Linq2DbTask.Entities;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask
{
    public class Northwind : DataConnection
    {
        public Northwind() : base("Northwind") { }

        public ITable<Category> Categories { get { return GetTable<Category>(); } }
        public ITable<Customer> Customers { get { return GetTable<Customer>(); } }
        public ITable<CustomerCustomerDemo> CustomerCustomerDemos { get { return GetTable<CustomerCustomerDemo>(); } }
        public ITable<CustomerDemographic> CustomerDemographics { get { return GetTable<CustomerDemographic>(); } }
        public ITable<Employee> Employees { get { return GetTable<Employee>(); } }
        public ITable<EmployeeTerritory> EmployeeTerritories { get { return GetTable<EmployeeTerritory>(); } }
        public ITable<Order> Orders { get { return GetTable<Order>(); } }
        public ITable<OrderDetails> OrdersDetails { get { return GetTable<OrderDetails>(); } }
        public ITable<Product> Products { get { return GetTable<Product>(); } }
        public ITable<Region> Regions { get { return GetTable<Region>(); } }
        public ITable<Shipper> Shippers { get { return GetTable<Shipper>(); } }
        public ITable<Supplier> Suppliers { get { return GetTable<Supplier>(); } }
        public ITable<Territory> Territories { get { return GetTable<Territory>(); } }
    }
}
