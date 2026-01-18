using ProductControl.Application.DTOs.UsersDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Application.Interfaces.Queries
{
    public interface IUsersQueries
    {
        public Task<IList<ReturnUserAndAddress>?> GetAllUsers();
        public Task<ReturnUserAndAddress?> GetByIdUser(Guid id);
    }
}
