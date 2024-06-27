function followUser(userId, followUrl) {
    $.ajax({
        type: 'POST',
        url: followUrl,
        data: { id: userId },
        success: function (result) {
            if (result.success) {
                console.log(result.message);
                console.log("User followed successfully: " + userId);
                $('button[data-user-id="' + userId + '"]')
                    .removeClass('btn-success')
                    .addClass('btn-danger')
                    .text('Unfollow')
                    .attr('onclick', 'unfollowUser("' + userId + '", "' + followUrl + '")');

                var followersCount = parseInt($('#followers-count').text());
                $('#followers-count').text(followersCount + 1);
            } else {
                console.log(result.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error following user:', error);
        }
    });
}

function unfollowUser(userId, unfollowUrl) {
    $.ajax({
        type: 'POST',
        url: unfollowUrl,
        data: { id: userId },
        success: function (result) {
            if (result.success) {
                console.log(result.message);
                console.log("User unfollowed successfully: " + userId);
                $('button[data-user-id="' + userId + '"]')
                    .removeClass('btn-danger')
                    .addClass('btn-success')
                    .text('Follow')
                    .attr('onclick', 'followUser("' + userId + '", "' + unfollowUrl + '")');

                var followersCount = parseInt($('#followers-count').text());
                $('#followers-count').text(followersCount - 1);
            } else {
                console.log(result.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error unfollowing user:', error);
        }
    });
}
