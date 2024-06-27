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

namespace SocialMedia.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public CommentsController( ICommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        // GET: Comments
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _commentService.GetAllComments());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentService.GetCommentById(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create(int postId)
        {
            var comment = new Comment { PostID = postId };
            return View(comment);
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Message,PostID")] Comment comment)
        {
            comment.UserID = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                //comment.UserID = _userManager.GetUserId(User);
                comment.UserID = _userManager.GetUserId(User);
                comment.CreationDate = DateTime.Now;
                await _commentService.CreateComment(comment);
                return RedirectToAction("Details", "Posts", new { id = comment.PostID });
            }
            else
            {
                // Log ModelState errors for debugging
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    if (modelStateVal.Errors.Any())
                    {
                        foreach (var error in modelStateVal.Errors)
                        {
                            // Log or Debug.WriteLine the error messages
                            Debug.WriteLine($"ModelState Error: {error.ErrorMessage}");
                        }
                    }
                }
            }
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            return View(comment);
        }


        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var comment = await _commentService.GetCommentById(id.Value);
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
            if (id != comment.CommentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingComment = await _commentService.GetCommentById(id);
                if(existingComment == null || existingComment.UserID != _userManager.GetUserId(User))
                {
                    return Forbid();
                }

                existingComment.Message = comment.Message;
                existingComment.CreationDate = comment.CreationDate;



                await _commentService.UpdateComment(existingComment);
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

            var comment = await _commentService.GetCommentById(id.Value);
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
            var comment = await _commentService.GetCommentById(id);
            if(comment != null && comment.UserID == _userManager.GetUserId(User))
            {
                await _commentService.DeleteComment(id);
            }
            return RedirectToAction("Details", "Posts", new { id = comment?.PostID });
        }

        private async Task<bool> CommentExists(int id)
        {
            return await _commentService.CommentExists(id);
        }
    }
}
