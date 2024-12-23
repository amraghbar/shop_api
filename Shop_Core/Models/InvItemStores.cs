using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Models
{
    public class InvItemStores
    {
        [ForeignKey(nameof(Stores))]
        public int Store_Id { get; set; }
        
        [ForeignKey(nameof(Items))]
        public int Item_Id { get; set; }
        public double Balance { get; set; }
        public double ReservedQuantity{ get; set; }
        public Items Items { get; set; }
        public Stores Stores { get; set; }
        public DateTime LastUpdated{ get; set; }
    }
}
