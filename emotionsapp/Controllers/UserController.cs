using EmotionsApp.Domain.Interfaces;
using EmotionsApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmotionsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(request.Login, request.Password);
                if (user == null)
                {
                    _logger.LogWarning("Попытка аутентификации с неверным логином или паролем.");
                    return Unauthorized(new { message = "Неверный логин/пароль" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при аутентификации.");
                return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await _userService.RegisterUserAsync(request.Login, request.Password);
                return Ok(new { message = "Пользователь успешно зарегистрирован" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя.");
                return BadRequest(new { message = "Не удалось зарегистрировать пользователя" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("Запрос пользователя с несуществующим id: {UserId}", id);
                    return NotFound(new { message = "Пользователь не найден" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении пользователя.");
                return StatusCode(500, new { message = "Внутренняя ошибка сервера" });
            }
        }

        [HttpPut("{id}/update-password")]
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] UpdatePasswordRequest request)
        {
            try
            {
                await _userService.UpdatePasswordAsync(id, request.NewPassword);
                return Ok(new { message = "Пароль обновлен успешно" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении пароля для пользователя с id: {UserId}", id);
                return BadRequest(new { message = "Не удалось обновить пароль" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok(new { message = "Пользователь удален успешно" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении пользователя с id: {UserId}", id);
                return BadRequest(new { message = "Не удалось удалить пользователя" });
            }
        }
    }

    public class AuthenticateRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class UpdatePasswordRequest
    {
        public string NewPassword { get; set; }
    }
}
