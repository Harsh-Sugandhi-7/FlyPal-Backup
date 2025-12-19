		$(document).ready(function(){
			//$(":submit").click(function(){
			//if ($(":submit").attr('CausesValidation')=='True')
			$(":submit,[href*='__doPostBack']").live("click",function(){
			if ((typeof(Page_ClientValidate) == 'function')){
			if (Page_ValidationActive) {
				if(!ValidatorCommonOnSubmit())
				{
				return false;
				}
			}
			//return true;
		//	else
		//	{
			//if(!Page_ClientValidate())
			//	{
		//		return false;
			//	}
			//}
			//else
			//{
			}

$("body").append('<div id="fadebody" class="clsFadeEffect"></div><div id="LoadMessege" class="clsLoad_ajax"><img width="48" height="48" alt="Loading.." title="Loading.." src="images/Loader.gif" /> </div>'); 
			
			var temp=$("body table:first").outerHeight(true) + 20;
			var tempWidth=$("body table:first").outerWidth(true);
			var width=($(window).width() > tempWidth ? $(window).width(): tempWidth);
			var hieght=($(window).height() > temp ? $(window).height(): temp);
			var popMargTop = ($(window).height()- $('#LoadMessege').outerHeight())/2  +  $(window).scrollTop();
			var popMargLeft = ($(window).width() - $('#LoadMessege').outerWidth())/2 + $(window).scrollLeft();
				//$('body').animate({ 'scrollTop': 0}, 'slow');
				$('#fadebody').css({'filter' : 'alpha(opacity=40)','width': width,'height':hieght}).fadeIn();
				$('#LoadMessege').css({'top' :popMargTop,'margin-left' : popMargLeft,'filter' : 'alpha(opacity=80)'}).fadeIn();
			return true;
		
				//}
			});
			
			
		});