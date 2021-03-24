using System;

namespace API.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderUserName { get; set; }
        virtual public AppUser Sender { get; set; }
        public string RecipientId { get; set; }
        public string RecipientUserName { get; set; }
        virtual public AppUser Recipient { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        public string Content { get; set; }
        
    }
}