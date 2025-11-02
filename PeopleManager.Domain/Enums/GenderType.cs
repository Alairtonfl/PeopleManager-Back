using System.ComponentModel;

namespace PeopleManager.Domain.Enums
{
    public enum GenderType
    {
        [Description("Feminino")]
        Female = 0,
        [Description("Masculino")]
        Male = 1,
        [Description("Outro")]
        Other = 2
    }
}
