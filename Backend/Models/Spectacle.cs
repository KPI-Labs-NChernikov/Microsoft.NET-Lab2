using Backend.Comparers;
using Backend.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Spectacle : IPerformance
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

        private readonly IEnumarebleEqualityComparer<Genre> _genreComparer = new();

        public override bool Equals(object? obj)
        {
            if (obj is not Spectacle other) 
                return false;
            return Name == other.Name
                && _genreComparer.Equals(Genres, other.Genres);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, _genreComparer.GetHashCode(Genres));
        }
    }
}
