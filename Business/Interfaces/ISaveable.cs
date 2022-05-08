namespace Business.Interfaces
{
    public interface ISaveable
    {
        /// <summary>
        /// An event that is invoked on any changes: Add, Update, Delete or Clear
        /// May be used to control the state of the context (saved or not)
        /// </summary>
        event Action? OnChange;
    }
}
