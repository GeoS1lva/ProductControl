using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Application.DTOs.UsersDTOs
{
    public class ResponseLogin
    {
        public string Token { get; set; }
        public string UserName { get; set; }
    }
}
