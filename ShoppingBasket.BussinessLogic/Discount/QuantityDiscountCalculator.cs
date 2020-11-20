namespace ShoppingBasket.BussinessLogic.Discount
{
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Common.Utils;
    using ShoppingBasket.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class QuantityDiscountCalculator : IQuantityDiscountCalculator
    {
        public DiscountResult Calculate(IEnumerable<BasketItem> items, QuantityDiscount model)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            BasketItem buyingProduct = items.FirstOrDefault(p => p.Product.Id == model.BuyingProduct.Id);
            if (buyingProduct == null)
            {
                return null;
            }

            BasketItem productForDiscount = items.FirstOrDefault(p => p.Product.Id == model.DiscountedProduct.Id);
            if (productForDiscount == null)
            {
                return null;
            }

            int quantity = buyingProduct.Quantity / model.Quantity;
            int numberOfDiscounts = quantity > productForDiscount.Quantity ? productForDiscount.Quantity : quantity;
            if (numberOfDiscounts == 0)
            {
                return null;
            }

            decimal currentPrice = productForDiscount.Product.Price;
            decimal discountedPrice = (currentPrice * (decimal)model.DiscountPercentage) * numberOfDiscounts;

            DiscountResult result = new DiscountResult();
            result.DiscountedPrice = discountedPrice;
            result.ProductPluralName = productForDiscount.Product.PluralName;
            result.DiscountPercentage = model.DiscountPercentage;

            return result;
        }
    }
}
