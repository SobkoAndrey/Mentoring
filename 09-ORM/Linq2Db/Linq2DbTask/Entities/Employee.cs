using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("Employees")]
    public class Employee
    {
        [Column("EmployeeID", IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        private EntitySet<EmployeeTerritory> employeeTerritory;
        [Association(Storage = "employeeTerritory", ThisKey = "Id", OtherKey = "EmployeeId")]
        public EntitySet<EmployeeTerritory> EmployeeTerritory
        {
            get { return this.employeeTerritory; }
            set { this.employeeTerritory.Assign(value); }
        }

        [Column]
        public string LastName { get; set; }
        [Column]
        public string FirstName { get; set; }
        [Column]
        public string Title { get; set; }
        [Column]
        public string TitleOfCourtesy { get; set; }
        [Column]
        public DateTime? BirthDate { get; set; }
        [Column]
        public DateTime? HireDate { get; set; }
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
        public string HomePhone { get; set; }
        [Column]
        public string Extension { get; set; }
        [Column]
        public byte[] Photo { get; set; }
        [Column]
        public string Notes { get; set; }

        [Column]
        public int? ReportsTo { get; set; }
        private EntityRef<Employee> manager;
        [Association(Storage = "manager", ThisKey = "ReportsTo", OtherKey = "Id")]
        public Employee Manager
        {
            get { return this.manager.Entity; }
            set { this.manager.Entity = value; }
        }

        [Column]
        public string PhotoPath { get; set; }
    }
}
