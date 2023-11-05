using PokeAPI.Models;

namespace PokeAPI.Services
{
    public interface IFtpService
    {
        Task SaveMarkdownFile(string username, string password, Pokemon pokemon);
    }
}
