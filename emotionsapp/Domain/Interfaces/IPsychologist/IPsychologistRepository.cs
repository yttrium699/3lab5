using EmotionsApp.Domain.Entities;

namespace EmotionsApp.Domain.Interfaces
{
    public interface IPsychologistRepository
    {
        Task<IEnumerable<Psychologist>> GetByUserIdAsync(Guid userId);
        Task<Psychologist?> GetByIdAsync(Guid id);
        Task AddAsync(Psychologist psychologist);
        Task UpdateAsync(Psychologist psychologist);
        Task DeleteAsync(Guid id);
    }
}
