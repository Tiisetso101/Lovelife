using System.Collections.Generic;
using System.Threading.Tasks;
using LoveLife.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LoveLife.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void add<T>(T entity) where T : class
        {
             _context.Add(entity);
        }

        public void delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
           var user = await _context.Users.Include(p => p.Photo).FirstOrDefaultAsync(u => u.Id == id);
           return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Photo).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
           return await _context.SaveChangesAsync() > 0;
        }
    }
}