using Backend.Interfaces;
using Backend.Models;
using Business.Services;

namespace Business.Interfaces
{
    public interface IApi
    {
        IActorService ActorService { get; }

        ActorInfoService ActorInfoService { get; }

        bool IsSaved { get; set; }

        Stream SaveStream { get; set; }

        string? SaveFile { get; set; } 

        void Save();
    }
}
