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
using SocialMedia.Models.ViewModels;
using System.Web;
using SocialMedia.Repositories.Interfaces;


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
            IEnumerable<int> postIds = new List<int>();
            if(userId != null)
            {
                var user = await _userService.GetUserById(userId);
                var postCreateViewModel = new PostPresentationViewModel
                {
                    UserID = userId,
                    User = _userService.GetUserPresentation(userId),
                };
                //var postIds = await _postService.GetPostRecommendationsAsync(userId);
                postIds = await _postService.RecommendationAlgorithm(userId);

                var model = new Tuple<IEnumerable<int>, PostPresentationViewModel>(postIds, postCreateViewModel);

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPostPresentation(int id)
        {
            Console.WriteLine("Starting GetPostPresentation("+id+") from PostsController");
            //get view model from post service
            var postPresentationModel = await _postService.GetPostPresentation(id);

            Console.WriteLine("PostPresentation user.FullName: " + postPresentationModel.User.FullName);
            Console.WriteLine("PostPresentation user.Id: " + postPresentationModel.User.Id);
            //return view model
            return PartialView("_Post", postPresentationModel);
        }



        [HttpGet]
        public async Task<IActionResult> GetPostDetails(int id)
        {
            Post post = await _postService.GetPostWithCommentsAsync(id);
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

            var post = await _postService.GetPostWithCommentsAsync((int)id);

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

            Console.WriteLine("Executing get create method.");
            var userId = _userManager.GetUserId(User);
            Console.WriteLine("GetCreate userId: " + userId);
            if (userId != null)
            {
                var post = new Post
                {
                    UserID = userId
                };
                post.UserID = userId;
                return View(post);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostPresentationViewModel model, IFormFile? ImageFile)
        {
            var userId = _userManager.GetUserId(User);

            // Print model data
            Console.WriteLine("POST CREATION MODEL OUTPUT:" + model.GetDetails("PostsController.Create"));

            var post = new Post
            {
                UserID = model.UserID,
                Message = model.Message,
                CreationDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                LikeCount = 0,
                DislikeCount = 0,
                CommentCount = 0,
                Image = null, // Default to no image
                ImageID = null,
            };

            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState.IsValid");
                // Handle image upload if an image file is provided
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await ImageFile.CopyToAsync(ms);
                        var imageBytes = ms.ToArray();

                        // Create and associate Image object
                        var image = new Image
                        {
                            Data = imageBytes,
                            ContentType = ImageFile.ContentType,
                            FileName = ImageFile.FileName,
                            UploadDate = DateTime.UtcNow,
                            UserId = model.UserID
                        };

                        // Associate Image with Post
                        post.Image = image;
                    }
                }
                else
                {
                    Console.WriteLine("ModelState.IsNotValid");
                }

                // Handle YouTube video embed if applicable
                if (!string.IsNullOrEmpty(post.Message) && post.Message.Contains("youtube.com/watch?v="))
                {
                    var videoId = ExtractYoutubeVideoId(post.Message);
                    if (!string.IsNullOrEmpty(videoId))
                    {
                        post.Embed = $"<iframe width='560' height='315' src='https://www.youtube.com/embed/{videoId}' frameborder='0' allowfullscreen></iframe>";
                    }
                }

                // Save Post
                await _postService.CreatePostAsync(post, User);
                return RedirectToAction(nameof(Index));
            }

            // Print model state errors for debugging
            foreach (var modelState in ModelState)
            {
                var key = modelState.Key;
                var errors = modelState.Value.Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                }
            }

            // Load post recommendations and return view with errors
            var postIds = await _postService.RecommendationAlgorithm(userId);
            var newModel = new PostPresentationViewModel();
            newModel.User = _userService.GetUserPresentation(userId);
            var modelTuple = new Tuple<IEnumerable<int>, PostPresentationViewModel>(postIds, newModel);
            return View("Index", modelTuple);
        }

        private string ExtractYoutubeVideoId(string message)
        {
            var uri = new Uri(message);
            var query = HttpUtility.ParseQueryString(uri.Query);
            return query["v"];
        }

        public async Task<IActionResult> AddComment(int postId)
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewData["UserID"] = user.Id;
                return View();
            }
            else
            {
                return NotFound();
            }
        }


        // GET: Posts/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            

            var post = await _postService.GetPostPresentation(id);
            post.ID = id;
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
        public async Task<IActionResult> Edit(PostPresentationViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                model.UserID = userId;
            }
            Console.WriteLine("EDIT MODEL MESSAGE="+model.Message);
            if (ModelState.IsValid && model.UserID != null)
            {
                var post = await _postService.GetPostByIdAsync(model.ID);

                if(post == null)
                {
                    return NotFound();
                }
                post.Message = model.Message;
                post.ModifiedDate = DateTime.UtcNow;

                await _postService.UpdatePostAsync(post);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
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
