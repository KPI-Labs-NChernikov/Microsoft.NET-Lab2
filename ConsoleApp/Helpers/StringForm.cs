namespace ConsoleApp.Helpers
{
    public class StringForm
    {
        public string? Name { get; set; }

        public Func<string?, bool>? IsValid { get; set; }

        public string? ErrorMessage { get; set; }

        public string GetString()
        {
            var initialColor = Console.ForegroundColor;
            var name = !string.IsNullOrEmpty(Name) ? Name : "string";
            Console.WriteLine($"Enter the {name}: ");
            bool isValid;
            string? entered;
            do
            {
                entered = Console.ReadLine();
                isValid = IsValid?.Invoke(entered) ?? true;
                if (!isValid)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    var errorMessage = !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : "wrong value";
                    Console.WriteLine($"Error: {errorMessage}");
                    Console.ForegroundColor = initialColor;
                    Console.WriteLine($"Please, enter the {name} once more");
                }
            }
            while (!isValid);
            return entered ?? string.Empty;
        }
    }
}
