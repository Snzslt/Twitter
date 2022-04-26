namespace Twitter.ViewModels;

public class AddTweetViewModel
{
    public string Text { get; set; }

    public IFormFile[] Images { get; set; }

    public int? ParentReTweetId { get; set; }
}