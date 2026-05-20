namespace NoExcusesFit.Domain.Entities
{
    public class UserRole
    {
        public Guid UserAccountId { get; private set; }
        public int RoleId { get; private set; }
        
        private UserRole() { }
        
        public UserRole(Guid userAccountId, int roleId)
        {
            if (userAccountId == Guid.Empty)
                throw new ArgumentException("UserAccountId inválido.");

            if (roleId <= 0)
                throw new ArgumentException("RoleId inválido.");

            UserAccountId = userAccountId;
            RoleId = roleId;
        }
    }
}
