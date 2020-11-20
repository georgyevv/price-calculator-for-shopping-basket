namespace ShoppingBasket.Data
{
    using ShoppingBasket.Data.Models;
    using System.Collections.Generic;

    public class ShoppingBasketDbContext : IShoppingBasketDbContext
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        public IEnumerable<QuantityDiscount> QuantityDiscounts { get; set; } = new List<QuantityDiscount>();

        public IEnumerable<TimespanDiscount> TimeSpanDiscounts { get; set; } = new List<TimespanDiscount>();
    }
}