//Created By Utkarsh on 18-Oct-2013 FOR ALL18102013

var Page;
var fixedPositionSupport = false;
function MsgBox(page, buttontype, title, message, width) {
    fixedPositionSupport = supportsPositionFixed();
    var wid = width || 400;
    Page = page;
    //var popMargLeft = -(wid/ 2);

    var popMargLeft = ($(window).width()) / 2 - 200;

    var str = "";
    if (fixedPositionSupport) {
        str = str + '<div id="MsgBox" style="position:fixed;top:50%;left:50%;margin-left:-200px;z-index: 99999;width:400px;height:auto;">';
    }
    else {
        str = str + '<div id="MsgBox" style="position:absolute;top:0;left:' + popMargLeft + ';z-index: 99999;width:400px;height:auto;">';
    }
    
    str = str + '<div class="clsMsgBoxOuter">';
    str = str + '<div>';
    str = str + '<div>';
    str = str + '<Label ID="lblMsgTitle" for=title Class="clsMsgBoxTitle" style="padding-left: 0px; padding-right: 0px;">' + title + '</Label>';
    str = str + '</div>';



    str = str + ' <div class="clsMsgBoxBody">';
    str = str + ' <div class="clsMsgBoxInnerBody">';
    //        str = str + '<td valign="center" align="left">';
    //       // str = str + '<img  title="alert" style="position:relative" alt="alert" src="images/alert_icon.png" width="40" height="40">'
    //        str = str + '</td>';
    str = str + '<div style="padding: 10px;min-height:20px;">';
    str = str + '<div class="clsMsgInfoIcon">';
    str = str + '</div>';
    str = str + '<div class="clsMsgContent">';
    str = str + '<Label ID="lblMsgText" for=title Class="clsMsgText">' + message + '</Label>';
    str = str + '</div>';
    str = str + '</div>';
    str = str + '</div>';
    str = str + '</div>';
    str = str + ' <div class="clsMsgBoxFooterWrap">';
    str = str + '<div class="clsMsgBoxFooter" id="ButtonDiv">';
    //add button type

    if (buttontype === 0) {
        str = str + '<input id="alertclose" type=submit value="Ok" width="100%" onclick="MessageResult(1)" class="clsButton"/>';
    }

    else if (buttontype === 1) {
        str = str + '<input id="msgbtnyes" type=submit value="Ok" width="100%" onclick="MessageResult(1)" class="clsButton"/>';
        str = str + '<label for="text" width=3px>&nbsp;</label>';
        str = str + '<input id="msgbtnno" type=submit value="Cancel" width="100%" onclick="MessageResult(2)" class="clsButton"/>';
    }
    else if (buttontype === 2) {
        str = str + '<input id="msgbtnyes" type=submit value="Abort" width="100%" onclick="MessageResult(3)" class="clsButton"/>';
        str = str + '<label for="text" width=3px>&nbsp;</label>';
        str = str + '<input id="msgbtnno" type=submit value="Retry" width="100%" onclick="MessageResult(4)" class="clsButton"/>';
        str = str + '<label for="text" width=3px>&nbsp;</label>';
        str = str + '<input id="msgbtnCancel" type=submit value="btnIgnore" width="100%" onclick="MessageResult(2)" class="clsButton"/>';
    }
    else if (buttontype == 3) {
        str = str + '<input id="msgbtnyes" type=submit value="Yes" width="100%" onclick="MessageResult(6)" class="clsButton"/>';
        str = str + '<label for="text" width=3px>&nbsp;</label>';
        str = str + '<input id="msgbtnno" type=submit value="No" width="100%" onclick="MessageResult(7)" class="clsButton"/>';
        str = str + '<label for="text" width=3px>&nbsp;</label>';
        str = str + '<input id="msgbtnCancel" type=submit value="Cancel" width="100%" onclick="MessageResult(2)" class="clsButton"/>';
    }
    else if (buttontype === 4) {
        //        str = str + '<div>';
        //        str = str + '<table border="0" cellpadding="1" cellspacing="1" width="100%">';
        //        str = str + '<tr>';
        //        str = str + '<td align=center valign="top" width="50%">';
        //        // str = str + '<div style="margin-top:5px">';
        str = str + '<input id="msgbtnyes" type=submit value="Yes" width="100%" onclick="MessageResult(6)" class="clsButton"/>';
        //        // str = str + '</div>';
        //        str = str + '</td>';
        //        str = str + '<td align=center valign="top" width="50%">';
        //        // str = str + '<div style="margin-top:5px">';
        str = str + '<label for="text" width=3px>&nbsp;</label>';
        str = str + '<input id="msgbtnno" type=submit value="No" width="100%" onclick="MessageResult(7)" class="clsButton"/>';
        // str = str + '</div>';
        //        str = str + '</td>';
        //        str = str + '</tr>';
        //        str = str + '</table>';
        //        str = str + '</div>';
    }
    else if (buttontype === 5) {
        str = str + '<input id="msgbtnyes" type=submit value="Retry" width="100%" onclick="MessageResult(4)" class="clsButton"/>';
        str = str + '<label for="text" width=3px>&nbsp;</label>';
        str = str + '<input id="msgbtnno" type=submit value="Cancel" width="100%" onclick="MessageResult(2)" class="clsButton"/>';
    }

    else {
        //        str = str + '<div align=center valign="top" width="100%">';
        //        // str = str + '<div style="margin-top:5px">';
        str = str + '<input id="alertclose" type=submit value="Ok" width="100%" onclick="MessageResult(1)" class="clsButton"/>';
        //        // str = str + '</div>';
        //        str = str + '</div>';

    }


    //End
    str = str + '</div>';
    str = str + '</div>';

    str = str + '</div>';
    str = str + '</div>';
    str = str + '</div>';

    $("body").append(str);

    var popID = 'popup_name';
    var popWidth = wid; //dim[0].split('=')[1];


    //$('#' + popID).fadeIn().css({ 'width': Number(popWidth) }).prepend('<a href="#" class="close"><img src="images/close2.png" style="position:relative;margin: -37 -37 0 395;" class="btn_close" title="Close" alt="Close" /></a>');
    var popMargTop;
    if (fixedPositionSupport) {
        popMargTop = ($('#MsgBox').outerHeight()) / 2;
        $('#MsgBox').css({ 'margin-top': -popMargTop });
    }
    else {
        popMargTop = ($(window).height() - $('#MsgBox').outerHeight()) / 2;
        $('#MsgBox').css({ 'margin-top': popMargTop });
    }
    

    

    //    $('#MsgBox').css({
    //        'margin-top': popMargTop,
    //        'margin-left': -popMargLeft
    //    });





    var temp = $("body table:first").outerHeight(true) + 20;
    var tempWidth = $("body table:first").outerWidth(true);
    var width = ($(window).width() > tempWidth ? $(window).width() : tempWidth);
    var height = ($(window).height() > temp ? $(window).height() : temp);

    $('body').append('<div id="fade"></div>');
    $('#fade').css({ 'filter': 'alpha(opacity=50)', 'opacity': '.50', 'width': width, 'height': height, 'background': '#000', 'position': 'absolute', 'left': '0', 'top': '0' }).fadeIn();

    $('#MsgBox').fadeIn('medium');
    return false;
}


$('a.close').live('click', function () {
    $('#fade ,#MsgBox').fadeOut(function () {
        $('#fade,#MsgBox').remove();
    });
    return false;

});

function MessageResult(buttonvalue) {
    $('#fade ,#MsgBox').fadeOut(function () {
        $('#fade,#MsgBox').remove();
    });
    document.location.replace(Page + '&MsgResult=' + buttonvalue);
}

$(window).scroll(function () {
    if ($('body,html').has('div[id=MsgBox]')) {
        var popMargTop = ($(window).height() - $('#MsgBox').outerHeight()) / 2 + $(document).scrollTop();
        //var popMargLeft = ($('#MsgBox').width()) / 2 - $(window).scrollLeft();
        var popMargLeft = ($(window).width()) / 2 - 200 + $(window).scrollLeft();
        //        $('#MsgBox').css({
        //            'margin-top': popMargTop,
        //            'margin-left': -popMargLeft
        //        });
        if (!fixedPositionSupport) {
            $('#MsgBox').animate({ marginTop: popMargTop, left: popMargLeft }, 10);
        }
    }
});

function supportsPositionFixed() {
    var w = window,
        ua = navigator.userAgent,
        ret = true;
        
    // Black list the following User Agents
    if (
        // IE less than 7.0
        (/MSIE (\d+\.\d+);/i.test(ua) && RegExp.$1 < 7 || (document.documentMode)<7) ||
        // iOS less than 5
        (/OS [2-4]_\d(_\d)? like Mac OS X/i.test(ua)) ||
        // Android less than 3
        (/Android ([0-9]+)/i.test(ua) && RegExp.$1 < 3) ||
        // Windows Phone less than 8
        (/Windows Phone OS ([0-9])+/i.test(ua) && RegExp.$1 < 8) ||
        // Opera Mini
        (w.operamini && ({}).toString.call( w.operamini ) === "[object OperaMini]") ||
        // Kindle Fire
        (/Kindle Fire/i.test(ua) || /Silk\//i.test(ua)) ||
        // Nokia Symbian, Opera Mobile, wOS
        (/Symbian/i.test(ua)) || (/Opera Mobi/i.test(ua)) || (/wOSBrowser/i.test(ua)) ||
        // Firefox Mobile less than 6
        (/Fennec\/([0-9]+)/i.test(ua) && RegExp.$1 < 6)
        // Optionally add additional browsers/devices here . . .
        ){
        ret = false;
    }
    return ret;
	}