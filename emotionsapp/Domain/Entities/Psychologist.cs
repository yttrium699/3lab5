namespace EmotionsApp.Domain.Entities
{
    public class Psychologist
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactInfo { get; set; } = null!;
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

    }
}
