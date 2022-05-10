using Backend.Models;
using Business.Interfaces;
using ConsoleApp.Helpers;

namespace ConsoleApp.Printers
{
    public static class ActorPrinter
    {
        public static IApi Api => ApiContainer.Api;

        public static IActorService Service => Api.ActorService;

        public static void ShowAll()
        {
            var result = Service.GetAll();
            HelperMethods.PrintHeader("Show all");
            foreach (var actor in result)
            {
                PrintActorWithFilmography(actor);
                Console.WriteLine();
            }
            HelperMethods.Continue();
        }

        public static void PrintActorWithFilmography(Actor actor)
        {
            PrintActor(actor);
            Console.WriteLine("Filmography:");
            foreach (var item in actor.Filmography)
            {
                Console.WriteLine($"\t{item.Performance.Name}");
                Console.Write($"\tRole: {item.Role}");
                if (item.IsMain)
                    Console.Write(" (main)");
                Console.WriteLine();
                Console.WriteLine($"\t{item.Performance.GetType().Name}:");
                var indent = "\t\t";
                if (item.Performance.GetType() == typeof(Movie))
                {
                    PrintMovie((Movie)item.Performance, indent);
                }
                else
                {
                    PrintSpectacle((Spectacle)item.Performance, indent);
                }
                Console.WriteLine();
            }
        }

        public static void PrintActor(Actor actor)
        {
            Console.WriteLine($"Name: {actor.FullName}");
            Console.WriteLine($"Year of birth: {actor.BirthYear}");
            if (actor.TheatricalCharacters.Any())
                Console.WriteLine($"Theatrical character: {string.Join("; ", actor.TheatricalCharacters)}");
        }

        public static void PrintSpectacle(Spectacle spectacle, string? start = null)
        {
            Console.WriteLine($"{start}Name: {spectacle.Name}");
            Console.WriteLine($"{start}Genres: {string.Join(", ", spectacle.Genres)}");
        }

        public static void PrintMovie(Movie movie, string? start = null)
        {
            Console.WriteLine($"{start}Name: {movie.Name}");
            Console.WriteLine($"{start}Year: {movie.Year}");
            Console.WriteLine($"{start}Genres: {string.Join(", ", movie.Genres)}");
            Console.WriteLine($"{start}Director: {movie.Director.FullName}" +
                $" (year of birth: {movie.Director.BirthYear})");
        }

        private static bool WhitespaceValidation(string? s) => !string.IsNullOrWhiteSpace(s);
        private static string WhitespaceValidationErrorMessage => "this field shouldn't be empty";

        public static void Add()
        {
            HelperMethods.PrintHeader("Add");
            var form = new StringForm
            {
                IsValid = WhitespaceValidation,
                Name = "first name",
                ErrorMessage = WhitespaceValidationErrorMessage
            };
            var actor = new Actor
            {
                FirstName = form.GetString()
            };
            form.Name = "last name";
            actor.LastName = form.GetString();
            form.Name = "patronymic (or leave empty if none)";
            form.IsValid = null;
            actor.Patronymic = form.GetString();
            if (actor.Patronymic == string.Empty)
                actor.Patronymic = null;
            var numberForm = new NumberForm<ushort>()
            {
                Min = 1895,
                Max = (ushort)DateTime.Now.Year,
                Handler = ushort.TryParse,
                Name = "year of birth"
            };
            actor.BirthYear = numberForm.GetNumber();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var dialog = new Dialog
            {
                Question = "Do you want to add a theatrical character?",
                YAction = () => AddTheatricalCharacter(actor.TheatricalCharacters)
            };
            dialog.Print();
            dialog.Question = "Do you want to add a filmography item (for ex., film and actor's role in it)?";
            dialog.YAction = () => AddFilmographyItem(actor.Filmography);
            dialog.Print();
            try
            {
                Service.Add(actor);
                Console.WriteLine($"Actor {actor.FullName} has been successfully added");
            }
            catch (ArgumentException exc)
            {
                HelperMethods.PrintErrorMessage(exc.Message);
            }
            HelperMethods.Continue();
        }

        public static void AddFilmographyItem(ICollection<FilmographyItem> filmography)
        {
            var form = new StringForm
            {
                IsValid = WhitespaceValidation,
                Name = "actor's role",
                ErrorMessage = WhitespaceValidationErrorMessage
            };
            var item = new FilmographyItem
            {
                Role = form.GetString()
            };
            var isMainDialog = new Dialog
            {
                Question = "Was it a main role?",
                YAction = () => { item.IsMain = true; }
            };
            isMainDialog.Print();
            var performanceTypesMenu = new LiteMenu
            {
                Name = "performance type",
                Items = new (string, Action?)[]
                {
                    ("Movie", () => { item.Performance = AddMovie(); }),
                    ("Spectacle", () => { item.Performance = AddSpectacle(); })
                }
            };
            performanceTypesMenu.Print();
            filmography.Add(item);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var dialog = new Dialog
            {
                Question = "Do you want to add one more filmography item?",
                YAction = () => AddFilmographyItem(filmography)
            };
            dialog.Print();
        }

        public static Movie AddMovie()
        {
            var form = new StringForm
            {
                IsValid = WhitespaceValidation,
                Name = "name of the movie",
                ErrorMessage = WhitespaceValidationErrorMessage
            };
            var movie = new Movie
            {
                Name = form.GetString()
            };
            var numberForm = new NumberForm<ushort>()
            {
                Min = 1895,
                Max = (ushort)DateTime.Now.Year,
                Handler = ushort.TryParse,
                Name = "year of birth"
            };
            movie.Year = numberForm.GetNumber();
            AddGenre(movie.Genres);
            movie.Director = AddDirector();
            return movie;
        }

        public static Person AddDirector()
        {
            var form = new StringForm
            {
                IsValid = WhitespaceValidation,
                Name = "director's first name",
                ErrorMessage = WhitespaceValidationErrorMessage
            };
            var director = new Actor
            {
                FirstName = form.GetString()
            };
            form.Name = "director's last name";
            director.LastName = form.GetString();
            form.Name = "director's patronymic (or leave empty if none)";
            form.IsValid = null;
            director.Patronymic = form.GetString();
            if (director.Patronymic == string.Empty)
                director.Patronymic = null;
            var numberForm = new NumberForm<ushort>()
            {
                Min = 1895,
                Max = (ushort)DateTime.Now.Year,
                Handler = ushort.TryParse,
                Name = "director's year of birth"
            };
            director.BirthYear = numberForm.GetNumber();
            return director;
        }

        public static Spectacle AddSpectacle()
        {
            var form = new StringForm
            {
                IsValid = WhitespaceValidation,
                Name = "name of the spectacle",
                ErrorMessage = WhitespaceValidationErrorMessage
            };
            var spectacle = new Spectacle
            {
                Name = form.GetString()
            };
            AddGenre(spectacle.Genres);
            return spectacle;
        }

        public static void AddGenre(ICollection<Genre> genres)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var form = new StringForm
            {
                IsValid = WhitespaceValidation,
                Name = "genre (if there are more than one, separated by comma)",
                ErrorMessage = WhitespaceValidationErrorMessage
            };
            var names = form.GetString().Split(',').Select(x => x.Trim());
            foreach (var name in names)
                genres.Add(new Genre
                {
                    Name = name
                });
        }

        public static void AddTheatricalCharacter(ICollection<TheatricalCharacter> collection)
        {
            var form = new StringForm
            {
                IsValid = WhitespaceValidation,
                Name = "theatrical character",
                ErrorMessage = WhitespaceValidationErrorMessage
            };
            collection.Add(new TheatricalCharacter
            {
                Name = form.GetString()
            });
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            var dialog = new Dialog
            {
                Question = "Do you want to add one more theatrical character?",
                YAction = () => AddTheatricalCharacter(collection)
            };
            dialog.Print();
        }

        public static void Delete()
        {
            var actors = Service.GetAll();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            HelperMethods.PrintHeader("Delete");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            for (int i = 0; i < actors.Count(); i++)
            {
                Console.WriteLine($"{i + 1})");
                PrintActor(actors.ElementAt(i));
                Console.WriteLine();
            }
            var form = new NumberForm<int>()
            {
                Min = 1,
                Max = actors.Count(),
                Handler = int.TryParse,
                Name = "actor's number"
            };
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("DELETE:");
            Console.ForegroundColor = ConsoleColor.Red;
            var number = form.GetNumber();
            number--;
            var name = actors.ElementAt(number).FullName;
            Service.Delete(number);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Actor with number {number + 1} ({name}) has been successfully deleted");
            HelperMethods.Continue();
        }

        public static void Clear()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            var dialog = new Dialog
            {
                Question = "Do you realy want to clear all data?",
                YAction = () =>
                {
                    Service.Clear();
                    Console.ForegroundColor= ConsoleColor.DarkGreen;
                    Console.WriteLine("Cleared successfully.");
                    ApiContainer.SeedData();
                    Console.WriteLine("Please, remember to save the changes.");
                },
                NAction = () =>
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Clearing canceled."); 
                }
            };
            dialog.Print();
            HelperMethods.Continue();
        }

        public static void Save()
        {
            Console.Clear();
            if (!Api.IsSaved)
            {
                ApiContainer.Save();
            }
            else
            {
                Console.WriteLine("Your data is already saved.");
                HelperMethods.Continue();
            }
        }
    }
}
