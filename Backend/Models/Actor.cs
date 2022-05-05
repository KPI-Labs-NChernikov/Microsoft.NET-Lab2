using Backend.Comparers;

namespace Backend.Models
{
    public class Actor : Person
    {
        public ICollection<FilmographyItem> Filmography { get; set; } = new List<FilmographyItem>();

        public ICollection<TheatricalCharacter> TheatricalCharacters { get; set; } = new List<TheatricalCharacter>();

        private readonly IEnumarebleEqualityComparer<FilmographyItem> _performanceComparer = new();

        private readonly IEnumarebleEqualityComparer<TheatricalCharacter> _filmItemComparer = new();

        public override bool Equals(object? obj)
        {
            if (obj is not Actor other)
                return false;
            return base.Equals(other) 
                && _performanceComparer.Equals(Filmography, other.Filmography)
                && _filmItemComparer.Equals(TheatricalCharacters, other.TheatricalCharacters);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), 
                _performanceComparer.GetHashCode(Filmography), 
                _filmItemComparer.GetHashCode(TheatricalCharacters));
        }
    }
}
