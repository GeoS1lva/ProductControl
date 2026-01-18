using ProductControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Interfaces.Repositories
{
    public interface IBlackListRepository
    {
        public Task RevokeTokenAsync(RevokedToken token);
        public Task<bool> IsTokenRevokedAsync(string jti);
    }
}
