using Backend.Comparers;
using Backend.Interfaces;

namespace Backend.Models
{
    public class Actor : Person
    {
        public ICollection<IPerformance> Performances { get; set; } = new List<IPerformance>();

        public ICollection<TheatricalCharacter> TheatricalCharacters { get; set; } = new List<TheatricalCharacter>();

        private readonly IEnumarebleEqualityComparer<IPerformance> _performanceComparer = new();

        private readonly IEnumarebleEqualityComparer<TheatricalCharacter> _tcComparer = new();

        public override bool Equals(object? obj)
        {
            if (obj is not Actor other)
                return false;
            return base.Equals(other) 
                && _performanceComparer.Equals(Performances, other.Performances)
                && _tcComparer.Equals(TheatricalCharacters, other.TheatricalCharacters);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), 
                _performanceComparer.GetHashCode(Performances), 
                _tcComparer.GetHashCode(TheatricalCharacters));
        }
    }
}
