using System;

namespace VideoRentalStore
{
    static class RentalUtils
    {
        public static void PrintHelp()
        {
            Console.WriteLine($"help | ?                print this");
            Console.WriteLine($"store | backpack        Print one of the inventories");
            Console.WriteLine($"clear                   Clear terminal");
            Console.WriteLine($"sleep                   Skip one day");
            Console.WriteLine($"add \"film name\" <n>   Rent one film for n days");
            Console.WriteLine($"bonuspay \"film name\"  Rent one film for 1 day with bonus points");
            Console.WriteLine($"return \"film name\"    Return one film (And pay extra money if returned late)");
            Console.WriteLine($"exit                    Exit program");
            Console.WriteLine($"work                    Gain +5 EUR");
            Console.WriteLine($"wallet                  Display wallet.");
        }
        public static void ReturnFIlm(string input, Inventory backpack, Inventory store, Customer customer)
        {
            static (string filmName, bool isOk) ParseArgs(string input)
            {
                string[] splitted = input.Split('"', '"', StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < splitted.Length; i++)
                {
                    splitted[i] = splitted[i].Trim();
                }
                if (splitted.Length != 2)
                {
                    Console.WriteLine("Error! Command is used wrong");
                    return (null, false);
                }

                string filmName = splitted[1];
                return (filmName, true);
            }
            var parsedArgs = ParseArgs(input);
            if (!parsedArgs.isOk) return;



            var f = backpack.FindFilm(parsedArgs.filmName);

            if (f.IsLate)
            {
                if (customer.Balance < f.Price)
                {
                    Console.WriteLine("Not enough money");
                    return;
                }

                backpack.RemoveFIlm(f.Name);
                store.AddFilm(f);
                f.DaysHeld = 0;

            }

        }
        public static void RentFilm(string input, bool isPaidWithBonus, Inventory store, Inventory backpack, Customer customer)
        {
            (string filmName, int days, bool isOk) ParseArgs(string input)
            {
                string[] splitted = input.Split('"', '"', StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < splitted.Length; i++)
                {
                    splitted[i] = splitted[i].Trim();
                }
                if (splitted.Length < (!isPaidWithBonus ? 3 : 2))
                {
                    Console.WriteLine("Error! Command is used wrong");
                    return (null, 0, false);
                }

                string filmName = splitted[1];
                int daysRent = new int();
                if (!isPaidWithBonus)
                {
                    string number = splitted[2];
                    if (!int.TryParse(number, out daysRent))
                    {
                        Console.WriteLine("Invalid value");
                        return (null, 0, false);
                    }
                    if (daysRent <= 0)
                    {
                        Console.WriteLine("Invalid value (Must be greater than 0)");
                        return (null, 0, false);
                    }
                }
                return (filmName, !isPaidWithBonus ? daysRent : 0, true);
            }
            var parsedArgs = ParseArgs(input);
            if (!parsedArgs.isOk) return;




            var f = store.FindFilm(parsedArgs.filmName);


            if (f == null)
            {
                Console.WriteLine($"Film {parsedArgs.filmName} not found");
                return;
            }
            f.DaysRent = parsedArgs.days;
            if ((!isPaidWithBonus && customer.Balance >= f.Price) || (isPaidWithBonus && customer.BonusPoints >= 25))
            {
                Console.WriteLine($"Added film {f}");
                if (!isPaidWithBonus)
                {
                    customer.Balance -= f.Price;
                    AccountFilm(f.Name);

                }
                else
                {
                    customer.BonusPoints -= 25;
                    Film bonusFilm = new(f.Name, f.Rental, 1);
                    bonusFilm.DaysRent = 0;
                    Console.Write($"{bonusFilm} 1 days (Paid with 25 bonus points)" +
                        $"\nTotal price : 0 EUR\n" +
                        $"\nRemaining Bonus points: {customer.BonusPoints}\n");
                    bonusFilm.DaysRent = 1;
                    AccountFilm(bonusFilm.Name);
                }
            }
            else
            {
                f.DaysRent = new int();
                Console.WriteLine(!isPaidWithBonus ? "Not enough money" : "Not enough bonus points");
            }

            void AccountFilm(string name)
            {
                store.RemoveFIlm(name);
                backpack.AddFilm(new Film(f.Name, f.Rental, parsedArgs.days));
                switch (f.Rental)
                {
                    case Film.RentalType.NewRelease:
                        customer.BonusPoints += 2;
                        break;
                    case Film.RentalType.RegularRental:
                    case Film.RentalType.OldFilm:
                        customer.BonusPoints++;
                        break;
                    default:
                        throw new ArgumentException("Invalid rental type"); // Explicit is better than implicit.
                }
            }
        }
    }
}