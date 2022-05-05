namespace Backend.Extensions
{
    public static class StringExtensions
    {
        public static string FirstToLower(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException($"The {nameof(input)} string should not be null or empty", nameof(input));
            return string.Concat(input.First().ToString().ToLower(), input.AsSpan(1));
        }
    }
}
