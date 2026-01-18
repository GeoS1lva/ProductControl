using ProductControl.Domain.Entities;

namespace ProductControl.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        public Task AddAsync(Users user);
        public void UpdateAsync(Users user);
        public Task<Users?> GetByIdAsync(Guid id);
        public Task<Users?> GetByUserNameAsync(string userName);
        public Task<IList<Users>?> GetAllAsync();
        public Task<bool> ValidateRegisteredUserName(string userName);
    }
}
