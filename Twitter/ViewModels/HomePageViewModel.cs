using Microsoft.AspNetCore.Http;
using Twitter.Data.Model;

namespace Twitter.ViewModels
{
    public class HomePageViewModel
    {
        public AddTweetViewModel AddTweetViewModel { get; set; }
        public AddCommentViewModel AddCommentViewModel { get; set; }
        public ApplicationUser CurrentUser { get; set; }
    }
}
