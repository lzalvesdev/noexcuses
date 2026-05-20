using Dapper;
using NoExcusesFit.Data.Persistence;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.Interfaces.Repositories;
using System.Data;

namespace NoExcusesFit.Data.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly DapperContext _dapperContext;

    public UserRoleRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<IEnumerable<string>> GetRolesAsync(Guid id)
    {
        const string sql = @"
            SELECT r.Description 
            FROM UserRole ur
            INNER JOIN Role r ON r.Id = ur.RoleId
            WHERE ur.UserAccountId = @UserAccountId";

        using var connection = _dapperContext.CreateConnection();

        var result = await connection.QueryAsync<string>(sql, new { UserAccountId = id });
        return result.ToList();
    }

    public async Task AssignUserRoleAsync(UserRole userRole)
    {
        const string sql = @"
            INSERT INTO UserRole (UserAccountId, RoleId)
            VALUES (@UserAccountId, @RoleId);";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, userRole);
    }

    public async Task AssignUserRoleAsync(UserRole userRole, IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"
            INSERT INTO UserRole (UserAccountId, RoleId)
            VALUES (@UserAccountId, @RoleId);";

        await connection.ExecuteAsync(sql, userRole, transaction);
    }

    public async Task DeleteRoleAsync(Guid userAccountId, int roleId)
    {
        const string sql = @"
            DELETE FROM UserRole
            WHERE UserAccountId = @UserAccountId AND RoleId = @RoleId;";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, new { UserAccountId = userAccountId, RoleId = roleId });
    }

    public async Task DeleteRoleAsync(Guid userAccountId, int roleId, IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"
            DELETE FROM UserRole
            WHERE UserAccountId = @UserAccountId AND RoleId = @RoleId;";

        await connection.ExecuteAsync(sql, new { UserAccountId = userAccountId, RoleId = roleId }, transaction);
    }
}