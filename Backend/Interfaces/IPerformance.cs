using Backend.Models;

namespace Backend.Interfaces
{
    public interface IPerformance
    {
        string Name { get; set; }

        ICollection<Genre> Genres { get; set; }
    }
}
