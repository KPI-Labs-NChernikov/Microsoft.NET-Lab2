using Backend.Interfaces;
using Backend.Models;
using ConsoleApp;
using ConsoleApp.Helpers;
using ConsoleApp.Printers;

Console.ForegroundColor = ConsoleColor.DarkGreen;
var api = ApiContainer.Api;
api.SaveFile = "actors.xml";
IXmlContext<Actor> context = api.Context;
try
{
    context.Load(api.SaveFile);
    api.IsSaved = true;
}
catch (FileNotFoundException)
{
    Console.WriteLine("It seems as if your context is empty now");
    ApiContainer.SeedData();
    HelperMethods.Continue();
}

IEnumerable<(string, Action)> mainMenuItems = new List<(string, Action)>()
{
    ("Show all", ActorPrinter.ShowAll),
    ("Add", ActorPrinter.Add),
    ("Delete", ActorPrinter.Delete),
    ("Clear", ActorPrinter.Clear),
    ("Save", ActorPrinter.Save),
    ("To queries", () => QueriesPrinter.Menu.Print()),
};
var mainMenu = new Menu
{
    Header = HelperMethods.GetHeader("Main"),
    Name = "command",
    Items = mainMenuItems
};
mainMenu.Print();

if (!api.IsSaved)
{
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.Clear();
    var dialog = new Dialog
    {
        Question = "You have some unsaved changes. Do you want to save them?",
        YAction = ApiContainer.Save
    };
    dialog.Print();
    Console.ResetColor();
}
