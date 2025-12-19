
$(document).ready(function(){
		var temp=$("body table:first").outerHeight(true);
		if(temp > $(window).height())
		{
					
				$pos=$("table span:first").position();
				var height=$("TABLE span:first").height();
				var width= $("body table:first").width();
				//return top and left positon of element
				var top= $pos.top;
				var left= $pos.left;
				var labelheight=top + height-7;

				$("body").append('<a id="bottom" class="clsScroll" href="#">Bottom</a>');
				$("body").append('<a id="top" class="clsScroll" href="#">Top</a>');
				
				var labelwidth=$("#bottom").innerWidth();
				var labelright=(width - labelwidth);
				
				
				
				$('#bottom').css({'margin-top' :labelheight,'margin-left' :labelright,'background':'transperent'});
				
				$('#top').css({'margin-top' :temp -5,'margin-left' :labelright,'background':'transperent'});
				
		}
	});
	
$(document).ready(function(){
	$("body table:first").resize(function(){onResize();
	});
});
$(document).ready(function(){

	$('#bottom').click(function(){
		$('body').animate({ 'scrollTop': $("body table:first").outerHeight(true)}, 'slow');
		return false;
	});
	$('#top').click(function(){
		$('body').animate({ 'scrollTop': 0}, 'slow');
		return false;
    });
    
}); 

function onResize()
{
	var temp=$("body table:first").outerHeight(true);
		if(temp > $(window).height())
		{
					
				$pos=$("table span:first").position();
				var height=$("TABLE span:first").height();
				var width= $("body table:first").width();
				//return top and left positon of element
				var top= $pos.top;
				var left= $pos.left;
				var labelheight=top + height-7;
				
				var labelwidth=$("#bottom").innerWidth();
				var labelright=(width - labelwidth);

				$("#bottom").show(); // show link
				$("#top").show();   // show link
				
				$('#bottom').css({'margin-top' :labelheight,'margin-left' :labelright,'background':'transperent'});
				
				$('#top').css({'margin-top' :temp -5,'margin-left' :labelright,'background':'transperent'});
}
else {
    $("#bottom").hide(); // hide link
    $("#top").hide();
}
}

$(window).load(function () {

    $('#bottom').click(function () {

        $("html, body").animate({ scrollTop: $(document).height() }, 1000);
        return false;
    });

    $('#top').click(function () {
        $("html, body").animate({ scrollTop: 0 }, 1000);
        return false;
    });

    $("body table:first").resize(function () {
        onResize();
    });
});
