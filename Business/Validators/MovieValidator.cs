using Backend.Models;

namespace Business.Validators
{
    public class MovieValidator : ModelValidator<Movie>
    {
        public override bool IsValid(Movie model, ICollection<string?>? errors = null)
        {
            var boolResults = new List<bool> { base.IsValid(model, errors) };
            var genreValidator = new ModelValidator<Genre>();
            var directorValidator = new ModelValidator<Person>();
            foreach (var genre in model.Genres)
                boolResults.Add(genreValidator.IsValid(genre, errors));
            boolResults.Add(directorValidator.IsValid(model.Director, errors));
            return boolResults.All(r => r);
        }
    }
}
