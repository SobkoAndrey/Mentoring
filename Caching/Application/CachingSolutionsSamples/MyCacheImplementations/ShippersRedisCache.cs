using CachingSolutionsSamples.MyInterfaces;
using NorthwindLibrary;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.MyCacheImplementations
{
    class ShippersRedisCache : IShippersCache
    {
        private ConnectionMultiplexer redisConnection;
        string prefix = "Cache_Shippers";
        DataContractSerializer serializer = new DataContractSerializer(
            typeof(IEnumerable<Shipper>));

        public ShippersRedisCache(string hostName)
        {
            redisConnection = ConnectionMultiplexer.Connect(hostName);
        }

        public IEnumerable<Shipper> Get(string forUser)
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(prefix + forUser);
            if (s == null)
                return null;

            return (IEnumerable<Shipper>)serializer
                .ReadObject(new MemoryStream(s));

        }

        public void Set(string forUser, IEnumerable<Shipper> shippers)
        {
            var db = redisConnection.GetDatabase();
            var key = prefix + forUser;

            if (shippers == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, shippers);
                db.StringSet(key, stream.ToArray());
            }
        }
    }
}
