using ProductControl.Domain.Common;

namespace ProductControl.Domain.Entities
{
    public class Address : EntitiyBase
    {
        public string Cep { get; private set; }
        public string Street { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }

        public Guid UserId { get; set; }

        private Address(string cep, string street, string neighborhood, string city, string state, string number, string complement)
        {
            Cep = cep;
            Street = street;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            Number = number;
            Complement = complement;
            CreatedAt = DateTime.UtcNow;
        }

        public static ResultPattern<Address> Create(string cep, string street, string neighborhood, string city, string state, string number, string complement)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return ResultPattern<Address>.Failure("CEP Inválido");

            if (string.IsNullOrWhiteSpace(street))
                return ResultPattern<Address>.Failure("Rua Inválida");

            if (string.IsNullOrWhiteSpace(neighborhood))
                return ResultPattern<Address>.Failure("Bairro Inválido");

            if (string.IsNullOrWhiteSpace(city))
                return ResultPattern<Address>.Failure("Cidade Inválida");

            if (string.IsNullOrWhiteSpace(state))
                return ResultPattern<Address>.Failure("Estado Inválido");

            if (string.IsNullOrWhiteSpace(number))
                return ResultPattern<Address>.Failure("Número Inválido");

            return ResultPattern<Address>.Success(new Address(cep, street, neighborhood, city, state, number, complement));
        }

        private Address() { }
    }
}
