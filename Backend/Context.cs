using Backend.Models;
using System.Xml;
using System.Xml.Linq;
using Backend.Extensions;
using System.Xml.Serialization;
using System.Reflection;
using Backend.Interfaces;

namespace Backend
{
    public class Context : IContext<Actor>
    {
        public ICollection<Actor> Items { get; set; } = new List<Actor>();

        public XDocument Document 
        { 
            get 
            {
                using var stream = new MemoryStream();
                Save(stream);
                stream.Position = 0;
                return XDocument.Load(stream);
            } 
        }

        public void Load(Stream stream)
        {
            XmlDocument doc = new();
            doc.Load(stream);
            Items = Read<List<Actor>>(doc.DocumentElement, nameof(Items), true) ?? new List<Actor>();
        }

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
                    var method = typeof(Context).GetMethod(nameof(Context.Read), 
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
                object obj = (T)Activator.CreateInstance(type)!;
                foreach (var prop in type.GetProperties())
                {
                    if (!prop.CustomAttributes.Any(a => a.AttributeType == typeof(XmlIgnoreAttribute)))
                    {
                        var method = typeof(Context).GetMethod(nameof(Context.Read), BindingFlags.NonPublic | BindingFlags.Instance);
                        var generic = method!.MakeGenericMethod(prop.PropertyType);
                        object?[] parameters = new object?[] { node[prop.Name.FirstToLower()], prop.PropertyType.ToString(), true };
                        var itemConverted = generic.Invoke(this, parameters);
                        if (itemConverted != default)
                            prop.SetValue(obj, itemConverted, null);
                    }
                }
                return (T?)obj;
            }
        }

        public void Load(string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Open);
            Load(fs);
        }

        public void Save(Stream stream)
        {
            XmlWriterSettings settings = new()
            {
                Indent = true
            };
            using XmlWriter writer = XmlWriter.Create(stream, settings);
            WriteElement(writer, Items, nameof(Items));
        }

        private static void WriteElement(XmlWriter writer, object? element, string name)
        {
            if (element == null)
                return;
            if (element.GetType().IsPrimitive || element.GetType().IsEnum || element.GetType() == typeof(string))
                writer.WriteElementString(name.FirstToLower(), element.ToString());
            else if (element is IEnumerable<object> enumerable)
            {
                if (enumerable.Any())
                {
                    writer.WriteStartElement(name.FirstToLower());
                    foreach (var item in enumerable)
                        WriteElement(writer, item, item.GetType().Name);
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteStartElement(name.FirstToLower());
                foreach (var prop in element.GetType().GetProperties())
                {
                    if (!prop.CustomAttributes.Any(a => a.AttributeType == typeof(XmlIgnoreAttribute)))
                        WriteElement(writer, prop.GetValue(element), prop.Name);
                }
                writer.WriteEndElement();
            }
        }

        public void Save(string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Create);
            Save(fs);
        }
    }
}
