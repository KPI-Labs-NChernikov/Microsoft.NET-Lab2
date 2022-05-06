using Backend.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class FilmographyItem
    {
        [Required(AllowEmptyStrings = false)]
        public string Role { get; set; } = string.Empty;

        [Required]
        public bool IsMain { get; set; }

        [Required]
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
