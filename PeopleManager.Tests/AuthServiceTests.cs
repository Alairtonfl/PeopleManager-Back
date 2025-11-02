using Microsoft.Extensions.Configuration;
using Moq;
using PeopleManager.Application.DTOs.Responses;
using PeopleManager.Application.Interfaces.Repositories;
using PeopleManager.Application.Interfaces.Security;
using PeopleManager.Application.Services;
using PeopleManager.Domain.Entities;
using PeopleManager.Domain.Enums;
using PeopleManager.Domain.Exceptions;
using Xunit;

namespace PeopleManager.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IPeopleRepository> _peopleRepositoryMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _peopleRepositoryMock = new Mock<IPeopleRepository>();

            var jwtSectionMock = new Mock<IConfigurationSection>();
            jwtSectionMock.Setup(s => s["Key"]).Returns("12345678901234567890123456789012");
            jwtSectionMock.Setup(s => s["Issuer"]).Returns("TestIssuer");
            jwtSectionMock.Setup(s => s["Audience"]).Returns("TestAudience");

            _configurationMock.Setup(c => c.GetSection("Jwt")).Returns(jwtSectionMock.Object);


            _authService = new AuthService(
                _configurationMock.Object,
                _passwordHasherMock.Object,
                _peopleRepositoryMock.Object
            );
        }

        [Fact]
        public async Task AuthenticateAsync_Should_ReturnPerson_When_CredentialsValid()
        {
            // Arrange
            var person = new Person
            {
                Id = 1,
                Name = "Alairton",
                CPF = "12345678900",
                Password = "hashed_password",
                Gender = GenderType.Male
            };

            _peopleRepositoryMock
                .Setup(r => r.GetByCpfAsync("12345678900", It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            _passwordHasherMock
                .Setup(h => h.VerifyPassword("12345678", "hashed_password"))
                .Returns(true);

            // Act
            var result = await _authService.AuthenticateAsync("123.456.789-00", "12345678", CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(person.Id, result.Id);
            Assert.Equal(person.Name, result.Name);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Throw_When_PersonNotFound()
        {
            // Arrange
            _peopleRepositoryMock
                .Setup(r => r.GetByCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person?)null);

            // Act & Assert
            await Assert.ThrowsAsync<AuthenticationException>(() =>
                _authService.AuthenticateAsync("000.000.000-00", "password", CancellationToken.None));
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Throw_When_PasswordInvalid()
        {
            // Arrange
            var person = new Person
            {
                Id = 1,
                Name = "Alairton",
                CPF = "12345678900",
                Password = "hashed_password"
            };

            _peopleRepositoryMock
                .Setup(r => r.GetByCpfAsync("12345678900", It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            _passwordHasherMock
                .Setup(h => h.VerifyPassword("wrongpassword", "hashed_password"))
                .Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<AuthenticationException>(() =>
                _authService.AuthenticateAsync("123.456.789-00", "wrongpassword", CancellationToken.None));
        }

        [Fact]
        public void GenerateJwtToken_Should_Return_TokenString()
        {
            // Arrange
            var personDto = new PeopleResponseDto
            {
                Id = 1,
                Name = "Alairton"
            };

            // Act
            var token = _authService.GenerateJwtToken(personDto);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));
            Assert.Contains(".", token);
        }

    }
}
