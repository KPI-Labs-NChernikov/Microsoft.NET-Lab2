using Backend.Interfaces;
using Backend.Models;
using Business.Interfaces;
using System.Xml.Linq;

namespace Business.Services
{
    public class ActorInfoService
    {
        private readonly IXDocumentHandler _handler;

        protected XDocument Document => _handler.RequestXDocument();

        public ActorInfoService(IXDocumentHandler handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }
    }
}
