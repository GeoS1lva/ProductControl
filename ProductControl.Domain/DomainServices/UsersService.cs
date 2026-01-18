using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Enums;
using ProductControl.Domain.Interfaces;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Domain.Interfaces.Services;
using ProductControl.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.DomainServices
{
    public class UsersService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IUsersService
    {
        public async Task<ResultPattern<Users>> CreateAsync(string fullName, string userName, Password password, Roles role, Address address)
        {
            var resultValidate = await unitOfWork.UserRepository.ValidateRegisteredUserName(userName);

            if (resultValidate is true)
                return ResultPattern<Users>.Failure("Esse Username Já está Cadastrado.");

            var resultCreateUser = Users.Create(fullName, userName, password, role, address);

            if (resultCreateUser.Error)
                return ResultPattern<Users>.Failure(resultCreateUser.ErrorMessage);

            return ResultPattern<Users>.Success(resultCreateUser.Value);
        }

        public async Task<ResultPattern> UpdateProfileAsync(Users user, string fullName, string userName)
        {
            if (user.UserName != userName)
            {
                var resultValidate = await unitOfWork.UserRepository.ValidateRegisteredUserName(userName);

                if (resultValidate is true)
                    return ResultPattern.Failure("Esse Username Já está Cadastrado.");
            }

            user.UpdateProfile(fullName, userName);
            return ResultPattern.Success();
        }

        public ResultPattern ValidatePasswordAsync(Users user, string password)
        {
            var resultValidatePassword = passwordHasher.ValidatePasswordInternal(password, user.Password.HashPassword, user.Password.SaltPassword);

            if (resultValidatePassword is false)
                return ResultPattern.Failure("Senha incorreta.");

            return ResultPattern.Success();
        }
    }
}
