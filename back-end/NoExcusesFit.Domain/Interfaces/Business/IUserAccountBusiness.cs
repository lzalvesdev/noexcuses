using NoExcusesFit.Domain.DTOs.UserAccount;

namespace NoExcusesFit.Domain.Interfaces.Business
{
    public interface IUserAccountBusiness
    {
        Task<Guid> Register(RegisterRequestDto request);
        Task<AuthResponse> Login(LoginRequestDto request);
        Task<UserAccountResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<UserAccountResponse>> GetAllAsync(int page, int pageSize);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        Task UpdateAsync(Guid id, UpdateUserAccountRequestDto request);
        Task DeleteAsync(Guid id);
    }
}
