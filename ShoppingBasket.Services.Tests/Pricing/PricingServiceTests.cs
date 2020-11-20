namespace ShoppingBasket.Services.Tests.Pricing
{
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Data;
    using ShoppingBasket.Data.Models;
    using ShoppingBasket.Services.Pricing;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class PricingServiceTests
    {
        private readonly IShoppingBasketDbContext dbContext = new ShoppingBasketDbContext();
        private readonly List<Product> products;
        private readonly IPricingService pricingService;

        public PricingServiceTests()
        {
            this.products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Beans", PluralName = "Beans", Price = 0.65m },
                new Product() { Id = 2, Name = "Bread", PluralName = "Breads", Price = 0.8m },
                new Product() { Id = 3, Name = "Milk", PluralName = "Milk", Price = 1.3m },
                new Product() { Id = 4, Name = "Apple", PluralName = "Apples", Price = 1.0m },
            };

            this.dbContext.Products = products;
            this.pricingService = new PricingService(this.dbContext);
        }

        [Fact]
        public void Calculate_ShouldReturnTotalPrice_WithCorrectProducts()
        {
            // Arrange 
            List<BasketItem> basketItems = new List<BasketItem>()
            {
                new BasketItem()
                {
                    Product = new BasketItemProduct() { Id = 4, Name = "Apple", PluralName = "Apples", Price = 1.0m },
                    Quantity = 2
                },
                new BasketItem()
                {
                    Product = new BasketItemProduct() { Id = 2, Name = "Bread", PluralName = "Breads", Price = 0.8m },
                    Quantity = 1
                },
            };

            // Act 
            decimal totalPrice = this.pricingService.Calculate(basketItems);

            // Assert
            Assert.Equal(2.8m, totalPrice);
        }

        [Fact]
        public void Calculate_ShouldReturn0TotalPrice_WithZeroProducts()
        {
            // Arrange 
            List<BasketItem> basketItems = new List<BasketItem>();

            // Act 
            decimal totalPrice = this.pricingService.Calculate(basketItems);

            // Assert
            Assert.Equal(0, totalPrice);
        }

        [Fact]
        public void Calculate_ShouldThrowException_WithNullProducts()
        {
            // Arrange 
            List<BasketItem> basketItems = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => this.pricingService.Calculate(basketItems));
        }
    }
}
