using Moq;
using ProductControl.Application.DTOs.UsersDTOs;
using ProductControl.Application.Interfaces.Queries;
using ProductControl.Application.Services;
using ProductControl.Domain.Common;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Enums;
using ProductControl.Domain.Interfaces;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Domain.Interfaces.Services;
using ProductControl.Domain.ValueObjects;

namespace ProductControl.Tests
{
    public class UsersAppServiceTests
    {
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ICepService> _cepServiceMock;
        private readonly Mock<IUsersService> _usersServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UsersAppService _service;

        public UsersAppServiceTests()
        {
            _passwordServiceMock = new Mock<IPasswordService>();
            _cepServiceMock = new Mock<ICepService>();
            _usersServiceMock = new Mock<IUsersService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var queriesMock = new Mock<IUsersQueries>();
            var tokenMock = new Mock<ITokenService>();

            _service = new UsersAppService(
                _passwordServiceMock.Object,
                _cepServiceMock.Object,
                _usersServiceMock.Object,
                _unitOfWorkMock.Object,
                queriesMock.Object,
                tokenMock.Object
            );
        }

        [Fact]
        public async Task CreateUser_WhenPasswordIsInvalid_ShouldReturnFailure()
        {
            var request = new CreateUser { Password = "123" };
            _passwordServiceMock.Setup(x => x.GeneratePassword(It.IsAny<string>()))
                .Returns(ResultPattern<Password>.Failure("Senha muito curta."));

            var result = await _service.CreateUserAsync(request);

            Assert.True(result.Error);
            Assert.Equal("Senha muito curta.", result.ErrorMessage);
            _cepServiceMock.Verify(x => x.GetAddressByCepAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CreateUser_WhenCepIsInvalid_ShouldReturnFailure()
        {
            var request = new CreateUser { Cep = "00000000" };

            _cepServiceMock.Setup(x => x.GetAddressByCepAsync(request.Cep))
                .ReturnsAsync((ExternalAddressData)null);

            var result = await _service.CreateUserAsync(request);

            Assert.True(result.Error);
            Assert.Equal("CEP Inválido!", result.ErrorMessage);
        }

        [Fact]
        public async Task CreateUser_WhenEverythingIsValid_ShouldSaveAndReturnSuccess()
        {
            var request = new CreateUser
            {
                FullName = "Geovana",
                UserName = "geovana.silva",
                Password = "Password123!",
                Cep = "01001000",
                ResidenceNumber = "10"
            };

            var hashFake = new byte[] { 0x01 };
            var saltFake = new byte[] { 0x02 };
            var fakePassword = Password.Create(hashFake, saltFake).Value;

            var fakeAddress = Address.Create("01001000", "Rua A", "Bairro B", "Cidade C", "SP", "10", "").Value;
            var fakeUser = Users.Create(request.FullName, request.UserName, fakePassword, Roles.administrator, fakeAddress).Value;

            _passwordServiceMock.Setup(x => x.GeneratePassword(request.Password))
                .Returns(ResultPattern<Password>.Success(fakePassword));

            _cepServiceMock.Setup(x => x.GetAddressByCepAsync(request.Cep))
                .ReturnsAsync(new ExternalAddressData("01001000", "Rua A", "Bairro B", "Cidade C", "SP"));

            _usersServiceMock.Setup(x => x.CreateAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Password>(), It.IsAny<Roles>(), It.IsAny<Address>()))
                .ReturnsAsync(ResultPattern<Users>.Success(fakeUser));

            _unitOfWorkMock.Setup(x => x.UserRepository.AddAsync(It.IsAny<Users>()))
                .Returns(Task.CompletedTask);

            var result = await _service.CreateUserAsync(request);

            Assert.False(result.Error);
        }
    }
}
