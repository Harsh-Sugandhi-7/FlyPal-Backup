$(document).ready(function () {
    var temp = $("body table:first").outerHeight(true);
    if (temp > $(window).height()) {
        $pos = $("table span:first").position();
        var height = $("TABLE span:first").height();
        var width = $("body table:first").width();
        //return top and left positon of element
        var top = $pos.top;
        var left = $pos.left;
        var labelheight = top + height - 7;
        if ($("#bottom").length > 0) {
            $("#bottom").show(); // show link
        }
        else {
            $("body").append('<a id="bottom" class="clsScroll" href="#">Bottom</a>');
        }
        if ($("#top").length > 0) {
            $("#top").show();
        }
        else {
            $("body").append('<a id="top" class="clsScroll" href="#">Top</a>');
        }
        var labelwidth = 53; //$("#bottom").innerWidth();
        var labelright = (width - labelwidth);
        labelright = labelright + left - 5;
        $('#bottom').css({ 'margin-top': labelheight, 'margin-left': labelright, 'background': 'transperent' });
        $('#top').css({ 'margin-top': temp - 3, 'margin-left': labelright, 'background': 'transperent' });
    }
});
//$("body table:first,html table:first").resize(function () {
$(this).resize(function () {
    onResize();
});
function pageLoad() {
    onResize();
}
$(document).ready(function () {
    $('#bottom').on('click', function () {
        $("html, body").animate({ 'scrollTop': $(document).height() }, 'slow');
        return false;
    });
    $('#top').on('click', function () {
        $("html, body").animate({ 'scrollTop': 0 }, 'slow');
        return false;
    });
   
});
function onResize() {
    //if ($("body table:first")) {
    var temp = $("body table:first").outerHeight(true);
    if (temp) {
        if (temp > $(window).height()) {

            $pos = $("table span:first").position();
            var height = $("TABLE span:first").height();
            var width = $("body table:first").width();
            //return top and left positon of element
            var top = $pos.top;
            var left = $pos.left;
            var labelheight = top + height - 7;
            //var margingleft = $("body table:first").css("margin-left");
            if ($("#bottom").length > 0) {
                $("#bottom").show(); // show link
            }
            else {
                $("body").append('<a id="bottom" class="clsScroll" href="#">Bottom</a>');
            }
            if ($("#top").length > 0) {
                $("#top").show();
            }
            else {
                $("body").append('<a id="top" class="clsScroll" href="#">Top</a>');
            }
            // show link
            var labelwidth = 53; //$("#bottom").innerWidth();
            var labelright = (width - labelwidth);
            labelright = labelright + left - 5;
            $('#bottom').css({ 'margin-top': labelheight, 'margin-left': labelright, 'background': 'transperent' });
            $('#top').css({ 'margin-top': temp - 3, 'margin-left': labelright, 'background': 'transperent' });
        }
        else {
            $("#bottom").remove(); // hide link
            $("#top").remove();
        }
    }
}
