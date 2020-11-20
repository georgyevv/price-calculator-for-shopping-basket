namespace ShoppingBasket.BussinessLogic.Tests.Discount
{
    using ShoppingBasket.BussinessLogic.Discount;
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Data.Models;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class QuantityDiscountCalculatorTests
    {
        [Fact]
        public void Calculate_ShouldReturnDiscountResult_WithCorrectProductsAndModel()
        {
            // Arrange 
            IQuantityDiscountCalculator timespanDiscountCalculator = new QuantityDiscountCalculator();
            List<Product> products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Beans", PluralName = "Beans", Price = 0.65m },
                new Product() { Id = 2, Name = "Bread", PluralName = "Breads", Price = 0.8m },
                new Product() { Id = 3, Name = "Milk", PluralName = "Milk", Price = 1.3m },
                new Product() { Id = 4, Name = "Apple", PluralName = "Apples", Price = 1.0m },
            };
            QuantityDiscount quantityDiscount = new QuantityDiscount()
            {
                Id = 1,
                BuyingProduct = products[0],
                Quantity = 2,
                DiscountedProduct = products[1],
                DiscountPercentage = 0.5f,
                IsActive = true
            };
            IEnumerable<BasketItem> items = new List<BasketItem>()
            {
                new BasketItem()
                {
                    Product = new BasketItemProduct() { Id = 1, Name = "Beans", PluralName = "Beans", Price = 0.65m },
                    Quantity = 2
                },
                new BasketItem()
                {
                    Product = new BasketItemProduct() { Id = 2, Name = "Bread", PluralName = "Breads", Price = 0.8m },
                    Quantity = 1
                },
            };

            // Act 
            DiscountResult discountResult = timespanDiscountCalculator.Calculate(items, quantityDiscount);

            // Assert
            Assert.Equal(0.5, discountResult.DiscountPercentage);
            Assert.Equal(0.40m, discountResult.DiscountedPrice);
            Assert.Equal("Breads", discountResult.ProductPluralName);
        }

        [Fact]
        public void Calculate_ShouldThrowException_WithNullProducts()
        {
            // Arrange 
            IQuantityDiscountCalculator timespanDiscountCalculator = new QuantityDiscountCalculator();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => timespanDiscountCalculator.Calculate(null, null));
        }
    }
}
