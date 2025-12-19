/*
 * jQuery UI Notification Message
 *
 * Depends:
 *	    ui.core.js
 */



(function($) {
    $.widget("ui.notificationpopup", {

        init: function() {
            $.ui.notificationpopup._bottompost = this.element.css("bottom");
            $.ui.notificationpopup._height = this.element.css("height");
           
        },

        show: function() {
            var o = this.options;
            if (this.element.is(":hidden")) {
                this.element.queue(function() { $.ui.notificationpopup.animations[o.animation](this, o); }).dequeue();

                $("#backgroundPopup").css({ "opacity": "0.7" });
                $("#backgroundPopup").fadeIn("slow"); 
                
                loadedByFunction = true;
            }
        },

        hide: function() {
            this.element.stop(true);
            var o = this.options;
            if (this.element.is(":visible")) {
                this.element.queue(function() { $.ui.notificationpopup.animations[o.animation](this, o); }).dequeue();

                $("#backgroundPopup").fadeOut("slow");
                
                endedByFunction = true;
                loadedByFunction = false;
                document.getElementById("ValueHiddenField").value = "hidden";
            }
        },

        keepstandby: function() {
            var o = this.options;
            if (this.element.is(":hidden")) {
                $.ui.notificationpopup.animations[o.animation](this, o);
            }
        }
    });
    $.ui.notificationpopup._bottompost = "0px";
    $.ui.notificationpopup._css;
    $.extend($.ui.notificationpopup, {
        defaults: {
            // provide a speed for the animation
            speed: 1000,
            // provide a period for the popup to keep showing
            period: 2000,
            // default the animation algorithm to the basic slide
            animation: 'slide'
        },
        animations: {
            standby: function(e, options) {
                
                $(e).css("height", "575px");
                $(e).css("display", "block");
                
            },
            slide: function(e, options) {
                if ($(e).is(":hidden")) {

                    //  animate
                    $anim = $(e).animate({ height: "show" }, options.speed);
                    
                    //  COMMENT this for disable automatic hidding notification

                    //                if(options.period && options.period > 0){
                    //                    $anim.animate({opacity: 1.0}, options.period)
                    //                        .animate({height: "hide"}, options.speed);
                    //                }
                }
                else {
                    $(e).animate({ height: "hide" }, options.speed)
                }

                $(e).css("height", $.ui.notificationpopup._height);
            },
            fade: function(e, options) {
                if ($(e).is(":hidden")) {

                    //  animate
                    $anim = $(e).animate({ opacity: "show" }, options.speed);

                    //  COMMENT this for disable automatic hidding notification

                    if (options.period && options.period > 0) {
                        $anim.animate({ opacity: 1.0 }, options.period)
                        .animate({ opacity: "hide" }, options.speed);
                    }
                }
                else {
                    $(e).animate({ opacity: "hide" }, options.speed);
                }

                $(e).css("opacity", 1.0);
            },
            slidethru: function(e, options) {
                //  set the position and left
                var b = $.ui.notificationpopup._bottompost;
                var h = $.ui.notificationpopup._height;
                if ($(e).is(":hidden")) {
                    //  animate
                    $anim = $(e).animate({ height: "show" }, options.speed);

                    if (options.period && options.period > 0) {
                        $anim.animate({ opacity: 1.0 }, options.period)
                        .animate({ height: "hide", bottom: h }, options.speed)
                        .animate({ bottom: b }, 1);
                    }
                }
                else {
                    $(e).css({ height: h, bottom: b });
                    $(e).animate({ height: "hide", bottom: h }, options.speed)
                    .animate({ bottom: b }, 1);
                }
                $(e).css({ height: h, bottom: b });

            }
        }
    });
})(jQuery);