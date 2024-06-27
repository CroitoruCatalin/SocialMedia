using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;

namespace SocialMedia.Services.Interfaces
{
    public interface ICommentLikeService
    {
        Task LikeComment(int commentId, string userId);
        Task DislikeComment(int commentId, string userId);

    }
}

