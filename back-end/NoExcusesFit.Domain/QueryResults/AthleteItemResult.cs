namespace NoExcusesFit.Domain.QueryResults
{
    public class AthleteItemResult
    {
        public Guid Id { get; set; }
        public Guid CoachId { get; set; }
        public Guid UserAccountId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
