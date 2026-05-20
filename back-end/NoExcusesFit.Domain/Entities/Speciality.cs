namespace NoExcusesFit.Domain.Entities
{
    public class Speciality
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Speciality() { }
        
        public Speciality(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição é obrigatória.");

            Description = description.Trim();
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Descrição é obrigatória.");

            Description = description.Trim();
        }
    }
}
