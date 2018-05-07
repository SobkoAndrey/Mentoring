using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.MyInterfaces
{
    public interface IShippersCache
    {
        IEnumerable<Shipper> Get(string forUser);
        void Set(string forUser, IEnumerable<Shipper> shippers);
    }
}
