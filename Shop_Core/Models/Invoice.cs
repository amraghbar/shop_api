﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Users))]
        public int Cus_Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public double NetPrice { get; set; }
        public int Transaction_Type { get; set; }
        public int Payment_Type { get; set; }
        public bool isPosted { get; set; }
        public bool isReviewed { get; set; }
        public bool isClosed { get; set; }

        public ICollection<InvoiceDetails> InvoiceDetails { get; set; } = new HashSet<InvoiceDetails>();
        public Users Users { get; set; }
    }
}
