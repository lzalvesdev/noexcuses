using Dapper;
using NoExcusesFit.Data.Persistence;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.Interfaces.Repositories;
using NoExcusesFit.Domain.QueryResults;
using System.Data;

namespace NoExcusesFit.Data.Repositories;

public class CoachRepository : ICoachRepository
{
    private readonly DapperContext _dapperContext;

    public CoachRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task AddAsync(Coach coach)
    {
        const string sql = @"
            INSERT INTO Coach (Id, UserAccountId)
            VALUES (@Id, @UserAccountId);";

        using var connection = _dapperContext.CreateConnection();

        await connection.ExecuteAsync(sql, coach);
    }

    public async Task AddAsync(Coach coach, IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"
            INSERT INTO Coach (Id, UserAccountId)
            VALUES (@Id, @UserAccountId);";

        await connection.ExecuteAsync(sql, coach, transaction);
    }

    public async Task<CoachQueryResult?> GetByIdAsync(Guid id)
    {
        using var connection = _dapperContext.CreateConnection();

        const string coachSql = @"
        SELECT c.Id, c.UserAccountId, u.FirstName, u.Email
        FROM Coach c
        LEFT JOIN UserAccount u ON c.UserAccountId = u.Id
        WHERE c.Id = @Id";

        var coach = await connection.QuerySingleOrDefaultAsync<CoachQueryResult>(coachSql, new { Id = id });
        if (coach == null) return null;

        const string specialitySql = @"
            SELECT s.Id, s.Description
            FROM CoachSpeciality cs
            INNER JOIN Speciality s ON s.Id = cs.SpecialityId
            WHERE cs.CoachId = @CoachId";

        var specialities = await connection.QueryAsync<SpecialityItemResult>(specialitySql, new { CoachId = id });
        coach.Specialities.AddRange(specialities);

        const string athleteSql = @"
            SELECT a.Id, a.CoachId, a.UserAccountId, u.FirstName, u.Email
            FROM Athlete a
            INNER JOIN UserAccount u ON u.Id = a.UserAccountId
            WHERE a.CoachId = @CoachId";

        var athletes = await connection.QueryAsync<AthleteItemResult>(athleteSql, new { CoachId = id });
        coach.Athletes.AddRange(athletes);

        return coach;
    }

    public async Task<List<CoachQueryResult>> GetAllAsync(int skip, int take)
    {
        using var connection = _dapperContext.CreateConnection();

        const string coachSql = @"
            SELECT c.Id, c.UserAccountId, u.FirstName, u.Email
            FROM Coach c
            LEFT JOIN UserAccount u ON c.UserAccountId = u.Id
            ORDER BY c.Id
            OFFSET @Skip ROWS
            FETCH NEXT @Take ROWS ONLY";

        var coaches = (await connection.QueryAsync<CoachQueryResult>(
            coachSql, 
            new { Skip = skip, Take = take }
        )).ToList();

        if (!coaches.Any()) return coaches;

        var coachIds = coaches.Select(c => c.Id).ToList();

        const string specialitySql = @"
            SELECT cs.CoachId, s.Id, s.Description
            FROM CoachSpeciality cs
            INNER JOIN Speciality s ON s.Id = cs.SpecialityId
            WHERE cs.CoachId IN @CoachIds";

        var specialities = await connection.QueryAsync<(Guid CoachId, int Id, string Description)>(
           specialitySql,
           new { CoachIds = coachIds }
        );

        var coachesDictionary = coaches.ToDictionary(c => c.Id);

        foreach (var speciality in specialities)
        {
            if (coachesDictionary.TryGetValue(speciality.CoachId, out var coach))
            {
                coach.Specialities.Add(new SpecialityItemResult
                {
                    Id = speciality.Id,
                    Description = speciality.Description,
                });
            }
        }

        const string athleteSql = @"
            SELECT a.Id, a.CoachId, a.UserAccountId, u.FirstName, u.Email
            FROM Athlete a
            INNER JOIN UserAccount u ON u.Id = a.UserAccountId
            WHERE a.CoachId IN @CoachIds";

        var athletes = await connection.QueryAsync<AthleteItemResult>(athleteSql, new { CoachIds = coachIds });

        foreach (var athlete in athletes)
        {
            if (coachesDictionary.TryGetValue(athlete.CoachId, out var coach))
            {
                coach.Athletes.Add(athlete);
            }
        }

        return coaches;
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = @"
            DELETE FROM Coach
            WHERE Id = @Id";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task DeleteAsync(Guid id, IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"
            DELETE FROM Coach
            WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new { Id = id }, transaction);
    }
} 