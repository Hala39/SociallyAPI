using System.Collections.Generic;

namespace API.Entities
{
    public class Like
    {
        virtual public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        virtual public Photo Photo { get; set; }
        public int PhotoId { get; set; }
        
    }
}