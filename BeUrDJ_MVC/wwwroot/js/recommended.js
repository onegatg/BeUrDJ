
const add_song_url = "https://beurdj.com/dj/addRecommendedTrack";
function AddSong(item) {
    var addedTrack = CreateSongObject(item);
    DisplayNote("Adding Recommended Song...");
    $.ajax({
        url: add_song_url,
        type: 'POST',
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(addedTrack),
        success: function (data) {
            var note = "Successfully Added Song!";
            UpdateNote(note);
            GetQueue();
        }
    })
}

function CreateSongObject(html) {
    var songUri = html.childNodes[1].innerText;
    var songImage = html.childNodes[2].currentSrc;
    var songName = html.childNodes[5].textContent;
    var artistName = html.childNodes[7].textContent;
    var song = {
        SongID: 1,
        SongUri: songUri,
        SongName: songName,
        ArtistName: artistName,
        SongImage: songImage,
        Likes: 0,
        SessionID: 0,
        PlayStateID: 0
    };
    return song;
}

function DisplayNote(noteText) {
    $('#note').text(noteText);
    $('#note').css('display', 'inline');
};
function UpdateNote(noteText) {
    $('#note').text(noteText);
    $('#note').css('display', 'none');
};
