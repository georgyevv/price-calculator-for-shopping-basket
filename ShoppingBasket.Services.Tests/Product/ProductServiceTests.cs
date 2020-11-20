namespace ShoppingBasket.Services.Tests.Product
{
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Data;
    using ShoppingBasket.Data.Models;
    using ShoppingBasket.Services.Basket;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class ProductServiceTests
    {
        private readonly IShoppingBasketDbContext dbContext = new ShoppingBasketDbContext();
        private readonly IBasketService basketService;

        public ProductServiceTests()
        {
            List<Product> products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Beans", PluralName = "Beans", Price = 0.65m },
                new Product() { Id = 2, Name = "Bread", PluralName = "Breads", Price = 0.8m },
                new Product() { Id = 3, Name = "Milk", PluralName = "Milk", Price = 1.3m },
                new Product() { Id = 4, Name = "Apple", PluralName = "Apples", Price = 1.0m },
            };

            this.dbContext.Products = products;
            this.basketService = new BasketService(this.dbContext);
        }

        [Fact]
        public void GetBasketItems_ShouldReturnProducts_WithCorrectProductNames()
        {
            // Arrange 
            string[] productNames = new string[] { "Beans", "Milk" };

            // Act 
            IEnumerable<BasketItem> items = this.basketService.GetItems(productNames);

            // Assert
            Assert.Equal(productNames.Length, items.Count());
            Assert.Collection(items,
                item => Assert.Equal(productNames[0], item.Product.Name),
                item => Assert.Equal(productNames[1], item.Product.Name)
            );
        }

        [Fact]
        public void GetBasketItems_ShouldReturnEmptyCollection_WithMissingProductNames()
        {
            // Arrange 
            string[] productNames = new string[] { "Test", "Banana" };

            // Act 
            IEnumerable<BasketItem> items = this.basketService.GetItems(productNames);

            // Assert
            Assert.Empty(items);
        }

        [Fact]
        public void GetBasketItems_ShouldReturnEmptyCollection_WithZeroProductNames()
        {
            // Arrange 
            string[] productNames = new string[] { };

            // Act 
            IEnumerable<BasketItem> items = this.basketService.GetItems(productNames);

            // Assert
            Assert.Empty(items);
        }

        [Fact]
        public void GetBasketItems_ShouldThrowException_WithNullProductNames()
        {
            // Arrange 
            string[] productNames = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => this.basketService.GetItems(productNames));
        }
    }
}
