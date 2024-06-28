$(document).ready(function () {
    var resultsDiv = $('#searchResults');
    $('#searchTerm').on('input', function () {
        var searchTerm = $(this).val().trim();

        if (searchTerm.length === 0) {
            resultsDiv.empty().hide();
            return;
        }

        $.ajax({
            url: '/Users/Search',
            type: 'GET',
            data: { searchTerm: searchTerm },
            success: function (data) {
                console.log('Received data:', data);

                resultsDiv.empty();

                var dropdown = $('<div class="dropdown-menu show py-2 dropdown-user-search-results" aria-labelledby="searchTerm"></div>');
                if (Array.isArray(data) && data.length > 0) {

                    $.each(data, function (index, user) {
                        var userName = user.fullName;
                        var profileLink = '/Users/Details/' + user.id;
                        var profilePicture = user.profilePicture
                            ? '<img src="data:' + user.profilePicture.contentType + ';base64,' + user.profilePicture.data + '" class="rounded-circle me-2" width="30" height="30" alt="' + userName + '" />'
                            : '<img src="/assets/images/default.png" class="rounded-circle me-2" width="30" height="30" alt="Default Profile Picture" />';

                        var userLink = $('<a class="dropdown-item d-flex align-items-center" href="' + profileLink + '">' + profilePicture + userName + '</a>');

                        dropdown.append(userLink);
                    });

                    resultsDiv.empty().append(dropdown).show();
                } else {
                    var notFound = '<a class="dropdown-item d-flex align-items-center" type="button">No users found.</button>';
                    
                    dropdown.append(notFound);
                    resultsDiv.append(dropdown).show();
                }
            },
            error: function (xhr, status, error) {
                console.error('Error:', status, error);
            }
        });
    });

    $(document).on('click', function (e) {
        if (!resultsDiv.is(e.target) && resultsDiv.has(e.target).length === 0) {
            resultsDiv.empty().hide();
        }
    });
});
