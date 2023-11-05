using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokeAPI.Models;
using PokeAPI.Services.DbContexts;

namespace PokeAPI.Services
{
    public class UsersService : IUserService
    {
        private PokeDbContext pokeDb;

        public UsersService([FromServices] PokeDbContext pokeDb)
        {
            this.pokeDb = pokeDb;
        }

        public async Task<User?> GetUserInfo(string username)
        {
            var user = await pokeDb.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));
            return user;
        }

        public async Task<User?> GetUser(string username, string password)
        {
            var user = await pokeDb.Users.FirstOrDefaultAsync(u => u.Username.Equals(username) && u.Password.Equals(password));
            return user;
        }
    }
}
