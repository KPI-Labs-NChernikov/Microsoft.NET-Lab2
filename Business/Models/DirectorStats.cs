using Backend.Models;

namespace Business.Models
{
    public record DirectorStats
    {
        public Person Director { get; init; } = new Person();

        public int MoviesCount { get; init; }
    }
}
