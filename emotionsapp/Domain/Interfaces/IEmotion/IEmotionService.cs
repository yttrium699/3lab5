using EmotionsApp.Domain.Entities;

namespace EmotionsApp.Domain.Interfaces
{
    public interface IEmotionService
    {
        Task<IEnumerable<Emotion>> GetEmotionsByUserAsync(Guid userId);
        Task<IEnumerable<Emotion>> GetEmotionsByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Emotion>> GetEmotionsByTypeAsync(Guid userId, string emotionType);
        Task AddEmotionAsync(Guid userId, string emotionType, DateTime stateDate);
        Task AddEmotionNoteAsync(Guid emotionId, string noteText);
        Task DeleteEmotionAsync(Guid emotionId);
    }
}

