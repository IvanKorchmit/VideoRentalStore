namespace VideoRentalStore
{
    class Program
    {
        private static void Main(string[] args)
        {
            CLI cli = new CLI();
            while (!cli.Exit)
            {
                cli.Interact();
            }
        }
    }

}
