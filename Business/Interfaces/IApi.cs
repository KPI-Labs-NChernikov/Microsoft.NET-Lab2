using Backend.Interfaces;
using Backend.Models;
using Business.Services;

namespace Business.Interfaces
{
    public interface IApi
    {
        IXmlContext<Actor> Context { get; }

        IActorService ActorService { get; }

        ActorInfoService ActorInfoService { get; }

        bool IsSaved { get; set; }

        string SaveFile { get; set; } 

        void Save();
    }
}
