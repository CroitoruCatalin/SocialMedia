using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SocialMedia.Models;
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
        public AuthenticationService(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<AuthenticationService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
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
