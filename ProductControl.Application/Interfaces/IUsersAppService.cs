using ProductControl.Application.DTOs.UsersDTOs;
using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Application.Interfaces
{
    public interface IUsersAppService
    {
        public Task<ResultPattern<Users>> CreateUserAsync(CreateUser user);
        public Task<ResultPattern<IList<ReturnUserAndAddress>>> ReturnUsersAsync();
        public Task<ResultPattern<ReturnUserAndAddress>> ReturnUserAsync(Guid userId);
        public Task<ResultPattern> UpdateUserAsync(Guid userId, UpdateUser upUser);
        public Task<ResultPattern> DesactiveUser(Guid userId);
        public Task<ResultPattern> ActivateUser(Guid userId);
        public Task<ResultPattern<ResponseLogin>> LoginAsync(RequestLogin request);
    }
}
