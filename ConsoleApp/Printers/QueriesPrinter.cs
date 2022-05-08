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
                ("Get all actors. Sort by full name, then - year of birth", null!),
                ("Get all films starting from any year. Sort by year descending, then by name ascending",
                null!),
                ("Get all films and spectacles where actor starred. Sort by: name",
                null!),
                ("Get all actors joined with their roles, then with films/spectacles",
                null!),
                ("Get the cast of the spectacle. sort by: type of the role", null!),
                ("Get movies grouped by genres. Sort by name of the genre", null!),
                ("Get top-N actors. Sort by quantity of main roles both in movies and speactacles.",
                null!),
                ("Find actors by fullname", null!),
                ("Get genres that were used both in movies and spectacles", null!),
                ("Get all actors that are directors too. Sort by year of birth",
                null!),
                ("Get all actors that starred in at least one movie or spectacle with given genre. " +
                "Sort by fullname, then - year of birth", null!),
                ("Find films by director's full name. Sort by film year descending",
                null!),
                ("Find all films and spectacles by name. Group by type - spectacle or movie",
                null!),
                ("Get genres with quantity of movies and spectacles of them. " +
                "Sort by quantity of movies desc., then - spectacles desc.", null!),
                ("Find spectacles of genre by start of the name", null!)
            };

        public static Menu Menu =>
            new()
            {
                Items = InfoMenuItems,
                Header = HelperMethods.GetHeader("Queries"),
                Name = "query"
            };

        public static ActorInfoService Service => ApiContainer.Api.ActorInfoService;
    }
}
