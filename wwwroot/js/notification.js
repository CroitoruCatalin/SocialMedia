$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationsHub")
        .configureLogging(signalR.LogLevel.Debug)
        .build();

    connection.on("ReceiveMessage", function (notification) {

        addNotification(notification);
        updateNotificationBadge();
    });

    connection.start()
        .then(function () {
            console.log("Connected to SignalR hub");
            fetchNotifications();
        })
        .catch(function (err) {
            console.error("Error during SignalR connection:", err.toString());
        });

    connection.onclose(function (error) {
        console.error("Connection closed with error:", error);
        setTimeout(function () {
            connection.start().catch(err => console.error("Reconnection failed:", err.toString()));
        }, 5000); // Try to reconnect after 5 seconds
    });

    function fetchNotifications() {
        $.get("/api/Notifications", function (notifications) {
            const unreadNotifications = notifications.filter(notification => !notification.isRead);

            const unreadCount = unreadNotifications.length;
            $("#notificationCount").text(unreadCount);

            if (unreadCount > 0) {
                $("#notificationCount").show();
                $("#notificationCount").text(unreadCount);
            } else {
                $("#notificationCount").hide();
            }

            $("#notificationDropdown").empty();

            unreadNotifications.forEach(notification => {
                addNotification(notification);
            });

            if (unreadNotifications.length === 0) {
                $("#notificationDropdown").append('<li>No notifications right now.</li>');
            }
        });
    }

    function addNotification(notification) {
        const notificationDropdown = $("#notificationDropdown");
        const notificationCount = $("#notificationCount");

        const notificationClass = notification.isRead ? "read" : "unread";
        const userProfilePicture = notification.userProfilePictureData
            ? `<img src="data:${notification.userProfilePictureContentType};base64,${notification.userProfilePictureData}" alt="${notification.userName}" class="rounded-circle me-2" width="30" height="30" />`
            : `<img src="/assets/images/default.png" class="rounded-circle me-2" width="30" height="30" alt="Default Profile Picture" />`;

        const userName = `<strong>${notification.userName}</strong>`;

        const newNotification = `
    <li class="notification-item ${notificationClass}">
        <a class="dropdown-item notification-link" href="${notification.url}" data-id="${notification.notificationID}">
            ${userProfilePicture}
            ${userName}: ${notification.message}
        </a>
    </li>`;

        notificationDropdown.prepend(newNotification);

        const currentCount = parseInt(notificationCount.text());
        notificationCount.text(currentCount + 1);

        $(".notification-link").off("click").on("click", function (e) {
            e.preventDefault();
            const notificationId = $(this).data("id");
            const url = $(this).attr("href");

            $.ajax({
                url: "/api/Notifications/MarkAsRead",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ notificationId: notificationId }), 
                success: function () {
                    window.location.href = url;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error marking notification as read:", textStatus, errorThrown);
                }
            });
        });

        updateNotificationBadge();
    }

    function updateNotificationBadge() {
        $.get("/api/Notifications", function (notifications) {
            const unreadNotifications = notifications.filter(notification => !notification.isRead);
            const unreadCount = unreadNotifications.length;

            if (unreadCount > 0) {
                $("#notificationCount").show().text(unreadCount);
            } else {
                $("#notificationCount").hide();
            }
        });
    }
});
