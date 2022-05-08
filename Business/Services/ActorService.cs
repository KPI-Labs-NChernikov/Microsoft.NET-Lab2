using Backend.Interfaces;
using Backend.Models;
using Business.Interfaces;
using Business.Validators;

namespace Business.Services
{
    public class ActorService : IActorService
    {
        /// <summary>
        /// An event that is invoked on any changes: Add, Update, Delete or Clear
        /// May be used to control the state of the context (saved or not)
        /// </summary>
        public event Action? OnChange;

        private readonly IXmlContext<Actor> _context;

        public ActorService(IXmlContext<Actor> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null");
        }

        /// <summary>
        /// Adds an item to the context. Uses an attributes validation
        /// </summary>
        /// <param name="actor">Item to add</param>
        /// <exception cref="ArgumentException">
        /// The actor object is not valid according to the attributes
        /// </exception>
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

        /// <summary>
        /// Deletes an item (actor) from the context
        /// </summary>
        /// <param name="index">Index of the item that should be deleted</param>
        /// <exception cref="ArgumentOutOfRangeException">Index is outside the bounds of the sequence.</exception>
        public void Delete(int index)
        {
            var item = _context.Items.ElementAt(index);
            _context.Items.Remove(item);
            OnChange?.Invoke();
        }

        /// <summary>
        /// Removes all items (actors) from the context
        /// </summary>
        public void Clear()
        {
            _context.Items.Clear();
            OnChange?.Invoke();
        }

        /// <summary>
        /// Returns all items (actors) of the context
        /// </summary>
        /// <returns>IEnumerable of Actor - all actors in the context</returns>
        public IEnumerable<Actor> GetAll() => _context.Items;
    }
}
