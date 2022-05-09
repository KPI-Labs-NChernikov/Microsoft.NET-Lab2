using Business.Services;
using ConsoleApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Printers
{
    public static class QueriesPrinter
    {
        public static IEnumerable<(string, Action)> InfoMenuItems =>
            new List<(string, Action)>
            {
                ("Get all actors with their filmographies and theatrical characters. " +
                "Sort by full name, then - year of birth", GetActors),
                ("Get all films starting from any year. Sort by year descending, then by name ascending",
                GetMoviesFromYear),
                ("Get all stakeholders with information about them and their professions." +
                "Sort by profession, then - fullname", GetStakeholders),
                ("Get directors with quantity of movies directed by them. " +
                "Order by quantity of movies descending, then by full name ascending",
                GetDirectorsStats),
                ("Get all spectacles. Sort by name", GetSpectacles),
                ("TODO", null!),
                ("Get top-N actors. Sort by quantity of main roles both in movies and speactacles.",
                null!),
                ("Find actors by fullname", null!),
                ("Get genres that were used both in movies and spectacles", null!),
                ("Get all actors that are directors too. Sort by year of birth",
                null!),
                ("TODO", null!),
                ("Find films by director's full name. Sort by film year descending",
                null!),
                ("Find all films and spectacles by name. Group by type - spectacle or movie",
                null!),
                ("Get genres with quantity of movies and spectacles of them. " +
                "Sort by quantity of movies desc., then - spectacles desc.", null!),
                ("Find spectacles of the specific genre", FindSpectaclesByGenre)
            };

        public static Menu Menu =>
            new()
            {
                Items = InfoMenuItems,
                Header = HelperMethods.GetHeader("Queries"),
                Name = "query"
            };

        public static ActorInfoService Service => ApiContainer.Api.ActorInfoService;

        public static void GetActors()
        {
            var result = Service.GetActors();
            HelperMethods.PrintHeader("Actors");
            foreach (var actor in result)
                ActorPrinter.PrintActorWithFilmography(actor);
            HelperMethods.Continue();
        }

        public static void GetMoviesFromYear()
        {
            HelperMethods.PrintHeader("Search movies from year:");
            var form = new NumberForm<ushort>()
            {
                Min = 1895,
                Max = (ushort)DateTime.Now.Year,
                Handler = ushort.TryParse
            };
            var minYear = form.GetNumber();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var result = Service.GetMoviesFromYear(minYear);
            HelperMethods.PrintHeader($"Results (movies from {minYear}):");
            foreach (var movie in result)
            {
                Console.WriteLine($"{movie.Name} ({movie.Year})");
                Console.WriteLine($"Genres: {string.Join(", ", movie.Genres)}");
                Console.WriteLine($"Director: {movie.Director.FullName} (year of birth: {movie.Director.BirthYear})");
                Console.WriteLine();
            }
            HelperMethods.Continue();
        }

        public static void GetStakeholders()
        {
            var result = Service.GetStakeholders();
            HelperMethods.PrintHeader("Stakeholders:");
            foreach (var profession in result)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"\t{profession.Key}s:");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                foreach (var person in profession)
                {
                    Console.WriteLine($"\t{person.FullName} (year of birth: {person.BirthYear})");
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            HelperMethods.Continue();
        }

        public static void GetDirectorsStats()
        {
            var result = Service.GetDirectorsStats();
            HelperMethods.PrintHeader("Directors' stats:");
            foreach (var stats in result)
            {
                Console.WriteLine($"Name: {stats.Director.FullName}");
                Console.WriteLine($"Year of birth: {stats.Director.BirthYear}");
                Console.WriteLine($"Movies: {stats.MoviesCount}");
                Console.WriteLine();
            }
            HelperMethods.Continue();
        }

        public static void GetSpectacles()
        {
            var result = Service.GetSpectacles();
            HelperMethods.PrintHeader("Spectacles:");
            foreach (var spectacle in result)
            {
                ActorPrinter.PrintSpectacle(spectacle);
                Console.WriteLine();
            }
            HelperMethods.Continue();
        }

        public static void FindSpectaclesByGenre()
        {
            HelperMethods.PrintHeader("Find spectacles by genre");
            var name = HelperMethods.Search("genre's name");
            var result = Service.FindSpectaclesByGenre(name);
            Console.Clear();
            HelperMethods.PrintHeader("Find spectacles by genre");
            HelperMethods.PrintHeader($"Results for \"{name}\":");
            foreach (var spectacle in result)
            {
                ActorPrinter.PrintSpectacle(spectacle);
                Console.WriteLine();
            }
            HelperMethods.Continue();
        }
    }
}
