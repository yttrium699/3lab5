namespace EmotionsApp.Domain.Entities
{
    public class Emotion
    {
        public Guid Id { get; set; }
        public DateTime StateDate { get; set; }
        public string EmotionType { get; set; } = null!;
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<EmotionNote> EmotionNotes { get; set; } = new List<EmotionNote>();

    }
}
