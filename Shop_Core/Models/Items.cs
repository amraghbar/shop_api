using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Models
{
    public class Items
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double price { get; set; }

        [ForeignKey(nameof(MainGroup))]
        public int MG_Id { get; set; }

        [ForeignKey(nameof(SubGroup))]
        public int Sub_Id { get; set; }
        
      

        public MainGroup MainGroup{ get; set; }
        public SubGroup SubGroup{ get; set; }
        public ICollection<InvItemStores> InvItemStores { get; set; }
        public ICollection<ItemsUnits> ItemsUnits { get; set; } = new HashSet<ItemsUnits>();

    }
}
