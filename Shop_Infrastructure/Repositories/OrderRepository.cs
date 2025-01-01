using Microsoft.EntityFrameworkCore;
using Shop_Core.DTOS.Items;
using Shop_Core.DTOS;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Core.DTOS.Order;

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

        public async Task<OrderDTOs> GetOrderByIdAsync(int orderId, int userId)
        {
            return await _context.orders
                .Where(o => o.Id == orderId && o.UserId == userId)
                .Select(o => new OrderDTOs
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    AddressLine1 = o.AddressLine1,
                    City = o.City,
                    State = o.State,
                    PostalCode = o.PostalCode,
                    Country = o.Country,
                    Status = o.Status,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                    {
                        Id = od.Id,
                        OrderId = od.OrderId,
                        ItemId = od.ItemId,
                        Quantity = (int)od.Quantity,
                        Price = od.Price,
                        Total = od.Total,
                        Items = new ItemOrD
                        {
                            Id = od.Items.Id,
                            price = od.Items.price
                        }
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<OrderDTOs>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderDTOs
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    AddressLine1 = o.AddressLine1,
                    City = o.City,
                    State = o.State,
                    PostalCode = o.PostalCode,
                    Country = o.Country,
                    Status = o.Status,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                    {
                        Id = od.Id,
                        OrderId = od.OrderId,
                        ItemId = od.ItemId,
                        Quantity = (int)od.Quantity,
                        Price = od.Price,
                        Total = od.Total,
                        Items = new ItemOrD
                        {
                            Id = od.Items.Id,
                            price = od.Items.price
                        }
                    }).ToList()
                })
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
