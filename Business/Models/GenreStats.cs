using Backend.Models;

namespace Business.Models
{
    public record GenreStats
    {
        public Genre Genre { get; init; } = new Genre();

        public int MoviesQuantity { get; init; }

        public int SpectaclesQuantity { get; init; }

        public int TotalQuantity => MoviesQuantity + SpectaclesQuantity;
    }
}
