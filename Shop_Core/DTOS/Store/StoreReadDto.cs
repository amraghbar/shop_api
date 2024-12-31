using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.DTOS.Store
{
    public class StoreReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GovName { get; set; }
        public string CityName { get; set; }
    }
}
