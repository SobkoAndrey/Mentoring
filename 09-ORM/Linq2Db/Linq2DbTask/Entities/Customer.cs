using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("Customers")]
    public class Customer
    {
        [Column("CustomerID", IsIdentity = true, IsPrimaryKey = true)]
        public string Id { get; set; }
        private EntitySet<CustomerCustomerDemo> customerDemo;
        [Association(Storage = "customerDemo", ThisKey = "Id", OtherKey = "CustomerId")]
        public EntitySet<CustomerCustomerDemo> CustomerDemo
        {
            get { return this.customerDemo; }
            set { this.customerDemo.Assign(value); }
        }

        [Column]
        public string CompanyName { get; set; }
        [Column]
        public string ContactName { get; set; }
        [Column]
        public string ContactTitle { get; set; }
        [Column]
        public string Address { get; set; }
        [Column]
        public string City { get; set; }
        [Column]
        public string Region { get; set; }
        [Column]
        public string PostalCode { get; set; }
        [Column]
        public string Country { get; set; }
        [Column]
        public string Phone { get; set; }
        [Column]
        public string Fax { get; set; }
    }
}
