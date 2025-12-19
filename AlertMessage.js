function OpenAlert()
		{
		$(document).ready(function(){
		
    var popID = $('a.poplight[href^=#]').attr('rel');
    var popURL = $('a.poplight[href^=#]').attr('href'); 

    
    var query= popURL.split('?');
    var dim= query[1].split('&amp;');
    var popWidth = dim[0].split('=')[1]; 

    
   $('#' + popID).fadeIn().css({ 'width': Number( popWidth ) }).prepend('<a href="#" class="close"><img src="images/close2.png" style="position:relative;margin: -47 -46 0 405;" class="btn_close" title="Close" alt="Close" /></a>');

    
    var popMargTop = ($(window).height() / 2)- $('#' + popID).height();
    var popMargLeft = ($('#' + popID).width() + 80) / 2;

    
    $('#' + popID).css({
        'margin-top' : popMargTop,
        'margin-left' : -popMargLeft
    });

    var temp = $("body table:first").outerHeight(true) + 20;
    var tempWidth = $("body table:first").outerWidth(true);
    var width = ($(window).width() > tempWidth ? $(window).width() : tempWidth);
    var height = ($(window).height() > temp ? $(window).height() : temp);

    $('body').append('<div id="fade"></div>');
    $('#fade').css({ 'filter': 'alpha(opacity=60)', 'width': width, 'height': height, 'background': '#000' }).fadeIn();

    return false;
 });

$('a.close').live('click', function() {
    $('#fade , .popup_block').fadeOut(function() {
        $('#fade, a.close').remove();
            });
    return false;
		
		});
	}