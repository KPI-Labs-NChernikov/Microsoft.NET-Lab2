using Backend;
using Backend.Interfaces;
using Backend.Models;
using ConsoleApp;
using ConsoleApp.Data;
using ConsoleApp.Interfaces;

Console.ForegroundColor = ConsoleColor.DarkGreen;
var dialog = new Dialog
{
    YAction = () => Console.WriteLine("Hello world"),
    Question = "Do you want to see hello message?"
};
dialog.Print();
var form = new StringForm
{
    IsValid = (string? s) => !string.IsNullOrWhiteSpace(s),
    Name = "actor's name",
    ErrorMessage = "the actor's name shouldn't be empty"
};
Console.WriteLine($"You entered: {form.GetString()}");
Console.ResetColor();
//var fileName = "actors.xml";
//IXmlContext<Actor> context = new XmlContext();
//context.Load(fileName);
//IDataSeeder<Actor> seeder = new DataSeeder(context);
//seeder.SeedData();
//context.Save(fileName);