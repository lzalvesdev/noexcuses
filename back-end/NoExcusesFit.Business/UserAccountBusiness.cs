using NoExcusesFit.Domain.DTOs.UserAccount;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.Enums;
using NoExcusesFit.Domain.Exceptions;
using NoExcusesFit.Domain.Interfaces;
using NoExcusesFit.Domain.Interfaces.Authentication;
using NoExcusesFit.Domain.Interfaces.Business;
using NoExcusesFit.Domain.Interfaces.Repositories;

namespace NoExcusesFit.Business
{
    public class UserAccountBusiness : IUserAccountBusiness
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserAccountBusiness(
                IUserAccountRepository userAccountRepository,
                IUserRoleRepository userRoleRepository,
                IJwtTokenGenerator jwtTokenGenerator,
                IRefreshTokenRepository refreshTokenRepository,
                IUnitOfWork unitOfWork)
        {
            _userAccountRepository = userAccountRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRoleRepository = userRoleRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;   
        }

        public async Task<IEnumerable<UserAccountResponse>> GetAllAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var users = await _userAccountRepository.GetAllAsync(skip, pageSize);

            return users.Select(u => new UserAccountResponse(
                 u.Id,
                 u.FirstName,
                 u.Email));
        }

        public async Task<UserAccountResponse> GetByIdAsync(Guid id)
        {
            var user = await _userAccountRepository.GetByIdAsync(id);
            if (user is null)
                throw new NotFoundException("Usuário não encontrado.");

            return new UserAccountResponse(
                 user.Id,
                 user.FirstName,
                 user.Email);
        }

        public async Task<Guid> Register(RegisterRequestDto request)
        {
            var userExisting = await _userAccountRepository.GetByEmailAsync(request.Email);
            if (userExisting is not null)
                throw new InvalidOperationException("Email já cadastrado.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new UserAccount(
                firstName: request.FirstName,
                email: request.Email,
                password: passwordHash
            );

            var defaultRole = new UserRole(user.Id, (int)UserRoleType.User);

            _unitOfWork.Begin();

            await _userAccountRepository.CreateAsync(user, _unitOfWork.Connection, _unitOfWork.Transaction);
            await _userRoleRepository.AssignUserRoleAsync(defaultRole, _unitOfWork.Connection, _unitOfWork.Transaction);

            _unitOfWork.Commit();
            return user.Id;
        }

        public async Task<AuthResponse> Login(LoginRequestDto request)
        {
            var user = await _userAccountRepository.GetByEmailAsync(request.Email);
            if (user is null)
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!passwordValid)
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            var roles = await _userRoleRepository.GetRolesAsync(user.Id);

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user.Id, user.FirstName, user.Email, roles);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(
                userAccountId: user.Id,
                token: refreshToken,
                expiresAt: DateTime.UtcNow.AddDays(30)
            );

            await _refreshTokenRepository.CreateAsync(refreshTokenEntity);

            return new AuthResponse(accessToken, refreshToken, user.FirstName, user.Email);
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (token is null)
                throw new UnauthorizedAccessException("Token inválido.");

            if (!token.IsActive)
                throw new UnauthorizedAccessException("Token expirado ou revogado.");

            var user = await _userAccountRepository.GetByIdAsync(token.UserAccountId);
            if (user is null)
                throw new UnauthorizedAccessException("Usuário não encontrado.");

            var roles = await _userRoleRepository.GetRolesAsync(user.Id);

            token.Revoke();

            _unitOfWork.Begin();

            await _refreshTokenRepository.UpdateAsync(token, _unitOfWork.Connection, _unitOfWork.Transaction);

            var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user.Id, user.FirstName, user.Email, roles);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshToken(
               userAccountId: user.Id,
               token: newRefreshToken,
               expiresAt: DateTime.UtcNow.AddDays(30)
           );

            await _refreshTokenRepository.CreateAsync(newRefreshTokenEntity, _unitOfWork.Connection, _unitOfWork.Transaction);

            _unitOfWork.Commit();

            return new AuthResponse(newAccessToken, newRefreshToken, user.FirstName, user.Email);
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (token is null)
                throw new UnauthorizedAccessException("Token inválido.");

            token.Revoke();

            await _refreshTokenRepository.UpdateAsync(token);
        }

        public async Task UpdateAsync(Guid id, UpdateUserAccountRequestDto request)
        {
            var user = await _userAccountRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("Usuário não encontrado.");

            if (user.Email != request.Email)
            {
                var userExisting = await _userAccountRepository.GetByEmailAsync(request.Email);
                if (userExisting is not null && userExisting.Id != id)
                    throw new InvalidOperationException("Email já cadastrado.");
            }

            user.UpdateInfo(request.FirstName, request.Email);

            await _userAccountRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userAccountRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("Usuário não encontrado.");

            await _userAccountRepository.DeleteAsync(id);
        }
    }
}
