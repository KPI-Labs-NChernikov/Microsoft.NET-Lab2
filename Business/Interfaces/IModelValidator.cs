namespace Business.Interfaces
{
    public interface IModelValidator<T> where T : class
    {
        bool IsValid(T model, ICollection<string?>? errors = null);
    }
}
