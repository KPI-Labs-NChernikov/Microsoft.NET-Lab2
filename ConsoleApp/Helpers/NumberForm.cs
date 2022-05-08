namespace ConsoleApp.Helpers
{
    public delegate bool TryParseHandler<T>(string? value, out T result);
    public class NumberForm<T> where T : struct, IComparable<T>
    {
        public string? Name { get; set; }

        public T? Min { get; set; }

        public T? Max { get; set; }

        public TryParseHandler<T> Handler { get; set; } = null!;

        public T GetNumber()
        {
            var initialColor = Console.ForegroundColor;
            var name = !string.IsNullOrEmpty(Name) ? Name : "number";
            Console.Write($"Enter the {name}");
            if (Min is not null)
                Console.Write($" from {Min}");
            if (Max is not null)
                Console.Write($" to {Max}");
            Console.WriteLine(": ");
            bool parsed;
            if (Handler is null)
                throw new InvalidOperationException("Handler cannot be null");
            parsed = Handler.Invoke(Console.ReadLine(), out T entered);
            while (!parsed 
                || (Min is not null && entered.CompareTo((T)Min) < 0) 
                || (Max is not null && entered.CompareTo((T)Max) > 0))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Error: wrong {name}");
                Console.ForegroundColor = initialColor;
                Console.Write($"Please, enter the {name}");
                if (Min is not null)
                    Console.Write($" from {Min}");
                if (Max is not null)
                    Console.Write($" to {Max}");
                Console.WriteLine(" once more: ");
                parsed = Handler.Invoke(Console.ReadLine(), out entered);
            }
            Console.ResetColor();
            return entered;
        }
    }
}
