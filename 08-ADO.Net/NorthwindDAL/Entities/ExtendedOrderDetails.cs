using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDAL.Entities
{
    public class ExtendedOrderDetails : OrderDetails
    {
        public string ProductName { get; set; }
    }
}
