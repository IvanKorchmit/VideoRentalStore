using System;
namespace VideoRentalStore
{
    public sealed class Customer
    {
        public int BonusPoints { get; set; }
        public int Balance { get; set; }
        public Customer()
        {
            Random random = new Random();
            Balance = random.Next(40, 70);
        }

        public void PrintBonusPoints()
        {
            Console.WriteLine($"Your amount of bonus points is: {BonusPoints}\nYou have {Balance} EUR");
        }

        public void Work()
        {
            Balance += 5;
        }
    }

}
