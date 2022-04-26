namespace Twitter.Data.Model;

public class TweetLike : BaseModel
{

    public int TweetId { get; set; }
    public Tweet Tweet { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}