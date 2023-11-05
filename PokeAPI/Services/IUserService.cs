using PokeAPI.Models;

namespace PokeAPI.Services
{
    public interface IUserService
    {
        Task<User?> GetUser(string username, string password);
        Task<User?> GetUserInfo(string username);
    }
}
