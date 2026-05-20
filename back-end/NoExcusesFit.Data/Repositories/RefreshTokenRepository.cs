using Dapper;
using NoExcusesFit.Data.Persistence;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.Interfaces.Repositories;
using System.Data;

namespace NoExcusesFit.Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DapperContext _dapperContext;

        public RefreshTokenRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            const string sql = @"
                INSERT INTO RefreshToken(Id, UserAccountId, Token, ExpiresAt, CreatedAt)
                VALUES (@Id, @UserAccountId, @Token, @ExpiresAt, @CreatedAt)";

            using var connection = _dapperContext.CreateConnection();

            await connection.ExecuteAsync(sql, refreshToken);
        }

        public async Task CreateAsync(RefreshToken refreshToken, IDbConnection connection, IDbTransaction transaction)
        {
            const string sql = @"
                INSERT INTO RefreshToken(Id, UserAccountId, Token, ExpiresAt, CreatedAt)
                VALUES (@Id, @UserAccountId, @Token, @ExpiresAt, @CreatedAt)";

            await connection.ExecuteAsync(sql, refreshToken, transaction);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            const string sql = @"
                SELECT Id, UserAccountId, Token, ExpiresAt, CreatedAt, RevokedAt
                FROM RefreshToken
                WHERE Token = @Token";

            using var connection = _dapperContext.CreateConnection();

            return await connection.QuerySingleOrDefaultAsync<RefreshToken>(sql, new { Token = token });
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            const string sql = @"
                UPDATE RefreshToken 
                SET RevokedAt = @RevokedAt
                WHERE Id = @Id";

            using var connection = _dapperContext.CreateConnection();

            await connection.ExecuteAsync(sql, refreshToken);
        }

        public async Task UpdateAsync(RefreshToken refreshToken, IDbConnection connection, IDbTransaction transaction)
        {
            const string sql = @"
                UPDATE RefreshToken 
                SET RevokedAt = @RevokedAt
                WHERE Id = @Id";

            await connection.ExecuteAsync(sql, refreshToken, transaction);
        }

        public async Task<int> DeleteExpiredAsync()
        {
            const string sql = @"
                DELETE FROM RefreshToken
                WHERE ExpiresAt < GETUTCDATE() OR RevokedAt IS NOT NULL";

            using var connection = _dapperContext.CreateConnection();
            return await connection.ExecuteAsync(sql);
        }

    }
}
