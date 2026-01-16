using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Infrastructure.Services.Cep
{
    public class CepService : ICepService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<ExternalAddressData?> GetAddressByCepAsync(string cep)
        {
            var url = $"https://viacep.com.br/ws/{cep}/json/";

            var response = await _httpClient.GetAsync(url);

            var addressData = await response.Content.ReadFromJsonAsync<ViaCepResponseJson>();

            return response.IsSuccessStatusCode ? new ExternalAddressData(
                addressData.cep,
                addressData.logradouro,
                addressData.bairro,
                addressData.localidade,
                addressData.uf
            ) : null;
        }
    }
}
