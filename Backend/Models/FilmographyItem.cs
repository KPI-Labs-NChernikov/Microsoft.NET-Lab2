using Backend.Interfaces;

namespace Backend.Models
{
    public class FilmographyItem
    {
        public string Role { get; set; } = string.Empty;

        public bool IsMain { get; set; }

        public IPerformance Performance { get; set; } = new Spectacle();

        public override bool Equals(object? obj)
        {
            if (obj is not FilmographyItem other)
                return false;
            return Role == other.Role
                && IsMain == other.IsMain
                && Performance == other.Performance;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Role, IsMain, Performance);
        }
    }
}
