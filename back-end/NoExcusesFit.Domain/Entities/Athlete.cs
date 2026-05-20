using NoExcusesFit.Domain.Entities.Base;

namespace NoExcusesFit.Domain.Entities
{
    public class Athlete : EntityBase
    {
        public Guid UserAccountId { get; private set; }
        public Guid CoachId { get; private set; }

        private Athlete() { }
        public Athlete(Guid userAccountId, Guid coachId)
        {
            if (userAccountId == Guid.Empty)
                throw new ArgumentException("UserAccountId inválido.");

            if (coachId == Guid.Empty)
                throw new ArgumentException("CoachId inválido.");

            UserAccountId = userAccountId;
            CoachId = coachId;
        }
    }
}
