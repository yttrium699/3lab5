using System.ComponentModel.DataAnnotations.Schema;
namespace EmotionsApp.Domain.Entities
{
    [Table("emotion_notes")]
    public class EmotionNote
    {
        public Guid Id { get; set; }
        public Guid EmotionId { get; set; }
        public string NoteText { get; set; } = null!;
        public virtual Emotion Emotion { get; set; } = null!;

    }
}
