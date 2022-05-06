using Backend.Models;

namespace Business.Validators
{
    public class ActorValidator : ModelValidator<Actor>
    {
        public override bool IsValid(Actor model, ICollection<string?>? errors = null)
        {
            var boolResults = new List<bool> { base.IsValid(model, errors) };
            if (model.TheatricalCharacters != null)
            {
                var validator = new ModelValidator<TheatricalCharacter>();
                foreach (var tc in model.TheatricalCharacters)
                    boolResults.Add(validator.IsValid(tc, errors));
            }
            if (model.Filmography != null)
            {
                var validator = new FilmographyItemValidator();
                foreach (var item in model.Filmography)
                    boolResults.Add(validator.IsValid(item, errors));
            }
            return boolResults.All(r => r);
        }
    }
}
