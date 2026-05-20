namespace NoExcusesFit.Domain.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(Guid userId, string firstName, string email, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}
