using System;
using System.Collections.Generic;

namespace VideoRentalStore
{
    public class CLI
    {

        private readonly Inventory store = new Inventory();
        private readonly Inventory backpack = new Inventory();
        private readonly Customer customer = new Customer();
        private bool exit;
        public bool Exit => exit;

        public Inventory Store => store;
        public Inventory Backpack => backpack;
        public Customer Customer => customer;

        private const int AMOUNT_OF_COMMANDS = 12; // Used for assertion


        /// <param name="input">Simulate user input. If the parameter is null, then Console.ReadLine() is called.</param>
        public void Interact(string input = null)
        {
            Console.Write("# ");
            string userInput = input == null ? Console.ReadLine() : input;
            Action<string> action = commands.GetValueOrDefault(userInput.Split(' ')[0]);


            // Handle this in case if give string is empty...
            if (!string.IsNullOrEmpty(userInput) && action == null)
                Console.WriteLine("Unknown command");
            else if (!string.IsNullOrEmpty(userInput))
                action(userInput);
        }
        /// <summary>
        /// string commandlets mapped to methods.
        /// </summary>
        private readonly Dictionary<string, Action<string>> commands;

        public CLI()
        {
            commands = new()
            {
                { "help", (_) => RentalUtils.PrintHelp() },

                { "?", (_) => RentalUtils.PrintHelp() },

                { "store", (_) => store.PrintFilms(false) },

                { "backpack", (_) => backpack.PrintFilms(true) },

                { "clear", (_) =>{if (!Console.IsOutputRedirected)Console.Clear();} },

                { "sleep", (_) => backpack.DayPass() },

                { "add", (input) => RentalUtils.RentFilm(input, false, store, backpack, customer) },

                { "bonuspay", (input) => RentalUtils.RentFilm(input, true, store, backpack, customer) },

                { "return", (input) => RentalUtils.ReturnFIlm(input, backpack, store, customer) },

                { "exit", (_) => exit = true },

                { "work", (_) => customer.Work()},

                { "wallet", (_) => customer.PrintBonusPoints() }
            };


            if (AMOUNT_OF_COMMANDS != commands.Count)
            {
                throw new Exception($"Commands probably has not been documented in the help section. Current amount of commands are {commands.Count}");
            }


            FillWithFilms();

            store.PrintFilms(false);

            Console.WriteLine("Enter ? or help for manual");


        }

        private void FillWithFilms()
        {
            store.AddFilm(new Film("Out of Africa", new Old()));
            store.AddFilm(new Film("Despicable Me 3", new NewRelease()));
            store.AddFilm(new Film("Despicable Me", new RegularRental()));
            store.AddFilm(new Film("Kung Fu Panda 3", new NewRelease()));
            store.AddFilm(new Film("Matrix 11", new NewRelease()));
            store.AddFilm(new Film("Black Adam", new NewRelease()));
            store.AddFilm(new Film("Shrek", new Old()));
            store.AddFilm(new Film("Spider man", new RegularRental()));
            store.AddFilm(new Film("Shrek 2", new RegularRental()));
            store.AddFilm(new Film("Texas Chainsaw Massacre", new Old()));
            store.AddFilm(new Film("Shrek 3", new NewRelease()));
            store.AddFilm(new Film("Creed III", new NewRelease()));
            store.AddFilm(new Film("Halloween Ends", new NewRelease()));
            store.AddFilm(new Film("Kung Fu Panda 2", new RegularRental()));
            store.AddFilm(new Film("Ant-Man and the Wasp: Quantumania", new NewRelease()));
            store.AddFilm(new Film("Enola Holmes 2", new NewRelease()));
            store.AddFilm(new Film("Saw", new NewRelease()));
            store.AddFilm(new Film("Nightmare Concert: Cat In The Brain", new Old()));
            store.AddFilm(new Film("Spider man 2", new RegularRental()));
            store.AddFilm(new Film("Despicable Me 2", new RegularRental()));
            store.AddFilm(new Film("Kung Fu Panda", new Old()));
        }
    }
}