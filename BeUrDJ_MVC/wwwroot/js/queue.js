


function GetDjProfile() {
    $.ajax({
        // Get Department PartialView
        url: "/dj/djProfile",
        type: 'GET',
        success: function (data) {
            jQuery("#DjProfile").html(data);
            /*
            jQuery("#startNav").empty();
            $("#startNav").prepend("<ul class='navbar-nav'><li class='nav-item'> <a class='nav-link text-white' asp-area='' asp-controller='Home' asp-action='Index'><img class='profileImage' src='https://scontent.xx.fbcdn.net/v/t31.0-1/c0.125.320.320a/p320x320/23467362_1506996659382041_7502020721667995357_o.jpg?_nc_cat=102&amp;_nc_sid=0c64ff&amp;_nc_ohc=LIM-EZy67ZAAX_2bNt0&amp;_nc_ht=scontent.xx&amp;oh=a63a3847f0bf5033bd98e2c1510ae911&amp;oe=5EBA56CC'/> Thomas</a></li></ul>");
            */
        },
        error: function (error) {
            alert("Error: Please try again.");
        }
    });
}

async function UpdateLike(songId, likes) {
    var url = window.location.href;
    var sessionId = url.replace('https://beurdj.com/dj/userQueue?sessionId=', '');
   await $.ajax({
        url: 'updateLike',
        type: 'POST',
        data: { songId: songId, likes: likes },
       success: function (data) {
           /*
           connection.invoke("UpdateQueue", parseInt(sessionId)).catch(function (err) {
               return console.error(err.toString());
           }); }
           */
           GetQueue();
       }
    });

};
function GetRecommended() {
    $.ajax({
        type: 'GET',
        url: "https://beurdj.com/dj/recommended",
        success: function (data) {
            $('#recommendedSongs').html(data);
        },
        error: function (xhr, ajaxOption, thorwnError) {
            console.log("Error")
        },
    });
}
function GetQueue() {
    var mobile = false;
    if (navigator.userAgent.includes('Mobile'))
    {
        mobile = true;
    }
    /*
    $.ajax({
        // Get Course PartialView
        url: "updateUserQueue",
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        cache: false,
        data: "isMobile=" + mobile,
        success: function (data) {
            $("#UpdateQueue").html(data);
            $('#UpdateQueue').show();
            var songCount = GetSongCount();
            if (songCount <= 2) {
                GetRecommended();
            }
        },
        error: function (error) {
            alert("Error: Please try again.");
        }
    });
    */
    $.post("updateUserQueue", { isMobile: mobile })
        .done(function (data) {
            $("#UpdateQueue").html(data);
            $('#UpdateQueue').show();
            var songCount = GetSongCount();
            var recommended = $('#recommendedSongs').html().trim();
            if (songCount <= 2 && recommended == '') {
                GetRecommended();
            }
            if (songCount > 2 && recommended != '') {
                $('#recommendedSongs').empty();
            }
        })
        .fail(function (error) {
            alert("Error: Please try again.");
        })
        
        
}

function GetQueueHeader() {
    $.ajax({
        // Get Course PartialView
        url: "/dj/getQueueHeader",
        type: 'POST',
        cache: false,
        success: function (data) {
            $("#QueueHeaderPartialView").html(data);     
            $('#search').click(function () {
                $.get("/dj/search", function (data) {
                    $("#searchModalBody").empty();
                    $("#searchModalBody").html(data);
                    $("#searchModal").modal('show');
                    document.getElementById('searchBar').addEventListener('keyup', songSearch, false);
                });
            });
        },
        error: function (error) {
            alert("Error: Please try again.");
        }
    });
}

function GetSongCount() {
    var rowCount = $('tbody tr').length;
    $('#songNumber').empty();
    $('#songNumber').append("Songs In Queue: " + rowCount);
    return rowCount;
}

function CheckMobile() {
    let intScreenSize = window.innerWidth;
    if (intScreenSize >= 768) {
        jQuery('#queueTable').append('<colgroup> ' +
            '<col style="width:10%">' +
            '<col style="width:20%">' +
            '<col style="width:30%">' +
            '<col style="width:10%">' +
            '<col style="width:30%">' +
            '</colgroup>');
        jQuery('#queueTable tbody tr td').css({ 'font-size': '25px', 'vertical-align': 'middle', 'align': 'center', 'font-weight': 'bold'});
        jQuery('#queueTable tbody tr td img').css({ 'height': 'auto', 'width': '80px' });
        jQuery('#queueTable tbody tr td .thisSongImage').css({ 'height': 'auto', 'width': '250px' });
    }
    else {
        jQuery('.current').remove();
        jQuery('.not').remove();
        jQuery('.next').remove();
        jQuery('#queueTable').append('<colgroup> ' +
            '<col style="width:200px">' +
            '<col style="width:30%">' +
            '<col style="width:1%">' +
            '<col style="width:60%">' +
            '</colgroup>');
        jQuery('#queueTable tbody tr td').css({ 'font-size': '16px', 'vertical-align': 'middle', 'align': 'center', 'font-weight': 'bold' });
        jQuery('#queueTable tbody tr td img').css({ 'height': 'auto', 'width': '60px', 'padding-bottom': '5px'});
        jQuery('#queueTable tbody tr td .thisSongImage').css({ 'height': 'auto', 'width': '100px' });
    }
}
jQuery(document).ready(function () {
    GetQueueHeader();
    GetDjProfile();
    GetQueue();
    setInterval(GetQueue, 10000);
});

function JoinSession() {
    var url = window.location.href;
    var sessionId = url.replace('https://beurdj.com/dj/userQueue?sessionId=', '');
    /*
    if (connection.connectionState != "Disconnected") {
        connection.invoke("JoinQueue", parseInt(sessionId)).catch(function (err) {
            return console.error(err.toString());
        });
    }
    */
};
function ExitSession(sessionId, userId) {
    var url = window.location.href;
    url = url.trim();
    /*
    connection.invoke("ExitQueue", sessionId, userId).catch(function (err) {
        return console.error(err.toString());
    });
    */
    window.location.href = 'session'
};