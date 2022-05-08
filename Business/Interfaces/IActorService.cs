using Backend.Models;

namespace Business.Interfaces
{
    public interface IActorService : ISaveable
    {
        IEnumerable<Actor> GetAll();

        void Add(Actor actor);

        void Delete(int index);

        public void Clear();
    }
}
