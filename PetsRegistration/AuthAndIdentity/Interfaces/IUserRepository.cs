using AuthAndIdentity.Models;

namespace AuthAndIdentity.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
    }
}