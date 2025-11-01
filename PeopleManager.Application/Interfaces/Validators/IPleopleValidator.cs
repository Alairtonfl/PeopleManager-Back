using PeopleManager.Application.DTOs.Requests;

namespace PeopleManager.Application.Interfaces.Validators
{
    public interface IPleopleValidator
    {
        Task ValidateAsync(CreatePeopleRequestDto dto, CancellationToken cancellationToken);
    }
}
