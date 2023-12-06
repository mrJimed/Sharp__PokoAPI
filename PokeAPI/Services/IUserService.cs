using PokeAPI.Models;

namespace PokeAPI.Services
{
    public interface IUserService
    {
        User? GetUser(string email);
        bool IsUserExists(string email);
        void ChangePassword(string email, string newPassword);
        void AddUser(string email, string password, byte[] salt);
    }
}
