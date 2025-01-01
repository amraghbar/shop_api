using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.DTOS.Invoice
{
    public class InvoiceSummaryDTO
    {
        public int InvoiceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public double NetPrice { get; set; }
        public bool IsPosted { get; set; }
        public bool IsReviewed { get; set; }
        public bool IsClosed { get; set; }
    }
}
