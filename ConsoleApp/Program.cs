using Backend;
using Backend.Interfaces;
using Backend.Models;
using ConsoleApp.Data;
using ConsoleApp.Interfaces;

var fileName = "actors.xml";
IXmlContext<Actor> context = new XmlContext();
context.Load(fileName);
IDataSeeder<Actor> seeder = new DataSeeder(context);
seeder.SeedData();
context.Save(fileName);