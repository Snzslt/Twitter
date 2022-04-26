using Microsoft.AspNetCore.Identity;

namespace Twitter.Data.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Bio { get; set; }

        public string? Avatar { get; set; }
        public string? Header { get; set; }

        public ICollection<Tweet> Tweets { get; set; }
        public ICollection<TweetLike> TweetLikes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
