namespace ShoppingBasket.Application.Commands.Product
{
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Services.Basket;
    using System.Collections.Generic;

    public class GetProductsByNameCommand : BaseCommand
    {
        public IEnumerable<BasketItem> Result;
        public IEnumerable<string> ProductNames;

        private readonly IBasketService productService;

        public GetProductsByNameCommand(IBasketService pricingService)
        {
            this.productService = pricingService;
        }

        public override void Execute()
        {
            this.Result = this.productService.GetItems(this.ProductNames);
        }
    }
}
