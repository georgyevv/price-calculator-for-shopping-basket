namespace ShoppingBasket.Services.Tests.Discount
{
    using Moq;
    using ShoppingBasket.BussinessLogic.Discount;
    using ShoppingBasket.Common.Models;
    using ShoppingBasket.Data;
    using ShoppingBasket.Data.Models;
    using ShoppingBasket.Services.Discount;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class DiscountServiceTests
    {
        private readonly IShoppingBasketDbContext dbContext = new ShoppingBasketDbContext();
        private readonly List<Product> products;
        private IDiscountService discountService;

        public DiscountServiceTests()
        {
            this.products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Beans", PluralName = "Beans", Price = 0.65m },
                new Product() { Id = 2, Name = "Bread", PluralName = "Breads", Price = 0.8m },
                new Product() { Id = 3, Name = "Milk", PluralName = "Milk", Price = 1.3m },
                new Product() { Id = 4, Name = "Apple", PluralName = "Apples", Price = 1.0m },
            };

            this.dbContext.Products = products;

            List<TimespanDiscount> timeSpanDiscounts = new List<TimespanDiscount>()
            {
                new TimespanDiscount() { Id = 1, BuyingProduct = products[3], From = new DateTime(2020, 11, 17), To = new DateTime(2020, 11, 20), DiscountedProduct = products[3], DiscountPercentage = 0.1f, IsActive = true },
            };
            this.dbContext.TimeSpanDiscounts = timeSpanDiscounts;

            List<QuantityDiscount> quantityDiscounts = new List<QuantityDiscount>()
            {
                new QuantityDiscount() { Id = 1, BuyingProduct = products[0], Quantity = 2, DiscountedProduct = products[1], DiscountPercentage = 0.5f, IsActive = true },
            };
            this.dbContext.QuantityDiscounts = quantityDiscounts;
        }

        [Fact]
        public void GetDiscounts_ShouldReturnOnlyTimespanDiscount_WithCorrectProductsForTimespan()
        {
            // Arrange 
            DiscountResult timespanDiscountResult = new DiscountResult()
            {
                DiscountedPrice = 0.1m,
                DiscountPercentage = 10,
                ProductPluralName = "Apples",
            };
            Mock<ITimespanDiscountCalculator> mockTimespanDiscountCalculator = new Mock<ITimespanDiscountCalculator>();
            mockTimespanDiscountCalculator.Setup(x => x.Calculate(It.IsAny<IEnumerable<BasketItem>>(), It.IsAny<TimespanDiscount>())).Returns(timespanDiscountResult);

            Mock<IQuantityDiscountCalculator> mockQuantityDiscountCalculator = new Mock<IQuantityDiscountCalculator>();
            mockQuantityDiscountCalculator.Setup(x => x.Calculate(It.IsAny<IEnumerable<BasketItem>>(), It.IsAny<QuantityDiscount>())).Returns<DiscountResult>(null);

            this.discountService = new DiscountService(this.dbContext, mockTimespanDiscountCalculator.Object, mockQuantityDiscountCalculator.Object);

            List<BasketItem> items = new List<BasketItem>()
            {
                new BasketItem()
                {
                    Product = new BasketItemProduct() { Id = 4, Name = "Apple", PluralName = "Apples", Price = 1.0m },
                    Quantity = 1
                }
            };

            // Act
            IEnumerable<DiscountResult> discounts = this.discountService.GetDiscounts(items);

            // Assert
            Assert.Single(discounts);
            Assert.Collection(discounts, discount =>
            {
                Assert.Equal(timespanDiscountResult.DiscountedPrice, discount.DiscountedPrice);
                Assert.Equal(timespanDiscountResult.DiscountPercentage, discount.DiscountPercentage);
                Assert.Equal(timespanDiscountResult.ProductPluralName, discount.ProductPluralName);
            });
        }

        [Fact]
        public void GetDiscounts_ShouldReturnOnlyQuantityDiscount_WithCorrectProductsForQuantity()
        {
            // Arrange 
            Mock<ITimespanDiscountCalculator> mockTimespanDiscountCalculator = new Mock<ITimespanDiscountCalculator>();
            mockTimespanDiscountCalculator.Setup(x => x.Calculate(It.IsAny<IEnumerable<BasketItem>>(), It.IsAny<TimespanDiscount>())).Returns<DiscountResult>(null);

            DiscountResult quantityDiscountResult = new DiscountResult()
            {
                DiscountedPrice = 0.4m,
                DiscountPercentage = 50,
                ProductPluralName = "Beans",
            };
            Mock<IQuantityDiscountCalculator> mockQuantityDiscountCalculator = new Mock<IQuantityDiscountCalculator>();
            mockQuantityDiscountCalculator.Setup(x => x.Calculate(It.IsAny<IEnumerable<BasketItem>>(), It.IsAny<QuantityDiscount>())).Returns(quantityDiscountResult);

            this.discountService = new DiscountService(this.dbContext, mockTimespanDiscountCalculator.Object, mockQuantityDiscountCalculator.Object);

            List<BasketItem> items = new List<BasketItem>()
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
            IEnumerable<DiscountResult> discounts = this.discountService.GetDiscounts(items);

            // Assert
            Assert.Single(discounts);
            Assert.Collection(discounts, discount =>
            {
                Assert.Equal(quantityDiscountResult.DiscountedPrice, discount.DiscountedPrice);
                Assert.Equal(quantityDiscountResult.DiscountPercentage, discount.DiscountPercentage);
                Assert.Equal(quantityDiscountResult.ProductPluralName, discount.ProductPluralName);
            });
        }

        [Fact]
        public void GetDiscounts_ShouldThrowException_WithNullProducts()
        {
            // Arrange 
            Mock<ITimespanDiscountCalculator> mockTimespanDiscountCalculator = new Mock<ITimespanDiscountCalculator>();
            mockTimespanDiscountCalculator.Setup(x => x.Calculate(It.IsAny<IEnumerable<BasketItem>>(), It.IsAny<TimespanDiscount>())).Returns<DiscountResult>(null);

            Mock<IQuantityDiscountCalculator> mockQuantityDiscountCalculator = new Mock<IQuantityDiscountCalculator>();
            mockQuantityDiscountCalculator.Setup(x => x.Calculate(It.IsAny<IEnumerable<BasketItem>>(), It.IsAny<QuantityDiscount>())).Returns<DiscountResult>(null);

            this.discountService = new DiscountService(this.dbContext, mockTimespanDiscountCalculator.Object, mockQuantityDiscountCalculator.Object);

            List<BasketItem> basketItems = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => this.discountService.GetDiscounts(basketItems));
        }
    }
}
