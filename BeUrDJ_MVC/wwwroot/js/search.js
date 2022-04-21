
var items = [];

function songSearch() {
    const song_url = "https://beurdj.com/dj/gettracks";
    var searchText = $('#searchBar').val();
    if (searchText.length == 0) {
        $('#songsFound').html("<p>Enter a title To Generate Songs</p>");
    }
    if (searchText.length >= 3) {
        DisplayNote("Searching Songs...");
        $.getJSON(song_url, {
            searchText: searchText
        })
            .done(function (data) {
                if (data.songs.length == 0) {
                    UpdateNote("No Results Found! Try Another Song...");
                    var genres = "";
                    $.each(data.filters, function (i, item) {
                        if (item == true) {
                            genres += "<li>" + i.charAt(0).toUpperCase() + i.slice(1); + "</li>";
                        }
                    });
                    if (genres != "") {
                        $('#songsFound').html("<h4>No Results Found! Try Searching for a Song in these Genres:</h4><ul>" + genres + "</ul>");
                    }
                    else {
                        $('#songsFound').html("<h5>No Results Found! All Genres are Available.</h5>");
                    }

                }
                else {
                    UpdateNote("Found Songs!");
                    items = data.songs;
                    $('#songsFound').html("");
                    $.each(items, function (i, item) {

                        $('#songsFound').append("<div class=songs id='song" + i.toString() + "' onclick=AddSearchSong(" + i.toString() + ")><div class='songImage'><img src=" + item.album.images[1].url + "></div><div class='limit' ><label class='songLabel songName'>" + item.name + "</label><br /><label class='songLabel' style='font-style: italic;'>" + item.artists[0].name + "</label><br/><img class='addSong' src='/img/addSong.png'></img></div></div>");
                    });

                }
            });
    }
}

function AddSearchSong(item) {
    song = items[parseInt(item)];
    DisplayNote("Adding Song...");
    $.post("https://beurdj.com/dj/addTrack", song)
        .done(function (data) {
            console.log(data);
            var note = "Successfully Added Song!";
            UpdateNote(note);
            GetQueue();
        })
        .fail(function (err) {
            console.log(err);
            var note = "Error Adding Song";
            UpdateNote(note);
            GetQueue();
        })
}

function DisplayNote(noteText) {
    $('#note').text(noteText);
    $('#note').css('display', 'inline');
};
function UpdateNote(noteText) {
    $('#note').text(noteText);
    //setTimeout(function () { $('#note').css('display', 'none'); }, 2000);
    setTimeout(function () {
        $('#note').css('display', 'none');
    }, 2000);
};

function CloseSearch() {
    $("#searchModal").modal('toggle');
};