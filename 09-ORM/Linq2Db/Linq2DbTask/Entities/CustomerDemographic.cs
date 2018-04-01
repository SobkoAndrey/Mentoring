using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("CustomerDemographics")]
    public class CustomerDemographic
    {
        [Column("CustomerTypeID", IsIdentity = true, IsPrimaryKey = true)]
        public string CustomerTypeId { get; set; }
        private EntitySet<CustomerCustomerDemo> customerDemo;
        [Association(Storage = "customerDemo", ThisKey = "CustomerTypeId", OtherKey = "CustomerTypeId")]
        public EntitySet<CustomerCustomerDemo> CustomerDemo
        {
            get { return this.customerDemo; }
            set { this.customerDemo.Assign(value); }
        }

        [Column]
        public string CustomerDesc { get; set; }
    }
}
