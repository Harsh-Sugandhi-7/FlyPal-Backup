$(document).ready(function () {
    var Mainclas;
    //$(":submit").live("mouseover focusin",function(){
    //$(this).addClass("ButtonHover");
    //$(this).css({'border-width':'2px'});
    var node = document.doctype;
    //if (node == null) {
        $(":submit").hover(function () {
            var wid = $(this).outerWidth();
            var hei = $(this).outerHeight();
            var font = $(this).css('font-size');
            Mainclas = $(this).attr('class');
            $newClass = 'ButtonHover';
            $(this).removeAttr('class');
            $(this).attr('class', $newClass);
            $(this).css({ 'width': wid, 'height': hei, 'font-size': font });
            //});
        }, function () {
            //$(":submit").live("mouseout focusout",function(){
            //$(this).css({'border-width':'1px'});
            $(this).removeClass('ButtonHover');
            $clas = Mainclas;
            $(this).attr('class', $clas);
        });

   // }

    //			$(":text,textarea").live("mouseover focusin",function(){
    //			//$(this).addClass("ButtonHover");
    //			    //$(this).css({'border-width':'2px'});      Commented by Yogita
    //			    $(this).css({ 'border-width': '1px' });
    //			    $(this).css({ 'border-color': '#00FF00' }); 
    //			});
    //			$(":text,textarea").live("mouseout focusout",function(){
    //			    //$(this).css({'border-width':'1px'});      Commented by Yogita
    //			    $(this).css({ 'border-width': '1px' });
    //			    $(this).css({ 'border-color': '#7EADD9' }); 
    //			});
   // if (node == null) {

        $(":text,textarea").live("mouseover focusin", function () {
            //$(this).addClass("ButtonHover");
            $(this).css({'border-width':'2px'});      //Commented by Yogita
            //$(this).addClass('Textboxhover');
            
        });
        $(":text,textarea").live("mouseout focusout", function () {
            $(this).css({'border-width':'1px'});      //Commented by Yogita
            //$(this).removeClass('Textboxhover');

        });

    //}
});