using PeopleManager.Application.DTOs.Requests;
using PeopleManager.Application.Interfaces.Repositories;
using PeopleManager.Application.Interfaces.Validators;
using PeopleManager.Application.Utilities;
using PeopleManager.Domain.Entities;
using PeopleManager.Domain.Exceptions;
using PeopleManager.Domain.Resources;

namespace PeopleManager.Application.Validators
{
    public class PleopleValidator : IPleopleValidator
    {
        private readonly IPeopleRepository _peopleRepository;
        public PleopleValidator(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }
        public async Task ValidateAsync(CreatePeopleRequestDto dto, CancellationToken cancellationToken)
        {
            if(dto.BirthDate > DateTime.UtcNow)
                throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0009, nameof(dto.BirthDate)));

            if (!Utilitie.IsCpfValid(dto.CPF))
                throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0009, nameof(dto.CPF)));

            if (dto.Email != null && !Utilitie.EmailIsValid(dto.Email))
                throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0009, nameof(dto.Email)));

            if (!Utilitie.PasswordIsValid(dto.Password))
                throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0009, nameof(dto.Password)));

            Person? person = await _peopleRepository.GetByCpfAsync(Utilitie.RemoveCpfMask(dto.CPF!), cancellationToken);
            if (person != null)
                throw new BusinessException(BusinessExceptionMsg.EXC0007);

            if(dto.Email != null)
            {
                person = await _peopleRepository.GetByEmailAsync(dto.Email, cancellationToken);
                if (person != null)
                    throw new BusinessException(BusinessExceptionMsg.EXC0002);
            }
        }

        public async Task ValidateAsync(Person person, UpdatePeopleRequestDto newPerson, CancellationToken cancellationToken)
        {
            if (newPerson.BirthDate > DateTime.UtcNow)
                throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0009, nameof(newPerson.BirthDate)));

            if (person.CPF != newPerson.CPF)
            {
                if (!Utilitie.IsCpfValid(newPerson.CPF))
                    throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0009, nameof(newPerson.CPF)));

                Person? existPerson = await _peopleRepository.GetByCpfAsync(Utilitie.RemoveCpfMask(newPerson.CPF!), cancellationToken);
                if (existPerson != null)
                    throw new BusinessException(BusinessExceptionMsg.EXC0007);
            }

            if (newPerson.Email != null && !Utilitie.EmailIsValid(newPerson.Email))
                throw new BusinessException(string.Format(BusinessExceptionMsg.EXC0009, nameof(newPerson.Email)));

            if(person.Email != newPerson.Email)
            {
                Person existPerson = await _peopleRepository.GetByEmailAsync(newPerson.Email, cancellationToken);
                if (existPerson != null)
                    throw new BusinessException(BusinessExceptionMsg.EXC0002);
            }
        }
    }
}
