using Critic.Controllers;
using Critic.Data;
using Critic.Models;
using Critic.Services;
using Critic.Test.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Critic.Test
{
    [TestClass]
    public class ReviewControllerTest : BaseControllerTest
    {

        

        [TestMethod]
        public async Task TestAddReview()
        {
            // Arrange
            var newReview = new Review { Id = 6, RestaurantID = 1, Comment = "Worst ever", Rating = 1, UserID = 3 };
            var restaurants = GetRestaurants();
            var reviews = GetReviews();
            var mockContext = GetDbContext(restaurants:restaurants, reviews: reviews);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new ReviewController(mockContext.Object, businessService);
            var restaurantService = new RestaurantController(mockContext.Object, businessService);
            var response = await service.AddReview(1, newReview) as OkObjectResult;
            var responseRestaurant = await restaurantService.GetRestaurant(1) as OkObjectResult;
            
            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(response.Value, newReview);
            Assert.That.AreJsonEquals((responseRestaurant.Value as RestaurantWithReviews).WorstReview, newReview);
            Assert.That.AreJsonEquals((responseRestaurant.Value as RestaurantWithReviews).Reviews.OrderBy(r => r.Id), reviews.Where(r => r.RestaurantID == 1));
            
            // Check also that there must be a new average
            Assert.That.AreJsonEquals((responseRestaurant.Value as RestaurantWithReviews).Restaurant.Rating, reviews.Where(r => r.RestaurantID == 1).Select(r => r.Rating).Average());
        }

        [TestMethod]
        public async Task TestGetReview()
        {
            // Arrange
            var restaurants = GetRestaurants();
            var reviews = GetReviews();
            var mockContext = GetDbContext(restaurants: restaurants, reviews: reviews);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new ReviewController(mockContext.Object, businessService);
            var response = await service.GetReview(1, 1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(response.Value, reviews[0]);
        }


        [TestMethod]
        public async Task TestEditReview()
        {
            // Arrange
            var newReview = new Review { Id = 1, RestaurantID = 1, Comment = "Excellent!", Rating = 5, UserID = 3 };
            var restaurants = GetRestaurants();
            var reviews = GetReviews();
            var mockContext = GetDbContext(restaurants: restaurants, reviews: reviews);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new ReviewController(mockContext.Object, businessService);
            var response = await service.EditReview(1, 1, newReview) as OkObjectResult;
            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull((response.Value as Review).Date != null);
            (response.Value as Review).Date = null;
            Assert.That.AreJsonEquals(response.Value, newReview);
        }

        [TestMethod]
        public async Task TestDeleteReview()
        {
            // Arrange
            var restaurants = GetRestaurants();
            var reviews = GetReviews();
            var mockContext = GetDbContext(restaurants: restaurants, reviews: reviews);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new ReviewController(mockContext.Object, businessService);
            var response = await service.DeleteReview(1, 1) as OkObjectResult;
            var restaurantService = new RestaurantController(mockContext.Object, businessService);
            var responseRestaurant = await restaurantService.GetRestaurant(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals((responseRestaurant.Value as RestaurantWithReviews).Reviews.OrderBy(r => r.Id), reviews.Where(r => r.RestaurantID == 1 && r.Id != 1));
            
            // Check also that there must be a new average
            Assert.That.AreJsonEquals((responseRestaurant.Value as RestaurantWithReviews).Restaurant.Rating, reviews.Where(r => r.RestaurantID == 1).Select(r => r.Rating).Average());
        }


        [TestMethod]
        public async Task TestGetPendingReviews()
        {
            // Arrange
            var restaurants = GetRestaurants();
            var reviews = GetReviews();
            var mockContext = GetDbContext(restaurants: restaurants, reviews: reviews);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new ReviewController(mockContext.Object, businessService);
            var response = await service.GetPendingReviews(2) as OkObjectResult;

            // Assert
            Assert.IsNotNull(response);
            var result = reviews.Join(restaurants,
                                      review => review.RestaurantID,
                                      restaurant => restaurant.Id,
                                      (review, restaurant) => new ReviewWithRestaurant { Review = review, Restaurant = restaurant })
                                .Where(o => o.Review.ReplyID == null && o.Restaurant.OwnerID == 2);

            Assert.That.AreJsonEquals(response.Value, result);
        }

        [TestMethod]
        public async Task TestReplyReview()
        {
            var reply = new Reply {ReviewID = 1, Comment = "Thank you for coming!", UserID = 2 };

            // Arrange
            var restaurants = GetRestaurants();
            var reviews = GetReviews();
            var replies = GetReplies();
            var mockContext = GetDbContext(restaurants: restaurants, reviews: reviews, replies: replies);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new ReviewController(mockContext.Object, businessService);
            var response = await service.ReplyReview(1, 1, reply) as OkObjectResult;

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull((response.Value as Review).Reply.Date);
            (response.Value as Review).Date = null;
            Assert.That.AreJsonEquals((response.Value as Review).Reply, reply);
        }

    }
 }
