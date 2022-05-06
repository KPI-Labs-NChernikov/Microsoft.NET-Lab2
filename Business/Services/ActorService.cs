using Backend.Interfaces;
using Backend.Models;
using Business.Interfaces;
using Business.Validators;

namespace Business.Services
{
    public class ActorService : IActorService
    {
        public event Action? OnChange;

        private readonly IXmlContext<Actor> _context;

        public ActorService(IXmlContext<Actor> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null");
        }

        public void Add(Actor actor)
        {
            var validator = new ActorValidator();
            var validationResults = new List<string?>();
            var isValid = validator.IsValid(actor, validationResults);
            if (!isValid)
            {
                if (validationResults.Count > 10)
                {
                    validationResults = validationResults.Take(10).ToList();
                    validationResults.Add("...");
                }
                throw new ArgumentException(string.Join(Environment.NewLine, validationResults), nameof(actor));
            }
            _context.Items.Add(actor);
            OnChange?.Invoke();
        }

        public void Delete(int index)
        {
            var item = _context.Items.ElementAt(index);
            _context.Items.Remove(item);
            OnChange?.Invoke();
        }

        public IEnumerable<Actor> GetAll() => _context.Items;
    }
}
