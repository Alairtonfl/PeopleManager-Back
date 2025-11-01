using PeopleManager.Application.DTOs.Requests;
using PeopleManager.Application.DTOs.Responses;
using PeopleManager.Application.Interfaces.Repositories;
using PeopleManager.Application.Interfaces.Security;
using PeopleManager.Application.Interfaces.Services;
using PeopleManager.Application.Interfaces.UoW;
using PeopleManager.Application.Interfaces.Validators;
using PeopleManager.Domain.Entities;

namespace PeopleManager.Application.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPeopleRepository _peopleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPleopleValidator _pleopleValidator;

        public PeopleService(IUnitOfWork unitOfWork, IPeopleRepository peopleRepository, IPasswordHasher passwordHasher, IPleopleValidator pleopleValidator)
        {
            _unitOfWork = unitOfWork;
            _peopleRepository = peopleRepository;
            _passwordHasher = passwordHasher;
            _pleopleValidator = pleopleValidator;
        }

        public async Task<PeopleResponseDto> CreateAsync(CreatePeopleRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                await _pleopleValidator.ValidateAsync(request, cancellationToken);

                Person person = new Person
                {
                    Name = request.Name,
                    CPF = request.CPF,
                    Email = request.Email,
                    BirthDate = request.BirthDate,
                    Gender = request.Gender,
                    Nationality = request.Nationality,
                    Naturality = request.Naturality,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    DeletionDate = null,
                    Password = _passwordHasher.HashPassword(request.Password)
                };

                await _peopleRepository.CreateAsync(person, cancellationToken);

                PeopleResponseDto peopleResponseDto = new PeopleResponseDto
                {
                    Name = person.Name,
                    CPF = person.CPF,
                    Email = person.Email,
                    BirthDate = person.BirthDate
                };

                await _unitOfWork.CommitAsync(cancellationToken);
                return peopleResponseDto;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw ex;
            }
        }
    }
}
