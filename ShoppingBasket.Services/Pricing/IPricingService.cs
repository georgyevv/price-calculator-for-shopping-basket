namespace ShoppingBasket.Services.Pricing
{
    using ShoppingBasket.Common.Models;
    using System.Collections.Generic;

    public interface IPricingService
    {
        decimal Calculate(IEnumerable<BasketItem> items);
    }
}
