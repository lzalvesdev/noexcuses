using Dapper;
using NoExcusesFit.Data.Persistence;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.Interfaces.Repositories;

namespace NoExcusesFit.Data.Repositories;

public class CoachSpecialityRepository : ICoachSpecialityRepository
{
    private readonly DapperContext _dapperContext;

    public CoachSpecialityRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task AddAsync(CoachSpeciality coachSpeciality)
    {
        const string sql = @"
            INSERT INTO CoachSpeciality (SpecialityId, CoachId)
            VALUES (@SpecialityId, @CoachId);";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, coachSpeciality);
    }

    public async Task<bool> ExistsAsync(Guid coachId, int specialityId)
    {
        const string sql = @"
            SELECT COUNT(1) FROM CoachSpeciality
            WHERE CoachId = @CoachId AND SpecialityId = @SpecialityId;";

        using var connection = _dapperContext.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(sql, new { CoachId = coachId, SpecialityId = specialityId });
    }

} 