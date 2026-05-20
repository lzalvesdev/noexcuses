using NoExcusesFit.Domain.DTOs.Athlete;

namespace NoExcusesFit.Domain.Interfaces.Business
{
    public interface IAthleteBusiness
    {  
        Task<IEnumerable<AthleteResponseDto>> GetAllAsync(int page, int pageSize);
        Task<AthleteResponseDto> GetByIdAsync(Guid id);
        Task<AthleteResponseDto> RegisterAsync(RegisterAthleteRequestDto request);
        Task<CreateAthleteResponseDto> AddAsync(CreateAthleteRequestDto request);
        Task UpdateAsync(Guid id, UpdateAthleteRequestDto request);
        Task DeleteAsync(Guid id);
    }
}
