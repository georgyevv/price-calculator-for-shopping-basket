namespace ShoppingBasket.Services.Basket
{
    using ShoppingBasket.Common.Models;
    using System.Collections.Generic;

    public interface IBasketService
    {
        IEnumerable<BasketItem> GetItems(IEnumerable<string> productNames);
    }
}
