using ProductControl.Domain.Common;
using ProductControl.Domain.Interfaces;
using ProductControl.Domain.Interfaces.Services;
using ProductControl.Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace ProductControl.Domain.DomainServices
{
    public class PasswordService(IPasswordHasher passwordHasher) : IPasswordService
    {
        private const string
            INVALID_SIZE = "A senha possui menos de 8 Caracteres!",
            INVALID_CHARACTER = "A senha precisa conter ao menos 1 caracter especial!",
            INVALID_UPPERCASE = "A senha precisa conter ao menos um caracter Maiúsculo!",
            INVALID_LOWERCASE = "A senha precisa conter ao menos um caracter Minúsculo!",
            INVALID_NUMBER = "A senha precisa conter ao menos 1 número!";

        public ResultPattern<Password> GeneratePassword(string attempt)
        {
            if (attempt.Length < 8)
                return ResultPattern<Password>.Failure(INVALID_SIZE);

            if (!Regex.IsMatch(attempt, @"[^a-zA-Z0-9]"))
                return ResultPattern<Password>.Failure(INVALID_CHARACTER);

            if (!attempt.Any(char.IsUpper))
                return ResultPattern<Password>.Failure(INVALID_UPPERCASE);

            if (!attempt.Any(char.IsLower))
                return ResultPattern<Password>.Failure(INVALID_LOWERCASE);

            if (!attempt.Any(char.IsDigit))
                return ResultPattern<Password>.Failure(INVALID_NUMBER);

            var saltPassword = passwordHasher.GenerateSaltUser();
            var hashPassword = passwordHasher.GeneratePasswordInternal(saltPassword, attempt);

            var result = Password.Create(hashPassword, saltPassword);

            if (result.Error)
                return ResultPattern<Password>.Failure(result.ErrorMessage);

            return ResultPattern<Password>.Success(result.Value);
        }
    }
}
