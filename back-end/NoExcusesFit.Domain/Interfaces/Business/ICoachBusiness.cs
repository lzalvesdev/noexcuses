using NoExcusesFit.Domain.DTOs.Coach;

namespace NoExcusesFit.Domain.Interfaces.Business;

public interface ICoachBusiness
{
    Task<IEnumerable<CoachResponseDto>> GetAllAsync(int page, int pageSize);
    Task<CoachResponseDto> GetByIdAsync(Guid id);
    Task<CoachResponseDto> RegisterAsync(RegisterCoachRequestDto request);
    Task<CoachCreatedResponseDto> AddAsync(CreateCoachRequestDto coach);
    Task AddSpecialityAsync(Guid id, int specialityId);
    Task DeleteAsync(Guid id);
}