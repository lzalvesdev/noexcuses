namespace NoExcusesFit.Domain.QueryResults
{
    public class CoachQueryResult
    {
        public Guid Id { get; set; }
        public Guid UserAccountId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<SpecialityItemResult> Specialities { get; set; } = new();
        public List<AthleteItemResult> Athletes { get; set; } = new();
    }
}
