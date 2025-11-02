using PeopleManager.Application.DTOs.Requests;
using PeopleManager.Application.DTOs.Responses;

namespace PeopleManager.Application.Interfaces.Services
{
    public interface IPeopleService
    {
        Task<PeopleResponseDto> CreateAsync(CreatePeopleRequestDto request, CancellationToken cancellationToken);
        Task<PeopleResponseDto> CreateV2Async(CreatePeopleRequestDto request, CancellationToken cancellationToken);
        Task<List<PeopleResponseDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<PeopleResponseDto> GetByCpfAsync(string cpf, CancellationToken cancellationToken);
        Task<PeopleResponseDto> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task DeleteByIdAsync(long id, CancellationToken cancellationToken);
        Task<PeopleResponseDto> UpdateByIdAsync(long id, UpdatePeopleRequestDto request, CancellationToken cancellationToken);
    }
}
