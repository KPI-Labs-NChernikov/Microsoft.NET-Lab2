namespace ConsoleApp.Helpers
{
    public class LiteMenu
    {
        public string? Name { get; set; }

        public IEnumerable<(string Text, Action? Action)> Items { get; set; } = new List<(string Text, Action? Action)>();

        public void Print()
        {
            if (Items is not null && Items.Any())
            {
                var initialColor = Console.ForegroundColor;
                var name = !string.IsNullOrEmpty(Name) ? Name : "item";
                Console.WriteLine($"Please, select the {name}:");
                var texts = Items.Select(i => i.Text);
                for (int i = 0; i < texts.Count(); i++)
                    Console.WriteLine($"{i + 1}. {texts.ElementAt(i)}");
                Console.WriteLine($"Select the number from 1 to {Items.Count()}: ");
                bool parsed;
                parsed = int.TryParse(Console.ReadLine(), out int selected);
                while (!parsed || selected < 1 || selected > Items.Count())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Error: wrong number");
                    Console.ForegroundColor = initialColor;
                    Console.WriteLine($"Please, select the number from 0 to {Items.Count()} once more");
                    parsed = int.TryParse(Console.ReadLine(), out selected);
                }
                selected--;
                Items.ElementAt(selected).Action?.Invoke();
            }
        }
    }
}
