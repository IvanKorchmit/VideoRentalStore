namespace VideoRentalStore
{
    public abstract class RentalBase
    {
        public abstract int BonusPoint { get; }
        public abstract int CalculatePrice(int days);
    }
    public abstract class BasicRental : RentalBase
    {
        protected const int BASIC_PRICE = 3;
    }



    public class Old : BasicRental 
    {
        private const int OLD_FILM_DAYS = 5;

        public override int BonusPoint => 1;

        public override int CalculatePrice(int days)
        {
            return days <= OLD_FILM_DAYS ? BASIC_PRICE : days - OLD_FILM_DAYS + days;
        }
        public override string ToString()
        {
            return "Old film";
        }
    }
    public class RegularRental : BasicRental
    {
        private const int REGULAR_FILM_DAYS = 3;

        public override int BonusPoint => 1;

        public override int CalculatePrice(int days)
        {
            return days <= REGULAR_FILM_DAYS ? BASIC_PRICE : days - REGULAR_FILM_DAYS + days + days - REGULAR_FILM_DAYS;
        }
    }
    public class NewRelease : RentalBase
    {
        private const int PREMIUM_PRICE = 4;

        public override int BonusPoint => 2;

        public override int CalculatePrice(int days)
        {
            return PREMIUM_PRICE * days;
        }
    }

}
