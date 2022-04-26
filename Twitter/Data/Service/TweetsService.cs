using Microsoft.EntityFrameworkCore;
using Twitter.Data.Model;
using Twitter.ViewModels;

namespace Twitter.Data.Service
{
    public class TweetsService
    {
        private readonly ApplicationDbContext _dbContext;

        public TweetsService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<TweetViewModel>> GetTimeListTweets(string userId, int page = 1)
        {
            var usersId = _dbContext.Follows.Where(x => x.ApplicationUserId == userId).Select(x => x.ToUserId).ToList();
            usersId.Add(userId);
            var data = await _dbContext.Tweets
                .AsNoTracking()
                .Include(x => x.ApplicationUser)
                .Include(x => x.TweetLikes)
                .Include(x => x.Comments)
                .Where(x => usersId.Contains(x.ApplicationUserId))
                .OrderByDescending(x => x.DateTime)
                .Skip(100 * (page - 1))
                .Take(100).ToListAsync();

            var model = new List<TweetViewModel>();
            foreach (var tweet in data)
            {
                var item = new TweetViewModel()
                {
                    Text = tweet.Text,
                    ParentReTweetId = tweet.ParentReTweetId,
                    Image1 = tweet.Image1,
                    ApplicationUserId = tweet.ApplicationUserId,
                    ApplicationUser = tweet.ApplicationUser,
                    Comments = tweet.Comments.ToList(),
                    DateTime = tweet.DateTime,
                    Id = tweet.Id,
                    Image2 = tweet.Image2,
                    Image3 = tweet.Image3,
                    Image4 = tweet.Image4,
                    TweetLikes = tweet.TweetLikes.ToList(),
                    CurrentUserLiked = tweet.TweetLikes.Any(x => x.ApplicationUserId == userId)

                };

                if (item.ParentReTweetId is not null)
                {
                    var reTweet = _dbContext.Tweets
                        .AsNoTracking()
                        .Include(x => x.TweetLikes)
                        .Include(x => x.ApplicationUser)
                        .Include(x => x.Comments)
                        .ThenInclude(x => x.ApplicationUser)
                        .FirstOrDefault(x => x.Id == item.ParentReTweetId);
                    if (reTweet != null)
                    {
                        item.ParentReTweet = new TweetViewModel()
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
                        item.ParentReTweetId = null;
                    }

                }
                model.Add(item);
            }
            return model;
        }

        public async Task<List<TweetViewModel>> GetUserListTweets(string userId, string currentUserId)
        {

            var data = await _dbContext.Tweets
                .AsNoTracking()
                .Include(x => x.ApplicationUser)
                .Include(x => x.TweetLikes)
                .Include(x => x.Comments)
                .Where(x => x.ApplicationUserId == userId)
                .OrderByDescending(x => x.DateTime).ToListAsync();

            var model = new List<TweetViewModel>();
            foreach (var tweet in data)
            {
                var item = new TweetViewModel()
                {
                    Text = tweet.Text,
                    ParentReTweetId = tweet.ParentReTweetId,
                    ApplicationUserId = tweet.ApplicationUserId,
                    ApplicationUser = tweet.ApplicationUser,
                    Comments = tweet.Comments.ToList(),
                    DateTime = tweet.DateTime,
                    Id = tweet.Id,
                    Image1 = tweet.Image1,
                    Image2 = tweet.Image2,
                    Image3 = tweet.Image3,
                    Image4 = tweet.Image4,
                    TweetLikes = tweet.TweetLikes.ToList(),
                    CurrentUserLiked = tweet.TweetLikes.Any(x => x.ApplicationUserId == userId)

                };

                if (item.ParentReTweetId is not null)
                {
                    var reTweet = _dbContext.Tweets
                        .AsNoTracking()
                        .Include(x => x.TweetLikes)
                        .Include(x => x.ApplicationUser)
                        .Include(x => x.Comments)
                        .ThenInclude(x => x.ApplicationUser)
                        .FirstOrDefault(x => x.Id == item.ParentReTweetId);
                    if (reTweet != null)
                    {
                        item.ParentReTweet = new TweetViewModel()
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
                        item.ParentReTweetId = null;
                    }

                }
                model.Add(item);
            }
            return model;
        }
    }
}
