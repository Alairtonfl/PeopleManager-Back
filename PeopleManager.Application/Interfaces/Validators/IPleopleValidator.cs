using PeopleManager.Application.DTOs.Requests;
using PeopleManager.Domain.Entities;

namespace PeopleManager.Application.Interfaces.Validators
{
    public interface IPleopleValidator
    {
        Task ValidateAsync(CreatePeopleRequestDto dto, CancellationToken cancellationToken);
        Task ValidateAsync(Person person, UpdatePeopleRequestDto newPerson, CancellationToken cancellationToken);
    }
}
