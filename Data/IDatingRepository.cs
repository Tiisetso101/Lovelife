using System.Collections.Generic;
using System.Threading.Tasks;
using LoveLife.API.Controllers.Helpers;
using LoveLife.API.Models;

namespace LoveLife.API.Data
{
    public interface IDatingRepository
    {
         void add <T> (T entity) where T: class;

         void delete <T> (T entity) where T:class;

         Task <bool>  SaveAll();

         Task <PagedList <User>> GetUsers(UserParams userParams);
         
         Task <User> GetUser(int id); 
         Task <Photos> GetPhoto(int id);
         Task <Photos> GetMainPhotoForUser(int UserId);

         Task<Like> GetLike(int userID, int recipientID);
        
    }
}