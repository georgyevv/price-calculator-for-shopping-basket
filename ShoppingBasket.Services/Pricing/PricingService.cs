namespace ShoppingBasket.Services.Pricing
{
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PricingService : BaseService, IPricingService
    {
        public PricingService(IShoppingBasketDbContext dbContext)
            : base(dbContext)
        {
        }

        public decimal Calculate(IEnumerable<BasketItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            decimal totalPrice = items.Sum(p => p.Product.Price * p.Quantity);

            return totalPrice;
        }
    }
}
