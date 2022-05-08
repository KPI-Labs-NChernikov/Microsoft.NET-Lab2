using System.Xml.Linq;

namespace Backend.Interfaces
{
    public interface IXmlContext<T>
    {
        /// <summary>
        /// Content of the context. Any ICollection<T>
        /// </summary>
        ICollection<T> Items { get; set; }

        /// <summary>
        /// Generate from the Items collection and then returnes
        /// XDocument for querying data using Linq to XML
        /// </summary>
        /// <returns>XDocument for querying data using Linq to XML that contains Items in the XML format</returns>
        XDocument GenerateXDocument();

        /// <summary>
        /// Loads a document from a stream to the Items collection
        /// </summary>
        /// <param name="stream">Stream containing XML data</param>
        void Load(Stream stream);

        /// <summary>
        /// Loads a document from a file to the Items collection
        /// </summary>
        /// <param name="fileName">Path to the file that contains XML data</param>
        void Load(string fileName);

        /// <summary>
        /// Saves a Items' content to a given stream
        /// </summary>
        /// <param name="stream">Stream to which an items' content should be saved</param>
        void Save(Stream stream);

        /// <summary>
        /// Saves a Items' content to a given stream
        /// </summary>
        /// <param name="fileName">File to which an items' content should be saved</param>
        void Save(string fileName);
    }
}
