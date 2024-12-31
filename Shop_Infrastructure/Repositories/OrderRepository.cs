using Microsoft.EntityFrameworkCore;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.ItemId == 0)
                {
                    throw new InvalidOperationException("ItemId cannot be 0.");
                }

                var itemExists = await _context.Items
                    .AnyAsync(i => i.Id == orderDetail.ItemId);
                if (!itemExists)
                {
                    throw new InvalidOperationException($"Item with ID {orderDetail.ItemId} does not exist.");
                }
            }

            await _context.orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }


        public async Task<Order> GetOrderByIdAsync(int orderId, int userId)
        {
            return await _context.orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Items) 
                .Where(o => o.Id == orderId && o.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> DeleteOrderAsync(int orderId, int userId)
        {
            var order = await _context.orders
                .Where(o => o.Id == orderId && o.UserId == userId)
                .FirstOrDefaultAsync();

            if (order == null) return false;

            _context.orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
