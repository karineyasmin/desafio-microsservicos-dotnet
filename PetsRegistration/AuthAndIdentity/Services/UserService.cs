using AuthAndIdentity.Interfaces;
using AuthAndIdentity.Models;
using System.Threading.Tasks;
using BCrypt.Net;

namespace AuthAndIdentity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }
            return user;
        }

        public async Task RegisterAsync(string username, string password, string role)
        {
            var user = new User
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role.ToLower() // Normaliza a string para minúsculas
            };
            await _userRepository.AddUserAsync(user);
        }
    }
}