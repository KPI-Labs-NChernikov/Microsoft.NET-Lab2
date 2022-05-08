using Backend;
using Backend.Interfaces;
using Backend.Models;
using Business;
using Business.Interfaces;
using ConsoleApp.Data;
using ConsoleApp.Helpers;

namespace ConsoleApp
{
    public static class ApiContainer
    {
        public static IApi Api { get; set; } = new Api(new XmlContext());

        public static void SeedData()
        {
            var dialog = new Dialog
            {
                Question = "Do you want to fill your context with a test data?",
                YAction = () =>
                {
                    Console.WriteLine();
                    var seeder = new DataSeeder(Api.Context);
                    seeder.SeedData();
                    Console.WriteLine("The test data has been seeded successfully.");
                }
            };
            dialog.Print();
        }

        public static void Save()
        {
            Api.Save();
            Console.WriteLine("Saved successfully.");
            HelperMethods.Continue();
        }
    }
}
