using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using SocialMedia.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public CommentsController(
            ICommentService commentService,
            UserManager<User> userManager,
            IPostService postService,
            IUserService userService)
        {
            _commentService = commentService;
            _userManager = userManager;
            _postService = postService;
            _userService = userService;
        }


        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentService.GetCommentByIdAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        [HttpGet]
        public async Task<IActionResult> Create(int postId)
        {
            var parentPost = await _postService.GetPostByIdAsync(postId);
            string? currentUserId = _userManager.GetUserId(User);
            if(currentUserId == null)
            {
                return NotFound();
            }

            User? currentUser = await _userService.GetUserById(currentUserId);

            if (parentPost == null || currentUser == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                PostID = parentPost.ID,
                Post = parentPost,
                UserID = currentUser.Id,
                User = currentUser
            };


            Console.WriteLine("Comment Creation for Post With ID: " + postId);
            return View(comment);
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (comment.Post == null)
            {
                Console.WriteLine("CREATECOMMENT comment.Post is null.");
                return NotFound();
            }
            if(currentUser == null)
            {
                Console.WriteLine("CREATECOMMENT currentUser is null.");
                return NotFound();
            }
            comment.User = currentUser;
            comment.UserID = currentUser.Id;
            if(currentUser != comment.User)
            {
                Console.WriteLine("CREATECOMMENT currentUser.Id="+currentUser.Id
                    +" while comment.User.Id="+comment.User.Id);
                return NotFound();
            }

            comment.CreationDate = DateTime.UtcNow;
            comment.ModifiedDate = comment.CreationDate;
            await _commentService.CreateCommentAsync(comment);
            return RedirectToAction("Details", "Posts", new { id = comment.PostID });

        }


        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var comment = await _commentService.GetCommentByIdAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            if(comment.UserID != _userManager.GetUserId(User))
            {
                return RedirectToAction("Details", "Posts", new { id = comment.PostID });
            }    
            
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentID,Message,PostID")] Comment comment)
        {
            if (id != comment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingComment = await _commentService.GetCommentByIdAsync(id);
                if(existingComment == null || existingComment.UserID != _userManager.GetUserId(User))
                {
                    return Forbid();
                }

                existingComment.Message = comment.Message;
                existingComment.CreationDate = comment.CreationDate;



                await _commentService.UpdateCommentAsync(existingComment);
                return RedirectToAction("Details", "Posts", new { id = comment.PostID });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentService.GetCommentByIdAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }
            if (comment.UserID != _userManager.GetUserId(User))
            {
                return RedirectToAction("Details", "Posts", new { id = comment.PostID });
            }
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if(comment != null && comment.UserID == _userManager.GetUserId(User))
            {
                await _commentService.DeleteCommentAsync(id);
            }
            return RedirectToAction("Details", "Posts", new { id = comment?.PostID });
        }

        private async Task<bool> CommentExists(int id)
        {
            return await _commentService.CommentExists(id);
        }
    }
}
