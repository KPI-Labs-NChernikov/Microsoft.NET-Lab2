using Backend.Interfaces;
using Backend.Models;
using Business.Interfaces;
using Business.Models;
using System.Xml.Linq;

namespace Business.Services
{
    public class ActorInfoService
    {
        private readonly IXDocumentHandler _handler;

        protected XDocument Document => _handler.RequestXDocument();

        public ActorInfoService(IXDocumentHandler handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// 1) Get all actors with their filmographies and theatrical characters.
        /// Sort by full name, then - year of birth
        /// </summary>
        /// <returns>IEnumerable of Actors, sorted by fullname, then - year of birth</returns>
        public IEnumerable<Actor> GetActors()
        {
            return Document
                .Descendants("actor")
                .Select(a => new Actor()
                {
                    FirstName = a.Element("firstName")!.Value,
                    LastName = a.Element("lastName")!.Value,
                    Patronymic = a.Element("patronymic")?.Value,
                    BirthYear = ushort.Parse(a.Element("birthYear")!.Value),
                    TheatricalCharacters = a.Descendants("theatricalCharacter")
                    .Select(t => new TheatricalCharacter { Name = t.Value }).ToList(),
                    Filmography = a.Descendants("filmographyItem")
                    .Select(t =>
                    {
                        var item = new FilmographyItem
                        {
                            Role = t.Element("role")!.Value,
                            IsMain = bool.Parse(t.Element("isMain")!.Value),
                            Performance = (t.Descendants("_type").First().Value == typeof(Movie).ToString())
                               ? new Movie
                               {
                                   Year = ushort.Parse(t.Descendants("year").First().Value),
                                   Director = new Person
                                   {
                                       FirstName = a.Descendants("firstName").First().Value,
                                       LastName = a.Descendants("lastName").First().Value,
                                       Patronymic = a.Descendants("patronymic").FirstOrDefault()?.Value,
                                       BirthYear = ushort.Parse(a.Descendants("birthYear").First().Value),
                                   }
                               }
                               : new Spectacle()
                        };
                        item.Performance.Name = t.Descendants("name").First().Value;
                        item.Performance.Genres = t.Descendants("genre")
                        .Select(g => new Genre { Name = g.Value })
                        .ToList();
                        return item;
                    }).ToList()
                })
                .OrderBy(a => a.FullName)
                .ThenBy(a => a.BirthYear);
        }

        /// <summary>
        /// 2) Get all films starting from $year. Sort by year descending, then by name ascending
        /// </summary>
        /// <param name="year">Start from</param>
        /// <returns>IEnumerable of Movies starting from $year. Sort by year descending, then by name ascending</returns>
        public IEnumerable<Movie> GetMoviesFromYear(ushort year)
        {
            return Document
                .Descendants("performance")
                .Where(t => t.Element("_type")!.Value == typeof(Movie).ToString()
                && ushort.Parse(t.Element("year")!.Value) >= year)
                .Select(t => new Movie
                {
                    Name = t.Element("name")!.Value,
                    Year = ushort.Parse(t.Element("year")!.Value),
                    Genres = t.Descendants("genre").Select(g => new Genre { Name = g.Value}).ToList(),
                    Director = new Person
                    {
                        FirstName = t.Descendants("firstName").First().Value,
                        LastName = t.Descendants("lastName").First().Value,
                        Patronymic = t.Descendants("patronymic").FirstOrDefault()?.Value,
                        BirthYear = ushort.Parse(t.Descendants("birthYear").First().Value)
                    }
                })
                .Distinct()
                .OrderByDescending(t => t.Year)
                .ThenBy(t => t.Name);
        }

        /// <summary>
        /// 3) Get all stakeholders with information about them and their professions
        /// </summary>
        /// <returns>IEnumerable of IGrouping<Profession, Person> with all stakeholders.
        /// Sort: for groups: actors, then directors. Inside groups: fullname</returns>
        public IEnumerable<IGrouping<Profession, Person>> GetStakeholders()
        {
            return Document
                .Descendants("actor")
                .Select(a => new { Profession = Profession.Actor, Element = a })
                .Concat(Document
                    .Descendants("director")
                    .Select(d => new { Profession = Profession.Director, Element = d }))
                .Select(p => new
                {
                    p.Profession,
                    Person = new Person
                    {
                        FirstName = p.Element.Element("firstName")!.Value,
                        LastName = p.Element.Element("lastName")!.Value,
                        Patronymic = p.Element.Element("patronymic")?.Value,
                        BirthYear = ushort.Parse(p.Element.Element("birthYear")!.Value)
                    }
                })
                .Distinct()
                .OrderBy(a => a.Person.FullName)
                .GroupBy(a => a.Profession, v => v.Person)
                .OrderBy(g => g.Key);
        }

        /// <summary>
        /// 4) Get directors with quantity of movies directed by them. 
        /// Order by quantity of movies descending, then by full name ascending
        /// </summary>
        /// <returns>IEnumerable of DirectorStats, 
        /// that contains Directors and Count of films directed by them</returns>
        public IEnumerable<DirectorStats> GetDirectorsStats()
        {
            return from movElem in Document.Descendants("performance")
                   where movElem.Element("_type")!.Value == typeof(Movie).ToString()
                   select new Movie
                   {
                       Name = movElem.Element("name")!.Value,
                       Year = ushort.Parse(movElem.Element("year")!.Value),
                       Genres = movElem.Descendants("genre").Select(g => new Genre { Name = g.Value }).ToList(),
                       Director = new Person
                       {
                           FirstName = movElem.Descendants("firstName").First().Value,
                           LastName = movElem.Descendants("lastName").First().Value,
                           Patronymic = movElem.Descendants("patronymic").FirstOrDefault()?.Value,
                           BirthYear = ushort.Parse(movElem.Descendants("birthYear").First().Value)
                       }
                   } into mov
                   group mov by mov into g
                   select g.Key into distMov
                   group distMov by distMov.Director into g
                   let count = g.Count()
                   orderby count descending, g.Key.FullName
                   select new DirectorStats()
                   {
                       Director = g.Key,
                       MoviesCount = count
                   };
        }

        /// <summary>
        /// 5) Get all spectacles. Sort by name
        /// </summary>
        /// <returns>IEnumerable of Spectacles, sorted by name</returns>
        public IEnumerable<Spectacle> GetSpectacles()
        {
            return from spec in Document.Descendants("performance")
                   where spec.Element("_type")!.Value == typeof(Spectacle).ToString()
                   let name = spec.Element("name")!.Value
                   orderby name
                   select new Spectacle
                   {
                       Name = name,
                       Genres = spec
                           .Descendants("genre")
                           .Select(g => new Genre { Name = g.Value }).ToList()
                   } into converted
                   group converted by converted into g
                   select g.Key;
        }

        /// <summary>
        /// 6) TODO
        /// </summary>
        /// <returns>IEnumerable of groups with genres and movies</returns>
        public IEnumerable<IGrouping<Genre, Movie>> Query6()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 7) Get top-N actors, sorted by quantity of main roles both in movies and speactacles.
        /// </summary>
        /// <param name="quantity">needed quantity (top N)</param>
        /// <returns>IEnumerable of tuple with actors and quantity of theire main roles</returns>
        public IEnumerable<ActorStats> GetTopMainRolesPopularActors(int quantity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 8) Find all actors that fullname contains $name
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public IEnumerable<ActorReduced> FindActorByName(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 9) Get genres that were used both in movies and spectacles
        /// </summary>
        /// <returns>IEnumerable of Genre with genres that were used both in movies and spectacles</returns>
        public IEnumerable<Genre> GetUniversalGenres()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 10) Get all actors that are also directors. Sort by year of birth
        /// </summary>
        /// <returns>IEnumerable of Actor that contains actors that were directors too, sorted by year of birth</returns>
        public IEnumerable<ActorReduced> GetActorsDirectors()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 11) TODO!!!
        /// </summary>
        /// <returns>IEnumerable of Actor that contains actors that starred in at least one movie 
        /// or spectacle with genre $genreId.</returns>
        public IEnumerable<Actor> Query11()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 12) Find all films by director whose full name contains $name. Sort by film year descending
        /// </summary>
        /// <param name="name"></param>
        /// <returns>IEnumerable of Movie that contains all films 
        /// by director whose fullname contains $name,
        /// sorted by film year descending</returns>
        public IEnumerable<Movie> FindMoviesByDirectorName(string name)
        {
            // Use Ancestors()!!!
            throw new NotImplementedException();
        }

        /// <summary>
        /// 13) Find all films and spectacles by name. Group by type - spectacle or movie
        /// </summary>
        /// <param name="name"></param>
        /// <returns>IEnumerable of Grouping of Type and its' performaances</returns>
        public IEnumerable<IGrouping<Type, IPerformance>> FindPerformancesByName(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 14) Get genres with quantity of movies and spectacles of them. 
        /// Sort by quantity of movies desc., then - spectacles desc.
        /// </summary>
        /// <returns>IEnumerable of GenreStats</returns>
        public IEnumerable<GenreStats> GetGenresStats()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 15) Find spectacles of the specific genre
        /// </summary>
        /// <param name="genre"></param>
        /// <returns>IEnumerable of Spectacle that contains spectacles of $genre</returns>
        public IEnumerable<Spectacle> FindSpectaclesByGenre(string? genre)
        {
            return Document
                .Descendants("genre")
                .Where(g => g!.Value.Trim().ToLower() == genre?.Trim().ToLower()
                    && g.Ancestors("performance").First().Element("_type")!.Value == typeof(Spectacle).ToString())
                .Select(g =>
                {
                    var performance = g.Ancestors("performance");
                    return new Spectacle
                    {
                        Name = performance.Elements("name")
                                  .First().Value,
                        Genres = performance
                                  .Descendants("genre")
                                  .Select(t => new Genre { Name = t.Value }).ToList()
                    };
                })
                .Distinct();
        }
    }
}
