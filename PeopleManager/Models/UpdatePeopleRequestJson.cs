using PeopleManager.Domain.Enums;

namespace PeopleManager.API.Models
{
    public class UpdatePeopleRequestJson
    {
        private string? _name;
        public string? Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private GenderType? _gender;
        public GenderType? Gender
        {
            get => _gender;
            set => _gender = value;
        }

        private string? _email;
        public string? Email
        {
            get => _email;
            set => _email = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private DateTime? _birthDate;
        public DateTime? BirthDate
        {
            get => _birthDate;
            set => _birthDate = value;
        }

        private string? _naturality;
        public string? Naturality
        {
            get => _naturality;
            set => _naturality = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private string? _nationality;
        public string? Nationality
        {
            get => _nationality;
            set => _nationality = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private string? _cpf;
        public string? CPF
        {
            get => _cpf;
            set => _cpf = string.IsNullOrWhiteSpace(value) ? null : value;
        }
    }
}
