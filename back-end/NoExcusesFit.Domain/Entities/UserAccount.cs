using NoExcusesFit.Domain.Entities.Base;

namespace NoExcusesFit.Domain.Entities
{
    public class UserAccount : EntityBase
    {
        public string FirstName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        
        private UserAccount() { }
        
        public UserAccount(
            string firstName,
            string email,
            string password
            )
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Nome é obrigatório.");
            
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email é obrigatório.");

            if(string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Senha é obrigatória.");
            
            FirstName = firstName.Trim();
            Email = email.ToLower().Trim();
            Password = password;
        }

        public void UpdateInfo(string firstName, string email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Nome é obrigatório.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email é obrigatório.", nameof(email));

            FirstName = firstName.Trim();
            Email = email.ToLower().Trim();
        }
    }
}
