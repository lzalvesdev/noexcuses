using NoExcusesFit.Domain.Entities.Base;

namespace NoExcusesFit.Domain.Entities
{
    public class Coach : EntityBase
    {
        public Guid UserAccountId { get; private set; }

        private Coach() { }

        public Coach(Guid userAccountId)
        {
            if (userAccountId == Guid.Empty)
                throw new ArgumentException("UserAccountId inválido.");

            UserAccountId = userAccountId;
        }
    }
}
