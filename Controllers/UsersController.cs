using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SocialMedia.Models;
using SocialMedia.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;


        public UsersController(IUserService userService, UserManager<User> userManager, ILogger<UsersController> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            _logger.LogInformation("\nUser roles: {Roles}", string.Join(",", userRoles));
            if (!userRoles.Contains("admin"))
            {
                _logger.LogWarning("\n========================>User does not have admin role, redirecting...");
                return RedirectToAction("Index", "Posts");
            }
            else
            {
                _logger.LogWarning("=>>>>>>>>>>>>>>>Current user not found.");
            }
            
            return View(await _userService.GetAllUsers());
        }

        public async Task<IActionResult> Browse()
        {
            var users = await _userService.GetAllUsers();
            var userList = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                FullName = u.FullName
            }).ToList();
            return View(userList);
        }
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserByIdWithPostsAndFollowers(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.CreateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _userService.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _userService.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string id)
        {
            var currentUserId = _userManager.GetUserId(User);

            if(currentUserId == id)
            {
                return Json(new
                {
                    success = false, message = "Cannot follow yourself"
                });
            }

            var isFollowing = await _userService.IsFollowing(currentUserId, id);
            int followersCount = 0;

            if(!isFollowing)
            {
                followersCount = await _userService.FollowUser(currentUserId, id);
            }

            return Json(new
            {
                success = true,
                message = "User followed successfully",
                followersCount
            }) ;
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(string id)
        {
            var currentUserId = _userManager.GetUserId(User);

            var isFollowing = await _userService.IsFollowing(currentUserId, id);
            int followersCount = 0;
            if (isFollowing)
            {
                followersCount = await _userService.UnfollowUser(currentUserId, id);
            }

            return Json(new
            {
                success = true,
                message = "User unfollowed successfully",
                followersCount
            });
        }

        public async Task<IActionResult> Followers(string id)
        {
            var followers = await _userService.GetFollowers(id);
            return View(followers);
        }

        private async Task<bool> UserExists(string id)
        {
            return await _userService.UserExists(id);
        }
    }
}
