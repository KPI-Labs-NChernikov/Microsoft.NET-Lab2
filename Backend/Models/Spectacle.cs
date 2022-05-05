using Backend.Comparers;
using Backend.Interfaces;

namespace Backend.Models
{
    public class Spectacle : IPerformance
    {
        public string Name { get; set; } = string.Empty;

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
