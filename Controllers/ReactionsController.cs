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
            var userId = _userManager.GetUserId(User);

            var reaction = new Reaction
            {
                UserID = userId,
                ReactableID = postId,
                ReactableType = ReactableType.Post,
                Value = 1
            };

            await _reactionService.AddOrUpdateReactionAsync(reaction, postId);

            var post = await _postService.GetPostByIdAsync(postId);

            return Json(new { likeCount = post.LikeCount, dislikeCount = post.DislikeCount});
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int postId)
        {
            var userId = _userManager.GetUserId(User);

            var reaction = new Reaction
            {
                UserID = userId,
                ReactableID = postId,
                ReactableType = ReactableType.Post,
                Value = -1
            };

            await _reactionService.AddOrUpdateReactionAsync(reaction, postId);
            var post = await _postService.GetPostByIdAsync(postId);

            return Json(new { likeCount = post.LikeCount, dislikeCount = post.DislikeCount });
        }

    }
}
