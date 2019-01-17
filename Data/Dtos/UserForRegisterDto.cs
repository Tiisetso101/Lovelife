using System.ComponentModel.DataAnnotations;
using System;

namespace LoveLife.API.Data.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string username { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength = 4 ,ErrorMessage= "Your password must be at least 4 characters long")]
        public string password { get; set; }
        [Required]
        public string gender {get;set;}
        [Required]
        public string knownAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        
        public DateTime LastActive { get; set; }
        public DateTime Created { get; set; }

        public UserForRegisterDto () {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}