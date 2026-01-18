using ProductControl.Infrastructure.Services.Cep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Tests
{
    public class ViaCepServiceIntegrationTests
    {
        [Fact]
        public async Task GetAddressByCepAsync_RealRequest_ShouldReturnValidData()
        {
            string cepValido = "87014010";
            var service = new CepService();

            var result = await service.GetAddressByCepAsync(cepValido);

            Assert.NotNull(result);
            Assert.Equal("Avenida Carneiro Leão", result.Street);
            Assert.Equal("Maringá", result.City);
            Assert.Equal("PR", result.State);
        }
    }
}
