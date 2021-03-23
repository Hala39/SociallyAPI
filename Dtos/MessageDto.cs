using System;

namespace API.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string SenderUserName { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string RecipientUserName { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool RecipientDeleted { get; set; }
        public bool SenderDeleted { get; set; }
        public string Content { get; set; }

    }
}