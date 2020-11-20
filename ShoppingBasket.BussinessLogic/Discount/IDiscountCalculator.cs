namespace ShoppingBasket.BussinessLogic.Discount
{
    using ShoppingBasket.Common.Models;
    using System.Collections.Generic;

    public interface IDiscountCalculator<T>
    {
        DiscountResult Calculate(IEnumerable<BasketItem> item, T model);
    }
}
