using System.Xml.Linq;

namespace Backend.Interfaces
{
    public interface IXmlContext<T>
    {
        ICollection<T> Items { get; set; }

        XDocument Document { get; }

        void Load(Stream stream);

        void Load(string fileName);

        void Save(Stream stream);

        void Save(string fileName);
    }
}
