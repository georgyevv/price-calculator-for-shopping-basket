namespace ShoppingBasket.Services
{
    using ShoppingBasket.Data;

    public abstract class BaseService
    {
        protected IShoppingBasketDbContext dbContext;

        public BaseService(IShoppingBasketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
