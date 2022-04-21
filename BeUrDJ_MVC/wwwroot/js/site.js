jQuery(document).ready(function () {
    CheckAccount();
});

function CheckAccount() {
    $.get("https://beurdj.com/dj/checkAccount").done(function (data) {
        var location = window.location.href;
        if (!data && location != "https://beurdj.com/dj/login" && location != "https://beurdj.com/" && location != "https://beurdj.com/Account/Register") {
            window.location.href = "/dj/login";
        }
        if (data) {
            jQuery("#startNav").empty();
            $("#startNav").prepend("<ul class='navbar-nav'><li class='nav-item' style='vertical-align:middle;'> <a class='nav-link text-white' style='vertical-align:middle;' href='/dj/session'>Sessions</a></li><li class='nav-item' > <a class='nav-link text-white' style='vertical-align:middle;' href='/dj/logout'>Logout</a></li></ul>");
        }
    }).fail(function () { window.location.href = "/dj/login"; });
}