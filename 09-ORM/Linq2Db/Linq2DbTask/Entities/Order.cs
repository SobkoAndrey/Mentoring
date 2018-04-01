using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Column("OrderID", IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column]
        public string CustomerId { get; set; }
        private EntityRef<Customer> customer;
        [Association(Storage = "customer", ThisKey = "CustomerId", OtherKey = "Id")]
        public Customer Customer
        {
            get { return this.customer.Entity; }
            set { this.customer.Entity = value; }
        }

        [Column]
        public int? EmployeeId { get; set; }
        private EntityRef<Employee> employee;
        [Association(Storage = "employee", ThisKey = "EmployeeId", OtherKey = "Id")]
        public Employee Employee
        {
            get { return this.employee.Entity; }
            set { this.employee.Entity = value; }
        }

        [Column]
        public DateTime? OrderDate { get; set; }
        [Column]
        public DateTime? RequiredDate { get; set; }
        [Column]
        public DateTime? ShippedDate { get; set; }

        [Column]
        public int? ShipVia { get; set; }
        private EntityRef<Shipper> shipper;
        [Association(Storage = "shipper", ThisKey = "ShipVia", OtherKey = "Id")]
        public Shipper Shipper
        {
            get { return this.shipper.Entity; }
            set { this.shipper.Entity = value; }
        }

        [Column]
        public decimal? Freight { get; set; }
        [Column]
        public string ShipName { get; set; }
        [Column]
        public string ShipAddress { get; set; }
        [Column]
        public string ShipCity { get; set; }
        [Column]
        public string ShipRegion { get; set; }
        [Column]
        public string ShipPostalCode { get; set; }
        [Column]
        public string ShipCountry { get; set; }
    }
}
