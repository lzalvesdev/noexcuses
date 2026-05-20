using NoExcusesFit.Domain.Entities;

namespace NoExcusesFit.Domain.Interfaces.Repositories
{
    public interface ISpecialityRepository
    {
        Task<int> AddAsync(Speciality speciality);
        Task<List<Speciality>> GetAllAsync();
        Task<Speciality?> GetByIdAsync(int id);
        Task UpdateAsync(Speciality speciality);
        Task DeleteAsync(int id);
    }
}
