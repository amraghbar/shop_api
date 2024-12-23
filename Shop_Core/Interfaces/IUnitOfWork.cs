using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Interfaces
{
    public interface IUnitOfWork
    {

        IItemsRepository ItemsRepository { get; }
        Task<int> saveAsync();

    }
}
