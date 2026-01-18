using ProductControl.Application.DTOs.UsersDTOs;
using ProductControl.Application.Interfaces;
using ProductControl.Application.Interfaces.Queries;
using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Domain.Interfaces.Services;

namespace ProductControl.Application.Services
{
    public class UsersAppService(IPasswordService passwordService, ICepService cepService, IUsersService usersService, IUnitOfWork unitOfWork, IUsersQueries usersQueries, ITokenService tokenService) : IUsersAppService
    {
        public async Task<ResultPattern<Users>> CreateUserAsync(CreateUser user)
        {
            var passwordResult = passwordService.GeneratePassword(user.Password);

            if (passwordResult.Error)
                return ResultPattern<Users>.Failure(passwordResult.ErrorMessage);

            var addressResult = await cepService.GetAddressByCepAsync(user.Cep);

            if (addressResult is null)
                return ResultPattern<Users>.Failure("CEP Inválido!");

            var address = Address.Create(
                addressResult.cep, 
                addressResult.Street,
                addressResult.Neighborhood, 
                addressResult.City,
                addressResult.State,
                user.ResidenceNumber,
                user.Complement
            );

            if (address.Error)
                return ResultPattern<Users>.Failure(address.ErrorMessage);

            var newUser = await usersService.CreateAsync(
                user.FullName,
                user.UserName,
                passwordResult.Value,
                user.Role,
                address.Value
            );

            if (newUser.Error)
                return ResultPattern<Users>.Failure(newUser.ErrorMessage);

            await unitOfWork.UserRepository.AddAsync(newUser.Value);
            await unitOfWork.SaveChangesAsync();

            return ResultPattern<Users>.Success(newUser.Value);
        }

        public async Task<ResultPattern<IList<ReturnUserAndAddress>>> ReturnUsersAsync()
        {
            var resultUsers = await usersQueries.GetAllUsers();

            if (resultUsers is null)
                return ResultPattern<IList<ReturnUserAndAddress>>.Failure("Nenhum usuário ativo encontrado!");

            return ResultPattern<IList<ReturnUserAndAddress>>.Success(resultUsers);
        }

        public async Task<ResultPattern<ReturnUserAndAddress>> ReturnUserAsync(Guid userId)
        {
            var resultUser = await usersQueries.GetByIdUser(userId);

            if (resultUser is null)
                return ResultPattern<ReturnUserAndAddress>.Failure("Usuário não encontrado!");

            return ResultPattern<ReturnUserAndAddress>.Success(resultUser);
        }

        public async Task<ResultPattern> UpdateUserAsync(Guid userId, UpdateUser upUser)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user is null)
                return ResultPattern.Failure("Usuário não encontrado!");

            var resultProfile = await usersService.UpdateProfileAsync(user, upUser.FullName, upUser.UserName);

            if (resultProfile.Error)
                return ResultPattern.Failure(resultProfile.ErrorMessage);

            if (user.Address.Cep != upUser.Cep)
            {
                var addressResult = await cepService.GetAddressByCepAsync(upUser.Cep);

                if (addressResult is null)
                    return ResultPattern.Failure("CEP Inválido!");

                var address = Address.Create(
                    addressResult.cep,
                    addressResult.Street,
                    addressResult.Neighborhood,
                    addressResult.City,
                    addressResult.State,
                    upUser.ResidenceNumber,
                    upUser.Complement
                );

                if (address.Error)
                    return ResultPattern.Failure(address.ErrorMessage);

                user.UpdateAddress(address.Value);
            }

            unitOfWork.UserRepository.UpdateAsync(user);
            await unitOfWork.SaveChangesAsync();

            return ResultPattern.Success();
        }

        public async Task<ResultPattern> DesactiveUser(Guid userId)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user is null)
                return ResultPattern.Failure("Usuário não encontrado!");

            var result = user.DeactivateUser();

            if (result.Error)
                return ResultPattern.Failure(result.ErrorMessage);

            await unitOfWork.SaveChangesAsync();

            return ResultPattern.Success();
        }

        public async Task<ResultPattern> ActivateUser(Guid userId)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user is null)
                return ResultPattern.Failure("Usuário não encontrado!");

            var result = user.ActivateUser();

            if (result.Error)
                return ResultPattern.Failure(result.ErrorMessage);

            await unitOfWork.SaveChangesAsync();

            return ResultPattern.Success();
        }

        public async Task<ResultPattern<ResponseLogin>> LoginAsync(RequestLogin request)
        {
            var user = await unitOfWork.UserRepository.GetByUserNameAsync(request.UserName);

            if (user is null)
                return ResultPattern<ResponseLogin>.Failure("Usuário inválido.");

            if (user.Status is false)
                return ResultPattern<ResponseLogin>.Failure("Usuário bloqueado/inativo.");

            var passwordValid = usersService.ValidatePasswordAsync(user, request.Password);

            if (passwordValid.Error)
                return ResultPattern<ResponseLogin>.Failure(passwordValid.ErrorMessage);

            var token = tokenService.TokeGenerator(user, user.Role.ToString());

            return ResultPattern<ResponseLogin>.Success(new ResponseLogin
            {
                Token = token,
                UserName = user.UserName
            });
        }
    }
}
