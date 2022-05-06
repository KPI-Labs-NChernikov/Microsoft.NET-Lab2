using Backend.Interfaces;
using Backend.Models;

namespace Business.Services
{
    public class ActorInfoService
    {
        private readonly IXmlContext<Actor> _context;

        public ActorInfoService(IXmlContext<Actor> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null");
        }
    }
}
