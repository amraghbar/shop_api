using Shop_Core.DTOS.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Core.Interfaces
{
    public interface ICartRepository
    {
        //تستخدم لإضافة كمية معينة من منتج محدد إلى سلة المستخدم.

        Task<string> AddBulkQuantityToCartAsync(CartItemDTO cartItemDTO, int? userId);
        //تستخدم لإضافة كمية واحدة فقط من منتج محدد إلى سلة المستخدم.

        Task<string> AddOneQuantityToCartAsync(CartItemDTO cartItemDTO, int? userId);
        //تستخدم لاسترجاع جميع العناصر الموجودة في سلة المستخدم.

        Task<IEnumerable<UserCartItemsDTO>> GetAllItemsFromCart(int? customerId);

        // إزالة عنصر معين من السلة
        Task<string> RemoveItemFromCartAsync(int itemId, int? userId);

        // إفراغ السلة بالكامل
        Task<string> ClearCartAsync(int? userId);

        // تحديث كمية عنصر في السلة
        Task<string> UpdateCartItemQuantityAsync(int itemId, int quantity, int? userId);

        // حساب الإجمالي الكلي للسلة
        Task<decimal> GetCartTotalAsync(int? userId);

        // فحص إذا كانت السلة فارغة
        Task<bool> IsCartEmptyAsync(int? userId);
    }

}
