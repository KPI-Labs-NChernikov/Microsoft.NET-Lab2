using Backend.Interfaces;
using Backend.Models;

namespace Business.Validators
{
    public class FilmographyItemValidator : ModelValidator<FilmographyItem>
    {
        public override bool IsValid(FilmographyItem model, ICollection<string?>? errors = null)
        {
            var boolResults = new List<bool> { base.IsValid(model, errors) };
            if (model.Performance is Movie mov)
            {
                var validator = new MovieValidator();
                boolResults.Add(validator.IsValid(mov, errors));
            }
            else
            {
                var validator = new SpectacleValidator();
                boolResults.Add(validator.IsValid((Spectacle)model.Performance, errors));
            }
            return boolResults.All(r => r);
        }
    }
}
