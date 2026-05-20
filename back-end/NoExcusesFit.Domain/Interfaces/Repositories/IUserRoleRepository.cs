using NoExcusesFit.Domain.Entities;
using System.Data;

namespace NoExcusesFit.Domain.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {
        Task AssignUserRoleAsync(UserRole userRole);
        Task AssignUserRoleAsync(UserRole userRole, IDbConnection connection, IDbTransaction transaction);
        Task<IEnumerable<string>> GetRolesAsync(Guid id);
        Task DeleteRoleAsync(Guid userAccountId, int roleId);
        Task DeleteRoleAsync(Guid userAccountId, int roleId, IDbConnection connection, IDbTransaction transaction);
    }
}
