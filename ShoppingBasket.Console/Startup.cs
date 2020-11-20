namespace ShoppingBasket.Console
{
    using ShoppingBasket.Application.Commands;
    using ShoppingBasket.Application.Commands.Discount;
    using ShoppingBasket.Application.Commands.Pricing;
    using ShoppingBasket.Application.Commands.Product;
    using ShoppingBasket.BussinessLogic.Discount;
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Common.Utils;
    using ShoppingBasket.Data;
    using ShoppingBasket.Data.Models;
    using ShoppingBasket.Services.Basket;
    using ShoppingBasket.Services.Discount;
    using ShoppingBasket.Services.Pricing;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    public class Startup
    {
        private static readonly IShoppingBasketDbContext dbContext = new ShoppingBasketDbContext();
        private static readonly IDictionary<string, ICommand> AppCommandsByName = new Dictionary<string, ICommand>();

        public static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB", false);

            SeedCommands();
            SeedDatabase();

            string input = Console.ReadLine();
            string[] inputArgs = input.Split(' ');
            string commandName = inputArgs[0].ToLower();
            string[] arguments = inputArgs.Skip(1).ToArray();

            if (!AppCommandsByName.ContainsKey(commandName))
            {
                Console.WriteLine("Command not supported!");
                return;
            }

            ICommandInvoker commandInvoker = new CommandInvoker();
            if (commandName == "pricecalculator")
            {
                CalculatePrice(commandName, arguments, commandInvoker);
            }
        }

        private static void CalculatePrice(string commandName, string[] arguments, ICommandInvoker commandInvoker)
        {
            IBasketService basketService = new BasketService(dbContext);
            GetProductsByNameCommand getProductByNameCommand = new GetProductsByNameCommand(basketService);
            getProductByNameCommand.ProductNames = arguments;
            commandInvoker.ExecuteCommand(getProductByNameCommand);
            IEnumerable<BasketItem> products = getProductByNameCommand.Result;
            if (products.Count() == 0)
            {
                Console.WriteLine("Given products are not available!");
                return;
            }

            CalculateTotalPriceCommand calculateTotalPriceCommand = AppCommandsByName[commandName] as CalculateTotalPriceCommand;
            calculateTotalPriceCommand.Items = products;
            commandInvoker.ExecuteCommand(calculateTotalPriceCommand);

            decimal basketItemsTotalPrice = calculateTotalPriceCommand.Result;
            Console.WriteLine($"Subtotal: {basketItemsTotalPrice:c}");

            IDateTimeUtil dateTimeUtil = new DateTimeUtil();
            ITimespanDiscountCalculator timespanDiscountCalculator = new TimespanDiscountCalculator(dateTimeUtil);
            IQuantityDiscountCalculator quantityDiscountCalculator = new QuantityDiscountCalculator();
            IDiscountService discountService = new DiscountService(dbContext, timespanDiscountCalculator, quantityDiscountCalculator);
            GetDiscountsCommand getDiscountCommand = new GetDiscountsCommand(discountService);
            getDiscountCommand.Items = products;
            commandInvoker.ExecuteCommand(getDiscountCommand);

            IEnumerable<DiscountResult> discounts = getDiscountCommand.Result;
            if (discounts.Count() > 0)
            {
                foreach (DiscountResult discount in discounts)
                {
                    Console.WriteLine($"{discount.ProductPluralName} {discount.DiscountPercentage:p0} off: -{discount.DiscountedPrice:c}");
                }
            }
            else
            {
                Console.WriteLine("(No offers available)");
            }

            decimal discountPrice = discounts.Sum(s => s.DiscountedPrice);
            decimal totalPrice = basketItemsTotalPrice - discountPrice;
            Console.WriteLine($"Total price: {totalPrice:c}");
        }

        private static void SeedCommands()
        {
            IPricingService pricingService = new PricingService(dbContext);
            AppCommandsByName["pricecalculator"] = new CalculateTotalPriceCommand(pricingService);
        }

        private static void SeedDatabase()
        {
            List<Product> products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Beans", PluralName = "Beans", Price = 0.65m },
                new Product() { Id = 2, Name = "Bread", PluralName = "Breads", Price = 0.8m },
                new Product() { Id = 3, Name = "Milk", PluralName = "Milk", Price = 1.3m },
                new Product() { Id = 4, Name = "Apple", PluralName = "Apples", Price = 1.0m },
            };
            dbContext.Products = products;

            List<TimespanDiscount> timeSpanDiscounts = new List<TimespanDiscount>()
            {
                new TimespanDiscount() { Id = 1, BuyingProduct = products[3], From = new DateTime(2020, 11, 17), To = new DateTime(2020, 11, 20), DiscountedProduct = products[3], DiscountPercentage = 0.1f, IsActive = true },
            };
            dbContext.TimeSpanDiscounts = timeSpanDiscounts;

            List<QuantityDiscount> quantityDiscounts = new List<QuantityDiscount>()
            {
                new QuantityDiscount() { Id = 1, BuyingProduct = products[0], Quantity = 2, DiscountedProduct = products[1], DiscountPercentage = 0.5f, IsActive = true },
            };
            dbContext.QuantityDiscounts = quantityDiscounts;
        }
    }
}
