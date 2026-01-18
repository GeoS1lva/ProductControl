using ProductControl.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProductControl.Application.DTOs.UsersDTOs
{
    public class CreateUser
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }

        public string Cep { get; set; }
        public string ResidenceNumber { get; set; }
        public string Complement { get; set; }
    }
}
