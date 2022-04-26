using Mapster;
using Microsoft.AspNetCore.Http;
using Twitter.Data.Model;

namespace Twitter.ViewModels;

public class ApplicationUserEditViewModel
{

    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Bio { get; set; }
    public string Email { get; set; }

    [AdaptIgnore]
    public IFormFile? Avatar { get; set; }

    [AdaptIgnore]
    public IFormFile? Header { get; set; }



}