using PeopleManager.Domain.Enums;

namespace PeopleManager.Application.DTOs.Responses
{
    public class PeopleResponseDto
    {
        public long? Id { get; set; }
        public string Name { get; set; }

        public GenderType? Gender { get; set; }

        public string? Email { get; set; }

        public DateTime BirthDate { get; set; }

        public string? Naturality { get; set; }

        public string? Nationality { get; set; }

        public string CPF { get; set; }
    }
}
