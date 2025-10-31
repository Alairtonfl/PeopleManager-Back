using System.ComponentModel.DataAnnotations;

namespace PeopleManager.API.Models
{
    public class CreatePeopleRequestJson
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string? Gender { get; set; }

        [EmailAddress(ErrorMessage = "The provided email is not valid.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid birth date format.")]
        public DateTime BirthDate { get; set; }

        public string? Naturality { get; set; }

        public string? Nationality { get; set; }

        public string CPF { get; set; }
    }
}
