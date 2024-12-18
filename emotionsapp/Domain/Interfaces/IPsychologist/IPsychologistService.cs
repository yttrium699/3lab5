using EmotionsApp.Domain.Entities;

namespace EmotionsApp.Domain.Interfaces
{
    public interface IPsychologistService
    {
        Task<IEnumerable<Psychologist>> GetPsychologistsByUserAsync(Guid userId);
        Task AddPsychologistAsync(Guid userId, string name, string contactInfo);
        Task UpdatePsychologistAsync(Guid psychologistId, string name, string contactInfo);
        Task DeletePsychologistAsync(Guid psychologistId);
    }
}
