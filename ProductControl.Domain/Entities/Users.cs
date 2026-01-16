using ProductControl.Domain.Common;
using ProductControl.Domain.Enums;
using ProductControl.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Entities
{
    public class Users : EntitiyBase
    {
        public string FullName { get; private set; }
        public string UserName { get; private set; }
        public Password Password { get; private set; }
        public Roles Role { get; private set; }

        public Address Address { get; set; }

        private Users(string fullName, string userName, Password password, Roles role, Address address)
        {
            FullName = fullName;
            UserName = userName;
            Password = password;
            Role = role;
            Address = address;
            CreatedAt = DateTime.Now;
        }

        public ResultPattern<Users> Create(string fullName, string userName, Password password, Roles role, Address address)
        {
            if (string.IsNullOrWhiteSpace(fullName) || !fullName.Contains(" "))
                return ResultPattern<Users>.Failure("Preencha corretamente o Nome Completo");

            if (string.IsNullOrWhiteSpace(userName))
                return ResultPattern<Users>.Failure("Preencha corretamente o UserName");

            var user = new Users(fullName, userName, password, role, address);

            return ResultPattern<Users>.Success(user);
        }

        private Users() { }
    }
}
