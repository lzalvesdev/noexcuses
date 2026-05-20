using NoExcusesFit.Domain.DTOs.Athlete;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.QueryResults;
using System.Data;

namespace NoExcusesFit.Domain.Interfaces.Repositories
{
    public interface IAthleteRepository
    {
        Task<AthleteItemResult?> GetByIdAsync(Guid id);
        Task<List<AthleteItemResult>> GetAllAsync(int skip, int take);
        Task AddAsync(Athlete athlete);
        Task AddAsync(Athlete athlete, IDbConnection connection, IDbTransaction transaction);
        Task UpdateAsync(Guid id, UpdateAthleteRequestDto request);
        Task DeleteAsync(Guid id);
        Task DeleteAsync(Guid id, IDbConnection connection, IDbTransaction transaction);
    }
}
