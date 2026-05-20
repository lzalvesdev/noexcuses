using NoExcusesFit.Domain.Entities;
using System.Data;

namespace NoExcusesFit.Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken refreshToken);
        Task CreateAsync(RefreshToken refreshToken, IDbConnection connection, IDbTransaction transaction);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task UpdateAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken, IDbConnection connection, IDbTransaction transaction);
        Task<int> DeleteExpiredAsync ();
    }
}
