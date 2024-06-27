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

        public async Task<IActionResult> Like(int postId)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var userId = _userManager.GetUserId(currentUser);
            await _postLikeService.LikePost(postId, userId);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Dislike(int postId)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var userId = _userManager.GetUserId(currentUser);
            await _postLikeService.DislikePost(postId, userId);
            return Redirect(Request.Headers["Referer"].ToString()); // Redirect back to the posts page
        }

    }
}
