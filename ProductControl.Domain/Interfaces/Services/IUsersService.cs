using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Enums;
using ProductControl.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        public Task<ResultPattern<Users>> CreateAsync(string fullName, string userName, Password password, Roles role, Address address);
        public Task<ResultPattern> UpdateProfileAsync(Users user, string fullName, string userName);
        public ResultPattern ValidatePasswordAsync(Users user, string password);
    }
}
