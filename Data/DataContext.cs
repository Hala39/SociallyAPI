using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<UserFollowing> UserFollowings  {get; set; }
        public DbSet<Message> Messages { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Photo)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserFollowing>(b =>
            {
                b.HasKey(k => new { k.ObserverId, k.TargetId });

                b.HasOne(o => o.Observer)
                    .WithMany(f => f.Followings)
                    .HasForeignKey(o => o.ObserverId)
                    .OnDelete(DeleteBehavior.NoAction);

                b.HasOne(o => o.Target)
                    .WithMany(f => f.Followers)
                    .HasForeignKey(o => o.TargetId)
                    .OnDelete(DeleteBehavior.NoAction);

            });

            modelBuilder.Entity<Like>(b => {
                b.HasKey(k => new { k.AppUserId, k.PhotoId });

                b.HasOne(u => u.AppUser)
                    .WithMany(l => l.LikedPhotos)
                    .HasForeignKey(u => u.AppUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                b.HasOne(p => p.Photo)
                    .WithMany(l => l.Likers)
                    .HasForeignKey(p => p.PhotoId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
                
            modelBuilder.Entity<Message>()
                .HasOne(r => r.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(s => s.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}