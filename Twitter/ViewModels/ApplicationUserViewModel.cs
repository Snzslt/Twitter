using Twitter.Data.Model;

namespace Twitter.ViewModels;

public class ApplicationUserViewModel
{
    public ApplicationUserViewModel()
    {
        TweetViewModels = new List<TweetViewModel>();
    }
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Header { get; set; }
    public string Bio { get; set; }

    public List<TweetViewModel> TweetViewModels { get; set; }
    public int Followings { get; set; }
    public int Follwers { get; set; }

    public string? Avatar { get; set; }


    public bool CurrentUserFollowedThisUser { get; set; } = false;
    public bool CurrentUserBlockedThisUser { get; set; } = false;

}