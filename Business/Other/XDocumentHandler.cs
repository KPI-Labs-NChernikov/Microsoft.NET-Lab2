using Backend.Interfaces;
using Backend.Models;
using Business.Interfaces;
using System.Xml.Linq;

namespace Business.Other
{
    public class XDocumentHandler : IXDocumentHandler
    {
        internal bool NeedsRegeneration { get; set; } = true;

        private XDocument _doc = new();

        private readonly IXmlContext<Actor> _context;

        public XDocumentHandler(IXmlContext<Actor> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null");
        }

        public XDocument RequestXDocument()
        {
            if (NeedsRegeneration)
            {
                _doc = _context.GenerateXDocument();
                NeedsRegeneration = false;
            }
            return _doc;
        }
    }
}
