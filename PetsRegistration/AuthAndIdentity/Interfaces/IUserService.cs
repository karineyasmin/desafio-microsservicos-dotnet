using AuthAndIdentity.Models;

namespace AuthAndIdentity.Interfaces;
public interface IUserService
{
    Task<User> AuthenticateAsync(string username, string password);
    Task RegisterAsync(string username, string password, string role);
}
