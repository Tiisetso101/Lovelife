using System.Collections.Generic;
using System.Threading.Tasks;
using LoveLife.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public async Task<Photos> GetPhoto(int id)
        {
           var photo = await _context.Photo.FirstOrDefaultAsync(p => p.Id == id);
           return photo;
        }

        public async Task<Photos> GetMainPhotoForUser(int UserId)
        {

           return  await _context.Photo.Where(u => u.UserId == UserId).FirstOrDefaultAsync(p => p.IsMain);
            //return photo;
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