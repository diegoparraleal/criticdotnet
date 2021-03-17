using Critic.Authorization;
using Critic.Data;
using Critic.Models;
using Critic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Critic.Controllers
{
    [Route("/api/1.0.0/")]

    public class ReviewController : Controller
    {
        private CriticDbContext _context;
        private CriticBusinessService _businessService;

        public ReviewController(CriticDbContext context, CriticBusinessService businessService)
        {
            _context = context;
            _businessService = businessService;
        }
        #region REST Methods

        /// <summary>
        /// Get all reviews from a restaurant
        /// </summary>
        /// <param name="restaurantId">ID of restaurant to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpGet]
        [Route("restaurants/{restaurantId}/reviews")]
        [GoogleAuthorize(AppUser.Roles.User)]
        public async Task<IActionResult> AddReview([FromRoute][Required] int? restaurantId, [FromQuery] int page = 0)
        {
            var reviews = await _businessService.FindRestaurantReviews(restaurantId, page);
            return Ok(reviews);
        }


        /// <summary>
        /// Post a review over a restaurant
        /// </summary>
        /// <param name="restaurantId">ID of restaurant to return</param>
        /// <param name="body"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpPost]
        [Route("restaurants/{restaurantId}/reviews")]
        [GoogleAuthorize(AppUser.Roles.User)]
        public async Task<IActionResult> AddReview([FromRoute][Required] int? restaurantId, [FromBody] Review review)
        {
            review.RestaurantID = restaurantId;
            review.Date = DateTime.Now;
            review.UserID = review.UserID;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            await _businessService.ReCalculateRestaurantRating(restaurantId);

            return await GetReview(restaurantId, review.Id);
        }

        /// <summary>
        /// Edit a review over a restaurant
        /// </summary>
        /// <param name="restaurantId">ID of restaurant to return</param>
        /// <param name="reviewId">ID of review to return</param>
        /// <param name="body"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpGet]
        [Route("restaurants/{restaurantId}/reviews/{reviewId}")]
        [GoogleAuthorize(AppUser.Roles.User, AppUser.Roles.Owner, AppUser.Roles.Admin)]
        public async Task<IActionResult> GetReview([FromRoute][Required] int? restaurantId, [FromRoute][Required] int? reviewId)
        {
            var review = await _businessService.FindReview(restaurantId, reviewId);
            if (review == null) return NotFound();
            if (review.ReplyID != null) review.Reply = await _businessService.FindReply(review.ReplyID);
            return Ok(review);
        }

        /// <summary>
        /// Edit a review over a restaurant
        /// </summary>
        /// <param name="restaurantId">ID of restaurant to return</param>
        /// <param name="reviewId">ID of review to return</param>
        /// <param name="body"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpPut]
        [Route("restaurants/{restaurantId}/reviews/{reviewId}")]
        [GoogleAuthorize(AppUser.Roles.Admin)]
        public async Task<IActionResult> EditReview([FromRoute][Required] int? restaurantId, [FromRoute][Required] int? reviewId, [FromBody] Review newReview)
        {
            var review = await _businessService.FindReview(restaurantId, reviewId);
            if (review == null) return NotFound();

            review.Comment = newReview.Comment;
            review.Rating = newReview.Rating;

            _context.Reviews.Update(review);

            if (newReview.Reply != null) {
                await ReplyReview(restaurantId, reviewId, newReview.Reply);
            }

            await _context.SaveChangesAsync();
            await _businessService.ReCalculateRestaurantRating(restaurantId);

            return await GetReview(restaurantId, review.Id);
        }

        /// <summary>
        /// Removes a review over a restaurant
        /// </summary>
        /// <param name="restaurantId">ID of restaurant to return</param>
        /// <param name="reviewId">ID of review to return</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpDelete]
        [Route("restaurants/{restaurantId}/reviews/{reviewId}")]
        [GoogleAuthorize(AppUser.Roles.Admin)]
        public async Task<IActionResult> DeleteReview([FromRoute][Required] int? restaurantId, [FromRoute][Required] int? reviewId)
        {
            var review = await _businessService.FindReview(restaurantId, reviewId);
            if (review == null) return NotFound();

            var replyId = review.ReplyID;
            _context.Reviews.Remove(review);
            if (replyId != null)
            {
                var reply = await _businessService.FindReply(replyId);
                _context.Replies.Remove(reply);
            }
            await _context.SaveChangesAsync();
            await _businessService.ReCalculateRestaurantRating(restaurantId);

            return Ok(null);
        }


        /// <summary>
        /// Get all pending reviews
        /// </summary>
        /// <response code="200">Successful operation</response>
        [HttpGet]
        [Route("reviews/pending")]
        [GoogleAuthorize(AppUser.Roles.Owner)]
        public async Task<IActionResult> GetPendingReviews([FromQuery] int? ownerId)
        {
            var pendingReviews = await _businessService.FindPendingReviews(ownerId);
            await _context.SaveChangesAsync();
            return Ok(pendingReviews);
        }


        /// <summary>
        /// Post or edit a reply over a review
        /// </summary>
        /// <param name="restaurantId">ID of restaurant to return</param>
        /// <param name="reviewId">ID of review to return</param>
        /// <param name="body"></param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpPost]
        [Route("restaurants/{restaurantId}/reviews/{reviewId}/reply")]
        [GoogleAuthorize(AppUser.Roles.Owner, AppUser.Roles.Admin)]
        public async Task<IActionResult> ReplyReview([FromRoute][Required] int? restaurantId, [FromRoute][Required] int? reviewId, [FromBody] Reply reply)
        {
            var review = await _businessService.FindReview(restaurantId, reviewId);
            if (review == null) return NotFound();

            reply.ReviewID = reviewId.Value;
            reply.Date = DateTime.Now;
            reply.UserID = reply.UserID;
            if (reply.Id == null)
                _context.Replies.Add(reply);
            else
                _context.Replies.Update(reply);

            await _context.SaveChangesAsync();

            review.ReplyID = reply.Id;
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();

            return await GetReview(restaurantId, review.Id);
        }
        #endregion

        #region Private methods
       
        #endregion

    }
}
