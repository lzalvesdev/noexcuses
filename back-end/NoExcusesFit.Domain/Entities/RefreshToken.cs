using NoExcusesFit.Domain.Entities.Base;

namespace NoExcusesFit.Domain.Entities
{
    public class RefreshToken : EntityBase
    {
        public Guid UserAccountId {get ; private set;}
        public string Token{ get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt is not null;
        public bool IsActive => !IsExpired && !IsRevoked;

        private RefreshToken() { }

        public RefreshToken(Guid userAccountId, string token, DateTime expiresAt)
        {
            if (userAccountId == Guid.Empty)
                throw new ArgumentException("UserAccountId inválido.");

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token inválido.");

            if (expiresAt <= DateTime.UtcNow)
                throw new ArgumentException("Data de expiração inválida.");

            UserAccountId = userAccountId;
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        public void Revoke()
        {
            if (!IsActive)
                throw new InvalidOperationException("Token já está inativo.");

            RevokedAt = DateTime.UtcNow;
        }

    }
}
