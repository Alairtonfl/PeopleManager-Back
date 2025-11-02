using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PeopleManager.Application.DTOs.Responses;
using PeopleManager.Application.Interfaces.Repositories;
using PeopleManager.Application.Interfaces.Security;
using PeopleManager.Application.Interfaces.Services;
using PeopleManager.Application.Utilities;
using PeopleManager.Domain.Entities;
using PeopleManager.Domain.Exceptions;
using PeopleManager.Domain.Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PeopleManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPeopleRepository _peopleRepository;
        public AuthService(IConfiguration configuration, IPasswordHasher passwordHasher, IPeopleRepository peopleRepository)
        {
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _peopleRepository = peopleRepository;
        }
        public async Task<PeopleResponseDto> AuthenticateAsync(string cpf, string password, CancellationToken cancellationToken)
        {
            try
            {
                Person? person = await _peopleRepository.GetByCpfAsync(Utilitie.RemoveCpfMask(cpf), cancellationToken);
                if (person == null || !_passwordHasher.VerifyPassword(password, person.Password))
                    throw new AuthenticationException(BusinessExceptionMsg.EXC0003);

                PeopleResponseDto peopleResponseDto = new PeopleResponseDto
                {
                    Id = person.Id,
                    BirthDate = person.BirthDate,
                    CPF = person.CPF!,
                    Email = person.Email,
                    Name = person.Name,
                    Nationality = person.Nationality,
                    Gender = person.Gender,
                    Naturality = person.Naturality
                };
                return peopleResponseDto;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateJwtToken(PeopleResponseDto person)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("Jwt");
            byte[] key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            Claim[] claims = new[]
            {
                    new Claim("UserId", person.Id.ToString()),
                    new Claim(ClaimTypes.Name, person.Name),
            };

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
