using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Thoughtwave.Data;
using Thoughtwave.Models;

namespace Thoughtwave.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly IThoughtwaveRepository _repository;
        private readonly ILogger _logger;
        private readonly UserManager<User> _userManager;

        public CommentsController(IThoughtwaveRepository repository,
            UserManager<User> userManager,
            ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<CommentsController>();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/thoughts/{thoughtId}/comments")]
        public async Task<IActionResult> Create(int? thoughtId, string content,
            string returnUrl)
        {
            if (!thoughtId.HasValue)
            {
                return BadRequest("No thought ID was provided");
            }

            // no content for comment
            if (String.IsNullOrWhiteSpace(content))
            {
                TempData["error"] = "No comment content was provided";
                return Redirect(returnUrl);
            }

            // create new comment
            var newComment = new Comment()
            {
                Content = content,
                CreatedOn = DateTime.Now,
                User = await GetCurrentUserAsync()
            };
            
            // add comment
            var result = await _repository.AddCommentAsync(thoughtId.Value, newComment);

            if (result)
            {
                if (await _repository.CommitChangesAsync())
                {
                    TempData["success"] = "Your comment has been added!";
                    return Redirect(returnUrl);
                }
                else
                {
                    _logger.LogError($"Unable to save comment for thought with id {thoughtId}");
                    TempData["error"] = "An error occurred, please try again.";
                    return Redirect(returnUrl);
                }
            }

            // comment was not added 
            _logger.LogInformation($"Unable to save new comment for Thought with id {thoughtId}");
            TempData["error"] = "This thought does not exist";
            return Redirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/thoughts/{thoughtId}/comments/{commentId}/update")]
        public async Task<IActionResult> Update(int? thoughtId, int? commentId,
            string updatedContent, string returnUrl, string userName)
        {
            if (!thoughtId.HasValue || !commentId.HasValue)
            {
                return BadRequest("A thought or comment ID was not provided");
            }
            
            if (String.IsNullOrWhiteSpace(updatedContent))
            {
                TempData["error"] = "No comment content was provided";
                return Redirect(returnUrl);
            }

            var currentUser = await GetCurrentUserAsync();

            // current user is comment user
            if (currentUser.UserName == userName)
            {
                var comment = await _repository.GetCommentByIdAsync(commentId.Value);

                if (comment == null)
                {
                    _logger.LogInformation($"No comment found with id {commentId}");
                    NotFound();
                }

                // commnet found, update it
                comment.Content = updatedContent;
                comment.CreatedOn = DateTime.Now;
                
                if (await _repository.CommitChangesAsync())
                {
                    TempData["success"] = "Your comment has been successfully updated";
                    return Redirect(returnUrl);
                }
                else
                {
                    _logger.LogError($"Unable to update comment {commentId} for thought {thoughtId}");
                    TempData["error"] = "An error occurred updatng your comment, please try again";
                    return Redirect(returnUrl);
                }
            }

            // user is not comment User
            return Forbid();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/thoughts/{thoughtId}/comments/{commentId}/delete")]
        public async Task<IActionResult> Delete(int? thoughtId, int? commentId,
            string returnUrl, string userName)
        {
            if (!thoughtId.HasValue || !commentId.HasValue)
            {
                return BadRequest("A thought or comment ID was not provided");
            }

            var currentUser = await GetCurrentUserAsync();

            // current user is comment user
            if (currentUser.UserName == userName)
            {
                var comment = await _repository.GetCommentByIdAsync(commentId.Value);

                if (comment == null)
                {
                    _logger.LogInformation($"No comment found with id {commentId}");
                    NotFound();
                }

                // commnet found, remove it
                var result = await _repository.RemoveCommentAsync(thoughtId.Value, comment);

                if (result)
                {
                    if (await _repository.CommitChangesAsync())
                    {
                        TempData["success"] = "Your comment has been removed";
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        _logger.LogError($"Unable to remove comment {commentId} for thought {thoughtId}");
                        TempData["error"] = "An error occurred, please try again";
                        return Redirect(returnUrl);
                    }
                }

                // comment was not added 
                _logger.LogInformation($"Unable to find Thought with id {thoughtId}");
                TempData["error"] = "This thought does not exist";
                return Redirect(returnUrl);
            }

            // user is not comment User
            return Forbid();
        }


        /* ------- HELPER METHODS ---------- */

        private string GetThoughtUrl(Thought thought)
        {
            var category = thought.Category.ToString().ToLower();
            var id = thought.Id;
            var slug = thought.Slug.ToLower();
            return $"/{category}/{id}/{slug}";
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
