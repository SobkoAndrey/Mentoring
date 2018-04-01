using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("Region")]
    public class Region
    {
        [Column("RegionID", IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column]
        public string RegionDescription { get; set; }
    }
}
