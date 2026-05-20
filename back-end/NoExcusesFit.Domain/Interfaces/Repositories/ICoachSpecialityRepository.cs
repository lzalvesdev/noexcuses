using NoExcusesFit.Domain.Entities;

namespace NoExcusesFit.Domain.Interfaces.Repositories
{
    public interface ICoachSpecialityRepository
    {
        Task AddAsync(CoachSpeciality coachSpeciality);
        Task<bool> ExistsAsync(Guid coachId, int specialityId);
    }
}
