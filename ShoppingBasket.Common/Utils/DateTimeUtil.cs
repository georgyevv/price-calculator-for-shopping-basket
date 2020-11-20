namespace ShoppingBasket.Common.Utils
{
    using System;

    public class DateTimeUtil : IDateTimeUtil
    {
        public DateTime Now() => DateTime.Now;
    }
}
