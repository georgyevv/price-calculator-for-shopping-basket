namespace ShoppingBasket.BussinessLogic.Tests.Discount
{
    using Moq;
    using ShoppingBasket.BussinessLogic.Discount;
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Common.Utils;
    using ShoppingBasket.Data.Models;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class TimespanDiscountCalculatorTests
    {
        [Fact]
        public void Calculate_ShouldReturnDiscountResult_WithCorrectProductsAndModel()
        {
            // Arrange 
            Mock<IDateTimeUtil> mockDateTimeUtil = new Mock<IDateTimeUtil>();
            mockDateTimeUtil.Setup(x => x.Now()).Returns(new DateTime(2020, 10, 15));
            ITimespanDiscountCalculator timespanDiscountCalculator = new TimespanDiscountCalculator(mockDateTimeUtil.Object);
            List<Product> products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Beans", PluralName = "Beans", Price = 0.65m },
                new Product() { Id = 2, Name = "Bread", PluralName = "Breads", Price = 0.8m },
                new Product() { Id = 3, Name = "Milk", PluralName = "Milk", Price = 1.3m },
                new Product() { Id = 4, Name = "Apple", PluralName = "Apples", Price = 1.0m },
            };
            TimespanDiscount timespanDiscount = new TimespanDiscount()
            {
                Id = 1,
                BuyingProduct = products[3],
                From = new DateTime(2020, 10, 10),
                To = new DateTime(2020, 10, 20),
                DiscountedProduct = products[3],
                DiscountPercentage = 0.1f,
                IsActive = true
            };
            IEnumerable<BasketItem> items = new List<BasketItem>()
            {
                new BasketItem()
                {
                    Product = new BasketItemProduct() { Id = 4, Name = "Apple", PluralName = "Apples", Price = 1.0m },
                    Quantity = 1
                },
            };

            // Act 
            DiscountResult discountResult = timespanDiscountCalculator.Calculate(items, timespanDiscount);

            // Assert
            Assert.Equal(0.1f, discountResult.DiscountPercentage);
            Assert.Equal(0.10m, discountResult.DiscountedPrice);
            Assert.Equal("Apples", discountResult.ProductPluralName);
        }

        [Fact]
        public void Calculate_ShouldThrowException_WithNullProducts()
        {
            // Arrange 
            IDateTimeUtil dateTimeUtil = new DateTimeUtil();
            ITimespanDiscountCalculator timespanDiscountCalculator = new TimespanDiscountCalculator(dateTimeUtil);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => timespanDiscountCalculator.Calculate(null, null));
        }
    }
}
