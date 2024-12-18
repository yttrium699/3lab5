using EmotionsApp.Domain.Entities;
using EmotionsApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmotionsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PsychologistController : ControllerBase
    {
        private readonly IPsychologistService _psychologistService;
        private readonly ILogger<PsychologistController> _logger;

        public PsychologistController(IPsychologistService psychologistService, ILogger<PsychologistController> logger)
        {
            _psychologistService = psychologistService ?? throw new ArgumentNullException(nameof(psychologistService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            try
            {
                var psychologists = await _psychologistService.GetPsychologistsByUserAsync(userId);
                return Ok(psychologists);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка: передан некорректный идентификатор пользователя");
                return BadRequest("Некорректный идентификатор пользователя");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла неизвестная ошибка при получении психологов для пользователя");
                return StatusCode(500, "Внутренняя ошибка сервера. Пожалуйста, попробуйте позже.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPsychologist([FromBody] AddPsychologistRequest request)
        {
            try
            {
                await _psychologistService.AddPsychologistAsync(request.UserId, request.Name, request.ContactInfo);
                return Ok(new { message = "Психолог добавлен успешно!" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка: некорректные данные для добавления психолога");
                return BadRequest("Некорректные данные для добавления психолога");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при добавлении психолога");
                return StatusCode(500, "Внутренняя ошибка сервера. Пожалуйста, попробуйте позже.");
            }
        }

        [HttpPut("{psychologistId}")]
        public async Task<IActionResult> UpdatePsychologist(Guid psychologistId, [FromBody] UpdatePsychologistRequest request)
        {
            try
            {
                await _psychologistService.UpdatePsychologistAsync(psychologistId, request.Name, request.ContactInfo);
                return Ok(new { message = "Психолог обновлен успешно!" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка: некорректные данные для обновления психолога");
                return BadRequest("Некорректные данные для обновления психолога");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при обновлении психолога");
                return StatusCode(500, "Внутренняя ошибка сервера. Пожалуйста, попробуйте позже.");
            }
        }

        [HttpDelete("{psychologistId}")]
        public async Task<IActionResult> DeletePsychologist(Guid psychologistId)
        {
            try
            {
                await _psychologistService.DeletePsychologistAsync(psychologistId);
                return Ok(new { message = "Психолог удален успешно!" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка: передан некорректный идентификатор психолога");
                return BadRequest("Некорректный идентификатор психолога");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при удалении психолога");
                return StatusCode(500, "Внутренняя ошибка сервера. Пожалуйста, попробуйте позже.");
            }
        }
    }

    public class AddPsychologistRequest
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
    }

    public class UpdatePsychologistRequest
    {
        public string Name { get; set; }
        public string ContactInfo { get; set; }
    }
}
