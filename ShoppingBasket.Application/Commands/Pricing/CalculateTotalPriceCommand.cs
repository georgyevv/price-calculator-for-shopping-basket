namespace ShoppingBasket.Application.Commands.Pricing
{
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Services.Pricing;
    using System.Collections.Generic;

    public class CalculateTotalPriceCommand : BaseCommand
    {
        public decimal Result;
        public IEnumerable<BasketItem> Items;

        private readonly IPricingService pricingService;

        public CalculateTotalPriceCommand(IPricingService pricingService)
        {
            this.pricingService = pricingService;
        }

        public override void Execute()
        {
            this.Result = this.pricingService.Calculate(this.Items);
        }
    }
}