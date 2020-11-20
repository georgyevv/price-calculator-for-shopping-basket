namespace ShoppingBasket.Services.Discount
{
    using ShoppingBasket.BussinessLogic.Discount;
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Data;
    using ShoppingBasket.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DiscountService : BaseService, IDiscountService
    {
        private readonly ITimespanDiscountCalculator timeSpanDiscountCalculator;
        private readonly IQuantityDiscountCalculator quantityDiscountCalculator;

        public DiscountService(
            IShoppingBasketDbContext dbContext,
            ITimespanDiscountCalculator timeSpanDiscountCalculator,
            IQuantityDiscountCalculator quantityDiscountCalculator)
            : base(dbContext)
        {
            this.timeSpanDiscountCalculator = timeSpanDiscountCalculator;
            this.quantityDiscountCalculator = quantityDiscountCalculator;
        }

        public IEnumerable<DiscountResult> GetDiscounts(IEnumerable<BasketItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            List<DiscountResult> discounts = new List<DiscountResult>();

            this.GetQuantityDiscounts(items, discounts);
            this.GetTimespanDiscounts(items, discounts);

            return discounts;
        }

        private void GetTimespanDiscounts(IEnumerable<BasketItem> items, List<DiscountResult> discounts)
        {
            IEnumerable<TimespanDiscount> timeSpanDiscounts = this.dbContext.TimeSpanDiscounts.Where(d => d.IsActive);
            foreach (TimespanDiscount timeSpanDiscount in timeSpanDiscounts)
            {
                DiscountResult discountResult = this.timeSpanDiscountCalculator.Calculate(items, timeSpanDiscount);
                if (discountResult != null)
                {
                    discounts.Add(discountResult);
                }
            }
        }

        private void GetQuantityDiscounts(IEnumerable<BasketItem> items, List<DiscountResult> discounts)
        {
            IEnumerable<QuantityDiscount> quantityDiscounts = this.dbContext.QuantityDiscounts.Where(d => d.IsActive);
            foreach (QuantityDiscount quantityDiscount in quantityDiscounts)
            {
                DiscountResult discountResult = this.quantityDiscountCalculator.Calculate(items, quantityDiscount);
                if (discountResult != null)
                {
                    discounts.Add(discountResult);
                }
            }
        }
    }
}
