namespace Twitter.Data.Model
{
    public class Tweet : BaseModel
    {
        public string Text { get; set; }

        public DateTime DateTime { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }

        public int? ParentReTweetId { get; set; }


        public ICollection<Comment> Comments { get; set; }
        public ICollection<TweetLike> TweetLikes { get; set; }
    }
}
