using PeopleManager.Domain.Enums;
using PeopleManager.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace PeopleManager.API.Models
{
    public class CreatePeopleRequestJson
    {
        [Required(ErrorMessageResourceType = typeof(BusinessExceptionMsg), ErrorMessageResourceName = "EXC0008")]
        public string? Name { get; set; }

        public GenderType? Gender { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(BusinessExceptionMsg), ErrorMessageResourceName = "EXC0008")]
        public DateTime BirthDate { get; set; }

        public string? Naturality { get; set; }

        public string? Nationality { get; set; }

        [Required(ErrorMessageResourceType = typeof(BusinessExceptionMsg), ErrorMessageResourceName = "EXC0008")]
        public string? CPF { get; set; }

        [Required(ErrorMessageResourceType = typeof(BusinessExceptionMsg), ErrorMessageResourceName = "EXC0008")]
        public string? Password { get; set; }
    }
}
