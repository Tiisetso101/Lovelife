using System;
using System.Collections.Generic;

namespace LoveLife.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte [] PasswordHash { get; set; }

        public byte [] PasswordSalt { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Introduction { get; set; }
        
        public string LookingFor { get; set; }

        public string Interest { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection <Photos> Photo { get; set; }

        public ICollection <Like>Liker { get; set; }
        
        public ICollection <Like> Likees { get; set; }

        public ICollection <Message> MessagesSent { get; set; }

        public ICollection<Message> MessageReceived { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalItems { get; set; }


    }
}