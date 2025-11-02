using PeopleManager.Application.DTOs.Requests;
using PeopleManager.Application.DTOs.Responses;
using PeopleManager.Application.Interfaces.Repositories;
using PeopleManager.Application.Interfaces.Security;
using PeopleManager.Application.Interfaces.Services;
using PeopleManager.Application.Interfaces.UoW;
using PeopleManager.Application.Interfaces.Validators;
using PeopleManager.Application.Utilities;
using PeopleManager.Domain.Entities;
using PeopleManager.Domain.Exceptions;
using PeopleManager.Domain.Resources;

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
                    CPF = Utilitie.RemoveCpfMask(request.CPF!),
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
                    Id = person.Id,
                    BirthDate = person.BirthDate,
                    CPF = person.CPF!,
                    Email = person.Email,
                    Name = person.Name!,
                    Nationality = person.Nationality,
                    Naturality = person.Naturality,
                    Gender = person.Gender,
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

        public async Task DeleteByIdAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                Person? person = await _peopleRepository.GetByIdAsync(id, cancellationToken);
                if (person == null)
                    throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0005));

                person.DeletionDate = DateTime.UtcNow;

                await _peopleRepository.DeleteAsync(person, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw ex;
            }
        }

        public async Task<List<PeopleResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                List<Person>? persons = await _peopleRepository.GetAll(cancellationToken);

                List<PeopleResponseDto> peopleResponseDtos = persons!.Select(person => new PeopleResponseDto
                {
                    Id = person.Id,
                    Name = person.Name,
                    CPF = person.CPF!,
                    Email = person.Email,
                    BirthDate = person.BirthDate,
                    Gender = person.Gender,
                    Nationality = person.Nationality,
                    Naturality = person.Naturality
                }).ToList();

                return peopleResponseDtos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PeopleResponseDto> GetByCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            try
            {
                Person? person = await _peopleRepository.GetByCpfAsync(cpf, cancellationToken);
                if (person == null)
                    throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0005));

                PeopleResponseDto peopleResponseDto = new PeopleResponseDto
                {
                    BirthDate = person.BirthDate,
                    CPF = person.CPF!,
                    Email = person.Email,
                    Name = person.Name,
                    Nationality = person.Nationality,
                    Naturality = person.Naturality,
                    Gender = person.Gender
                };
                return peopleResponseDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PeopleResponseDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                Person? person = await _peopleRepository.GetByIdAsync(id, cancellationToken);
                if (person == null)
                    throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0005));

                PeopleResponseDto peopleResponseDto = new PeopleResponseDto
                {
                    BirthDate = person.BirthDate,
                    CPF = person.CPF!,
                    Email = person.Email,
                    Name = person.Name,
                    Nationality = person.Nationality,
                    Naturality = person.Naturality,
                    Gender = person.Gender,
                    Id = person.Id
                };

                return peopleResponseDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PeopleResponseDto> UpdateByIdAsync(long id, UpdatePeopleRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                Person? person = await _peopleRepository.GetByIdAsync(id, cancellationToken);
                if (person == null)
                    throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0005));

                person.BirthDate = request.BirthDate == null ? person.BirthDate : request.BirthDate.Value;
                person.CPF = request.CPF == null ? person.CPF : Utilitie.RemoveCpfMask(request.CPF!);
                person.Email = request.Email == null ? person.Email : request.Email!;
                person.Name = request.Name == null ? person.Name : request.Name!;
                person.Nationality = request.Nationality == null ? person.Nationality : request.Nationality;
                person.Naturality = request.Naturality == null ? person.Naturality : request.Naturality;
                person.Gender = request.Gender == null ? person.Gender : request.Gender;
                person.UpdateDate = DateTime.UtcNow;

                await _pleopleValidator.ValidateAsync(person, request, cancellationToken);

                await _peopleRepository.UpdateAsync(person, cancellationToken);

                PeopleResponseDto peopleResponseDto = new PeopleResponseDto
                {
                    BirthDate = person.BirthDate,
                    CPF = person.CPF!,
                    Email = person.Email,
                    Name = person.Name,
                    Nationality = person.Nationality,
                    Gender = person.Gender,
                    Naturality = person.Naturality
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
