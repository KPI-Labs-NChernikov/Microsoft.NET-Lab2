using Backend.Interfaces;
using Backend.Models;
using ConsoleApp.Interfaces;

namespace ConsoleApp.Data
{
    public class DataSeeder : IDataSeeder<Actor>
    {
        public IXmlContext<Actor> Context { get; }

        public DataSeeder(IXmlContext<Actor> context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null");
        }

        public void SeedData()
        {
            var genres = new List<Genre>()
            {
                new Genre()         // 0
                {
                    Name = "Action"
                },
                new Genre()         // 1
                {
                    Name = "Romantic"
                },
                new Genre()         // 2
                {
                    Name = "Fantasy"
                },
                new Genre()         // 3
                {
                    Name = "Drama"
                },
                new Genre()         // 4
                {
                    Name = "Science fiction"
                },
                new Genre()         // 5
                {
                    Name = "Dramedy"
                },
                new Genre()         // 6
                {
                    Name = "Fairy tale"
                },
                new Genre()         // 7
                {
                    Name = "Western"
                },
                new Genre()         // 8
                {
                    Name = "Comedy"
                },
                new Genre()         // 9
                {
                    Name = "Adventure"
                }
            };

            var spectacles = new List<Spectacle>()
            {
                new Spectacle()     // 0
                {
                    Name = "Mowgli",
                    Genres = new List<Genre>{genres[6], genres[9] }
                },
                new Spectacle()     // 1
                {
                    Name = "Caidashi",
                    Genres = new List<Genre>{genres[5] }
                },
                new Spectacle()     // 2
                {
                    Name = "The Master and Margarita",
                    Genres = new List<Genre>{ genres[3]}
                },
                new Spectacle()    // 3
                { 
                    Name = "Forever alive",
                    Genres = new List<Genre>{genres[3]}
                }
            };

            var directors = new List<Person>()
            {
                new Actor() //0
                {
                    FirstName = "Clint",
                    LastName = "Eastwood",
                    BirthYear = 1930
                },
                new Person()  //1
                {
                    FirstName = "Gregor",
                    LastName = "Verbinski",
                    Patronymic = "Justin",
                    BirthYear = 1963
                },
                new Person()  //2
                {
                    FirstName = "Peter",
                    LastName = "Jackson",
                    Patronymic = "Robert",
                    BirthYear = 1961
                },
                new Person()  //3
                {
                    FirstName = "Martin",
                    LastName = "Scorsese",
                    Patronymic = "Charles",
                    BirthYear = 1942
                },
                new Person()  //4
                {
                    FirstName = "James",
                    LastName = "Cameron",
                    Patronymic = "Francis",
                    BirthYear = 1954
                },
                new Person()  //5
                {
                    FirstName = "Yuri",
                    LastName = "Illienko",
                    Patronymic = "Herasymovych",
                    BirthYear = 1936
                },
                new Person()  //6
                {
                    FirstName = "Sergio",
                    LastName = "Leone",
                    BirthYear = 1929
                }
            };

            var movies = new List<Movie>()
            {
                new Movie() //0
                {
                    Name = "The White Bird Marked with Black",
                    Year = 1971,
                    Director = directors[5],
                    Genres = new List<Genre>{ genres[3] }
                },
                new Movie() //1
                {
                    Name = "Bronco Billy",
                    Year = 1980,
                    Director = directors[0],
                    Genres = new List<Genre>{ genres[7]}
                },
                new Movie() //2
                {
                    Name = "Titanic",
                    Year = 1997,
                    Director = directors[4],
                    Genres = new List<Genre>{genres[1]}
                },
                new Movie() //3
                {
                    Name = "The Wolf of Wall Street",
                    Year = 2013,
                    Director = directors[3],
                    Genres = new List<Genre>{genres[3]}
                },
                new Movie() //4
                {
                    Name = "Dollars Trilogy",
                    Year = 1966,
                    Director = directors[6],
                    Genres = new List<Genre>{ genres[7]}
                },
                new Movie() //5
                {
                    Name = "The Lord of the Rings: The Fellowship of the Ring",
                    Year = 2001,
                    Director = directors[2],
                    Genres = new List<Genre> { genres[4] }
                },
                new Movie() //6
                {
                    Name = "Pirates of the Caribbean: The Curse of the Black Pearl",
                    Year = 2003,
                    Director = directors[1],
                    Genres = new List<Genre> { genres[0] }
                },
                new Movie() //7
                {
                    Name = "The Mexican",
                    Year = 2001,
                    Director = directors[1],
                    Genres = new List<Genre> { genres[9], genres[8] }
                }
            };

            var characters = new List<TheatricalCharacter>()
            {
                new TheatricalCharacter() //0
                {
                    Name = "The Good, the Bad, and the Very Ugly"
                },
                new TheatricalCharacter() //1
                {
                    Name = "Typical western cowboy"
                },
                new TheatricalCharacter() //2
                {
                    Name = "Many different characters"
                },
                new TheatricalCharacter() //3
                {
                    Name = "Negative characters"
                },
                new TheatricalCharacter() //4
                {
                    Name = "Hero lover"
                },
                new TheatricalCharacter() //5
                {
                    Name = "Villain"
                },
                new TheatricalCharacter() //6
                {
                    Name = "Hero"
                },
                new TheatricalCharacter() //7
                {
                    Name = "Villain"
                },
                new TheatricalCharacter() //8
                {
                    Name = "Roles of an acute plan"
                },
                new TheatricalCharacter()   //9
                {
                    Name = "Cowboy girlfriend"
                }
            };

            var actors = new List<Actor>
            {
                new Actor() //0
                {
                    FirstName = "Sandra",
                    LastName = "Anderson",
                    Patronymic = "Louise",
                    BirthYear = 1944,
                    TheatricalCharacters = new List<TheatricalCharacter>
                    {
                        characters[0], characters[8], characters[9]
                    },
                    Filmography = new List<FilmographyItem>
                    {
                        new FilmographyItem
                        {
                            Performance = movies[1],
                            Role = "Antoinette Lily",
                            IsMain = false
                        }
                    }
                },
                new Actor() //1
                {
                    FirstName = "Ella",
                    LastName = "Sanko",
                    Patronymic = "Ivanivna",
                    BirthYear = 1947,
                    TheatricalCharacters = new List<TheatricalCharacter> { characters[2] },
                    Filmography = new List<FilmographyItem>
                    {
                        new FilmographyItem
                        {
                            Performance = spectacles[0],
                            Role = "Raksha",
                            IsMain = true
                        },
                        new FilmographyItem
                        {
                            Performance = spectacles[1],
                            Role = "Paraska",
                            IsMain = false
                        },
                    }
                },
                new Actor() //2
                {
                    FirstName = "Bohdan",
                    LastName = "Stupka",
                    Patronymic = "Sylvestrovych",
                    BirthYear = 1941,
                    TheatricalCharacters = new List<TheatricalCharacter> { characters[3] },
                    Filmography = new List<FilmographyItem>
                    {
                        new FilmographyItem
                        {
                            Performance = spectacles[2],
                            Role = "Jeshua",
                            IsMain = true
                        },
                        new FilmographyItem
                        {
                            Performance = movies[0],
                            Role = "Orest",
                            IsMain = true
                        },
                    }
                },
                new Actor() //3
                {
                    FirstName = "William",
                    LastName = "Pitt",
                    Patronymic = "Bradley",
                    BirthYear = 1963,
                    TheatricalCharacters = new List<TheatricalCharacter> { characters[4] },
                    Filmography = new List<FilmographyItem>
                    {
                        new FilmographyItem
                        {
                            Performance = movies[7],
                            Role = "Jerry Welbach",
                            IsMain = true
                        }
                    }
                },
                new Actor() //4
                {
                    FirstName = "Leonardo",
                    LastName = "DiCaprio",
                    Patronymic = "Wilhelm",
                    BirthYear = 1974,
                    TheatricalCharacters = new List<TheatricalCharacter> { characters[5] },
                    Filmography = new List<FilmographyItem>
                    {
                        new FilmographyItem
                        {
                            Performance = movies[2],
                            Role = "Jack Dawson",
                            IsMain = true
                        },
                        new FilmographyItem
                        {
                            Performance = movies[3],
                            Role = "Jordan Belfort",
                            IsMain = true
                        },
                    }
                },
                new Actor() //5
                {
                    FirstName = "Orlando",
                    LastName = "Bloom",
                    BirthYear = 1977,
                    TheatricalCharacters = new List<TheatricalCharacter> { characters[6] },
                    Filmography = new List<FilmographyItem>
                    {
                        new FilmographyItem
                        {
                            Performance = movies[5],
                            Role = "Legolas Greenleaf",
                            IsMain = true
                        },
                        new FilmographyItem
                        {
                            Performance = movies[6],
                            Role = "Will Turner",
                            IsMain = false
                        },
                    }
                },
                new Actor() //6
                {
                    FirstName = "Johnny",
                    LastName = "Depp",
                    BirthYear = 1963,
                    TheatricalCharacters = new List<TheatricalCharacter> { characters[7] },
                    Filmography = new List<FilmographyItem>
                    {
                        new FilmographyItem
                        {
                            Performance = movies[6],
                            Role = "Jack Sparrow",
                            IsMain = true
                        },
                    }
                },
                new Actor() //7
                {
                    FirstName = "Volodymyr",
                    LastName = "Velyanyk",
                    Patronymic = "Volodymyrovych",
                    BirthYear = 1970,
                    Filmography = new List<FilmographyItem>
                    {
                        new FilmographyItem
                        {
                            Performance = spectacles[1],
                            Role = "Omelko",
                            IsMain = true
                        }
                    }
                },
                new Actor() //8
                {
                    FirstName = "Vasyl",
                    LastName = "Kukharsky",
                    BirthYear = 1981,
                    Filmography= new List<FilmographyItem>
                    {
                        new FilmographyItem
                        {
                            Performance= spectacles[3],
                            Role = "Borys",
                            IsMain = true
                        },
                        new FilmographyItem
                        {
                            Performance= spectacles[3],
                            Role = "Mark",
                            IsMain = true
                        }
                    }
                }
            };
            var actor = (Actor)directors[0];        // 9 (as actor)
            actor.Filmography = new List<FilmographyItem>
            {
                new FilmographyItem
                {
                    Performance = movies[1],
                    Role = "Billy \"Bronco Billy\" McCoy",
                    IsMain = true
                },
                new FilmographyItem
                {
                    Performance = movies[4],
                    Role = "Man with No Name",
                    IsMain = true
                }
            };
            actor.TheatricalCharacters = new List<TheatricalCharacter>() { characters[1] };
            actors.Add(actor);

            Context.Items = actors;
        }
    }
}
