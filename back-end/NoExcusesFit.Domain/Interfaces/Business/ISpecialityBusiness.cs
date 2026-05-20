using NoExcusesFit.Domain.DTOs.Speciality;
using NoExcusesFit.Domain.Entities;

namespace NoExcusesFit.Domain.Interfaces.Business
{
    public interface ISpecialityBusiness
    {
        Task<SpecialityResponseDto> AddAsync(CreateSpecialityRequestDto speciality);
        Task<IEnumerable<SpecialityDto>> GetAllAsync();
        Task UpdateAsync(int id, UpdateSpecialityRequestDto speciality);
        Task DeleteAsync(int id);
    }
}
