using ProductControl.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Infrastructure.Services.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public byte[] GenerateSaltUser()
        {
            return RandomNumberGenerator.GetBytes(16);
        }

        public byte[] GeneratePasswordInternal(byte[] salt, string simplePassword)
        {
            using var hash = new Rfc2898DeriveBytes(simplePassword, salt, 100_000, HashAlgorithmName.SHA256);
            return hash.GetBytes(32);
        }

        public bool ValidatePasswordInternal(string attempt, byte[] hash, byte[] salt)
        {
            byte[] newHash = GeneratePasswordInternal(salt, attempt);

            return newHash.SequenceEqual(hash);
        }
    }
}
