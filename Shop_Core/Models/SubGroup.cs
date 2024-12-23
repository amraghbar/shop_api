using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Models
{
    public class SubGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(MainGroup))]
        public int MG_Id { get; set; }
        public MainGroup MainGroup{ get; set; }

        public ICollection<Items> Items { get; set; } = new HashSet<Items>();

    }
}
