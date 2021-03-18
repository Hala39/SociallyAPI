using System;

namespace API.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        virtual public AppUser AppUser { get; set; }
        virtual public Photo Photo { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        
    }
}