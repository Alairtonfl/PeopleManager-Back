using PeopleManager.Domain.Enums;

namespace PeopleManager.Application.DTOs.Requests
{
    public class UpdatePeopleRequestDto
    {
        public string? Name { get; set; }

        public GenderType? Gender { get; set; }

        public string? Email { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Naturality { get; set; }

        public string? Nationality { get; set; }

        public string? CPF { get; set; }
    }
}
