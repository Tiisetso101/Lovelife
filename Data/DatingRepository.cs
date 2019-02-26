using System.Collections.Generic;
using System.Threading.Tasks;
using LoveLife.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using LoveLife.API.Controllers.Helpers;

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

        public async Task<Like> GetLike(int userID, int recipientID)
        {
            return await _context.Likes
            .FirstOrDefaultAsync(u => u.LikerId == userID && u.LikeeId == recipientID);
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

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users =  _context.Users.Include(p => p.Photo)
            .OrderByDescending(u => u.LastActive).AsQueryable();
            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.Gender == userParams.Gender);

            if(userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if(userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if(userParams.minAge != 18 || userParams.maxage != 99)
            {
                var minDOB = DateTime.Today.AddYears(-userParams.maxage -1);
                var maxDOB = DateTime.Today.AddYears(-userParams.minAge -1);
                users = users.Where(u => u.DateOfBirth >= minDOB && u.DateOfBirth <= maxDOB);
            } 

            if(!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case  "created":
                    users = users.OrderByDescending(u => u.Created);
                    break;
                    default: 
                    users = users.OrderByDescending(u => u.LastActive);
                    break;
                }
            }
            
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.pageSize);

        }

        private async Task<IEnumerable<int>> GetUserLikes (int id, bool likers)
        {
            var user = await _context.Users
            .Include(x => x.Liker)
            .Include(x => x.Likees)
            .FirstOrDefaultAsync(u => id == u.Id);

            if(likers)
            {
                return user.Liker.Where(u => u.LikeeId == id).Select(i => i.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);
            }
        }

        public async Task<bool> SaveAll()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id ==id);
        }

        public Task<PagedList<Message>> GetMessagesForUser()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            throw new NotImplementedException();
        }
    }
}