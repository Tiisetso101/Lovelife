using System;

namespace LoveLife.API.Dtos
{
    public class MessageForCreationDto
    {
        public int senderId { get; set; }
        public int recipientId { get; set; }

        public DateTime  MessageSent { get; set; }

        public string Content { get; set; }
        public MessageForCreationDto()
        {
            this.MessageSent = DateTime.Now;
        }
    }
}