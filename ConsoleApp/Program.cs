using Backend;
using Backend.Models;

Console.WriteLine("Hello, World!");
var actors = new List<Actor>
{
    new Actor() //1
                {
                    FirstName = "Sandra",
                    LastName = "Anderson",
                    Patronymic = "Louise",
                    BirthYear = 1944,
                    TheatricalCharacters = new List<TheatricalCharacter>
                    {
                        new TheatricalCharacter()
                        {
                            Name = "Roles of an acute plan"
                        },
                        new TheatricalCharacter()
                        {
                            Name = "Cowboy girlfriend"
                        }
                    }
                },
                new Actor() //2
                {
                    FirstName = "Clint",
                    LastName = "Eastwood",
                    BirthYear = 1930
                },
                new Actor() //5
                {
                    FirstName = "Ella",
                    LastName = "Sanko",
                    Patronymic = "Ivanivna",
                    BirthYear = 1947
                },
                new Actor() //6
                {
                    FirstName = "Bohdan",
                    LastName = "Stupka",
                    Patronymic = "Sylvestrovych",
                    BirthYear = 1941
                },
                new Actor() //8
                {
                    FirstName = "William",
                    LastName = "Pitt",
                    Patronymic = "Bradley",
                    BirthYear = 1963
                },
                new Actor() //9
                {
                    FirstName = "Leonardo",
                    LastName = "DiCaprio",
                    Patronymic = "Wilhelm",
                    BirthYear = 1974
                },
                new Actor() //10
                {
                    FirstName = "Orlando",
                    LastName = "Bloom",
                    BirthYear = 1977
                },
                new Actor() //11
                {
                    FirstName = "Johnny",
                    LastName = "Depp",
                    BirthYear = 1963
                },
                new Actor() //18
                {
                    FirstName = "Volodymyr",
                    LastName = "Velyanyk",
                    Patronymic = "Volodymyrovych",
                    BirthYear = 1970
                }
};

var context = new Context();
context.Load("hello.xml");
var temp = context.Document;
context.Save("hello.xml");