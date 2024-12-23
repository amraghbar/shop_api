using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Models
{
    public class InvoiceDetails
    {
        [ForeignKey(nameof(Invoice))]
        public int Invoice_Id { get; set; }

        [ForeignKey(nameof(Items))]
        public int Item_Id { get; set; }

        public int price { get; set; }

        public int Unit_Id { get; set; }

        public double Quantity { get; set; }

        public double Factor { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Items Items { get; set; }
        public Invoice Invoice { get; set; }

    }
}
