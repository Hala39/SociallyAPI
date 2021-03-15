using System.Collections.Generic;
using API.Entities;

namespace API.Dtos
{
    public class AccountDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public List<Photo> Photos { get; set; }
        public string Education { get; set; }
        public string Bio { get; set; }
        public string Address { get; set; }
        public string Work { get; set; }
    }
}