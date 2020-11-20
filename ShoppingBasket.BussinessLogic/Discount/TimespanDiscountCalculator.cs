namespace ShoppingBasket.BussinessLogic.Discount
{
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Common.Utils;
    using ShoppingBasket.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TimespanDiscountCalculator : ITimespanDiscountCalculator
    {
        private readonly IDateTimeUtil dateTimeUtil;

        public TimespanDiscountCalculator(IDateTimeUtil dateTimeUtil)
        {
            this.dateTimeUtil = dateTimeUtil;
        }

        public DiscountResult Calculate(IEnumerable<BasketItem> items, TimespanDiscount model)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            DateTime currentDate = this.dateTimeUtil.Now();
            if (currentDate < model.From || currentDate > model.To)
            {
                return null;
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

            decimal currentPrice = productForDiscount.Product.Price;
            decimal discountedPrice = (currentPrice * (decimal)model.DiscountPercentage) * productForDiscount.Quantity;

            DiscountResult result = new DiscountResult();
            result.DiscountedPrice = discountedPrice;
            result.ProductPluralName = productForDiscount.Product.PluralName;
            result.DiscountPercentage = model.DiscountPercentage;

            return result;
        }
    }
}
