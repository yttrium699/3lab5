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
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Не найдено заметок для эмоции с ID: {EmotionId}", emotionId);
                return NotFound(new { message = "Заметки для указанной эмоции не найдены" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении заметок для эмоции с ID: {EmotionId}", emotionId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Внутренняя ошибка сервера, мы уже работаем над этим" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] AddNoteRequest request)
        {
            try
            {
                await _emotionNoteService.AddNoteAsync(request.EmotionId, request.NoteText);
                return Ok(new { message = "Заметки добавлены успешно" });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Ошибка при добавлении заметки: {Request}", request);
                return BadRequest(new { message = "Параметры для добавления заметки некорректны" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении заметки: {Request}", request);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Внутренняя ошибка сервера, мы уже работаем над этим" });
            }
        }

        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateNote(Guid noteId, [FromBody] UpdateNoteRequest request)
        {
            try
            {
                await _emotionNoteService.UpdateNoteAsync(noteId, request.NoteText);
                return Ok(new { message = "Заметки обновлены успешно" });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Ошибка при обновлении заметки с ID: {NoteId}", noteId);
                return NotFound(new { message = "Заметка с указанным ID не найдена" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении заметки с ID: {NoteId}", noteId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Внутренняя ошибка сервера, мы уже работаем над этим" });
            }
        }

        [HttpDelete("{noteId}")]
        public async Task<IActionResult> DeleteNote(Guid noteId)
        {
            try
            {
                await _emotionNoteService.DeleteNoteAsync(noteId);
                return Ok(new { message = "Заметки удалены успешно" });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Ошибка при удалении заметки с ID: {NoteId}", noteId);
                return NotFound(new { message = "Заметка с указанным ID не найдена" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении заметки с ID: {NoteId}", noteId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Внутренняя ошибка сервера, мы уже работаем над этим" });
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
