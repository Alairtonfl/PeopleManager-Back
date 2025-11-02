using Moq;
using PeopleManager.Application.DTOs.Requests;
using PeopleManager.Application.Interfaces.Repositories;
using PeopleManager.Application.Interfaces.Security;
using PeopleManager.Application.Interfaces.UoW;
using PeopleManager.Application.Interfaces.Validators;
using PeopleManager.Application.Services;
using PeopleManager.Domain.Entities;
using PeopleManager.Domain.Enums;
using PeopleManager.Domain.Exceptions;
using Xunit;

namespace PeopleManager.Tests
{
    public class PeopleServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPeopleRepository> _peopleRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IPleopleValidator> _validatorMock;
        private readonly PeopleService _peopleService;

        public PeopleServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _peopleRepositoryMock = new Mock<IPeopleRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _validatorMock = new Mock<IPleopleValidator>();

            _peopleService = new PeopleService(
                _unitOfWorkMock.Object,
                _peopleRepositoryMock.Object,
                _passwordHasherMock.Object,
                _validatorMock.Object
            );
        }

        [Fact]
        public async Task CreateAsync_Should_CreatePerson()
        {
            // Arrange
            var request = new CreatePeopleRequestDto
            {
                Name = "Alairton",
                CPF = "123.456.789-00",
                Password = "12345678",
                BirthDate = DateTime.UtcNow,
                Gender = GenderType.Male
            };

            _passwordHasherMock.Setup(h => h.HashPassword(It.IsAny<string>())).Returns("hashed_password");

            // Act
            var result = await _peopleService.CreateAsync(request, CancellationToken.None);

            // Assert
            _peopleRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal("Alairton", result.Name);
            Assert.Equal("hashed_password", _peopleRepositoryMock.Invocations[0].Arguments[0] is Person p ? p.Password : null);
        }

        [Fact]
        public async Task CreateV2Async_Should_Throw_When_Address_Null()
        {
            // Arrange
            var request = new CreatePeopleRequestDto
            {
                Name = "Alairton",
                CPF = "123.456.789-00",
                Password = "12345678",
                BirthDate = DateTime.UtcNow,
                Gender = GenderType.Male,
                Address = null
            };

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(() =>
                _peopleService.CreateV2Async(request, CancellationToken.None)
            );
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnPerson_WhenExists()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "Alairton" };
            _peopleRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(person);

            // Act
            var result = await _peopleService.GetByIdAsync(1, CancellationToken.None);

            // Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("Alairton", result.Name);
        }

        [Fact]
        public async Task UpdateByIdAsync_Should_UpdatePerson()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "OldName", CPF = "123" };
            _peopleRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(person);

            var request = new UpdatePeopleRequestDto
            {
                Name = "NewName"
            };

            // Act
            var result = await _peopleService.UpdateByIdAsync(1, request, CancellationToken.None);

            // Assert
            Assert.Equal("NewName", result.Name);
            _peopleRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_Should_Call_Delete_WhenPersonExists()
        {
            // Arrange
            var person = new Person { Id = 1 };
            _peopleRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(person);

            // Act
            await _peopleService.DeleteByIdAsync(1, CancellationToken.None);

            // Assert
            _peopleRepositoryMock.Verify(r => r.DeleteAsync(person, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_Should_ReturnAllPersons()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person { Id = 1, Name = "Alairton" },
                new Person { Id = 2, Name = "Maria" }
            };
            _peopleRepositoryMock.Setup(r => r.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(persons);

            // Act
            var result = await _peopleService.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Name == "Alairton");
            Assert.Contains(result, r => r.Name == "Maria");
        }

        [Fact]
        public async Task GetByCpfAsync_Should_ReturnPerson_WhenExists()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "Alairton", CPF = "12345678900" };
            _peopleRepositoryMock.Setup(r => r.GetByCpfAsync("12345678900", It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            // Act
            var result = await _peopleService.GetByCpfAsync("123.456.789-00", CancellationToken.None);

            // Assert
            Assert.Equal(person.Id, result.Id);
            Assert.Equal(person.Name, result.Name);
        }

        [Fact]
        public async Task GetByCpfAsync_Should_Throw_WhenNotFound()
        {
            // Arrange
            _peopleRepositoryMock.Setup(r => r.GetByCpfAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(() =>
                _peopleService.GetByCpfAsync("000.000.000-00", CancellationToken.None)
            );
        }

        [Fact]
        public async Task CreateAsync_Should_Rollback_OnException()
        {
            // Arrange
            var request = new CreatePeopleRequestDto
            {
                Name = "Alairton",
                CPF = "123.456.789-00",
                Password = "12345678",
                BirthDate = DateTime.UtcNow,
                Gender = GenderType.Male
            };

            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Validation failed"));

            // Act
            await Assert.ThrowsAsync<Exception>(() => _peopleService.CreateAsync(request, CancellationToken.None));

            // Assert rollback called
            _unitOfWorkMock.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateByIdAsync_Should_Rollback_OnException()
        {
            // Arrange
            var person = new Person { Id = 1, Name = "OldName" };
            _peopleRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(person);

            _validatorMock.Setup(v => v.ValidateAsync(person, It.IsAny<UpdatePeopleRequestDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Validation failed"));

            var request = new UpdatePeopleRequestDto { Name = "NewName" };

            // Act
            await Assert.ThrowsAsync<Exception>(() =>
                _peopleService.UpdateByIdAsync(1, request, CancellationToken.None));

            // Assert rollback called
            _unitOfWorkMock.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteByIdAsync_Should_Rollback_WhenPersonNotFound()
        {
            // Arrange
            _peopleRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Person?)null);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(() =>
                _peopleService.DeleteByIdAsync(1, CancellationToken.None));

            _unitOfWorkMock.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }


    }
}
