namespace ShoppingBasket.Data.Models
{
    using System;

    public class TimespanDiscount
    {
        public int Id { get; set; }

        public Product BuyingProduct { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public Product DiscountedProduct { get; set; }

        public float DiscountPercentage { get; set; }

        public bool IsActive { get; set; }
    }
}
