using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2DbTask.Entities
{
    [Table("Order Details")]
    public class OrderDetails
    {
        [Column(IsPrimaryKey = true, PrimaryKeyOrder = 1)]
        public int OrderId { get; set; }
        private EntityRef<Order> order;
        [Association(Storage = "order", ThisKey = "OrderId", OtherKey = "Id")]
        public Order Order
        {
            get { return this.order.Entity; }
            set { this.order.Entity = value; }
        }

        [Column(IsPrimaryKey = true, PrimaryKeyOrder = 2)]
        public int ProductId { get; set; }
        private EntityRef<Product> product;
        [Association(Storage = "product", ThisKey = "ProductId", OtherKey = "Id")]
        public Product Product
        {
            get { return this.product.Entity; }
            set { this.product.Entity = value; }
        }

        [Column]
        public decimal UnitPrice { get; set; }
        [Column]
        public int Quantity { get; set; }
        [Column]
        public double Discount { get; set; }
    }
}
