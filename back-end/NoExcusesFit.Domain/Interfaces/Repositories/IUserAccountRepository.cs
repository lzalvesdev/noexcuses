using NoExcusesFit.Domain.Entities;
using System.Data;

namespace NoExcusesFit.Domain.Interfaces.Repositories
{
    public interface IUserAccountRepository
    {
        Task CreateAsync(UserAccount userAccount);
        Task CreateAsync(UserAccount userAccount, IDbConnection connection, IDbTransaction transaction);
        Task<UserAccount?> GetByEmailAsync(string email);
        Task<UserAccount?> GetByIdAsync(Guid id);
        Task<List<UserAccount>> GetAllAsync(int skip, int take);
        Task UpdateAsync(UserAccount userAccount);
        Task DeleteAsync(Guid id);
    }
}
