namespace ConsoleApp
{
    public static class HelperMethods
    {
        public static void Quit()
        {
            Console.WriteLine("Press enter co continue");
            Console.ReadLine();
        }

        public static void PrintHeader(string header)
        {
            Console.WriteLine($"{header}\n");
        }

        public static void PrintErrorMessage()
        {
            var initialColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Ooops, unknown error occured...");
            Console.ForegroundColor = initialColor;
        }

        public static string? Search(string toFind)
        {
            Console.WriteLine($"Please, enter the {toFind}: ");
            return Console.ReadLine();
        }
    }
}
