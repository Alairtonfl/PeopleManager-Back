using PeopleManager.Application.DTOs.Requests;
using PeopleManager.Application.DTOs.Responses;

namespace PeopleManager.Application.Interfaces.Services
{
    public interface IPeopleService
    {
        Task<PeopleResponseDto> CreateAsync(CreatePeopleRequestDto request, CancellationToken cancellationToken);
    }
}
