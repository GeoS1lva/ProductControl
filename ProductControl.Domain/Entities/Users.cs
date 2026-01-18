using ProductControl.Domain.Common;
using ProductControl.Domain.Enums;
using ProductControl.Domain.ValueObjects;

namespace ProductControl.Domain.Entities
{
    public class Users : EntitiyBase
    {
        public string FullName { get; private set; }
        public string UserName { get; private set; }
        public Password Password { get; private set; }
        public Roles Role { get; private set; }
        public bool Status { get; private set; }

        public Address Address { get; set; }
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

        private Users(string fullName, string userName, Password password, Roles role, Address address)
        {
            FullName = fullName;
            UserName = userName;
            Password = password;
            Role = role;
            Address = address;
            Status = true;
            CreatedAt = DateTime.UtcNow;
        }

        public static ResultPattern<Users> Create(string fullName, string userName, Password password, Roles role, Address address)
        {
            if (string.IsNullOrWhiteSpace(fullName) || !fullName.Contains(" "))
                return ResultPattern<Users>.Failure("Preencha corretamente o Nome Completo");

            if (string.IsNullOrWhiteSpace(userName))
                return ResultPattern<Users>.Failure("Preencha corretamente o UserName");

            return ResultPattern<Users>.Success(new Users(fullName, userName, password, role, address));
        }

        private Users() { }

        public void UpdateProfile(string fullName, string userName)
        {
            if (!string.IsNullOrWhiteSpace(fullName) && fullName.Contains(" "))
                FullName = fullName;

            if (!string.IsNullOrWhiteSpace(userName))
                UserName = userName;
        }

        public void UpdateAddress(Address address)
        {
            Address = address;
        }

        public void UpdatePassword(Password password)
        {
            Password = password;
        }

        public ResultPattern DeactivateUser()
        {
            if (Status is false)
                return ResultPattern.Failure("Esse usuário já está desativado.");

            Status = false;
            return  ResultPattern.Success();

        }

        public ResultPattern ActivateUser()
        {
            if (Status is true)
                return ResultPattern.Failure("Esse usuário já está ativo.");

            Status = true;
            return ResultPattern.Success();
        }
    }
}
