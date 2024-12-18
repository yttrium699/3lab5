using EmotionsApp.Domain.Interfaces;
using EmotionsApp.Domain.Entities;

namespace EmotionsApp.Domain.Services
{
    public class PsychologistService : IPsychologistService
    {
        private readonly IPsychologistRepository _psychologistRepository;

        public PsychologistService(IPsychologistRepository psychologistRepository)
        {
            _psychologistRepository = psychologistRepository;
        }

        public async Task<IEnumerable<Psychologist>> GetPsychologistsByUserAsync(Guid userId)
        {
            return await _psychologistRepository.GetByUserIdAsync(userId);
        }

        public async Task AddPsychologistAsync(Guid userId, string name, string contactInfo)
        {
            var psychologist = new Psychologist
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name,
                ContactInfo = contactInfo
            };

            await _psychologistRepository.AddAsync(psychologist);
        }

        public async Task UpdatePsychologistAsync(Guid psychologistId, string name, string contactInfo)
        {
            var psychologist = await _psychologistRepository.GetByIdAsync(psychologistId);
            if (psychologist == null)
            {
                throw new Exception("Психолог не найден");
            }

            psychologist.Name = name;
            psychologist.ContactInfo = contactInfo;

            await _psychologistRepository.UpdateAsync(psychologist);
        }

        public async Task DeletePsychologistAsync(Guid psychologistId)
        {
            await _psychologistRepository.DeleteAsync(psychologistId);
        }
    }
}
