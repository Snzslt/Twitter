namespace Twitter.ViewModels;

public class AddCommentViewModel
{
    public string Text { get; set; }

    public IFormFile? Image { get; set; }

    public int TweetId { get; set; }
}