using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("EmployeeTerritories")]
    public class EmployeeTerritory
    {
        public EmployeeTerritory() { }

        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        private EntityRef<Employee> employee;
        [Association(Storage = "employee", ThisKey = "EmployeeId", OtherKey = "Id")]
        public Employee Employee
        {
            get { return this.employee.Entity;  }
            set { this.employee.Entity = value; }
        }

        [Column("TerritoryID")]
        public string TerritoryId { get; set; }
        private EntityRef<Territory> territory;
        [Association(Storage = "territory", ThisKey = "TerritoryId", OtherKey = "Id")]
        public Territory Territory
        {
            get { return this.territory.Entity; }
            set { this.territory.Entity = value; }
        }
    }
}
