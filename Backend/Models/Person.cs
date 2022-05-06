using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Backend.Models
{
    public class Person
    {
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; } = string.Empty;

        public string? Patronymic { get; set; }

        [Required]
        public ushort BirthYear { get; set; }

        [XmlIgnore]
        public string FullName => $"{LastName} {FirstName} {Patronymic}".TrimEnd();

        public override bool Equals(object? obj)
        {
            if (obj is not Person other)
                return false;
            return FirstName == other.FirstName 
                && LastName == other.LastName 
                && Patronymic == other.Patronymic 
                && BirthYear == other.BirthYear;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, Patronymic, BirthYear);
        }
    }
}
