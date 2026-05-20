using Dapper;
using NoExcusesFit.Data.Persistence;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.Interfaces.Repositories;
using System.Data;

namespace NoExcusesFit.Data.Repositories;

public class UserAccountRepository : IUserAccountRepository
{
    private readonly DapperContext _dapperContext;

    public UserAccountRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }
    
    public async Task CreateAsync(UserAccount userAccount)
    {
        const string sql = @"
            INSERT INTO UserAccount (Id, FirstName, Password, Email)
            VALUES (@Id, @FirstName, @Password, @Email);";

        using var connection = _dapperContext.CreateConnection();

        await connection.ExecuteAsync(sql, userAccount);
    }

    public async Task CreateAsync(UserAccount userAccount, IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"
            INSERT INTO UserAccount (Id, FirstName, Password, Email)
            VALUES (@Id, @FirstName, @Password, @Email);";

        await connection.ExecuteAsync(sql, userAccount, transaction);
    }

    public async Task<UserAccount?> GetByEmailAsync(string email)
    {
        const string sql = @"
            SELECT * 
            FROM UserAccount
            WHERE Email = @Email";

        using var connection = _dapperContext.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<UserAccount>(sql, new { Email = email });
    }

    public async Task<UserAccount?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT * 
            FROM UserAccount
            WHERE Id = @Id";

        using var connection = _dapperContext.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<UserAccount>(sql, new { Id = id });
    }

    public async Task<List<UserAccount>> GetAllAsync(int skip, int take)
    {
        const string sql = @"
            SELECT * FROM UserAccount
            ORDER BY Id
            OFFSET @Skip ROWS 
            FETCH NEXT @Take ROWS ONLY";

        using var connection = _dapperContext.CreateConnection();

        var result = await connection.QueryAsync<UserAccount>(sql, new { Skip = skip, Take = take });
        return result.ToList();
    }

    public async Task UpdateAsync(UserAccount userAccount)
    {
        const string sql = @"
            UPDATE UserAccount
            SET FirstName = @FirstName,
                Email = @Email
            WHERE Id = @Id";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, userAccount);
    }

    public async Task DeleteAsync(Guid id)
    {
       const string sql = @"
            DELETE FROM UserAccount
            WHERE Id = @Id";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}