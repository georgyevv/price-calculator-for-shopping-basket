namespace ShoppingBasket.Data.Models
{
    public class QuantityDiscount
    {
        public int Id { get; set; }

        public Product BuyingProduct { get; set; }

        public int Quantity { get; set; }

        public Product DiscountedProduct { get; set; }

        public float DiscountPercentage { get; set; }

        public bool IsActive { get; set; }
    }
}
