using Backend.Models;
using System.Xml;
using System.Xml.Linq;
using Backend.Extensions;
using Backend.Interfaces;
using System.Xml.Serialization;

namespace Backend
{
    public class Context
    {
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();

        public XDocument Document 
        { 
            get 
            {
                using var stream = new MemoryStream();
                Save(stream);
                stream.Position = 0;
                return XDocument.Load(stream);
            } 
        }

        public void Load(Stream stream)
        {
            XmlDocument doc = new();
            doc.Load(stream);
            Actors = new List<Actor>();
            if (doc.DocumentElement is not null)
            {
                foreach (XmlNode node in doc.DocumentElement)
                {
                    var actor = new Actor
                    {
                        FirstName = node[nameof(Actor.FirstName).FirstToLower()]?.InnerText
                            ?? throw new ArgumentNullException(nameof(Actor.FirstName).FirstToLower()),
                        LastName = node[nameof(Actor.LastName).FirstToLower()]?.InnerText
                            ?? throw new ArgumentNullException(nameof(Actor.LastName).FirstToLower()),
                        Patronymic = node[nameof(Actor.Patronymic).FirstToLower()]?.InnerText,
                        BirthYear = ushort.Parse(node[nameof(Actor.BirthYear).FirstToLower()]?.InnerText
                            ?? throw new ArgumentNullException(nameof(Actor.BirthYear).FirstToLower()))
                    };
                    var tcs = node[nameof(Actor.TheatricalCharacters).FirstToLower()]?.ChildNodes;
                    if (tcs is not null)
                        foreach (XmlNode tc in tcs)
                            actor.TheatricalCharacters.Add(new TheatricalCharacter() { Name = tc.InnerText });
                    var performances = node[nameof(Actor.Performances).FirstToLower()]?.ChildNodes;
                    if (performances is not null)
                        foreach (XmlNode performance in performances)
                        {
                            IPerformance performanceConverted;
                            if (performance.Name == nameof(Movie).FirstToLower())
                            {
                                performanceConverted = new Movie
                                {
                                    Year = ushort.Parse(performance[nameof(Movie.Year).FirstToLower()]?.InnerText
                                        ?? throw new ArgumentNullException(nameof(Movie.Year).FirstToLower()))
                                };
                                var director = performance[nameof(Movie.Director).FirstToLower()];
                                if (director is not null)
                                {
                                    ((Movie)performanceConverted).Director = new Person
                                    {
                                        FirstName = director[nameof(Person.FirstName).FirstToLower()]?.InnerText
                                        ?? throw new ArgumentNullException(
                                            nameof(Person.FirstName).FirstToLower()),
                                        LastName = director[nameof(Person.LastName).FirstToLower()]?.InnerText
                                        ?? throw new ArgumentNullException(nameof(Person.LastName).FirstToLower()),
                                        Patronymic = director[nameof(Person.Patronymic).FirstToLower()]?.InnerText,
                                        BirthYear = ushort.Parse(director[nameof(Person.BirthYear).FirstToLower()]?
                                            .InnerText ?? throw new ArgumentNullException(
                                                nameof(Person.BirthYear).FirstToLower()))
                                    };
                                }
                            }
                            else
                                performanceConverted = new Spectacle();
                            performanceConverted.Name = performance[nameof(IPerformance.Name).FirstToLower()]?.InnerText
                                        ?? throw new ArgumentNullException(nameof(IPerformance.Name).FirstToLower());
                            var genres = performance[nameof(Movie.Director).FirstToLower()];
                            if (genres is not null)
                                foreach (XmlNode genre in genres)
                                    performanceConverted.Genres.Add(new Genre { Name = genre.InnerText });
                        }
                    Actors.Add(actor);
                }
            }
        }

        public void Load(string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Open);
            Load(fs);
        }

        public void Save(Stream stream)
        {
            XmlWriterSettings settings = new()
            {
                Indent = true
            };
            using XmlWriter writer = XmlWriter.Create(stream, settings);
            WriteElement(writer, Actors, nameof(Actors));
        }

        private void WriteElement(XmlWriter writer, object? element, string name)
        {
            if (element == null)
                return;
            if (element.GetType().IsPrimitive || element.GetType().IsEnum || element.GetType() == typeof(string))
                writer.WriteElementString(name.FirstToLower(), element.ToString());
            else if (element is IEnumerable<object> enumerable)
            {
                if (enumerable.Any())
                {
                    writer.WriteStartElement(name.FirstToLower());
                    foreach (var item in enumerable)
                        WriteElement(writer, item, item.GetType().Name);
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteStartElement(name.FirstToLower());
                foreach (var prop in element.GetType().GetProperties())
                {
                    if (!prop.CustomAttributes.Any(a => a.AttributeType == typeof(XmlIgnoreAttribute)))
                        WriteElement(writer, prop.GetValue(element), prop.Name);
                }
                writer.WriteEndElement();
            }
        }

        public void Save(string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Create);
            Save(fs);
        }
    }
}
