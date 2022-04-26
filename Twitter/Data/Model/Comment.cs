namespace Twitter.Data.Model;

public class Comment : BaseModel
{
    public string Text { get; set; }

    public string? Image { get; set; }


    public int TweetId { get; set; }
    public Tweet Tweet { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public DateTime DateTime { get; set; }
}