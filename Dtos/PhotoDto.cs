

using System;

namespace API.Dtos
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }        
        public bool IsCover { get; set; }
        public DateTime Time  { get; set; }
        public bool Following { get; set; }
        
    }
}