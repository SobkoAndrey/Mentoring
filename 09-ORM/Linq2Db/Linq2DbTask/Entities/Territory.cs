using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("Territories")]
    public class Territory
    {
        [Column("TerritoryID", IsIdentity = true, IsPrimaryKey = true)]
        public string Id { get; set; }
        private EntityRef<EmployeeTerritory> employeeTerritory;
        [Association(Storage = "employeeTerritory", ThisKey = "Id", OtherKey = "TerritoryId")]
        public EmployeeTerritory EmployeeTerritory
        {
            get { return this.employeeTerritory.Entity; }
            set { this.employeeTerritory.Entity = value; }
        }

        [Column]
        public string TerritoryDescription { get; set; }

        [Column]
        public int RegionId { get; set; }
        private EntityRef<Region> region;
        [Association(Storage = "region", ThisKey = "RegionId", OtherKey = "Id")]
        public Region Region
        {
            get { return this.region.Entity; }
            set { this.region.Entity = value; }
        }
    }
}
