using System;
using System.Linq;
using System.Threading.Tasks;
using LoveLife.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LoveLife.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext _Context { get; }

        public AuthRepository(DataContext context)
        {
            _Context = context;

        }
        public async Task<User> Login(string username, string password)
        {
           var user = await _Context.Users.FirstOrDefaultAsync(x => x.UserName == username);
           if(user == null)
           return null;

           if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
           {
               return null;
           }
           return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
          {
              passwordSalt = hmac.Key;
              var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
              for(int i = 0; i < ComputeHash.Length; i++)
              {
                  if(ComputeHash[i] != passwordHash[i])
                  {
                      return false;
                  }
              }

          } ;
          return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt ;

            await _Context.Users.AddAsync(user);
            await _Context.SaveChangesAsync();

            return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
          using(var hmac = new System.Security.Cryptography.HMACSHA512())
          {
              passwordSalt = hmac.Key;
              passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
          } ;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _Context.Users.AnyAsync(x => x.UserName == username ))
            return true ;

            return false;
        }

        /* public Task<User> Login(User user, string password)
        {
            throw new NotImplementedException();
        }*/
    }
}