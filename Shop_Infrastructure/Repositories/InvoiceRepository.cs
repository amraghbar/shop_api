using Microsoft.EntityFrameworkCore;
using Shop_Core.DTOS;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext appDbContext;

        public InvoiceRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<string> CreateInvoiceFromOrderAsync(int orderId)
        {
            var order = await appDbContext.orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Items)
                .ThenInclude(i => i.ItemsUnits) 
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return "Order not found";
            }

            if (order.OrderDetails == null || !order.OrderDetails.Any())
            {
                return "No items in the order to create invoice";
            }

            var unavailableItems = new List<string>();
            double totalNetPrice = 0;

            // تحقق من توفر العناصر في المخزون
            var itemStores = await appDbContext.InvItemStores
                .Where(i => order.OrderDetails.Select(od => od.ItemId).Contains(i.Item_Id))
                .ToListAsync();

            foreach (var orderDetail in order.OrderDetails)
            {
                var itemStore = itemStores.FirstOrDefault(i => i.Item_Id == orderDetail.ItemId);

                if (itemStore == null || orderDetail.Quantity > itemStore.Balance - itemStore.ReservedQuantity)
                {
                    unavailableItems.Add(orderDetail.Items.Name);
                    continue;
                }
            }

            if (unavailableItems.Any())
            {
                return $"The following items are unavailable: {string.Join(", ", unavailableItems)}";
            }

            // إنشاء الفاتورة
            var invoice = new Invoice
            {
                Cus_Id = order.UserId,  // ربط الفاتورة بالعميل
                CreatedAt = DateTime.Now,
                NetPrice = 0,
                Transaction_Type = 1,
                Payment_Type = 1,
                isPosted = true,
                isClosed = false,
                isReviewed = false,
                Order_Id = orderId // ربط الفاتورة بالطلب
            };

            await appDbContext.Invoice.AddAsync(invoice);
            await appDbContext.SaveChangesAsync();

            // إضافة تفاصيل الفاتورة
            var invoiceDetails = new List<InvoiceDetails>();
            var updatedItemStores = new List<InvItemStores>();

            foreach (var orderDetail in order.OrderDetails)
            {
                var itemStore = itemStores.FirstOrDefault(i => i.Item_Id == orderDetail.ItemId);
                double unitPrice = (double)orderDetail.Price;
                double itemTotalPrice = orderDetail.Quantity * unitPrice;
                totalNetPrice += itemTotalPrice;

                var unit = orderDetail.Items.ItemsUnits.FirstOrDefault();
                if (unit == null)
                {
                    return $"No unit found for item {orderDetail.Items.Name}";
                }

                var invoiceDetail = new InvoiceDetails
                {
                    Invoice_Id = invoice.Id,
                    Item_Id = orderDetail.ItemId,
                    Quantity = orderDetail.Quantity,
                    Factor = 1,
                    price = (int)unitPrice,
                    Unit_Id = unit.Unit_Id,
                    CreatedAt = DateTime.Now
                };

                invoiceDetails.Add(invoiceDetail);

                itemStore.ReservedQuantity += orderDetail.Quantity;
                updatedItemStores.Add(itemStore);
            }

            appDbContext.InvoiceDetails.AddRange(invoiceDetails);
            appDbContext.InvItemStores.UpdateRange(updatedItemStores);
            invoice.NetPrice = totalNetPrice;

            await appDbContext.SaveChangesAsync();

            // تفريغ السلة
            var cartItems = await appDbContext.ShoppingCartItems
                .Where(c => c.Cus_Id == order.UserId)
                .ToListAsync();

            appDbContext.ShoppingCartItems.RemoveRange(cartItems);
            await appDbContext.SaveChangesAsync();

            return $"Invoice created successfully from order {orderId} with total price: {totalNetPrice}";
        }

        public async Task<InvoiceDTO> GetInvoiceReciept(int customerId, int invoiceId)
        {
            var invoice = await appDbContext.Invoice
                .Include(i => i.InvoiceDetails)
                .ThenInclude(x => x.Items)
                .FirstOrDefaultAsync(i => i.Cus_Id == customerId && i.Id == invoiceId);

            if (invoice == null)
            {
                return null;
            }

            double totalPrice = 0;
            foreach (var item in invoice.InvoiceDetails)
            {
                double itemPrice = item.Quantity * item.price;
                totalPrice += itemPrice;
            }

            invoice.NetPrice = totalPrice;
            await appDbContext.SaveChangesAsync();

            var receipt = new InvoiceDTO
            {
                invoice_id = invoiceId,
                customer_id = invoice.Cus_Id,
                created_at = invoice.CreatedAt,
                total_price = totalPrice,
                items = invoice.InvoiceDetails.Select(d => new InvoiceItemsDTO
                {
                    item_name = d.Items.Name,
                    quantity = d.Quantity,
                    unit_name = appDbContext.Units.FirstOrDefault(u => u.Id == d.Unit_Id)?.Name ?? "unknown",
                    price_per_unit = d.price,
                    total_price = (d.Quantity * d.price)
                }).ToList()
            };

            return receipt;
        }

        public async Task<string> UpdateInvoiceStatusAsync(int invoiceId, bool? isPosted, bool? isReviewed, bool? isClosed)
        {
            var invoice = await appDbContext.Invoice
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
            {
                return "Invoice not found";
            }

            // تحديث القيم فقط إذا كانت تحتوي على قيمة (أي ليست null)
            if (isPosted.HasValue)
            {
                invoice.isPosted = isPosted.Value;
            }

            if (isReviewed.HasValue)
            {
                invoice.isReviewed = isReviewed.Value;
            }

            if (isClosed.HasValue)
            {
                invoice.isClosed = isClosed.Value;
            }

            // حفظ التغييرات في قاعدة البيانات
            appDbContext.Invoice.Update(invoice);
            await appDbContext.SaveChangesAsync();

            return "Invoice status updated successfully";
        }



        public async Task<List<InvoiceSummaryDTO>> GetInvoicesByCustomerIdAsync(int customerId)
        {
            var invoices = await appDbContext.Invoice
                .Where(i => i.Cus_Id == customerId)  // جلب الفواتير الخاصة بالمستخدم
                .Select(i => new InvoiceSummaryDTO
                {
                    InvoiceId = i.Id,
                    CreatedAt = i.CreatedAt,
                    NetPrice = i.NetPrice,
                    IsPosted = i.isPosted,
                    IsReviewed = i.isReviewed,
                    IsClosed = i.isClosed
                })
                .ToListAsync();

            return invoices;
        }
    }





}
