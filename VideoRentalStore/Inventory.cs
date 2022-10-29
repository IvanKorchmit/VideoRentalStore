using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace VideoRentalStore
{
    public class Inventory
    {
        private readonly List<Film> stock = new List<Film>();


        public ReadOnlyCollection<Film> RentedFilms => stock.AsReadOnly();

        public void AddFilm(Film film) => stock.Add(film);

        public Film FindFilm(string name)
        {
            Film found = stock.Find((match) => name == match.Name);
            return found;
        }
        public void RemoveFIlm(string film)
        {
            Film found = FindFilm(film);
            stock.Remove(found);
        }

        private int PriceSum() => stock.Sum((f) => f.Price);
        /// <param name="sum">Should this method also print total cost?</param>
        public void PrintFilms(bool sum)
        {
            Console.WriteLine();
            foreach (Film f in stock)
            {
                Console.WriteLine(f);
            }
            if (sum) Console.WriteLine($"Total Price : {PriceSum()} EUR");
            Console.WriteLine();
        }
        public void DayPass()
        {
            Console.WriteLine("Day passed");
            foreach (Film film in stock)
            {
                film.DaysHeld++;
            }
        }
    }

}
