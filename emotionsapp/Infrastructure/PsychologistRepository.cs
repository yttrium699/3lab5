using EmotionsApp.Domain.Entities;
using EmotionsApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmotionsApp.Infrastructure
{
    public class PsychologistRepository : IPsychologistRepository
    {
        private readonly EmotionsAppContext _context;

        public PsychologistRepository(EmotionsAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Psychologist>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Psychologists
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Psychologist?> GetByIdAsync(Guid id)
        {
            return await _context.Psychologists
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Psychologist psychologist)
        {
            _context.Psychologists.Add(psychologist);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Psychologist psychologist)
        {
            _context.Psychologists.Update(psychologist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var psychologist = await GetByIdAsync(id);
            if (psychologist != null)
            {
                _context.Psychologists.Remove(psychologist);
                await _context.SaveChangesAsync();
            }
        }
    }
}
