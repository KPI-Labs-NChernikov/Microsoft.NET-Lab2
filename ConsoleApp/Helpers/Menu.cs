namespace ConsoleApp.Helpers
{
    public class Menu
    {
        public string? Header { get; set; }

        public string? Name { get; set; }

        public ConsoleColor Color { get; set; } = ConsoleColor.DarkGreen;

        public IEnumerable<(string Text, Action Action)> Items { get; set; } = new List<(string Text, Action Action)>();

        public void Print(bool closeAfter = false)
        {
            if (Items is not null && Items.Any())
            {
                Console.Clear();
                Console.ForegroundColor = Color;
                if (Header is not null)
                {
                    HelperMethods.PrintHeader(Header);
                }
                var name = !string.IsNullOrEmpty(Name) ? Name : "item";
                Console.WriteLine($"Please, select the {name}:");
                var texts = Items.Select(i => i.Text);
                for (int i = 0; i < texts.Count(); i++)
                    Console.WriteLine($"{i + 1}. {texts.ElementAt(i)}");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("0. Quit\n");
                Console.ForegroundColor = Color;
                Console.WriteLine($"Select the number from 0 to {Items.Count()}: ");
                bool parsed;
                parsed = int.TryParse(Console.ReadLine(), out int selected);
                while (!parsed || selected < 0 || selected > Items.Count())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Error: wrong number");
                    Console.ForegroundColor = Color;
                    Console.WriteLine($"Please, select the number from 0 to {Items.Count()} once more");
                    parsed = int.TryParse(Console.ReadLine(), out selected);
                }
                if (selected != 0)
                {
                    selected--;
                    Console.Clear();
                    Items.ElementAt(selected).Action?.Invoke();
                    Console.Clear();
                    if (!closeAfter)
                        Print();
                }
            }
            Console.ResetColor();
        }
    }
}
