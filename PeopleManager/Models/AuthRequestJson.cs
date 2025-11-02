using PeopleManager.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace PeopleManager.API.Models
{
    public class AuthRequestJson
    {
        [Required(ErrorMessageResourceType = typeof(BusinessExceptionMsg), ErrorMessageResourceName = "EXC0008")]
        [StringLength(255, MinimumLength = 8)]
        public string Cpf { get; set; }

        [Required(ErrorMessageResourceType = typeof(BusinessExceptionMsg), ErrorMessageResourceName = "EXC0008")]
        [StringLength(255, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
