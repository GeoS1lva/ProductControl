using ProductControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Application.DTOs.UsersDTOs
{
    public class ReturnAddress
    {
        public Guid AddressId { get; set; }
        public string Cep { get; set; }
        public string Street { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
    }

    public class ReturnUserAndAddress
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public Roles Role { get; set; }
        public string CreateAt { get; set; }
        public ReturnAddress Address { get; set; }
    }
}
