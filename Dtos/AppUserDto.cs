using System.Collections.Generic;
using API.Entities;

namespace API.Dtos
{
    public class AppUserDto
    {
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string Education { get; set; }
        public string Work { get; set; }
        public string Address { get; set; }  
        public List<PhotoDto> Photos { get; set; }
        public string PPUrl { get; set; }
        public string CoverUrl { get; set; }
        public bool Following { get; set; }
        public int FollowingCount { get; set; }
        public int FollowersCount { get; set; }
        
    }
}