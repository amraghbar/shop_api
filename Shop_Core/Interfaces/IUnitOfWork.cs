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
        ICartRepository CartRepository { get; }
        IUnitRepository UnitRepository { get; }
        IStoreRepository StoreRepository { get; }
        IOrderRepository OrderRepository { get; }
        IMainGroupRepository MainGroupRepository { get; }
        ISubGroupRepository SubGroupRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        Task<int> saveAsync();

    }
}
