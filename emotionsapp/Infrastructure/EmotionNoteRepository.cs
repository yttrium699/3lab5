using EmotionsApp.Domain.Entities;
using EmotionsApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmotionsApp.Infrastructure
{
    public class EmotionNoteRepository : IEmotionNoteRepository
    {
        private readonly EmotionsAppContext _context;

        public EmotionNoteRepository(EmotionsAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmotionNote>> GetByEmotionIdAsync(Guid emotionId)
        {
            return await _context.EmotionNotes
                .Where(n => n.EmotionId == emotionId)
                .ToListAsync();
        }

        public async Task<EmotionNote?> GetByIdAsync(Guid id)
        {
            return await _context.EmotionNotes
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task AddAsync(EmotionNote emotionNote)
        {
            _context.EmotionNotes.Add(emotionNote);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmotionNote emotionNote)
        {
            _context.EmotionNotes.Update(emotionNote);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var note = await GetByIdAsync(id);
            if (note != null)
            {
                _context.EmotionNotes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }
    }
}
