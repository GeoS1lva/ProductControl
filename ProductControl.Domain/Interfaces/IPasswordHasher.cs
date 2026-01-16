using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        public byte[] GenerateSaltUser();
        public byte[] GeneratePasswordInternal(byte[] salt, string simplePassword);
        public bool ValidatePasswordInternal(string attempt, byte[] hash, byte[] salt);
    }
}
