using Backend.Interfaces;
using Backend.Models;
using Business.Interfaces;
using Business.Services;

namespace Business
{
    public class Api : IApi
    {
        public IActorService ActorService { get; }

        public ActorInfoService ActorInfoService { get; }

        public IXmlContext<Actor> Context { get; }

        public bool IsSaved { get; set; }

        public Api(IXmlContext<Actor> context, string saveFile)
        {
            Context = context;
            ActorService = new ActorService(context);
            void changeSaved() { IsSaved = false; }
            ActorService.OnChange += changeSaved;
            ActorInfoService = new ActorInfoService(context);
            SaveFile = saveFile;
        }

        public string SaveFile
        {
            get
            {
                return _saveFile;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(nameof(value));
                _saveFile = value;
                IsSaved = false;
            }
        }

        private string _saveFile = string.Empty;

        public void Save()
        {
            Context.Save(SaveFile);
            IsSaved = true;
        }
    }
}
