using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser
    {
        public string Bio { get; set; }
        public string Education { get; set; }
        public string Work { get; set; }
        public string Address { get; set; }
        virtual public ICollection<UserFollowing> Followings { get; set; }
        virtual public ICollection<UserFollowing> Followers { get; set; }
        virtual public List<Photo> Photos { get; set; }
        virtual public ICollection<Like> LikedPhotos { get; set; }
        virtual public ICollection<Message> MessagesReceived { get; set; }
        virtual public ICollection<Message> MessagesSent { get; set; }
    }
}