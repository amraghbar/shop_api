using Shop_Core.DTOS;
using Shop_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Interfaces
{
    public interface IUnitRepository
    {
        IEnumerable<UD> GetAll();
        UD GetById(int id);
        void Add(UD unit);
        void Update(UD unit);
        void Delete(int id);
    }
}
