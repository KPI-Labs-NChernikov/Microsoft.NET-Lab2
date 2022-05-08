using Backend;
using Backend.Interfaces;
using Backend.Models;
using Business;
using ConsoleApp;
using ConsoleApp.Data;
using ConsoleApp.Helpers;
using ConsoleApp.Interfaces;
using System.Xml;

Console.ForegroundColor = ConsoleColor.DarkGreen;
var api = ApiContainer.Api;
IXmlContext<Actor> context = api.Context;
bool error = false;
try
{
    context.Load(api.SaveFile);
    api.IsSaved = true;
}
catch (FileNotFoundException)
{
    Console.WriteLine("It seems as if your context is empty now");
    ApiContainer.SeedData();
}
catch (XmlException exc)
{
    error = true;
    HelperMethods.PrintErrorMessage(exc.Message);
    HelperMethods.Continue();
}
if (!error)
{
    Console.Clear();
    if (!api.IsSaved)
    {
        var dialog = new Dialog
        {
            Question = "You have some unsaved changes. Do you want to save them?",
            YAction = ApiContainer.Save
        };
        dialog.Print();
    }
}
Console.ResetColor();