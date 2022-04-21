popVal = parseFloat($('#popularity').val());
tempoVal = parseFloat($('#tempo').val());
danceVal = parseFloat($('#danceability').val());


function createTempoStaff() {
    $('<div id="tempoGradient" class="rs-gradient" />').insertBefore($('#tempoSlider .rs-tooltip'));
}
function createDanceStaff() {
    $('<div id="danceGradient" class="rs-gradient" />').insertBefore($('#danceSlider .rs-tooltip'));
}
function createPopStaff() {
    $('<div id="popGradient" class="rs-gradient" />').insertBefore($('#popSlider .rs-tooltip'));
}
function createTestStaff() {
    $('<div id="testGradient" class="rs-gradient" />').insertBefore($('#testSlider .rs-tooltip'));
}



function updateTempo() {
    $('#tempo').attr('value', $('#tempoSlider .rs-tooltip-text').html());
    tempoVal = -230 + parseFloat($('#tempoSlider .rs-tooltip-text').html() * 7.5);
    $('#tempoGradient.rs-gradient').css({
        background: 'hsl(' + tempoVal * 12 + ', 100%, 57%)'
    });
    $('#handle1 .rs-tooltip div').css({
        background: 'hsl(' + tempoVal * 12 + ', 100%, 57%)'
    })
}

function updateDance() {
    $('#danceability').attr('value', $('#danceSlider .rs-tooltip-text').html());
    danceVal = -230 + parseFloat($('#danceSlider .rs-tooltip-text').html() * 7.5);
    $('#danceGradient.rs-gradient').css({
        background: 'hsl(' + danceVal * 12 + ', 100%, 57%)'
    });
}

function updatePop() {
    $('#popularity').attr('value', $('#popSlider .rs-tooltip-text').html());
    popVal = -230 + parseFloat($('#popSlider .rs-tooltip-text').html() * 7.5);
    var css = $('#popGradient.rs-gradient');
    $('#popGradient.rs-gradient').css({
        background: 'hsl(' + popVal * 12 + ', 100%, 57%)'
    });
}


$("#tempoSlider").roundSlider({
    radius: 80,
    width: 15,
    min: 0,
    max: 200,
    circleShape: "pie",
    handleSize: "+16",
    handleShape: "dot",
    sliderType: "min-range",
    startAngle: 315,
    endAngle:"+.01",
    value: tempoVal,
    step: 10,
    editableTooltip: false,
    mouseScrollAction: true,
    change: updateTempo,
    drag: updateTempo,
    create: function () {
        createTempoStaff();
        updateTempo();
    }

})
$("#danceSlider").roundSlider({
    radius: 80,
    width: 15,
    min: 0,
    max: 1,
    circleShape: "pie",
    handleSize: "+16",
    handleShape: "dot",
    sliderType: "min-range",
    startAngle: 315,
    value: danceVal,
    step: 0.25,
    editableTooltip: false,
    mouseScrollAction: true,
    change: updateDance,
    drag: updateDance,
    create: function () {
        createDanceStaff();
        updateDance();
    }

})

$("#popSlider").roundSlider({
    radius: 80,
    width: 15,
    min: 0,
    max: 1,
    circleShape: "pie",
    handleSize: "+16",
    handleShape: "dot",
    sliderType: "min-range",
    startAngle: 315,
    value: popVal,
    step: 0.25,
    editableTooltip: false,
    mouseScrollAction: true,
    change: updatePop,
    drag: updatePop,
    create: function () {
        createPopStaff();
        updatePop();
    }

})

jQuery(document).ready(function () {
    $("#handle1").roundSlider({
        sliderType: "min-range",
        editableTooltip: false,
        radius: 80,
        width: 16,
        min: 0,
        max: 1,
        value: popVal,
        handleSize: 25,
        step: 0.25,
        handleShape: "round",
        circleShape: "pie",
        mouseScrollAction: true,
        startAngle: 315,
        tooltipFormat: "changeTooltip"
    });
    /*
    $('.genreFilter').click(function () {
        var check = $(this).find('input').checked;
        if (check){
            $(this).find('input').prop("checked", false);
            $(this).find('input').val('false');
            $(this).find('label').removeClass('selected');
            $(this).find('label').addClass('unselected');
        }
        else {
            $(this).find('input').prop("checked", true);
            $(this).find('input').val('true');
            $(this).find('label').removeClass('unselected');
            $(this).find('label').addClass('selected');
        }
    });
    */
    $("input[type=checkbox]").change(function () {
        var check = $(this).prop("checked");
        if (check == true) {
            //$(this).prop("checked", true);
            $(this).val(true);
            $(this).next().removeClass('unselected');
            $(this).next().addClass('selected');
        }
        else {
            //$(this).prop("checked", false);
            $(this).val(false);
            $(this).next().removeClass('selected');
            $(this).next().addClass('unselected');
        }
    });
});
function changeTooltip(e) {
    var val = e.value;
    var speed = "";
    if (val < 20) speed = "Slow";
    else if (val < 40) speed = "Normal";
    else if (val < 70) speed = "Speed";
    else speed = "Very Speed";

    return "<div>" + val + " km/h" + "<br>" + speed;
}
