function likePost(postId, likeUrl) {
    $.ajax({
        type: 'POST',
        url: likeUrl,
        data: { postId: postId },
        success: function (result) {
            $('#like-count-' + postId).text(result.likeCount);
            $('#dislike-count-' + postId).text(result.dislikeCount);
            console.log("Post Like Successful!");
            console.log(result);
        },
        error: function (req, status, error) {
            console.error('Error liking post:', error);
        }
    });
}

function dislikePost(postId, dislikeUrl) {
    $.ajax({
        type: 'POST',
        url: dislikeUrl,
        data: { postId: postId },
        success: function (result) {
            $('#like-count-' + postId).text(result.likeCount);
            $('#dislike-count-' + postId).text(result.dislikeCount);
            console.log("Post Dislike Successful!");
            console.log(result);
        },
        error: function (xhr, status, error) {
            console.error('Error disliking post:', error);
        }
    });
}