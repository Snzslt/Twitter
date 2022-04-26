using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Twitter.Data.Model;

namespace Twitter.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Comment>().HasOne(x => x.Tweet).WithMany(x => x.Comments).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<TweetLike>().HasOne(x => x.Tweet).WithMany(x => x.TweetLikes).OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TweetLike> TweetLikes { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Blocked> Blockeds { get; set; }
    }
}