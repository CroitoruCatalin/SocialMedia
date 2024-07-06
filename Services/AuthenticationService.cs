using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Services.Interfaces;
using System.Security.Policy;
using System.Text;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
namespace SocialMedia.Services
{
    public class AuthenticationService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IUserService _userService;
        private readonly SocialContext _context;
        public AuthenticationService(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<AuthenticationService> logger,
            IUserService userService,
            SocialContext socialContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = socialContext;
            _userService = userService;
        }

        public async Task<SignInResult> LoginAsync(string email, string password, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
            }
            return result;
        }

        public async Task<IdentityResult> RegisterAsync(string fullName, string email, string password)
        {
            var user = new User { UserName = email, Email = email, FullName = fullName };

            //check if username already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => (u.FullName.ToUpper() == fullName.ToUpper() )|| (u.NormalizedEmail == email.ToUpper()));

            if(existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateUser",
                    Description = "A user with the same username or email already exists."
                });
            }

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                await _userManager.AddToRoleAsync(user, "User");
                if (!result.Succeeded)
                {
                    // Log errors if adding user to role failed
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError(error.Description);
                    }
                }
            }
            else
            {
                // Log errors if user creation failed
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                }
            }
            return result;
        }

    }
}
