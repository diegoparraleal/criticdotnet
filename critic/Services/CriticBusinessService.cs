using Critic.Data;
using Critic.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Critic.Services
{
    public class CriticBusinessService
    {
        private CriticDbContext _context;
        private const int REVIEWS_PER_PAGE = 5;

        public CriticBusinessService(CriticDbContext context)
        {
            _context = context;
        }

        #region User methods

        public async Task<AppUser> FindUser(int? userId)
        {
            if (userId == null) return null;
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == userId);
            if (user == null) return null;
            return user;
        }

        public async Task<AppUser> FindUserByEmail(string email)
        {
            if (email == null) return null;
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == email);
            if (user == null) return null;
            return user;
        }

        public AppUser FindUserByEmailSync(string email)
        {
            if (email == null) return null;
            var user = _context.Users.FirstOrDefault(m => m.Email == email);
            if (user == null) return null;
            return user;
        }
        #endregion

        #region Restaurant methods
        public async Task<Restaurant> FindRestaurant(int? restaurantId)
        {
            if (restaurantId == null) return null;
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(m => m.Id == restaurantId);
            if (restaurant == null) return null;
            return restaurant;
        }

        public async Task<List<Review>> FindRestaurantReviews(int? restaurantId, int page = 0, int reviewsPerPage = REVIEWS_PER_PAGE)
        {
            if (restaurantId == null) return null;
            var result = await _context.Reviews.Where(r => r.RestaurantID == restaurantId)
                                                .OrderByDescending(r => r.Date)
                                                .Skip(page * REVIEWS_PER_PAGE)
                                                .Take(reviewsPerPage)
                                                .ToListAsync();

            await AddReplyAndUserImages(result);

            return result;
        }

        public async Task<IEnumerable<Review>> AddReplyAndUserImages(IEnumerable<Review> reviews) {
            foreach (var review in reviews)
            {
                await AddReplyInfo(review);
                await AddUserImage(review);
            }
            return reviews;
        }

        public async Task<IEnumerable<ReviewWithRestaurant>> AddReplyAndUserImages(IEnumerable<ReviewWithRestaurant> reviewsWithRestaurant)
        {
            foreach (var item in reviewsWithRestaurant)
            {
                await AddReplyInfo(item.Review);
                await AddUserImage(item.Review);
            }
            return reviewsWithRestaurant;
        }


        public async Task<Review> FindRestaurantBestReview(Restaurant restaurant)
        {
            if (restaurant == null) return null;
            var result = await _context.Reviews.Where(r => r.RestaurantID == restaurant.Id)
                                               .OrderByDescending(r => r.Rating)
                                               .FirstOrDefaultAsync();
            if (result != null)
            {
                await AddUserImage(result);
                await AddReplyInfo(result);
            }

            return result;
        }

        public async Task<Review> FindRestaurantWorstReview(Restaurant restaurant)
        {
            if (restaurant == null) return null;
            var result = await _context.Reviews.Where(r => r.RestaurantID == restaurant.Id)
                                               .OrderBy(r => r.Rating)
                                               .FirstOrDefaultAsync();
            if (result != null)
            {
                await AddUserImage(result);
                await AddReplyInfo(result);
            }

            return result;
        }

        public async Task<IEnumerable<ReviewWithRestaurant>> FindPendingReviews(int? ownerId)
        {
            var result = await _context.Reviews.Join(_context.Restaurants,
                                                             review => review.RestaurantID,
                                                             restaurant => restaurant.Id,
                                                             (review, restaurant) => new ReviewWithRestaurant { Review = review, Restaurant = restaurant})
                                                       .Where(o => o.Review.ReplyID == null && o.Restaurant.OwnerID == ownerId)
                                                       .ToListAsync();

            await AddReplyAndUserImages(result);
            return result;
        }

        public async Task AddReplyInfo(Review review)
        {
            if (review.ReplyID != null) 
            { 
                review.Reply = await FindReply(review.ReplyID);
                if (review?.Reply?.UserID != null) {
                    review.Reply.User = await FindUser(review.Reply.UserID);
                    review.Reply.UserImage = review.Reply.User.Image;
                }
            }
        }

        public async Task AddUserImage(Review review)
        {
            if (review.UserID != null)
            {
                review.User = await FindUser(review.UserID);
                review.UserImage = review.User.Image;
            }
        }
        #endregion

        #region Review methods
        public async Task<Review> FindReview(int? restaurantId, int? reviewId)
        {
            if (reviewId == null) return null;
            var review = await _context.Reviews.FirstOrDefaultAsync(m => m.Id == reviewId);
            if (review == null) return null;
            if (review.RestaurantID != restaurantId) return null;
            return review;
        }

        public async Task<Reply> FindReply(int? replyId)
        {
            if (replyId == null) return null;
            var reply = await _context.Replies.FirstOrDefaultAsync(m => m.Id == replyId);
            if (replyId == null) return null;
            return reply;
        }

        public async Task ReCalculateRestaurantRating(int? restaurantId)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == restaurantId);
            var average = await _context.Reviews.Where(r => r.RestaurantID == restaurantId).AverageAsync(r => r.Rating);
            restaurant.Rating = average;
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
