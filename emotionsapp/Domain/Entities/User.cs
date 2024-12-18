namespace EmotionsApp.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string PassHash { get; set; } = null!;
        public virtual ICollection<Emotion> Emotions { get; set; } = new List<Emotion>();
        public virtual ICollection<Psychologist> Psychologists { get; set; } = new List<Psychologist>();

    }
}
