using EmotionsApp.Domain.Interfaces;
using EmotionsApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmotionsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmotionController : ControllerBase
    {
        private readonly IEmotionService _emotionService;

        public EmotionController(IEmotionService emotionService)
        {
            _emotionService = emotionService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var emotions = await _emotionService.GetEmotionsByUserAsync(userId);
            return Ok(emotions);
        }

        [HttpGet("{userId}/date-range")]
        public async Task<IActionResult> GetByDateRange(Guid userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var emotions = await _emotionService.GetEmotionsByDateRangeAsync(userId, startDate, endDate);
            return Ok(emotions);
        }

        [HttpGet("{userId}/type/{emotionType}")]
        public async Task<IActionResult> GetByType(Guid userId, string emotionType)
        {
            var emotions = await _emotionService.GetEmotionsByTypeAsync(userId, emotionType);
            return Ok(emotions);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmotion([FromBody] AddEmotionRequest request)
        {
            await _emotionService.AddEmotionAsync(request.UserId, request.EmotionType, request.StateDate);
            return Ok(new { message = "Эмоции успешно добавлены" });
        }

        [HttpPost("{emotionId}/add-note")]
        public async Task<IActionResult> AddEmotionNote(Guid emotionId, [FromBody] AddEmotionNoteRequest request)
        {
            try
            {
                await _emotionService.AddEmotionNoteAsync(emotionId, request.NoteText);
                return Ok(new { message = "Заметки добавлены успешно" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{emotionId}")]
        public async Task<IActionResult> DeleteEmotion(Guid emotionId)
        {
            await _emotionService.DeleteEmotionAsync(emotionId);
            return Ok(new { message = "Эмоции успешно удалены" });
        }
    }

    public class AddEmotionRequest
    {
        public Guid UserId { get; set; }
        public string EmotionType { get; set; }
        public DateTime StateDate { get; set; }
    }

    public class AddEmotionNoteRequest
    {
        public string NoteText { get; set; }
    }
}