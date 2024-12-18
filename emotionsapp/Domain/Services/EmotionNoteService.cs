using EmotionsApp.Domain.Interfaces;
using EmotionsApp.Domain.Entities;

namespace EmotionsApp.Domain.Services
{
    public class EmotionNoteService : IEmotionNoteService
    {
        private readonly IEmotionNoteRepository _emotionNoteRepository;

        public EmotionNoteService(IEmotionNoteRepository emotionNoteRepository)
        {
            _emotionNoteRepository = emotionNoteRepository;
        }

        public async Task<IEnumerable<EmotionNote>> GetNotesByEmotionAsync(Guid emotionId)
        {
            return await _emotionNoteRepository.GetByEmotionIdAsync(emotionId);
        }

        public async Task AddNoteAsync(Guid emotionId, string noteText)
        {
            var emotionNote = new EmotionNote
            {
                Id = Guid.NewGuid(),
                EmotionId = emotionId,
                NoteText = noteText
            };

            await _emotionNoteRepository.AddAsync(emotionNote);
        }

        public async Task UpdateNoteAsync(Guid noteId, string noteText)
        {
            var note = await _emotionNoteRepository.GetByIdAsync(noteId);
            if (note == null)
            {
                throw new Exception("Заметки не найдены");
            }

            note.NoteText = noteText;

            await _emotionNoteRepository.UpdateAsync(note);
        }

        public async Task DeleteNoteAsync(Guid noteId)
        {
            await _emotionNoteRepository.DeleteAsync(noteId);
        }
    }
}

