using Backend.Models;
using System.Xml;
using System.Xml.Linq;
using Backend.Extensions;
using System.Xml.Serialization;
using System.Reflection;
using Backend.Interfaces;
using Backend.Attributes;
using Backend.Other;

namespace Backend
{
    public class XmlContext : IXmlContext<Actor>
    {
        /// <summary>
        /// Content of the context. Any ICollection<Actor>
        /// </summary>
        public ICollection<Actor> Items { get; set; } = new List<Actor>();

        /// <summary>
        /// Generate from the Items collection and then returnes
        /// XDocument for querying data using Linq to XML
        /// </summary>
        /// <returns>XDocument for querying data using Linq to XML that contains Items in the XML format</returns>
        public XDocument GenerateXDocument()
        {
            using var stream = new MemoryStream();
            Save(stream);
            stream.Position = 0;
            return XDocument.Load(stream);
        }

        /// <summary>
        /// Loads a document from a stream to the Items collection
        /// </summary>
        /// <param name="stream">Stream containing XML data</param>
        /// <exception cref="XmlException">Thrown if any load or parse error occurs</exception>
        /// <exception cref="MissingMethodException">
        /// It is not possible to create an instance of the given type,
        /// because it is an interface, an abstract class or doesn't have parameterless constructor
        /// and no _type tag has been explicitly specified
        /// </exception>
        public void Load(Stream stream)
        {
            XmlDocument doc = new();
            doc.Load(stream);
            Items = Read<List<Actor>>(doc.DocumentElement, nameof(Items), true) ?? new List<Actor>();
        }

        private const string typeName = "_type";

        private T? Read<T>(XmlNode? node, string name, bool isNullable)
        {
            if (node == null)
                return default;
            var type = typeof(T);
            if (type.IsPrimitive || type.IsEnum || type == typeof(string))
                return (T?)Convert.ChangeType(node.InnerText, type);
            else if (typeof(IEnumerable<object>).IsAssignableFrom(type))
            {
                var innerType = type.GetGenericArguments()[0];
                Type listType = typeof(List<>).MakeGenericType(innerType);
                var enumerable = Activator.CreateInstance(listType);
                foreach (XmlNode item in node)
                {
                    var method = typeof(XmlContext).GetMethod(nameof(XmlContext.Read), 
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    var generic = method!.MakeGenericMethod(innerType);
                    object[] parameters = new object[] { item, innerType.Name, true };
                    var itemConverted = generic.Invoke(this, parameters);
                    enumerable!.GetType().GetMethod("Add")!.Invoke(enumerable, new[] { itemConverted });
                }
                return (T?)enumerable;
            }
            else
            {
                if (node[typeName] != null)
                    type = Type.GetType(node[typeName]!.InnerText)!;
                if (type.IsAbstract)
                {
                    throw new InvalidOperationException("Unable to create an object from the node.\n" +
                        "More likely, your type is an interface or an abstract class" +
                        " and no <type> child node was specified");
                }
                object obj = (T)Activator.CreateInstance(type)!;
                foreach (var prop in type.GetProperties())
                {
                    if (!prop.CustomAttributes.Any(a => a.AttributeType == typeof(XmlIgnoreAttribute)))
                    {
                        var method = typeof(XmlContext).GetMethod(nameof(XmlContext.Read), 
                            BindingFlags.NonPublic | BindingFlags.Instance);
                        var generic = method!.MakeGenericMethod(prop.PropertyType);
                        object?[] parameters = new object?[] 
                        { node[prop.Name.FirstToLower()], prop.PropertyType.ToString(), true };
                        var itemConverted = generic.Invoke(this, parameters);
                        if (itemConverted != default)
                            prop.SetValue(obj, itemConverted, null);
                    }
                }
                return (T?)obj;
            }
        }

        /// <summary>
        /// Loads a document from a file to the Items collection
        /// </summary>
        /// <param name="fileName">Path to the file that contains XML data</param>
        /// <exception cref="XmlException">Thrown if any load or parse error occurs</exception>
        /// <exception cref="FileNotFoundException">Such a file does not exist</exception>
        /// <exception cref="MissingMethodException">
        /// It is not possible to create an instance of the given type,
        /// because it is an interface, an abstract class or doesn't have parameterless constructor
        /// and no _type tag has been explicitly specified
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Path is an empty string (""), contains only white space, 
        /// or contains one or more invalid characters.
        /// -or- path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc.
        /// in an NTFS environment.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        /// non-NTFS environment.
        /// </exception>
        /// <exception cref="ArgumentNullException"> Path is null.</exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified path is invalid, such as being on an unmapped drive.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        public void Load(string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Open);
            Load(fs);
        }

        /// <summary>
        /// Saves a Items' content to a given stream
        /// </summary>
        /// <param name="stream">Stream to which an items' content should be saved</param>
        /// <exception cref="ArgumentNullException">Stream is null.</exception>
        /// <exception cref="StackOverflowException">
        /// More likely, your objects contains loop(s)
        /// -or-
        /// the property object type is derived from property's type, it contains loop(s)
        /// and no attribute [XmlIgnoreInheritance] has been specified to such a property(ies)
        /// </exception>
        public void Save(Stream stream)
        {
            XmlWriterSettings settings = new()
            {
                Indent = true
            };
            using XmlWriter writer = XmlWriter.Create(stream, settings);
            WriteElement(writer, Items, "Actors");
        }

        private static void WriteElement(XmlWriter writer, object? element, string name, TypeParams typeParams = default)
        {
            if (element == null)
                return;
            if (typeParams.Type == null)
                typeParams.Type = element.GetType();
            if (typeParams.Type.IsPrimitive || typeParams.Type.IsEnum || typeParams.Type == typeof(string))
                writer.WriteElementString(name.FirstToLower(), element.ToString());
            else if (element is IEnumerable<object> enumerable)
            {
                writer.WriteStartElement(name.FirstToLower());
                Type innerType;
                try
                {
                    innerType = enumerable.GetType().GetGenericArguments().First();
                }
                catch (InvalidOperationException)
                {
                    innerType = typeof(object);
                }
                foreach (var item in enumerable)
                {
                    var itemType = item.GetType();
                    WriteElement(writer, item, innerType.Name, new TypeParams()
                    { Type = itemType, WriteType = innerType != itemType });
                }
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteStartElement(name.FirstToLower());
                if (typeParams.WriteType)
                    writer.WriteElementString(typeName, element.GetType().ToString());
                var props = typeParams.Type.GetProperties();
                foreach (var prop in typeParams.Type.GetProperties())
                {
                    if (!prop.CustomAttributes.Any(a => a.AttributeType == typeof(XmlIgnoreAttribute)) 
                        && prop.GetValue(element) is not null)
                    {
                        var ignoreInheritance = prop.CustomAttributes
                            .Any(a => a.AttributeType == typeof(XmlIgnoreInheritanceAttribute));
                        var innerType = ignoreInheritance
                            ? prop.PropertyType : prop.GetValue(element)!.GetType();
                        var writeType = prop.PropertyType.IsInterface || !ignoreInheritance;
                        WriteElement(writer, prop.GetValue(element), prop.Name, 
                            new TypeParams() { Type = innerType, WriteType = writeType });
                    }
                }
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Saves a Items' content to a given stream
        /// </summary>
        /// <param name="fileName">File to which an items' content should be saved</param>
        /// <exception cref="StackOverflowException">
        /// More likely, your objects contains loop(s)
        /// -or-
        /// the property object type is derived from property's type, it contains loop(s)
        /// and no attribute [XmlIgnoreInheritance] has been specified to such a property(ies)
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Path is an empty string (""), contains only white space, 
        /// or contains one or more invalid characters.
        /// -or- path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc.
        /// in an NTFS environment.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Path refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a
        /// non-NTFS environment.
        /// </exception>
        /// <exception cref="ArgumentNullException"> Path is null.</exception>
        /// <exception cref="System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">Path specifies a file that is read-only.</exception>
        /// <exception cref="DirectoryNotFoundException">
        /// The specified path is invalid, such as being on an unmapped drive.
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        public void Save(string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Create);
            Save(fs);
        }
    }
}
