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
    public class RestaurantControllerTest: BaseControllerTest
    {

        
        [TestMethod]
        public async Task TestGetRestaurants()
        {
            // Arrange
            var data = GetRestaurants();
            var mockContext = GetDbContext(restaurants: data);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new RestaurantController(mockContext.Object, businessService);
            var response = await service.GetRestaurants() as OkObjectResult;
            
            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(data.OrderByDescending(r => r.Rating), response.Value);
        }

        [TestMethod]
        public async Task TestCreateRestaurant()
        {
            // Arrange
            var restaurant = new Restaurant { Id = 4, City = "Chia", Name = "Tierra Roja", Price = 20, Rating = (decimal)4.9 };
            var data = GetRestaurants();
            var mockContext = GetDbContext(restaurants: data);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new RestaurantController(mockContext.Object, businessService);
            var response = await service.CreateRestaurant(restaurant) as OkObjectResult;
            var allRestaurants = await service.GetRestaurants() as OkObjectResult;

            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(restaurant, (response.Value as RestaurantWithReviews).Restaurant);
            Assert.That.AreJsonEquals(data.OrderByDescending(r => r.Rating), allRestaurants.Value);
        }

        [TestMethod]
        public async Task TestGetRestaurant()
        {
            // Arrange
            var data = GetRestaurants();
            var reviews = GetReviews();
            var mockContext = GetDbContext(restaurants: data);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new RestaurantController(mockContext.Object, businessService);
            var response = await service.GetRestaurant(1) as OkObjectResult;

            // Assert
            var expected = new RestaurantWithReviews { Restaurant = data[0], BestReview = reviews[0], WorstReview = reviews[1], Reviews = reviews.Where(r => r.RestaurantID == 1).ToList() };
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(expected, response.Value);
        }

        [TestMethod]
        public async Task TestEditRestaurant()
        {
            // Arrange
            var newRestaurant = new Restaurant { Id = 2, City = "Bogota - modified", Name = "Harry Sazon - modified", Price = 30, Rating = (decimal)4.1, OwnerID = 2};
            var data = GetRestaurants();
            var mockContext = GetDbContext(restaurants: data);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new RestaurantController(mockContext.Object, businessService);
            var response = await service.EditRestaurant(2, newRestaurant) as OkObjectResult;
            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(newRestaurant, (response.Value as RestaurantWithReviews).Restaurant);
        }

        [TestMethod]
        public async Task TestDeleteRestaurant()
        {
            // Arrange
            var data = GetRestaurants();
            var mockContext = GetDbContext(restaurants: data);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new RestaurantController(mockContext.Object, businessService);
            var response = await service.DeleteRestaurant(2) as OkObjectResult;
            var allRestaurants = await service.GetRestaurants() as OkObjectResult;

            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(data.OrderByDescending(r => r.Rating), allRestaurants.Value);
        }
    }
 }
