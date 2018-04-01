using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("Shippers")]
    public class Shipper
    {
        [Column("ShipperID", IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column]
        public string CompanyName { get; set; }
        [Column]
        public string Phone { get; set; }
    }
}
