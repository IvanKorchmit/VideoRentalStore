﻿using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

namespace VideoRentalStore
{
    public class CLI
    {

        private readonly Inventory store = new Inventory();
        private readonly Inventory backpack = new Inventory();
        private readonly Customer customer = new Customer();
        private bool exit;
        public bool Exit => exit;
        private const int AMOUNT_OF_COMMANDS = 12; // Used for assertion
        public void Interact(string input = null)
        {
            Console.Write("# ");
            string userInput = input == null ? Console.ReadLine() : input;
            Action<string> action = commands.GetValueOrDefault(userInput.Split(' ')[0]);

            if (userInput != string.Empty && action == null)
                Console.WriteLine("Unknown command");
            else if (userInput != string.Empty)
                action(userInput);
        }


        private readonly Dictionary<string, Action<string>> commands;
        public CLI()
        {
            commands = new()
            {
                { "help", (_) => RentalUtils.PrintHelp() },
                { "?", (_) => RentalUtils.PrintHelp() },
                { "store", (_) => store.PrintFilms(false) },
                { "backpack", (_) => backpack.PrintFilms(true) },
                { "clear", (_) => Console.Clear() },
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
            store.AddFilm(new Film("Out of Africa", Film.RentalType.OldFilm));
            store.AddFilm(new Film("Despicable Me 3", Film.RentalType.NewRelease));
            store.AddFilm(new Film("Despicable Me", Film.RentalType.RegularRental));
            store.AddFilm(new Film("Kung Fu Panda 3", Film.RentalType.NewRelease));
            store.AddFilm(new Film("Matrix 11", Film.RentalType.NewRelease));
            store.AddFilm(new Film("Black Adam", Film.RentalType.NewRelease));
            store.AddFilm(new Film("Shrek", Film.RentalType.OldFilm));
            store.AddFilm(new Film("Spider man", Film.RentalType.RegularRental));
            store.AddFilm(new Film("Shrek 2", Film.RentalType.RegularRental));
            store.AddFilm(new Film("Texas Chainsaw Massacre", Film.RentalType.OldFilm));
            store.AddFilm(new Film("Shrek 3", Film.RentalType.NewRelease));
            store.AddFilm(new Film("Creed III", Film.RentalType.NewRelease));
            store.AddFilm(new Film("Halloween Ends", Film.RentalType.NewRelease));
            store.AddFilm(new Film("Kung Fu Panda 2", Film.RentalType.RegularRental));
            store.AddFilm(new Film("Ant-Man and the Wasp: Quantumania", Film.RentalType.NewRelease));
            store.AddFilm(new Film("Enola Holmes 2", Film.RentalType.NewRelease));
            store.AddFilm(new Film("Saw", Film.RentalType.NewRelease));
            store.AddFilm(new Film("Nightmare Concert: Cat In The Brain", Film.RentalType.OldFilm));
            store.AddFilm(new Film("Spider man 2", Film.RentalType.RegularRental));
            store.AddFilm(new Film("Despicable Me 2", Film.RentalType.RegularRental));
            store.AddFilm(new Film("Kung Fu Panda", Film.RentalType.OldFilm));
        }
    }
}