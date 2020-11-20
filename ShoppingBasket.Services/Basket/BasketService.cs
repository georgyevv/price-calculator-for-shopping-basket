namespace ShoppingBasket.Services.Basket
{
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BasketService : BaseService, IBasketService
    {
        public BasketService(IShoppingBasketDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<BasketItem> GetItems(IEnumerable<string> productNames)
        {
            if (productNames == null)
            {
                throw new ArgumentNullException(nameof(productNames));
            }

            List<BasketItem> basketItems = dbContext.Products
                .Where(p => productNames.Contains(p.Name))
                .Select(p => new BasketItem()
                {
                    Product = new BasketItemProduct()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        PluralName = p.PluralName,
                        Price = p.Price,
                    },
                    Quantity = productNames.Where(c => c == p.Name).Count(),
                })
                .ToList();

            return basketItems;
        }
    }
}
