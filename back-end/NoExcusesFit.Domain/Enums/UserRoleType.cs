namespace NoExcusesFit.Domain.Enums
{
    public enum UserRoleType
    {
        Coach = 1,
        Athlete = 2,
        User = 3,
        Manager = 4,
        Admin = 99
    }

    public static class Roles
    {
        public const string User = "User";
        public const string Athlete = "Athlete";
        public const string Coach = "Coach";
        public const string Manager = "Manager";
        public const string Admin = "Admin";
    }
}
