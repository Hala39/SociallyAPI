namespace API.Entities
{
    public class UserFollowing
    {
        public string ObserverId { get; set; }
        virtual public AppUser Observer { get; set; }
        public string TargetId { get; set; }
        virtual public AppUser Target { get; set; }
    }
}