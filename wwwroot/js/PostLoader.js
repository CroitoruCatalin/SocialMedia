document.addEventListener('DOMContentLoaded', function () {
    const postsContainer = document.getElementById('posts-container');
    const loadingIndicator = document.getElementById('loading');

    console.log('postsContainer:', postsContainer);
    console.log('loadingIndicator:', loadingIndicator);

    if (!loadingIndicator) {
        console.error('Loading indicator not found!');
        return;
    }

    //postIds contains the recommendations and is sent thorough the razor views
    let currentIndex = 0; //is initialized with 0

    
    function loadPostDetails(postId) {
        loadingIndicator.style.display = 'block';

        fetch(`/Posts/GetPostPresentation?id=${postId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.text();
            })
            .then(data => {
                postsContainer.insertAdjacentHTML('beforeend', data);
                loadingIndicator.style.display = 'none';
            })
            .catch(error => {
                console.error('Error fetching post details:', error);
                loadingIndicator.style.display = 'none';
            });
    }

    // Load more posts when the user scrolls to the bottom
    function onScroll() {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
            if (currentIndex < postIds.length) {
                const postId = postIds[currentIndex];
                currentIndex++;
                loadPostDetails(postId);
            }
        }
    }

    // Initial load of first set of posts
    for (currentIndex = 0; currentIndex < 3 && currentIndex < postIds.length; currentIndex++) {
        const postId = postIds[currentIndex];
        loadPostDetails(postId);
    }

    // Attach scroll event listener
    window.addEventListener('scroll', onScroll);
});
