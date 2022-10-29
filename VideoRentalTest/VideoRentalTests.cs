using VideoRentalStore;



namespace VideoRentalStore.Test
{
    public class CLITests
    {
        [Test]
        public void CLI_Exit()
        {
            CLI cli = new CLI();

            while (!cli.Exit)
            {
                cli.Interact("exit");
            }

            Assert.Pass();
        }
        [Test]
        public void CLI_Interaction()
        {
            CLI cli = new CLI();
            string[] macros = new string[]
            {
                "help",
                "?",
                "store",
                "backpack",
                "clear",
                "sleep",
                "add",
                "bonuspay",
                "return",
                "exit",
                "work",
                "wallet",
                "asdsad",
                "kghmdkogmhkd"
            };
            StreamWriter fileCommands = new StreamWriter("commands_used.txt");
            foreach (string command in macros)
            {
                try
                {
                    cli.Interact(command);
                    fileCommands.WriteLine(command);
                }
                catch (Exception ex)
                {
                    fileCommands.WriteLine($"Failed at: {command}");
                    fileCommands.WriteLine($"{ex.Message}\n{ex.StackTrace}");
                    fileCommands.Close();
                    Assert.Fail();
                }
            }
            fileCommands.WriteLine("<----Success---->");
            fileCommands.Close();
        }
    }
    public class CustomerTests 
    {
        [Test]
        public void Customer_NegativeBalanceCheck()
        {
            CLI cli = new CLI();
            cli.Customer.Balance -= 500;
            Assert.That(cli.Customer.Balance, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        public void Customer_NegativeBPCheck()
        {
            CLI cli = new CLI();
            cli.Customer.BonusPoints -= 500;
            Assert.That(cli.Customer.BonusPoints, Is.GreaterThanOrEqualTo(0));
        }
        [Test]
        public void Customer_BonusPointFilms()
        {
            CLI cli = new CLI();

            Assert.Multiple(() =>
            {
                cli.Customer.Balance = 500;
                cli.Interact("add \"Matrix 11\" 10");
                Assert.That(cli.Customer.BonusPoints, Is.EqualTo(2));

                cli.Customer.BonusPoints = 0;
                cli.Interact("add \"Texas Chainsaw Massacre\" 2");
                Assert.That(cli.Customer.BonusPoints, Is.EqualTo(1));

                cli.Interact("add \"Shrek\" 2");
                Assert.That(cli.Customer.BonusPoints, Is.EqualTo(2));
            });
        }
        [Test]
        public void Customer_BPPay()
        {
            CLI cli = new CLI();
            cli.Customer.BonusPoints = 175;

            Assert.Multiple(() =>
            {
                cli.Interact("bonuspay \"Matrix 11\" 1");
                Assert.That(cli.Store.FindFilm("Matrix 11"), Is.Not.EqualTo(null));
                cli.Interact("bonuspay \"Matrix 11\"");
                Assert.That(cli.Customer.BonusPoints, Is.EqualTo(175 - 25));
                Assert.That(cli.Backpack.FindFilm("Matrix 11").DaysRent, Is.EqualTo(1));
                Assert.That(cli.Store.FindFilm("Matrix 11"), Is.EqualTo(null));

                cli.Interact("bonuspay \"Shrek 2\"");

                Assert.That(cli.Backpack.FindFilm("Matrix 11"), Is.Not.EqualTo(null));
                Assert.That(cli.Customer.BonusPoints, Is.Not.EqualTo(175 - 25 - 25));
                cli.Interact("bonuspay \"Shrek\"");
                Assert.That(cli.Backpack.FindFilm("Shrek"), Is.EqualTo(null));
                Assert.That(cli.Store.FindFilm("Shrek"), Is.Not.EqualTo(null));
            });
        }
        [Test]
        public void Customer_Work()
        {
            CLI cli = new CLI();
            int old = cli.Customer.Balance;
            cli.Customer.Work();
            Assert.That(cli.Customer.Balance, Is.EqualTo(old + 5));
        }

    }
    public class InventoryTests
    {
        [Test]
        public void Inventory_RentFilm()
        {
            CLI cli = new CLI();
            cli.Customer.Balance = 100;

            cli.Interact("add \"Shrek\" 1");
            Assert.Multiple(() =>
            {
                Assert.That(cli.Store.FindFilm("Shrek"), Is.EqualTo(null));
                Assert.That(cli.Backpack.FindFilm("Shrek"), Is.Not.EqualTo(null));
                Assert.That(cli.Customer.Balance, Is.EqualTo(100 - 3));
            });
        }
        [Test]
        public void Inventory_ReturnLate()
        {
            CLI cli = new CLI();

            Assert.Multiple(() =>
            {
                cli.Customer.Balance = 100;
                cli.Interact("add \"Matrix 11\" 1");
                Assert.That(cli.Store.FindFilm("Matrix 11"), Is.EqualTo(null));
                cli.Interact("sleep"); // 4
                cli.Interact("return \"Matrix 11\"");
                Assert.That(cli.Customer.Balance, Is.EqualTo(100 - 4));
            });
        }
        [Test]
        public void Inventory_RemoveFilm()
        {
            CLI cli = new CLI();
            cli.Store.RemoveFIlm("Matrix 11");
            Assert.That(cli.Store.FindFilm("Matrix 11"), Is.EqualTo(null));
        }
    }
    public class FilmTests
    {
        [Test]
        public void Film_PriceCheck()
        {
            Film oldFIlm = new Film("Old Film", new Old(), 7);
            Film regFilm = new Film("Regular Film", new RegularRental(), 5);
            Film newFilm = new Film("New Film", new NewRelease(), 5);
            Assert.Multiple(() =>
            {
                Assert.That(oldFIlm.Price, Is.EqualTo(9));
                Assert.That(regFilm.Price, Is.EqualTo(9));
                Assert.That(newFilm.Price, Is.EqualTo(5 * 4));
            });
            oldFIlm = new Film("Old Film", new Old(), 3);
            regFilm = new Film("Regular Film", new RegularRental(), 3);
            Assert.Multiple(() =>
            {
                Assert.That(oldFIlm.Price, Is.EqualTo(3));
                Assert.That(regFilm.Price, Is.EqualTo(3));
            });
        }
        [Test]
        public void Film_LateAndPrice()
        {
            CLI cli = new CLI();
            cli.Interact("add \"Matrix 11\" 1");
            cli.Interact("sleep");
            Assert.That(!cli.Backpack.FindFilm("Matrix 11").IsLate);
            cli.Interact("sleep");
            Assert.That(cli.Backpack.FindFilm("Matrix 11").IsLate);
            Assert.That(cli.Backpack.FindFilm("Matrix 11").Price, Is.EqualTo(8));
        }

        [Test]
        public void FIlm_Comparison()
        {
            Film a = new Film("A");
            Film anotherA = new Film("A");
            Film b = new Film("B");
            Film? nullFilm = null;
            Assert.IsTrue(a != b);
            Assert.IsTrue(nullFilm != b);
            Assert.IsTrue(a != nullFilm);
            Assert.IsTrue(a == anotherA);
        }
    }

}