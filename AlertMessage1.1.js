function OpenAlert1_1(title, message, width) {
    var wid = width || 400;
         var str = "";
        str = str + '<a class="poplight" href="#?w='+ wid +'" rel="popup_name"></a>';
        str = str + '<div id="popup_name" class="popup_block1_1" align="center">';
        str = str + '<div style="width: ' + wid + 'px; height: auto" align="left">';
        str = str + '<table border="0" cellpadding="0" cellspacing="0" width=100%>';
        str = str + '<tr>';
        str = str + '<td>';  //valign="center" width="80" align="left">';
        //str = str + '<img  title="alert" style="position:relative" alt="alert" src="images/alert_icon.png" width="40" height="40">'
        str = str + '</td>';
        str = str + '<td>';
        str = str + '<table border="0" cellpadding="0" cellspacing="0" width=100%>';
        str = str + '<tr>';
        str = str + '<td>';
        str = str + '<Label ID="lblAlertTitle" for=title Class="clsTitleAlertLabel" width="100%">'+title+'</Label>';
        str = str + '<hr />';
        str = str + '</td>';
        str = str + '</tr>';
        str = str + '<tr>';
        str = str + '<td>';
        str = str + '<Label ID="lblAlertMessage" Class="clsAlertLabel" for="message">' + message + '</Label>';
        str = str + '</td>';
        str = str + '</tr>';
        //        str = str + '<tr>';
        //        str = str + '<td align=center valign="top" width="100%">';
        //       // str = str + '<div style="margin-top:5px">';
        //        str = str + '<input id="alertclose" type=button value="Ok" class="btnClose"/>';
        //       // str = str + '</div>';
        //        str = str + '</td>';
        //        str = str + '</tr>';
        str = str + '</table>';
        str = str + '</td>';
        str = str + '</tr>';
        str = str + '<tr>';
        str = str + '<td align=center valign="top" width="100%" colspan=2>';
        // str = str + '<div style="margin-top:5px">';
        str = str + '<input id="alertclose" type=button value="Ok" width="100%" class="btnClose"/>';
        // str = str + '</div>';
        str = str + '</td>';
        str = str + '</tr>';
        str = str + '</table>';
        str = str + '</div>';
        str = str + '</div>';

        $("body").append(str);
       
         var popID = 'popup_name';
           //var popURL = $('rel=popup_name').attr('href');




           // var query = popURL.split('?');
           // var dim = query[1].split('&amp;');
            var popWidth = wid; //dim[0].split('=')[1];


            //$('#' + popID).fadeIn().css({ 'width': Number(popWidth) }).prepend('<a href="#" class="close"><img src="images/close2.png" style="position:relative;margin: -37 -37 0 395;" class="btn_close" title="Close" alt="Close" /></a>');
            $('#' + popID).fadeIn().css({ 'width': Number(popWidth) }).prepend('<a href="#"><img  title="alert" class="btn_close" style="position:relative;margin: -43 -43 0 -44;float:left;" alt="alert" src="images/alert_icon.png" width="42" height="42"></a>').prepend('<a href="#" class="close"><img src="images/close2.png" style="position:relative;margin: -43 -43 0 ' + (wid) + ';" class="btn_close" title="Close" alt="Close" /></a>');

            var popMargTop = ($(window).height() - $('#' + popID).outerHeight())/2;
            var popMargLeft = ($('#' + popID).width() + 80) / 2;


            $('#' + popID).css({
                'margin-top': popMargTop,
                'margin-left': -popMargLeft
            });

            var temp = $("body table:first").outerHeight(true) + 20;
            var tempWidth = $("body table:first").outerWidth(true);
            var width = ($(window).width() > tempWidth ? $(window).width() : tempWidth);
            var height = ($(window).height() > temp ? $(window).height() : temp);

            $('body').append('<div id="fade"></div>');
           $('#fade').css({ 'filter': 'alpha(opacity=60)', 'width': width, 'height': height, 'background': '#000' }).fadeIn();
           $('.popup_block1_1').fadeIn();
            
            return false;
        }


    $('a.close,#alertclose').live('click', function () {
        $('#fade , .popup_block1_1').fadeOut(function () {
            $('#fade, a.close').remove();
        });
        return false;

    });

    $("#alertclose").live('mouseover', function () {
        $(this).attr('class', 'btnClose_hover');
    });
    $("#alertclose").live('mouseout', function () {
        $(this).attr('class', 'btnClose');
    });
