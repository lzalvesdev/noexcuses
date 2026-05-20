namespace NoExcusesFit.Domain.DTOs.Speciality
{
    public record CreateSpecialityRequestDto(string Description);

    public record SpecialityDto(
        int Id,
        string Description,
        DateTime CreatedAt
    );

    public record SpecialityResponseDto(int Id);
    public record UpdateSpecialityRequestDto(string Description);
    public record SpecialitySummaryDto(int Id, string Description);

}
