using Backend.Attributes;
using Backend.Comparers;
using Backend.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Movie : IPerformance
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public ushort Year { get; set; }

        [Required]
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

        [Required]
        [XmlIgnoreInheritance]
        public Person Director { get; set; } = new Person();

        private readonly IEnumarebleEqualityComparer<Genre> _genreComparer = new();

        public override bool Equals(object? obj)
        {
            if (obj is not Movie other)
                return false;
            return Name == other.Name
                && Year == other.Year
                && Director.Equals(other.Director)
                && _genreComparer.Equals(Genres, other.Genres);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Year, Director, _genreComparer.GetHashCode(Genres));
        }
    }
}
