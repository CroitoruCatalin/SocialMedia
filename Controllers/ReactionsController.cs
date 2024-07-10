using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Services;
using static System.Collections.Specialized.BitVector32;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class ReactionsController : Controller
    {
        private readonly SocialContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IReactionService _reactionService;
        private readonly IPostService _postService;
        public ReactionsController(
            SocialContext context, 
            UserManager<User> userManager,
            IReactionService reactionService,
            IPostService postService
            )
        {
            _context = context;
            _userManager = userManager;
            _reactionService = reactionService;
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> Like(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            var post = await _postService.GetPostByIdAsync(postId);

            Console.WriteLine("EXECUTING POST LIKE ON postId: "+ postId);
            if (post == null)
            {
                return RedirectToAction("Home");
            }

            if (user != null)
            {

                Console.WriteLine("EXECUTING POST LIKE by UserID: " + user.Id + "\n\t and FullName: " + user.FullName);
                var reaction = new Reaction
                {
                    UserID = user.Id,
                    User = user,
                    ReactableID = postId,
                    ReactableType = ReactableType.Post,
                    Value = 1
                };

                await _reactionService.AddOrUpdateReactionAsync(reaction, post);

                return Json(new { likeCount = post.LikeCount, dislikeCount = post.DislikeCount });
            }

            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            var post = await _postService.GetPostByIdAsync(postId);
            if(post != null && user != null)
            {
                var reaction = new Reaction
                {
                    UserID = user.Id,
                    ReactableID = postId,
                    ReactableType = ReactableType.Post,
                    Value = -1
                };

                await _reactionService.AddOrUpdateReactionAsync(reaction, post);

                return Json(new { likeCount = post.LikeCount, dislikeCount = post.DislikeCount });
            }
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

    }
}
