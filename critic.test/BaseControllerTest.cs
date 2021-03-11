using Critic.Data;
using Critic.Models;
using Critic.Test.Util;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Critic.Test
{
    public class BaseControllerTest
    {
        protected List<Restaurant> GetRestaurants()
        {
            return new List<Restaurant>
            {
                new Restaurant { Id = 1, City = "Bogota", Name = "Criterion", Price = 30, Rating = (decimal) 4.5, OwnerID = 2 },
                new Restaurant { Id = 2, City = "Bogota", Name = "Harry Sazon", Price = 25, Rating = (decimal) 4.1, OwnerID = 2 },
                new Restaurant { Id = 3, City = "Bogota", Name = "Leo", Price = 40, Rating = (decimal) 4.8, OwnerID = 2 },
            };
        }

        protected List<Review> GetReviews()
        {
            return new List<Review>
            {
                new Review { Id = 1, RestaurantID = 1, Comment = "Excellent!", Rating = 5, UserID = 3 },
                new Review { Id = 2, RestaurantID = 1, Comment = "Bad service!", Rating = 2, UserID = 3},
                new Review { Id = 3, RestaurantID = 1, Comment = "Was ok ...", Rating = (decimal) 3.5, UserID = 3, ReplyID = 1, Reply = new Reply { Id = 1, Comment = "Hope you can come back", UserID = 2, ReviewID = 3 }},
                new Review { Id = 4, RestaurantID = 2, Comment = "Average", Rating = 3, UserID = 3},
                new Review { Id = 5, RestaurantID = 3, Comment = "Average", Rating = 3, UserID = 3},
            };
        }

        protected List<AppUser> GetUsers()
        {
            return new List<AppUser>
            {
                new AppUser { Id = 1, Email = "user1@gmail.com", Image = null, Role = AppUser.Roles.Admin },
                new AppUser { Id = 2, Email = "user2@gmail.com", Image = null, Role = AppUser.Roles.Owner },
                new AppUser { Id = 3, Email = "user3@gmail.com", Image = null, Role = AppUser.Roles.User },
            };
        }
        protected List<Reply> GetReplies()
        {
            return new List<Reply>
            {
                new Reply { Id = 1, Comment = "Hope you can come back", UserID = 2, ReviewID = 3 },
            };
        }

        protected Mock<CriticDbContext> GetDbContext(List<Restaurant> restaurants = null, List<Review> reviews = null, List<Reply> replies = null, List<AppUser> users = null)
        {
            if (restaurants == null) restaurants = GetRestaurants();
            if (reviews == null) reviews = GetReviews();
            if (replies == null) replies = GetReplies();
            if (users == null) users = GetUsers();

            var userMockSet = users.AsQueryable().BuildMockDbSet();
            var restaurantsMockSet = restaurants.AsQueryable().BuildMockDbSet();
            var reviewsMockSet = reviews.AsQueryable().BuildMockDbSet();
            var replyMockSet = replies.AsQueryable().BuildMockDbSet();
            var mockContext = new Mock<CriticDbContext>();

            mockContext.Setup(c => c.Restaurants).Returns(restaurantsMockSet.Object);
            mockContext.Setup(c => c.Reviews).Returns(reviewsMockSet.Object);
            mockContext.Setup(c => c.Replies).Returns(replyMockSet.Object);
            mockContext.Setup(c => c.Users).Returns(userMockSet.Object);

            restaurantsMockSet.Setup(d => d.Add(It.IsAny<Restaurant>())).Callback<Restaurant>(s => restaurants.Add(s));
            restaurantsMockSet.Setup(d => d.Update(It.IsAny<Restaurant>())).Callback<Restaurant>(s => restaurants.ReplaceItem(s, u => u.Id));
            restaurantsMockSet.Setup(d => d.Remove(It.IsAny<Restaurant>())).Callback<Restaurant>(s => restaurants.Remove(s));

            reviewsMockSet.Setup(d => d.Add(It.IsAny<Review>())).Callback<Review>(s => reviews.Add(s));
            reviewsMockSet.Setup(d => d.Update(It.IsAny<Review>())).Callback<Review>(s => reviews.ReplaceItem(s, u => u.Id));
            reviewsMockSet.Setup(d => d.Remove(It.IsAny<Review>())).Callback<Review>(s => reviews.Remove(s));

            replyMockSet.Setup(d => d.Add(It.IsAny<Reply>())).Callback<Reply>(s => { s.Id = replies.Count +1;  replies.Add(s); });
            replyMockSet.Setup(d => d.Update(It.IsAny<Reply>())).Callback<Reply>(s => replies.ReplaceItem(s, u => u.Id));

            userMockSet.Setup(d => d.Add(It.IsAny<AppUser>())).Callback<AppUser>(s => users.Add(s));
            userMockSet.Setup(d => d.Update(It.IsAny<AppUser>())).Callback<AppUser>(s => users.ReplaceItem(s, u => u.Id));
            userMockSet.Setup(d => d.Remove(It.IsAny<AppUser>())).Callback<AppUser>(s => users.Remove(s));

            return mockContext;
        }

    }
}
