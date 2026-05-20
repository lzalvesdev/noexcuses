namespace NoExcusesFit.Domain.DTOs.UserAccount
{
    //public record CreateUserRequest(string FirstName, string Email);

    public record UpdateUserAccountRequestDto(string FirstName, string Email);

    public record LoginRequestDto(string Email, string Password);

    public record RegisterRequestDto(string FirstName, string Email, string Password);

    public record UserAccountResponse(Guid Id, string FirstName, string Email);

    public record AuthResponse(
        string Token,
        string RefreshToken,
        string Name,
        string Email
    );
}
