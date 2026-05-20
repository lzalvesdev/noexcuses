using NoExcusesFit.Domain.DTOs.Coach;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.QueryResults;
using System.Data;

namespace NoExcusesFit.Domain.Interfaces.Repositories;

public interface ICoachRepository
{
    Task AddAsync(Coach coach);
    Task AddAsync(Coach coach, IDbConnection connection, IDbTransaction transaction);
    Task<CoachQueryResult?> GetByIdAsync(Guid id);
    Task<List<CoachQueryResult>> GetAllAsync(int skip, int take);
    Task DeleteAsync(Guid id);
    Task DeleteAsync(Guid id, IDbConnection connection, IDbTransaction transaction);
}