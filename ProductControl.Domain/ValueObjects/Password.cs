using ProductControl.Domain.Common;
using ProductControl.Domain.Interfaces;

namespace ProductControl.Domain.ValueObjects
{
    public record Password
    {
        public byte[] HashPassword { get; }
        public byte[] SaltPassword { get; }

        private Password(byte[] hashPassword, byte[] saltPassword)
        {
            HashPassword = hashPassword;
            SaltPassword = saltPassword;
        }

        public static ResultPattern<Password> Create(byte[] hashPassword, byte[] saltPassword)
        {
            if (hashPassword == null || hashPassword.Length == 0)
                return ResultPattern<Password>.Failure("Hash Inválido!");

            if (saltPassword == null || saltPassword.Length == 0)
                return ResultPattern<Password>.Failure("Salt Inválido!");

            return ResultPattern<Password>.Success(new Password(hashPassword, saltPassword));
        }

        public ResultPattern ValidatePassword(string attempt, IPasswordHasher hasher)
        {
            var result = hasher.ValidatePasswordInternal(attempt, HashPassword, SaltPassword);

            if (!result)
                return ResultPattern.Failure("Senha Incorreta!");

            return ResultPattern.Success();
        }
    }
}
