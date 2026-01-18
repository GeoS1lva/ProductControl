using ProductControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        public string TokeGenerator(Users user, string role);
    }
}
