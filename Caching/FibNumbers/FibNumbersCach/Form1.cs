using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FibNumbersCach
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            using (RedisClient cache = new RedisClient("localhost", 6379))
            {
                IRedisTypedClient<int> cacheValue = cache.As<int>();
                cacheValue.DeleteAll();
            }
        }

        /// <summary>
        /// Рассчитать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var text = maxValue.Text;
            if (text == null || text.Any(_ => !Char.IsDigit(_)))
                return;

            var number = Convert.ToInt32(text);

            if (number < 0)
                return;

            var result = new List<int>() { 0 };

            if (number > 0)
                result.Add(1);

            var prevLastNumber = 0;
            var lastNumber = 1;

            var cache = MemoryCache.Default;

            var key = "fib";

            List<int> cacheValue = cache.Get(key) as List<int>;

            if (cacheValue == null)
            {
                while (prevLastNumber + lastNumber <= number)
                {
                    result.Add(prevLastNumber + lastNumber);
                    var tempPrev = prevLastNumber;
                    prevLastNumber = lastNumber;
                    lastNumber = tempPrev + lastNumber;
                }

                cache.Set(key, result, ObjectCache.InfiniteAbsoluteExpiration);
            }
            else if (cacheValue.Last() > number)
            {
                result = cacheValue.TakeWhile(_ => _ <= number).ToList();
            }
            else
            {
                result = cacheValue;

                prevLastNumber = result.OrderByDescending(_ => _).Skip(1).First();
                lastNumber = result.Last();

                while (prevLastNumber + lastNumber <= number)
                {
                    result.Add(prevLastNumber + lastNumber);
                    var tempPrev = prevLastNumber;
                    prevLastNumber = lastNumber;
                    lastNumber = tempPrev + lastNumber;
                }

                cache.Set(key, result, ObjectCache.InfiniteAbsoluteExpiration);
            }

            StringBuilder sb = new StringBuilder();

            foreach (var item in result)
            {
                if (item != result.Last())
                    sb.Append(item + ", ");
                else if (item == result.Last() && item == 1 && !sb.ToString().Contains("1"))
                    sb.Append(item + ", ");
                else
                    sb.Append(item);
            }

            resultLabel.Text = sb.ToString();
        }

        /// <summary>
        /// Расчет с Redis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var text = maxValue.Text;
            if (text == null || text.Any(_ => !Char.IsDigit(_)))
                return;

            var number = Convert.ToInt32(text);

            if (number < 0)
                return;

            if (number == 0)
            {
                resultLabel.Text = "0";
                return;
            }

            var result = new List<int>() { 0 };

            if (number > 0)
                result.Add(1);

            var prevLastNumber = 0;
            var lastNumber = 1;

            using (RedisClient cache = new RedisClient("localhost", 6379))
            {
                IRedisTypedClient<int> cacheValue = cache.As<int>();

                var cacheValu = cacheValue.GetAll();

                if (cacheValu.Count == 0)
                {
                    while (prevLastNumber + lastNumber <= number)
                    {
                        result.Add(prevLastNumber + lastNumber);
                        var tempPrev = prevLastNumber;
                        prevLastNumber = lastNumber;
                        lastNumber = tempPrev + lastNumber;
                    }

                    cacheValue.StoreAll(result);
                }
                else if (cacheValu.Last() > number)
                {
                    result.Add(1);
                    result.AddRange(cacheValu.Skip(2).TakeWhile(_ => _ <= number).ToList());
                }
                else
                {
                    result.Add(1);
                    result.AddRange(cacheValu.Skip(2).ToList());

                    prevLastNumber = result.OrderByDescending(_ => _).Skip(1).First();
                    lastNumber = result.Last();

                    while (prevLastNumber + lastNumber <= number)
                    {
                        result.Add(prevLastNumber + lastNumber);
                        var tempPrev = prevLastNumber;
                        prevLastNumber = lastNumber;
                        lastNumber = tempPrev + lastNumber;
                    }

                    cacheValue.StoreAll(result);
                }

                StringBuilder sb = new StringBuilder();

                foreach (var item in result)
                {
                    if (item != result.Last())
                        sb.Append(item + ", ");
                    else if (item == result.Last() && item == 1 && !sb.ToString().Contains("1"))
                        sb.Append(item + ", ");
                    else
                        sb.Append(item);
                }

                resultLabel.Text = sb.ToString();
            }
        }
    }
}
