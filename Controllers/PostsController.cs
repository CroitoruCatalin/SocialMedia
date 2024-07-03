using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Services.Interfaces;


namespace SocialMedia.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly SocialContext _context;
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly UserManager<User> _userManager;

        public PostsController(SocialContext context, IUserService userService, IPostService postService, UserManager<User> userManager)
        {
            _context = context;
            _userService = userService;
            _postService = postService;
            _userManager = userManager;
        }

        // GET: Posts
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            //var postIds = await _postService.GetPostRecommendationsAsync(userId);
            var postIds = await _postService.RecommendationAlgorithm(userId);

            return View(postIds);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostDetails(int id)
        {
            Post post = _postService.GetPostWithCommentsAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return PartialView("_Post", post);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _postService.GetPostWithCommentsAsync((int)id);

            if (post == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            ViewData["CurrentUserId"] = userId;

            return View(post);
        }

        // GET: Posts/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            var userId = _userManager.GetUserId(User);
            if (ModelState.IsValid && userId != null)
            {
                await _postService.CreatePostAsync(post, User);
                return RedirectToAction(nameof(Index));
            }

            foreach (var modelState in ModelState)
            {
                var key = modelState.Key;
                var errors = modelState.Value.Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                }
            }
            return View(post);
        }

        public async Task<IActionResult> AddComment(int postId)
        {
            var users = await _userService.GetAllUsers();
            var userSelectList = new SelectList(users, "UserID", "FullName");
            ViewData["UserID"] = userSelectList;
            return View();
        }


        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }
            if (post.UserID != _userManager.GetUserId(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostID,Content,UserID")] Post post)
        {
            if (id != post.ID)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            post.UserID = userId;

            if (ModelState.IsValid)
            {
                await _postService.UpdatePostAsync(post);
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostByIdAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }
            if (post.UserID != _userManager.GetUserId(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postService.DeletePostAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _postService.GetPostByIdAsync(id) != null ;
        }
    }
}
