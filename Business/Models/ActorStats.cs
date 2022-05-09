namespace Business.Models
{
    public record ActorStats
    {
        public ActorReduced Actor { get; init; } = new ActorReduced();

        public int MainRolesQuantity { get; init; }
    }
}
