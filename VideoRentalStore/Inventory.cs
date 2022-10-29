using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace VideoRentalStore
{
    sealed partial class Inventory
    {
        // Fields
        private List<Film> rentedFilms = new List<Film>();


        // Properties
        public ReadOnlyCollection<Film> RentedFilms => rentedFilms.AsReadOnly();

        // Methods
        public void AddFilm(Film film) => rentedFilms.Add(film);

        public Film FindFilm(string name)
        {
            Film found = rentedFilms.Find((match) => name == match.Name);
            return found;
        }
        public void RemoveFIlm(string film)
        {
            Film found = FindFilm(film);
            rentedFilms.Remove(found);
        }

        private int PriceSum() => rentedFilms.Sum((f) => f.Price);

        public void PrintFilms(bool sum)
        {
            Console.WriteLine();
            foreach (Film f in rentedFilms)
            {
                Console.WriteLine(f);
            }
            if (sum) Console.WriteLine($"Total Price : {PriceSum()} EUR");
            Console.WriteLine();
        }
        public void DayPass()
        {
            Console.WriteLine("Day passed");
            foreach (Film film in rentedFilms)
            {
                film.DaysHeld++;
            }
        }

    }

}
