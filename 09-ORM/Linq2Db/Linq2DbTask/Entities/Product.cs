using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace Linq2DbTask.Entities
{
    [Table("Products")]
    public class Product
    {
        [Column("ProductID", IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        [Column]
        public string ProductName { get; set; }

        [Column]
        public int? SupplierId { get; set; }
        private EntityRef<Supplier> supplier;
        [Association(Storage = "supplier", ThisKey = "SupplierId", OtherKey = "Id")]
        public Supplier Supplier
        {
            get { return this.supplier.Entity; }
            set { this.supplier.Entity = value; }
        }

        [Column]
        public int? CategoryId { get; set; }
        private EntityRef<Category> category;
        [Association(Storage = "category", ThisKey = "CategoryId", OtherKey = "Id")]
        public Category Category
        {
            get { return this.category.Entity; }
            set { this.category.Entity = value; }
        }

        [Column]
        public string QuantityPerUnit { get; set; }
        [Column]
        public decimal? UnitPrice { get; set; }
        [Column]
        public int? UnitsInStock { get; set; }
        [Column]
        public int? UnitsOnOrder { get; set; }
        [Column]
        public int? ReorderLevel { get; set; }
        [Column]
        public int Discontinued { get; set; }
    }
}
