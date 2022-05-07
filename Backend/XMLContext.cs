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
                if (type.IsInterface || type.IsAbstract)
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
                if (enumerable.Any())
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

        public void Save(string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Create);
            Save(fs);
        }
    }
}
