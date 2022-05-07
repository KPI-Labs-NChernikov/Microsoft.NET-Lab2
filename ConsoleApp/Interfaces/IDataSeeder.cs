using Backend.Interfaces;

namespace ConsoleApp.Interfaces
{
    public interface IDataSeeder<T> where T : class
    {
        IXmlContext<T> Context { get; }

        void SeedData();
    }
}
