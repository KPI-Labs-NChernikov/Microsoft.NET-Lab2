using Backend.Interfaces;
using Backend.Models;
using Business.Interfaces;
using System.Xml.Linq;

namespace Business.Services
{
    public class ActorInfoService
    {
        public XDocumentHandler Handler { get; set; }

        private XDocument Document => Handler.RequestXDocument();

        public ActorInfoService(IXmlContext<Actor> context)
        {
            Handler = new XDocumentHandler(context);
        }
    }
}
