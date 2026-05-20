using FluentAssertions;
using Moq;
using NoExcusesFit.Domain.DTOs.UserAccount;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.Interfaces;
using NoExcusesFit.Domain.Interfaces.Authentication;
using NoExcusesFit.Domain.Interfaces.Business;
using NoExcusesFit.Domain.Interfaces.Repositories;
using System.Data;

namespace NoExcusesFit.Business.Tests
{
    public class UserAccountBusinessTests
    {
        private readonly Mock<IUserAccountRepository> _userAccountRepository;
        private readonly Mock<IUserRoleRepository> _userRoleRepository;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IUserAccountBusiness _business;

        public UserAccountBusinessTests()
        {
            _userAccountRepository = new Mock<IUserAccountRepository>();
            _userRoleRepository = new Mock<IUserRoleRepository>();
            _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _business = new UserAccountBusiness(
                _userAccountRepository.Object,
                _userRoleRepository.Object,
                _jwtTokenGenerator.Object,
                _refreshTokenRepository.Object,
                _unitOfWork.Object
            );
        }

        [Fact]
        public async Task Register_WhenEmailAlreadyExists_ShouldThrowInvalidOperationException()
        {
            //Arrange
            var existingUser = new UserAccount("luiz", "luiz@gmail.com", BCrypt.Net.BCrypt.HashPassword("123"));

            _userAccountRepository
                .Setup(r => r.GetByEmailAsync("luiz@gmail.com"))
                .ReturnsAsync(existingUser);

            //Act
            var act = async () => await _business.Register(new RegisterRequestDto("luiz", "luiz@gmail.com", "123"));

            //Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Email já cadastrado.");
        }

        [Fact]
        public async Task Login_WhenEmailNotExists_ShouldReturnUnauthorizedAcessException()
        {
            //Arrange
            _userAccountRepository
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserAccount?)null);

            //Act
            var act = async () => await _business.Login(new LoginRequestDto("luiz@gmail.com", "123"));

            //Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Credenciais inválidas.");
        }

        [Fact]
        public async Task Login_WhenPasswordWrong_ShouldReturnUnauthorizedAcessException()
        {
            //Arrange
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("sena123");
            var userFake = new UserAccount("luiz", "luiz@gmail.com", passwordHash);

            _userAccountRepository
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(userFake);

            //Act
            var act = async () => await _business.Login(new LoginRequestDto("luiz@gmail.com", "123"));

            //Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Credenciais inválidas.");
        }

        [Fact]
        public async Task Login_WhenSuccess_ShouldReturnTokens()
        {
            //Arrange
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("123");
            var userFake = new UserAccount("luiz", "luiz@gmail.com", passwordHash);

            _userAccountRepository
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(userFake);

            _userRoleRepository
                .Setup(r => r.GetRolesAsync(userFake.Id))
                .ReturnsAsync(new List<string> { "User", "Coach" });

            _jwtTokenGenerator
                .Setup(j => j.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns("access-token-fake");

            _jwtTokenGenerator
                .Setup(j => j.GenerateRefreshToken())
                .Returns("refresh-token-fake");

            _refreshTokenRepository
                .Setup(r => r.CreateAsync(It.IsAny<RefreshToken>()))
                .Returns(Task.CompletedTask);

            //Act
            var result = await _business.Login(new LoginRequestDto("luiz@gmail.com", "123"));

            //Assert
            result.Should().NotBeNull();
            result.Token.Should().Be("access-token-fake");
            result.RefreshToken.Should().Be("refresh-token-fake");
            result.Email.Should().Be("luiz@gmail.com");
        }

        [Fact]
        public async Task RefreshToken_WhenTokenIsRevoked_ShouldThrowUnauthorizedException()
        {
            // Arrange
            var revokedToken = new RefreshToken(Guid.NewGuid(), "token-revogado", DateTime.UtcNow.AddDays(30));
            revokedToken.Revoke();

            _refreshTokenRepository
                .Setup(r => r.GetByTokenAsync("token-revogado"))
                .ReturnsAsync(revokedToken);

            // Act
            var act = async () => await _business.RefreshTokenAsync("token-revogado");

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Token expirado ou revogado.");
        }

    }

}
