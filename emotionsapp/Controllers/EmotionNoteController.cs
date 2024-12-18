using EmotionsApp.Domain.Entities;
using EmotionsApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmotionsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmotionNoteController : ControllerBase
    {
        private readonly IEmotionNoteService _emotionNoteService;
        private readonly ILogger<EmotionNoteController> _logger;

        public EmotionNoteController(IEmotionNoteService emotionNoteService, ILogger<EmotionNoteController> logger)
        {
            _emotionNoteService = emotionNoteService ?? throw new ArgumentNullException(nameof(emotionNoteService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{emotionId}")]
        public async Task<IActionResult> GetNotesByEmotion(Guid emotionId)
        {
            try
            {
                var notes = await _emotionNoteService.GetNotesByEmotionAsync(emotionId);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при получении заметок для emotionId: {EmotionId}", emotionId);
                return StatusCode(500, new { message = "При обработке вашего запроса произошла ошибка." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] AddNoteRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Запрос AddNote имеет значение null");
                return BadRequest(new { message = "Недопустимый запрос." });
            }

            try
            {
                await _emotionNoteService.AddNoteAsync(request.EmotionId, request.NoteText);
                return Ok(new { message = "Заметка успешно добавлена." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при добавлении заметки для emotionId: {EmotionId}", request.EmotionId);
                return StatusCode(500, new { message = "При обработке вашего запроса произошла ошибка." });
            }
        }

        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateNote(Guid noteId, [FromBody] UpdateNoteRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Значение запроса UpdateNote равно нулю");
                return BadRequest(new { message = "Недопустимый запрос" });
            }

            try
            {
                await _emotionNoteService.UpdateNoteAsync(noteId, request.NoteText);
                return Ok(new { message = "Заметка успешно обновлена." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "При обновлении NoteID произошла ошибка: {NoteId}", noteId);
                return StatusCode(500, new { message = "При обработке вашего запроса произошла ошибка" });
            }
        }

        [HttpDelete("{noteId}")]
        public async Task<IActionResult> DeleteNote(Guid noteId)
        {
            try
            {
                await _emotionNoteService.DeleteNoteAsync(noteId);
                return Ok(new { message = "Заметка успешно удалена." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "При удалении NoteID произошла ошибка: {NoteId}", noteId);
                return StatusCode(500, new { message = "При обработке вашего запроса произошла ошибка." });
            }
        }
    }

    public class AddNoteRequest
    {
        public Guid EmotionId { get; set; }
        public string NoteText { get; set; }
    }

    public class UpdateNoteRequest
    {
        public string NoteText { get; set; }
    }
}
