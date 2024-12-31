using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop_Core.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Users))]
        public int Cus_Id { get; set; }

        [ForeignKey(nameof(Order))]
        public int Order_Id { get; set; } // الربط مع الطلب

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public double NetPrice { get; set; }
        public int Transaction_Type { get; set; } // مثل Debit أو Credit
        public int Payment_Type { get; set; } // Cash أو Online
        public bool isPosted { get; set; }
        public bool isReviewed { get; set; }
        public bool isClosed { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public ICollection<InvoiceDetails> InvoiceDetails { get; set; } = new HashSet<InvoiceDetails>();
        public Users Users { get; set; }
    }
}
