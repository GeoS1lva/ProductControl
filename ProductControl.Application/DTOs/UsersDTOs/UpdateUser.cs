using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Application.DTOs.UsersDTOs
{
    public class UpdateUser
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Cep { get; set; }
        public string ResidenceNumber { get; set; }
        public string Complement { get; set; }
    }
}
