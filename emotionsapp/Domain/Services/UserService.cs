using System.Security.Cryptography;
using System.Text;
using EmotionsApp.Domain.Entities;
using EmotionsApp.Domain.Interfaces;

namespace EmotionsApp.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateAsync(string login, string password)
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null) return null;

            var hash = ComputeHash(password);
            return user.PassHash == hash ? user : null;
        }

        public async Task RegisterUserAsync(string login, string password)
        {
            if (await _userRepository.GetByLoginAsync(login) != null)
            {
                throw new Exception("Пользователь с этим логином уже существует");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                PassHash = ComputeHash(password)
            };

            await _userRepository.AddAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdatePasswordAsync(Guid userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }

            user.PassHash = ComputeHash(newPassword);
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            if (!await _userRepository.ExistsAsync(id))
            {
                throw new Exception("Пользователь не найден");
            }

            await _userRepository.DeleteAsync(id);
        }

        private string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}

