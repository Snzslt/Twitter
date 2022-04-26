using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twitter.Common;
using Twitter.Data;
using Twitter.Data.Model;
using Twitter.ViewModels;

namespace Twitter.Controllers
{
    [Authorize]
    public class TweetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TweetController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public IActionResult Index(int tweetId)
        {
            var currentUserId = User.Identity.GetUserId();
            var tweet = _context.Tweets
                .AsNoTracking()
                .Include(x => x.TweetLikes)
                .Include(x => x.ApplicationUser)
                .Include(x => x.Comments)
                .ThenInclude(x => x.ApplicationUser)
                .FirstOrDefault(x => x.Id == tweetId);
            var model = new TweetViewModel()
            {
                ApplicationUserId = tweet.ApplicationUserId,
                DateTime = tweet.DateTime,
                Comments = tweet.Comments.ToList(),
                ApplicationUser = tweet.ApplicationUser,
                Id = tweet.Id,
                Image1 = tweet.Image1,
                Image2 = tweet.Image2,
                Image3 = tweet.Image3,
                Image4 = tweet.Image4,
                ParentReTweetId = tweet.ParentReTweetId,
                Text = tweet.Text,
                TweetLikes = tweet.TweetLikes.ToList(),
                CurrentUserLiked = tweet.TweetLikes.Any(x => x.ApplicationUserId == currentUserId),
            };
            if (model.ParentReTweetId is not null)
            {
                var reTweet = _context.Tweets
                    .AsNoTracking()
                    .Include(x => x.TweetLikes)
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.Comments)
                    .ThenInclude(x => x.ApplicationUser)
                    .FirstOrDefault(x => x.Id == model.ParentReTweetId);
                if (reTweet != null)
                {
                    model.ParentReTweet = new TweetViewModel()
                    {
                        ApplicationUserId = reTweet.ApplicationUserId,
                        ApplicationUser = reTweet.ApplicationUser,
                        DateTime = reTweet.DateTime,
                        Id = reTweet.Id,
                        Text = reTweet.Text,
                    };
                }
                else
                {
                    model.ParentReTweetId = null;
                }

            }
            return View(model);
        }



        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = _context.Comments.FirstOrDefault(x => x.Id == commentId);
            if (comment is not null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { tweetId = comment.TweetId });
        }


        public async Task<IActionResult> Like(int tweetId)
        {
            var currentUserId = User.Identity.GetUserId();
            _context.TweetLikes.Add(new TweetLike()
            {
                ApplicationUserId = User.Identity.GetUserId(),
                TweetId = tweetId
            });
            await _context.SaveChangesAsync();
            return Ok();
        }
        public async Task<IActionResult> UnLike(int tweetId)
        {
            var currentUserId = User.Identity.GetUserId();

            var item = _context.TweetLikes.FirstOrDefault(x => x.TweetId == tweetId && x.ApplicationUserId == currentUserId);
            if (item is not null)
            {
                _context.TweetLikes.Remove(item);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        public async Task<IActionResult> ReTweet(int tweetId)
        {
            var currentUserId = User.Identity.GetUserId();
            var tweet = new Tweet()
            {
                ApplicationUserId = currentUserId,
                DateTime = DateTime.Now,
                ParentReTweetId = tweetId,
                Text = "-",
            };
            await _context.AddAsync(tweet);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { tweetId = tweetId });
        }
    }
}
