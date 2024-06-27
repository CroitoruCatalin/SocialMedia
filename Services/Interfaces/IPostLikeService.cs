using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;

namespace SocialMedia.Services.Interfaces
{
    public interface IPostLikeService
    {
        Task LikePost(int postId, string userId);
        Task DislikePost(int postId, string userId);

    }
}

