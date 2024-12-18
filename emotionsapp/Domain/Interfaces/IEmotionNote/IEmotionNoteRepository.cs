using EmotionsApp.Domain.Entities;

namespace EmotionsApp.Domain.Interfaces
{
    public interface IEmotionNoteRepository
    {
        Task<IEnumerable<EmotionNote>> GetByEmotionIdAsync(Guid emotionId);
        Task<EmotionNote?> GetByIdAsync(Guid id);
        Task AddAsync(EmotionNote emotionNote);
        Task UpdateAsync(EmotionNote emotionNote);
        Task DeleteAsync(Guid id);
    }
}
