using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser
    {
        virtual public List<Photo> Photos { get; set; }
        public string Bio { get; set; }
        public string Education { get; set; }
        public string Work { get; set; }
        public string Address { get; set; }
        virtual public ICollection<UserFollowing> Followings { get; set; }
        virtual public ICollection<UserFollowing> Followers { get; set; }
        
    }
}