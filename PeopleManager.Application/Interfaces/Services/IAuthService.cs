using PeopleManager.Application.DTOs.Responses;

namespace PeopleManager.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<PeopleResponseDto> AuthenticateAsync(string cpf, string password, CancellationToken cancellationToken);
        string GenerateJwtToken(PeopleResponseDto person);
    }
}
