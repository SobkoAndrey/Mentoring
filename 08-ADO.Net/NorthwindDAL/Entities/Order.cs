using NorthwindDAL.Entities.OrdersLinked;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDAL.Entities
{
    namespace OrdersLinked
    {
        public enum OrderStatuses
        {
            New,
            InWork,
            Done
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public OrderStatuses Status
        {
            get
            {
                if (OrderDate == null)
                    return OrderStatuses.New;
                else if (ShippedDate == null)
                    return OrderStatuses.InWork;
                else
                    return OrderStatuses.Done;
            }
        }

    }
}