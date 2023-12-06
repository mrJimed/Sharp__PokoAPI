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

        public User? GetUser(string email)
        {
            return pokeDb.Users.FirstOrDefault(user => user.Email.Equals(email));
        }

        public bool IsUserExists(string email)
        {
            var user = pokeDb.Users.FirstOrDefault(u => u.Email.Equals(email));
            return user != null;
        }

        public void AddUser(string email, string password, byte[] salt)
        {
            var user = new User() 
            { 
                Email = email, 
                Password = password, 
                Salt = Convert.ToBase64String(salt)
            };
            pokeDb.Users.Add(user);
            pokeDb.SaveChanges();
        }

        public void ChangePassword(string email, string newPassword)
        {
            var user = GetUser(email);
            user.Password = newPassword;
            pokeDb.SaveChanges();
        }
    }
}
