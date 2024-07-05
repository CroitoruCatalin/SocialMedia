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
using System.Diagnostics;
using SocialMedia.Models.ViewModels;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        private readonly IPostService _postService;

        public CommentsController( 
            ICommentService commentService, 
            UserManager<User> userManager,
            IPostService postService)
        {
            _commentService = commentService;
            _userManager = userManager;
            _postService = postService;
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
        public async Task<IActionResult> Create(int postId)
        {
            var parentPost = await _postService.GetPostByIdAsync(postId);

            if (parentPost == null)
            {
                return NotFound();
            }

            var viewModel = new CommentCreateViewModel
            {
                Comment = new Comment { PostID = postId},
                Post = parentPost
            };

            Console.WriteLine("Comment Creation for Post With ID: " + postId);
            return View(viewModel);
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentCreateViewModel viewModel)
        {
            Comment comment = viewModel.Comment;
            comment.UserID = _userManager.GetUserId(User);

            comment.CreationDate = DateTime.UtcNow;
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
