using NorthwindDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindDAL.Interfaces
{
    //•	DAL предоставляет объектный интерфейс. Т.е. данные которые он принимает и выдает на вход являются обычными POCO (plain old CLR object) объектами
    // Только по этой причине в такие методы как Update и Delete я передаю не айдишник, а целую модель. Не уверен что это очень круто, но задание я понял так.
    // Так же и с установкой дат, я бы передавал в метод саму дату и айдишник заказа, но пока оставлю так, если надо - переделаю.
    public interface IOrderRepository
    {
        List<Order> GetAll();
        List<ExtendedOrderDetails> GetOrderInfoList(Order order);
        void Add(Order order);
        Order Update(Order order);
        bool Delete(Order order);
        Order SetOrderDate(Order order);
        Order SetShippedDate(Order order);
        Dictionary<string, int> GetStatistic(string customerId);
    }
}
