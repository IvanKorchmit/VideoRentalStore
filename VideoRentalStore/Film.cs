using System;
using System.Collections.Generic;
namespace VideoRentalStore
{

    public partial class Film
    {
        public string Name { get; private set; }
        public int Price
        {
            get
            {
                return Rental.CalculatePrice(DaysRent);
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
        public RentalBase Rental { get; set; }


        public Film(string name,
                    RentalBase filmType = null,
                    int days = 0)
        {
            // Map RentalType enum to human readable representation

            Rental = filmType;
            oldDays = days;
            Name = name;
            DaysRent = days;
        }
        public static bool operator ==(Film a, Film b)
        {
            return (a?.Name ?? null) == (b?.Name ?? null);
        }

        public static bool operator !=(Film a, Film b)
        {
            return !(a == b);
        }


        public override string ToString()
        {
            if (DaysRent == 0)
            {
                return $"{Name}({Rental})";
            }
            if (!isLate)
            {
                return $"{Name}({Rental}) {DaysRent} days {Price} EUR";
            }
            return $"{Name}({Rental}) {DaysHeld - oldDays + 1} extra days {Price} EUR";
        }
        public override bool Equals(object o)
        {
            return base.Equals(o as Film);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, DaysRent);
        }
    }
}
