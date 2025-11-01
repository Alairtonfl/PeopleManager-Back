namespace PeopleManager.Application.DTOs.Responses
{
    public class PeopleResponseDto
    {
        public string Name { get; set; }

        public string? Gender { get; set; }

        public string? Email { get; set; }

        public DateTime BirthDate { get; set; }

        public string? Naturality { get; set; }

        public string? Nationality { get; set; }

        public string CPF { get; set; }
    }
}
