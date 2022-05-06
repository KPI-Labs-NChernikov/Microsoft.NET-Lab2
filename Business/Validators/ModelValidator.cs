using Business.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Business.Validators
{
    public class ModelValidator<T> : IModelValidator<T> where T : class
    {
        public virtual bool IsValid(T model, ICollection<string?>? errors = null)
        {
            var results = (errors == null) ? null : new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), results, true);
            if (results != null)
            {
                foreach (var result in results)
                    errors!.Add(result.ErrorMessage);
            }
            return isValid;
        }
    }
}
