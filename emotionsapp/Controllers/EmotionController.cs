using EmotionsApp.Domain.Entities;
using EmotionsApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmotionsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmotionController : ControllerBase
    {
        private readonly IEmotionService _emotionService;
        private readonly ILogger<EmotionController> _logger;

        public EmotionController(IEmotionService emotionService, ILogger<EmotionController> logger)
        {
            _emotionService = emotionService ?? throw new ArgumentNullException(nameof(emotionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            try
            {
                var emotions = await _emotionService.GetEmotionsByUserAsync(userId);
                if (emotions == null || !emotions.Any())
                {
                    return NotFound(new { message = "Эмоции для указанного пользователя не найдены" });
                }
                return Ok(emotions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении эмоций для пользователя с ID: {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpGet("{userId}/date-range")]
        public async Task<IActionResult> GetByDateRange(Guid userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var emotions = await _emotionService.GetEmotionsByDateRangeAsync(userId, startDate, endDate);
                if (emotions == null || !emotions.Any())
                {
                    return NotFound(new { message = "Эмоции в указанном диапазоне не найдены" });
                }
                return Ok(emotions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении эмоций для пользователя с ID: {UserId} за период {StartDate} - {EndDate}", userId, startDate, endDate);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpGet("{userId}/type/{emotionType}")]
        public async Task<IActionResult> GetByType(Guid userId, string emotionType)
        {
            try
            {
                var emotions = await _emotionService.GetEmotionsByTypeAsync(userId, emotionType);
                if (emotions == null || !emotions.Any())
                {
                    return NotFound(new { message = "Эмоции указанного типа не найдены" });
                }
                return Ok(emotions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении эмоций типа {EmotionType} для пользователя с ID: {UserId}", emotionType, userId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmotion([FromBody] AddEmotionRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.EmotionType))
            {
                return BadRequest(new { message = "Некорректные параметры для добавления эмоции" });
            }

            try
            {
                await _emotionService.AddEmotionAsync(request.UserId, request.EmotionType, request.StateDate);
                return Ok(new { message = "Эмоции успешно добавлены" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении эмоции: {Request}", request);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpDelete("{emotionId}")]
        public async Task<IActionResult> DeleteEmotion(Guid emotionId)
        {
            try
            {
                await _emotionService.DeleteEmotionAsync(emotionId); // Выполняем удаление без проверки результата 
                return Ok(new { message = "Эмоция успешно удалена" }); // Всегда возвращаем успешный результат 
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка: передан некорректный идентификатор эмоции");
                return BadRequest(new { message = "Некорректный идентификатор эмоции" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении эмоции с ID: {EmotionId}", emotionId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Внутренняя ошибка сервера" });
            }
        }

        public class AddEmotionRequest
    {
        public Guid UserId { get; set; }
        public string EmotionType { get; set; }
        public DateTime StateDate { get; set; }
    }
}
}
