using PeopleManager.Domain.Entities;

namespace PeopleManager.Application.Interfaces.Repositories
{
    public interface IPeopleRepository
    {
        Task CreateAsync(Person person, CancellationToken cancellationToken = default);
        Task<Person?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Person?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);
        Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<List<Person>?> GetAll(CancellationToken cancellationToken = default);
        Task UpdateAsync(Person person, CancellationToken cancellationToken = default);
        Task DeleteAsync(Person person, CancellationToken cancellationToken = default);
    }
}
