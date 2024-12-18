using EmotionsApp.Domain.Entities;
using EmotionsApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmotionsApp.Infrastructure
{
    public class EmotionRepository : IEmotionRepository
    {
        private readonly EmotionsAppContext _context;

        public EmotionRepository(EmotionsAppContext context)
        {
            _context = context;
        }

        public async Task<Emotion?> GetByIdAsync(Guid id)
        {
            return await _context.Emotions
                .Include(e => e.EmotionNotes)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Emotion>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Emotions
                .Where(e => e.UserId == userId)
                .Include(e => e.EmotionNotes)
                .ToListAsync();
        }

        public async Task AddAsync(Emotion emotion)
        {
            _context.Emotions.Add(emotion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Emotion emotion)
        {
            _context.Emotions.Update(emotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var emotion = await GetByIdAsync(id);
            if (emotion != null)
            {
                _context.Emotions.Remove(emotion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Emotion>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Emotions
                .Where(e => e.UserId == userId && e.StateDate >= startDate && e.StateDate <= endDate)
                .Include(e => e.EmotionNotes)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emotion>> GetByTypeAsync(Guid userId, string emotionType)
        {
            return await _context.Emotions
                .Where(e => e.UserId == userId && e.EmotionType == emotionType)
                .Include(e => e.EmotionNotes)
                .ToListAsync();
        }
    }
}

