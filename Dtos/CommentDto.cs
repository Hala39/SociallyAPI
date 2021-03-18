using System;

namespace API.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        
    }
}