using Shop_Core.DTOS.Order;
using Shop_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<OrderDTOs> GetOrderByIdAsync(int orderId, int userId);
        Task<IEnumerable<OrderDTOs>> GetOrdersByUserIdAsync(int userId);
        Task<bool> DeleteOrderAsync(int orderId, int userId);
    }
}
