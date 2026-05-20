using NoExcusesFit.Domain.DTOs.Athlete;
using NoExcusesFit.Domain.DTOs.Speciality;

namespace NoExcusesFit.Domain.DTOs.Coach
{
    public record CreateCoachRequestDto(Guid UserAccountId);
    public record CoachCreatedResponseDto(Guid Id);
    public record CoachAthleteResponseDto(Guid Id, string Description);
    public record CoachResponseDto(
        Guid Id,
        Guid UserAccountId,
        string FirstName,
        string Email,
        List<SpecialitySummaryDto> Specialities,
        List<AthleteSumaryDto> Athletes
    );
    public record RegisterCoachRequestDto(string FirstName, string Email, string Password);
}
