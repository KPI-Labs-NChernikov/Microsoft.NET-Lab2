using System.Diagnostics.CodeAnalysis;

namespace Backend.Comparers
{
    public class IEnumarebleEqualityComparer<T> : IEqualityComparer<IEnumerable<T>> where T : class
    {
        public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return x.SequenceEqual(y);
        }

        public int GetHashCode([DisallowNull] IEnumerable<T> obj)
        {
            var hash = new HashCode();
            foreach (var item in obj)
                hash.Add(item);
            return hash.ToHashCode();
        }
    }
}
