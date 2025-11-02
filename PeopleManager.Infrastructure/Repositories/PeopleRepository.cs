using Microsoft.EntityFrameworkCore;
using PeopleManager.Application.Interfaces.Repositories;
using PeopleManager.Domain.Entities;
using PeopleManager.Infrastructure.Data.Context;

namespace PeopleManager.Infrastructure.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly AppDbContext _context;
        public PeopleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Person person, CancellationToken cancellationToken = default)
        {
            await _context.Person.AddAsync(person, cancellationToken);
        }

        public async Task DeleteAsync(Person person, CancellationToken cancellationToken = default)
        {
            _context.Person.Update(person);
        }

        public async Task<List<Person>?> GetAll(CancellationToken cancellationToken = default)
        {
            return await _context.Person.Where(p => p.DeletionDate == null).ToListAsync(cancellationToken);
        }

        public async Task<Person?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
        {
            return await _context.Person.FirstOrDefaultAsync(p => p.CPF == cpf && p.DeletionDate == null, cancellationToken);
        }

        public async Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Person.FirstOrDefaultAsync(p => p.Email == email && p.DeletionDate == null, cancellationToken);
        }

        public async Task<Person?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Person.FirstOrDefaultAsync(p => p.Id == id && p.DeletionDate == null, cancellationToken);
        }

        public async Task UpdateAsync(Person person, CancellationToken cancellationToken = default)
        {
            _context.Person.Update(person);
        }
    }
}
