namespace Business.Models
{
    public record GenreStats
    {
        public string Genre { get; init; } = string.Empty;

        public int MoviesQuantity { get; init; }

        public int SpectaclesQuantity { get; init; }

        public int TotalQuantity => MoviesQuantity + SpectaclesQuantity;
    }
}
