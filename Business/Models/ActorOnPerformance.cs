using Backend.Models;

namespace Business.Models
{
    public record ActorOnPerformance
    {
        public Person Actor { get; init; } = new Person();

        public string Role { get; init; } = string.Empty;

        public bool IsMain { get; init; }
    }
}
