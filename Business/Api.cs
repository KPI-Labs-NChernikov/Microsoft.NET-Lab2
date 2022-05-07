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

        private readonly IXmlContext<Actor> _context;

        public bool IsSaved { get; set; }

        public Api(IXmlContext<Actor> context)
        {
            _context = context;
            ActorService = new ActorService(context);
            void changeSaved() { IsSaved = false; }
            ActorService.OnChange += changeSaved;
            ActorInfoService = new ActorInfoService(context);
            using var stream = new MemoryStream();
            _saveStream = stream;
        }

        public Stream SaveStream
        {
            get
            {
                return _saveStream;
            }
            set
            {
                _saveStream = value ?? throw new ArgumentNullException(nameof(value));
                _saveFile = null;
                IsSaved = false;
            }
        }

        private Stream _saveStream;

        public string? SaveFile
        {
            get
            {
                return _saveFile;
            }
            set
            {
                _saveFile = value ?? throw new ArgumentNullException(nameof(value));
                using var fs = new FileStream(_saveFile, FileMode.Create);
                _saveStream = fs;
                IsSaved = false;
            }
        }

        private string? _saveFile;

        public void Save()
        {
            _context.Save(SaveStream);
            IsSaved = true;
        }
    }
}
