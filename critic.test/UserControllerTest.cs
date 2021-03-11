using Critic.Controllers;
using Critic.Data;
using Critic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Critic.Test.Util;
using Critic.Services;

namespace Critic.Test
{
    [TestClass]
    public class UserControllerTest: BaseControllerTest
    {

        [TestMethod]
        public async Task TestGetUsers()
        {
            // Arrange
            var data = GetUsers();
            var mockContext = GetDbContext(users: data);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new UserController(mockContext.Object, businessService);
            var response = await service.GetUsers() as OkObjectResult;
            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(data, response.Value);
        }


        [TestMethod]
        public async Task TestCreateUser()
        {
            // Arrange
            var user = new AppUser { Id = 4, Email = "newuser@gmail.com", Role = AppUser.Roles.User };
            var data = GetUsers();
            var mockContext = GetDbContext(users: data);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new UserController(mockContext.Object, businessService);
            var response = await service.CreateUser(user) as OkObjectResult;
            var allUsers = await service.GetUsers() as OkObjectResult;

            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(user, response.Value);
            Assert.That.AreJsonEquals(data, allUsers.Value);
        }

        [TestMethod]
        public async Task TestGetUser()
        {
            // Arrange
            var data = GetUsers();
            var mockContext = GetDbContext(users: data);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new UserController(mockContext.Object, businessService);
            var response = await service.GetUser(1) as OkObjectResult;
            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(data[0], response.Value);
        }

        [TestMethod]
        public async Task TestEditUser()
        {
            // Arrange
            var newUser = new AppUser { Id = 2, Email = "usermodified@gmail.com", Role = AppUser.Roles.Owner };
            var data = GetUsers();
            var mockContext = GetDbContext(users: data);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new UserController(mockContext.Object, businessService);
            var response = await service.EditUser(2, newUser) as OkObjectResult;
            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(newUser, response.Value);
        }

        [TestMethod]
        public async Task TestDeleteUser()
        {
            // Arrange
            var data = GetUsers();
            var mockContext = GetDbContext(users: data);
            var businessService = new CriticBusinessService(mockContext.Object);

            // Act
            var service = new UserController(mockContext.Object, businessService);
            var response = await service.DeleteUser(2) as OkObjectResult;
            var allUsers = await service.GetUsers() as OkObjectResult;

            // Assert
            Assert.IsNotNull(response);
            Assert.That.AreJsonEquals(data, allUsers.Value);
        }
    }
}
