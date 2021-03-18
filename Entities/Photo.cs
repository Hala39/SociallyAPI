using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public bool IsCover { get; set; }
        public string PublicId { get; set; }
        public string UserId { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        virtual public AppUser AppUser { get; set; }
        virtual public ICollection<Comment> Comments { get; set; }
    }
}