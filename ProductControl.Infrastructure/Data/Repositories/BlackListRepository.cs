using Microsoft.EntityFrameworkCore;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Infrastructure.Data.Context;

namespace ProductControl.Infrastructure.Data.Repositories
{
    public class BlackListRepository(PostgreDbContext _context) : IBlackListRepository
    {
        public async Task RevokeTokenAsync(RevokedToken token)
        {
            await _context.RevokedTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenRevokedAsync(string jti)
            => await _context.RevokedTokens.AnyAsync(x => x.Jti == jti);
    }
}
