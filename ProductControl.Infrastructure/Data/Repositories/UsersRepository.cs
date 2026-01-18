using Microsoft.EntityFrameworkCore;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Infrastructure.Data.Context;

namespace ProductControl.Infrastructure.Data.Repositories
{
    public class UsersRepository(PostgreDbContext _context) : IUsersRepository
    {
        public async Task AddAsync(Users user)
            => await _context.Users.AddAsync(user);

        public void UpdateAsync(Users user)
            => _context.Users.Update(user);

        public async Task<Users?> GetByIdAsync(Guid id)
            => await _context.Users
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Users?> GetByUserNameAsync(string userName)
            => await _context.Users
            .Include(x => x.Password)
            .FirstOrDefaultAsync(x => x.UserName == userName);

        public async Task<IList<Users>?> GetAllAsync()
            => await _context.Users.ToListAsync();

        public async Task<bool> ValidateRegisteredUserName(string userName)
            => await _context.Users.AnyAsync(u => u.UserName == userName);
    }
}
