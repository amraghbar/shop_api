using Shop_Core.DTOS;
using Shop_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<string> CreateInvoiceFromOrderAsync(int orderId);
        Task<InvoiceDTO> GetInvoiceReciept(int customerId, int invoiceId);
        Task<string> UpdateInvoiceStatusAsync(int invoiceId, bool? isPosted, bool? isReviewed, bool? isClosed);
        Task<List<InvoiceSummaryDTO>> GetInvoicesByCustomerIdAsync(int customerId);

    }

}
