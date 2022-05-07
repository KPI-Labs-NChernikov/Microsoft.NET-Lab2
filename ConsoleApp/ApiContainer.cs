using Backend;
using Business;
using Business.Interfaces;

namespace ConsoleApp
{
    internal static class ApiContainer
    {
        internal static IApi Api { get; set; } = new Api(new XmlContext());
    }
}
