using ProductControl.Domain.Common;
using ProductControl.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Interfaces.Services
{
    public interface IPasswordService
    {
        public ResultPattern<Password> GeneratePassword(string attempt);
    }
}
