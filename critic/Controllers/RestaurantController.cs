using Critic.Authorization;
using Critic.Data;
using Critic.Models;
using Critic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Critic.Controllers
{
    [Route("/api/1.0.0/restaurants")]
    public class RestaurantController : Controller
    {
        private CriticDbContext _context;
        private CriticBusinessService _businessService;
        private const int RESTAURANTS_PER_PAGE = 5;

        public RestaurantController(CriticDbContext context, CriticBusinessService businessService)
        {
            _context = context;
            _businessService = businessService;
        }

        #region REST Methods
        /// <summary>
        /// Get all available restaurants
        /// </summary>
        /// <param name="rating">Filter</param>
        /// <param name="name">Filter</param>
        /// <response code="200">Successful operation</response>
        [HttpGet]
        [Route("")]
        [GoogleAuthorize(AppUser.Roles.User, AppUser.Roles.Owner, AppUser.Roles.Admin)]

        public async Task<IActionResult> GetRestaurants([FromQuery] decimal? rating = 0, [FromQuery] string name = null, [FromQuery] int? ownerId = null,
                                                        [FromQuery] int page = 0)
        {
            var restaurants = await _context.Restaurants.Where(r => r.Rating >= rating && 
                                                                    (name != null ? r.Name.Contains(name) : true) &&
                                                                    (ownerId != null ? r.OwnerID == ownerId : true))
                                                        .OrderByDescending(r => r.Rating)
                                                        .Skip(page * RESTAURANTS_PER_PAGE)
                                                        .Take(RESTAURANTS_PER_PAGE)
                                                        .ToListAsync();
            return Ok(restaurants);
        }

        /// <summary>
        /// Creates a new restaurant
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Successful operation</response>
        [HttpPost]
        [Route("")]
        [GoogleAuthorize(AppUser.Roles.Owner)]
        public async Task<IActionResult> CreateRestaurant([FromBody] Restaurant restaurant)
        {
            restaurant.OwnerID = restaurant.OwnerID ?? 1;
            restaurant.Rating = 0;
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return await GetRestaurant(restaurant.Id);
        }


        /// <summary>
        /// Get details for a specific restaurant
        /// </summary>
        /// <param name="restaurantId">ID of restaurant to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpGet]
        [Route("{restaurantId}")]
        [GoogleAuthorize(AppUser.Roles.User, AppUser.Roles.Owner, AppUser.Roles.Admin)]
        public async Task<IActionResult> GetRestaurant([FromRoute][Required] int? restaurantId)
        {
            var restaurant  = await _businessService.FindRestaurant(restaurantId);
            if (restaurant == null) return NotFound();
            var reviews = await _businessService.FindRestaurantReviews(restaurant.Id);
            var bestReview = await _businessService.FindRestaurantBestReview(restaurant);
            var worstReview = await _businessService.FindRestaurantWorstReview(restaurant);
            return Ok(new RestaurantWithReviews { Restaurant = restaurant, BestReview = bestReview, WorstReview = worstReview, Reviews = reviews });
        }


        /// <summary>
        /// Edits a specific user
        /// </summary>
        /// <param name="restaurantId">ID of restaurant to return</param>
        /// <param name="body"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpPut]
        [Route("{restaurantId}")]
        [GoogleAuthorize(AppUser.Roles.Owner, AppUser.Roles.Admin)]
        public async Task<IActionResult> EditRestaurant([FromRoute][Required] int? restaurantId, [FromBody] Restaurant newRestaurant)
        {
            var restaurant = await _businessService.FindRestaurant(restaurantId);
            if (restaurant == null) return NotFound();

            restaurant.City = newRestaurant.City;
            restaurant.Image = newRestaurant.Image;
            restaurant.Name = newRestaurant.Name;
            restaurant.Description = newRestaurant.Description;
            restaurant.Price = newRestaurant.Price;
            // Rating & Owner cannot be changed

            _context.Restaurants.Update(restaurant);

            await _context.SaveChangesAsync();
            return await GetRestaurant(restaurant.Id);
        }

        /// <summary>
        /// Removes a specific user
        /// </summary>
        /// <param name="restaurantId">ID of restaurant to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpDelete]
        [Route("{restaurantId}")]
        [GoogleAuthorize(AppUser.Roles.Admin)]
        public async Task<IActionResult> DeleteRestaurant([FromRoute][Required] int? restaurantId)
        {
            var restaurant = await _businessService.FindRestaurant(restaurantId);
            if (restaurant == null) return NotFound();

            _context.Restaurants.Remove(restaurant);

            await _context.SaveChangesAsync();
            return Ok(null);
        }
        #endregion
    }
}
