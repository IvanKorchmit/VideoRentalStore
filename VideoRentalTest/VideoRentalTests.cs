using VideoRentalStore;



namespace VideoRentalTest
{
    public class Tests
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
            Assert.Pass();
        }
    }
}