using System;

namespace LoveLife.API.Models
{
    public class Photos
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string  Description { get; set; }

        public DateTime DateAdded { get; set; }
        
        public bool IsMain { get; set; }
        public User user { get; set; }
        public int UserId { get; set; }
    }
}