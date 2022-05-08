namespace ConsoleApp.Helpers
{
    public class Dialog
    {
        public string Question { get; set; } = string.Empty;

        public Action? YAction { get; set; }

        public Action? NAction { get; set; }

        public void Print()
        {
            var initialColor = Console.ForegroundColor;
            Console.WriteLine($"{Question} [Y/n]");
            bool parsed;
            do
            {
                parsed = true;
                var entered = Console.ReadLine()?.ToLower();
                switch (entered)
                {
                    case "y":
                        YAction?.Invoke();
                        break;
                    case "n":
                        NAction?.Invoke();
                        break;
                    default:
                        parsed = false;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Error: wrong value");
                        Console.ForegroundColor = initialColor;
                        Console.WriteLine($"Please, enter Y or n once more");
                        break;
                }
            } while (!parsed);
        }
    }
}
