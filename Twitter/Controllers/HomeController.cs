using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Twitter.Common;
using Twitter.Data;
using Twitter.Data.Model;
using Twitter.Data.Service;
using Twitter.Models;
using Twitter.ViewModels;

namespace Twitter.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly TweetsService _tweetsService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment, TweetsService tweetsService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _tweetsService = tweetsService;
        }

        public IActionResult Index()
        {
            var model = new HomePageViewModel();
            var userId = User.Identity.GetUserId();
            model.CurrentUser = _dbContext.ApplicationUsers.FirstOrDefault(x => x.Id == userId);
            return View(model);
        }

        public async Task<IActionResult> GetTweets(int page = 1)
        {
            var model = await _tweetsService.GetTimeListTweets(User.Identity.GetUserId(), page);
            return PartialView("_Tweets", model);
        }

        public async Task<IActionResult> Search(string term)
        {
            var data = _dbContext.ApplicationUsers.Where(x => x.UserName.Contains(term));
            var model = new List<SearchResult>();

            foreach (var item in data)
            {
                model.Add(new SearchResult()
                {
                    Avatar = item.Avatar,
                    UserId = item.Id,
                    UserName = item.UserName,
                    DisplayName = $"{item.FirstName} {item.LastName}"
                });
            }
            return PartialView("_SearchResult", model);
        }


        public async Task<IActionResult> AddTweet(HomePageViewModel model)
        {
            var tweet = new Tweet
            {
                ApplicationUserId = User.Identity.GetUserId(),
                Text = model.AddTweetViewModel.Text,
                DateTime = DateTime.Now

            };
            await ProcessImages(model.AddTweetViewModel, tweet);
            _dbContext.Tweets.Add(tweet);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddComment(HomePageViewModel model)
        {
            var comment = new Comment()
            {
                ApplicationUserId = User.Identity.GetUserId(),
                Text = model.AddCommentViewModel.Text,
                DateTime = DateTime.Now,
                TweetId = model.AddCommentViewModel.TweetId,
            };
            if (model.AddCommentViewModel.Image is not null)
                comment.Image = await UploadFile(model.AddCommentViewModel.Image);
            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Tweet", new { tweetId = model.AddCommentViewModel.TweetId });
        }


        private async Task ProcessImages(AddTweetViewModel modelAddTweetViewModel, Tweet tweet)
        {
            if (modelAddTweetViewModel.Images?.Any() ?? false)
            {
                var files = modelAddTweetViewModel.Images.Take(4);

                foreach (IFormFile file in files)
                {
                    if (file.Length > 0)
                    {
                        SetImageUrl(tweet, await UploadFile(file));
                    }
                }

            }


        }
        private async Task<string> UploadFile(IFormFile file)
        {
            string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

            string strFileExtension = System.IO.Path.GetExtension(file.FileName);
            var fileName =
                $"{DateTime.Today.Ticks}-{new Random().Next(0000, 999999999)}.{strFileExtension}";
            string filePath = Path.Combine(uploads, fileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }
        private void SetImageUrl(Tweet tweet, string fileName)
        {
            if (tweet.Image1 is null)
            {
                tweet.Image1 = fileName;
                return;
            }

            if (tweet.Image2 is null)
            {
                tweet.Image2 = fileName;
                return;
            }

            if (tweet.Image3 is null)
            {
                tweet.Image3 = fileName;
                return;
            }

            if (tweet.Image4 is null)
            {
                tweet.Image4 = fileName;
                return;
            }
        }
    }
}