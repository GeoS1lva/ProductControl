using Microsoft.EntityFrameworkCore;
using ProductControl.Application.DTOs.UsersDTOs;
using ProductControl.Application.Interfaces.Queries;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Infrastructure.Data.Queries
{
    public class UsersQueries(PostgreDbContext postgreDbContext) : IUsersQueries
    {
        public async Task<IList<ReturnUserAndAddress>?> GetAllUsers()
            => await postgreDbContext.Users.AsNoTracking()
            .Where(u => u.Status == true)
            .Select(user => new ReturnUserAndAddress
            {
                UserId = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Role = user.Role,
                CreateAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Address = new ReturnAddress
                {
                    AddressId = user.Address.Id,
                    Cep = user.Address.Cep,
                    Street = user.Address.Street,
                    Neighborhood = user.Address.Neighborhood,
                    City = user.Address.City,
                    State = user.Address.State,
                    Number = user.Address.Number,
                    Complement = user.Address.Complement
                }
            }).ToListAsync();

        public async Task<ReturnUserAndAddress?> GetByIdUser(Guid id)
            => await postgreDbContext.Users.AsNoTracking()
            .Where(u => u.Id == id)
            .Select(user => new ReturnUserAndAddress
            {
                UserId = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Role = user.Role,
                CreateAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Address = new ReturnAddress
                {
                    AddressId = user.Address.Id,
                    Cep = user.Address.Cep,
                    Street = user.Address.Street,
                    Neighborhood = user.Address.Neighborhood,
                    City = user.Address.City,
                    State = user.Address.State,
                    Number = user.Address.Number,
                    Complement = user.Address.Complement
                }
            }).FirstOrDefaultAsync();
    }
}
