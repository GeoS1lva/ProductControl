using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Domain.Interfaces
{
    public record ExternalAddressData(string cep, string Street, string Neighborhood, string City, string State);

    public interface ICepService
    {
        public Task<ExternalAddressData?> GetAddressByCepAsync(string cep);
    }
}
