using EmotionsApp.Domain.Entities;

namespace EmotionsApp.Domain.Interfaces
{
    public interface IEmotionNoteService
    {
        Task<IEnumerable<EmotionNote>> GetNotesByEmotionAsync(Guid emotionId);
        Task AddNoteAsync(Guid emotionId, string noteText);
        Task UpdateNoteAsync(Guid noteId, string noteText);
        Task DeleteNoteAsync(Guid noteId);
    }
}
