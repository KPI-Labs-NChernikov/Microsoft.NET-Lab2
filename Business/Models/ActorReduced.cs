using Backend.Models;

namespace Business.Models
{
    public class ActorReduced : Person
    {
        public IEnumerable<TheatricalCharacter> TheatricalCharacters { get; set; } = new List<TheatricalCharacter>();
    }
}
