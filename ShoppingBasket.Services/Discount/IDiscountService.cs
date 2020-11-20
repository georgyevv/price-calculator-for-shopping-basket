namespace ShoppingBasket.Services.Discount
{
    using ShoppingBasket.Common.Models;
    using System.Collections.Generic;

    public interface IDiscountService
    {
        IEnumerable<DiscountResult> GetDiscounts(IEnumerable<BasketItem> items);
    }
}
