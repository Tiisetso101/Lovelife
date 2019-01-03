using System.Collections.Generic;
using System.Threading.Tasks;
using LoveLife.API.Models;

namespace LoveLife.API.Data
{
    public interface IDatingRepository
    {
         void add <T> (T entity) where T: class;

         void delete <T> (T entity) where T:class;

         Task <bool>  SaveAll();

         Task <IEnumerable <User>> GetUsers();
         
         Task <User> GetUser(int id); 
         Task <Photos> GetPhoto(int id);
        
    }
}