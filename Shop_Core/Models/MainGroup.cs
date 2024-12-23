using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Models
{
    public class MainGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Items> Items { get; set; } = new HashSet<Items>();
        public ICollection<SubGroup> SubGroup{ get; set; } = new HashSet<SubGroup>();


    }
}
