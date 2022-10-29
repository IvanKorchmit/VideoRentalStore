using System;
namespace VideoRentalStore
{
    public sealed class Customer
    {
        private int bonusPoints;
        public int BonusPoints
        {
            get
            {
                return bonusPoints;
            }
            set
            {
                bonusPoints = value;
                bonusPoints = Math.Clamp(bonusPoints, 0, int.MaxValue);
            }
        }
        private int balance;
        public int Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
                balance = Math.Clamp(balance, 0, int.MaxValue);
            }
        }
        public Customer()
        {
            Random random = new Random();
            balance = random.Next(40, 70);
        }

        public void PrintBonusPoints()
        {
            Console.WriteLine($"Your amount of bonus points is: {BonusPoints}\nYou have {Balance} EUR");
        }

        public void Work()
        {
            balance += 5;
        }
    }

}
