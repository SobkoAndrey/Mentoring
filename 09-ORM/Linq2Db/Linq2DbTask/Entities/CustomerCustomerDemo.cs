using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("CustomerCustomerDemo")]
    public class CustomerCustomerDemo
    {
        [Column("CustomerID")]
        public string CustomerId { get; set; }

        [Column("CustomerTypeID")]
        public string CustomerTypeId { get; set; }
    }
}
