namespace ShoppingBasket.Application.Commands.Discount
{
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Services.Discount;
    using System.Collections.Generic;

    public class GetDiscountsCommand : BaseCommand
    {
        public IEnumerable <DiscountResult> Result;
        public IEnumerable<BasketItem> Items;

        private readonly IDiscountService discountService;

        public GetDiscountsCommand(IDiscountService discountService)
        {
            this.discountService = discountService;
        }

        public override void Execute()
        {
            this.Result = this.discountService.GetDiscounts(this.Items);
        }
    }
}
