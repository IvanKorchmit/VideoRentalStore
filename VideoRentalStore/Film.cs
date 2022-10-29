using System;
using System.Collections.Generic;
namespace VideoRentalStore
{

    public partial class Film
    {
        #region Constants
        private const int PREMIUM_PRICE = 4;
        private const int BASIC_PRICE = 3;

        private const int OLD_FILM_DAYS = 5;
        private const int REGULAR_FILM_DAYS = 3;
        #endregion
        #region Properties
        public string Name { get; private set; }
        public int Price
        {
            get
            {
                return Rental switch
                {
                    RentalType.RegularRental => DaysRent <= REGULAR_FILM_DAYS ? BASIC_PRICE : DaysRent - REGULAR_FILM_DAYS + DaysRent + DaysRent - REGULAR_FILM_DAYS,

                    RentalType.NewRelease => PREMIUM_PRICE * DaysRent,

                    RentalType.OldFilm => DaysRent <= OLD_FILM_DAYS ? BASIC_PRICE : DaysRent - OLD_FILM_DAYS + DaysRent,
                    // In case of expanding enum types... We will get explicitly thrown exception. Explicit > Implicit
                    _ => throw new ArgumentException($"Unknown enum {Rental}"),
                };
            }
        }
        public int DaysRent { get; set; }
        private int daysHeld;
        private bool isLate;
        public bool IsLate => isLate;

        private readonly int oldDays;
        public int DaysHeld
        {
            get
            {
                return daysHeld;
            }
            set
            {
                // We cannot assign value lesser than current
                if (value > 0 && value < daysHeld) return;
                daysHeld = value;
                if (daysHeld > DaysRent)
                {
                    isLate = true;
                    DaysRent = daysHeld;
                }
            }
        }
        public void ResetDaysRent()
        {
            DaysRent = 0;
            daysHeld = 0;
        }
        public RentalType Rental { get; set; }
        #endregion
        private readonly Dictionary<RentalType, string> rentalToString;
        public Film(string name,
                    RentalType filmType = RentalType.OldFilm,
                    int days = 0)
        {
            // Map RentalType enum to human readable representation
            rentalToString = new Dictionary<RentalType, string>()
            {
                { RentalType.NewRelease, "New Release"},
                { RentalType.OldFilm, "Old film"},
                { RentalType.RegularRental, "Regular rental"}
            };

            Rental = filmType;
            oldDays = days;
            Name = name;
            DaysRent = days;
        }
        #region Operator Overloading
        public static bool operator ==(Film a, Film b)
        {
            return (a?.Name ?? null) == (b?.Name ?? null);
        }

        public static bool operator !=(Film a, Film b)
        {
            return a.Name != b.Name;
        }
        #endregion


        #region Overrides
        public override string ToString()
        {
            if (DaysRent == 0)
            {
                return $"{Name}({rentalToString[Rental]})";
            }
            if (!isLate)
            {
                return $"{Name}({rentalToString[Rental]}) {DaysRent} days {Price} EUR";
            }
            return $"{Name}({rentalToString[Rental]}) {DaysHeld - oldDays + 1} extra days {Price} EUR";
        }
        public override bool Equals(object o)
        {
            return base.Equals(o as Film);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, DaysRent);
        }
        #endregion
    }
}
