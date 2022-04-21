
function CloseFilter() {
    $("#filterModal").modal('toggle');
    return false;
};
/*
function SubmitFilters(e) {
    const formEntries = new FormData(myForm).entries();

    e.preventDefault();
    var form = $('#filterForm');
    var mydata = JSON.stringify($('#filterForm').serializeObject());
    $('#filterForm').serializeObject().done(function (o) {

            if (window.console) console.log(o);

            var j = JSON.stringify(o);

            alert(j);

            //window.open("data:image/png;base64," + o.userfile.data);

        });
};
*/
function SubmitFilters(model) {
    $.ajax({
        // Get Course PartialView
        url: "/dj/filters",
        type: 'POST',
        data: model,
        cache: false,
        success: function (data) {
            $("#filterModal").modal('toggle');
            return false;
        },
        error: function (error) {
            alert("Error: Please try again.");
        }
    });
}
