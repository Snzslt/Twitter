using Twitter.Data.Model;

namespace Twitter.ViewModels
{

    public class TweetViewModel
    {

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }

        public int? ParentReTweetId { get; set; }
        public TweetViewModel? ParentReTweet { get; set; }

        public bool CurrentUserLiked { get; set; }
        public List<Comment> Comments { get; set; }
        public List<TweetLike> TweetLikes { get; set; }
    }
}
