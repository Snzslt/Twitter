using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twitter.Common;
using Twitter.Data;
using Twitter.Data.Model;
using Twitter.Data.Service;
using Twitter.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace Twitter.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TweetsService _tweetsService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(ApplicationDbContext context, TweetsService tweetsService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _tweetsService = tweetsService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Profile(string userId)
        {
            var currentUserId = User.Identity.GetUserId();
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == userId);
            var follwers = _context.Follows.Count(x => x.ToUserId == userId);
            var follwings = _context.Follows.Count(x => x.ApplicationUserId == userId);
            var model = new ApplicationUserViewModel()
            {
                Avatar = user.Avatar,
                Email = user.Email,
                FirstName = user.FirstName,
                Followings = follwings,
                Follwers = follwers,
                Id = user.Id,
                Header = user.Header,
                Bio = user.Bio,
                LastName = user.LastName,
                TweetViewModels = await _tweetsService.GetUserListTweets(userId, currentUserId),
                UserName = user.UserName,
                CurrentUserFollowedThisUser = _context.Follows.Any(x => x.ApplicationUserId == currentUserId && x.ToUserId == userId),
                CurrentUserBlockedThisUser = _context.Blockeds.Any(x => x.ApplicationUserId == currentUserId && x.ToUserId == userId)
            };

            return View(model);
        }
        public async Task<IActionResult> Followers(string userId)
        {

            var follwers = _context.Follows.AsNoTracking().Where(x => x.ToUserId == userId).ToList();
            var model = new List<SearchResult>();
            foreach (var follwer in follwers)
            {
                var fUser = _context.ApplicationUsers.FirstOrDefault(x => x.Id == follwer.ApplicationUserId);
                model.Add(new SearchResult()
                {
                    Avatar = fUser.Avatar,
                    DisplayName = $"{fUser.FirstName} {fUser.LastName}",
                    UserId = fUser.Id,
                    UserName = fUser.UserName,
                });
            }
            return View(model);
        }
        public async Task<IActionResult> Following(string userId)
        {

            var follwings = _context.Follows.Where(x => x.ApplicationUserId == userId).ToList();
            var model = new List<SearchResult>();
            foreach (var follwing in follwings)
            {
                var fUser = _context.ApplicationUsers.FirstOrDefault(x => x.Id == follwing.ToUserId);
                model.Add(new SearchResult()
                {
                    Avatar = fUser.Avatar,
                    DisplayName = $"{fUser.FirstName} {fUser.LastName}",
                    UserId = fUser.Id,
                    UserName = fUser.UserName,
                });
            }
            return View(model);
        }


        public async Task<IActionResult> DeleteTweet(int tweetId)
        {
            var tweet = _context.Tweets.FirstOrDefault(x => x.Id == tweetId);
            if (tweet is not null)
            {
                _context.Tweets.Remove(tweet);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profile", new { userId = User.Identity.GetUserId() });
        }

        public IActionResult EditProfile()
        {
            var currentUserId = User.Identity.GetUserId();
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == currentUserId);
            var model = user.Adapt<ApplicationUserEditViewModel>();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUserEditViewModel vm)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == vm.Id);
            if (user is null) return NotFound();

            if (vm.Avatar is not null)
            {
                user.Avatar = await UploadFile(vm.Avatar);
            }

            if (vm.Header is not null)
            {
                user.Header = await UploadFile(vm.Header);
            }

            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Bio = vm.Bio;
            user.FirstName = vm.FirstName;
            user.Email = vm.Email;
            user.UserName = vm.UserName;
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile", new { userId = vm.Id });
        }



        public async Task<IActionResult> Follow(string userId)
        {
            _context.Follows.Add(new Follow()
            {
                ApplicationUserId = User.Identity.GetUserId(),
                ToUserId = userId,
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile", new { userId = userId });
        }
        public async Task<IActionResult> UnFollow(string userId)
        {
            var currentUserId = User.Identity.GetUserId();

            var item = _context.Follows.FirstOrDefault(x => x.ApplicationUserId == currentUserId && x.ToUserId == userId);
            if (item is not null)
            {
                _context.Follows.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profile", new { userId = userId });
        }


        public async Task<IActionResult> Block(string userId)
        {
            _context.Blockeds.Add(new Blocked()
            {
                ApplicationUserId = User.Identity.GetUserId(),
                ToUserId = userId,
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile", new { userId = userId });
        }
        public async Task<IActionResult> UnBlock(string userId)
        {
            var currentUserId = User.Identity.GetUserId();

            var item = _context.Blockeds.FirstOrDefault(x => x.ApplicationUserId == currentUserId && x.ToUserId == userId);
            if (item is not null)
            {
                _context.Blockeds.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profile", new { userId = userId });
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
    }
}
