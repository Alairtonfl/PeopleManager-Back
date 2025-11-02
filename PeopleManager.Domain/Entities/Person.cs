using PeopleManager.Domain.Enums;

namespace PeopleManager.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }

        public GenderType? Gender { get; set; }

        public string? Email { get; set; }

        public string Password { get; set; }

        public DateTime BirthDate { get; set; }

        public string? Naturality { get; set; }

        public string? Nationality { get; set; }

        public string? CPF { get; set; }
        public string? Address { get; set; }
    }
}
