using EmotionsApp.Domain.Entities;

namespace EmotionsApp.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string login, string password);
        Task RegisterUserAsync(string login, string password);
        Task<User?> GetUserByIdAsync(Guid id);
        Task UpdatePasswordAsync(Guid userId, string newPassword);
        Task DeleteUserAsync(Guid id);
    }
}
