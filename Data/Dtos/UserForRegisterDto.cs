using System.ComponentModel.DataAnnotations;

namespace LoveLife.API.Data.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string username { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength = 4 ,ErrorMessage= "Your password must be at least 4 characters long")]
        public string password { get; set; }
    }
}