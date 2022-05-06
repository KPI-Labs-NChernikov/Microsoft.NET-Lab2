using Backend.Models;

namespace Business.Validators
{
    public class SpectacleValidator : ModelValidator<Spectacle>
    {
        public override bool IsValid(Spectacle model, ICollection<string?>? errors = null)
        {
            var boolResults = new List<bool> { base.IsValid(model, errors) };
            var genreValidator = new ModelValidator<Genre>();
            foreach (var genre in model.Genres)
                boolResults.Add(genreValidator.IsValid(genre, errors));
            return boolResults.All(r => r);
        }
    }
}
