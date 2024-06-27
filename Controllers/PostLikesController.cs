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

namespace SocialMedia.Controllers
{
    [Authorize]
    public class PostLikesController : Controller
    {
        private readonly SocialContext _context;
        private readonly IPostLikeService _postLikeService;
        private readonly UserManager<User> _userManager;
        public PostLikesController(SocialContext context, IPostLikeService postLikeService, UserManager<User> userManager)
        {
            _context = context;
            _postLikeService = postLikeService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Like(int postId)
        {
            var userId = _userManager.GetUserId(User);
            await _postLikeService.LikePost(postId, userId);

            var post = await _context.Posts.Include(p => p.Likes).FirstOrDefaultAsync(p => p.PostID == postId);
            var likeCount = post.Likes.Count(l => l.LikeValue == true);
            var dislikeCount = post.Likes.Count(l => l.LikeValue == false);

            return Json(new { likeCount, dislikeCount });
        }
        [HttpPost]
        public async Task<IActionResult> Dislike(int postId)
        {
            var userId = _userManager.GetUserId(User);
            await _postLikeService.DislikePost(postId, userId);

            var post = await _context.Posts.Include(p => p.Likes).FirstOrDefaultAsync(p => p.PostID == postId);
            var likeCount = post.Likes.Count(l => l.LikeValue == true);
            var dislikeCount = post.Likes.Count(l => l.LikeValue == false);

            return Json(new { likeCount, dislikeCount });
        }

    }
}
