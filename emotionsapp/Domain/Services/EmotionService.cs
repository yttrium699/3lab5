using EmotionsApp.Domain.Interfaces;
using EmotionsApp.Domain.Entities;

namespace EmotionsApp.Domain.Services
{
    public class EmotionService : IEmotionService
    {
        private readonly IEmotionRepository _emotionRepository;

        public EmotionService(IEmotionRepository emotionRepository)
        {
            _emotionRepository = emotionRepository;
        }

        public async Task<IEnumerable<Emotion>> GetEmotionsByUserAsync(Guid userId)
        {
            return await _emotionRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Emotion>> GetEmotionsByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _emotionRepository.GetByDateRangeAsync(userId, startDate, endDate);
        }

        public async Task<IEnumerable<Emotion>> GetEmotionsByTypeAsync(Guid userId, string emotionType)
        {
            return await _emotionRepository.GetByTypeAsync(userId, emotionType);
        }

        public async Task AddEmotionAsync(Guid userId, string emotionType, DateTime stateDate)
        {
            var emotion = new Emotion
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EmotionType = emotionType,
                StateDate = stateDate
            };

            await _emotionRepository.AddAsync(emotion);
        }

        public async Task AddEmotionNoteAsync(Guid emotionId, string noteText)
        {
            var emotion = await _emotionRepository.GetByIdAsync(emotionId);
            if (emotion == null)
            {
                throw new Exception("Эмоции не найдены");
            }

            var note = new EmotionNote
            {
                Id = Guid.NewGuid(),
                EmotionId = emotionId,
                NoteText = noteText
            };

            emotion.EmotionNotes.Add(note);
            await _emotionRepository.UpdateAsync(emotion);
        }

        public async Task DeleteEmotionAsync(Guid emotionId)
        {
            await _emotionRepository.DeleteAsync(emotionId);
        }
    }
}

