using Dapper;
using NoExcusesFit.Data.Persistence;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.Interfaces.Repositories;

namespace NoExcusesFit.Data.Repositories;

public class SpecialityRepository : ISpecialityRepository
{
    private readonly DapperContext _dapperContext;

    public SpecialityRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<int> AddAsync(Speciality speciality)
    {
        const string sql = @"
            INSERT INTO Speciality (Description, CreatedAt)
            OUTPUT INSERTED.Id
            VALUES (@Description, @CreatedAt);";

        using var connection = _dapperContext.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(sql, speciality);
    }


    public async Task<List<Speciality>> GetAllAsync()
    {
        const string sql = @"
            SELECT s.*
            FROM Speciality s";

        using var connection = _dapperContext.CreateConnection();

        var result = await connection.QueryAsync<Speciality>(sql);
        return result.ToList();
    }

    public async Task<Speciality?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT * FROM Speciality 
            WHERE Id = @Id";

        using var connection = _dapperContext.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Speciality>(sql, new { Id = id });
    }

    public async Task UpdateAsync(Speciality speciality)
    {
        const string sql = @"
            UPDATE Speciality
            SET Description = @Description
            WHERE Id = @Id";
        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, speciality);
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = @"
            DELETE FROM Speciality
            WHERE Id = @Id";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
} 