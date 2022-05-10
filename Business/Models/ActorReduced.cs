using Backend.Models;

namespace Business.Models
{
    public class ActorReduced : Person
    {
        public IEnumerable<TheatricalCharacter> TheatricalCharacters { get; set; } = new List<TheatricalCharacter>();

        public static implicit operator Actor(ActorReduced actorRed)
        {
            return new Actor
            {
                BirthYear = actorRed.BirthYear,
                TheatricalCharacters = actorRed.TheatricalCharacters.ToList(),
                FirstName = actorRed.FirstName,
                LastName = actorRed.LastName,
                Patronymic = actorRed.Patronymic
            };
        }

        public static explicit operator ActorReduced(Actor actor)
        {
            return new ActorReduced
            {
                BirthYear = actor.BirthYear,
                TheatricalCharacters = actor.TheatricalCharacters,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                Patronymic = actor.Patronymic
            };
        }
    }
}
