namespace Backend.Models
{
    public class TheatricalCharacter
    {
        public string Name { get; set; } = string.Empty;

        public override string ToString() => Name;

        public override bool Equals(object? obj)
        {
            if (obj is not TheatricalCharacter other)
                return false;
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
