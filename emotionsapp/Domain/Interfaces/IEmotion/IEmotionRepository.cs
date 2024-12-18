using EmotionsApp.Domain.Entities;

namespace EmotionsApp.Domain.Interfaces
{
    public interface IEmotionRepository
    {
        Task<Emotion?> GetByIdAsync(Guid id);
        Task<IEnumerable<Emotion>> GetByUserIdAsync(Guid userId);
        Task AddAsync(Emotion emotion);
        Task UpdateAsync(Emotion emotion);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Emotion>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Emotion>> GetByTypeAsync(Guid userId, string emotionType);
    }
}

