namespace ShoppingBasket.Data
{
    using ShoppingBasket.Data.Models;
    using System.Collections.Generic;

    public interface IShoppingBasketDbContext
    {
        IEnumerable<Product> Products { get; set; }

        IEnumerable<QuantityDiscount> QuantityDiscounts { get; set; }

        IEnumerable<TimespanDiscount> TimeSpanDiscounts { get; set; }
    }
}
