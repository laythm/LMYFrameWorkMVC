//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com


if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.SlimScrollWrapper = function () {

}

LMYFrameWork.SlimScrollWrapper.prototype = {
    init: function () {
        var context = this;

        $('.slimscroller').each(function () {
            context.initScroller(this);
        });
    },

    initScroller: function (el, scollingEventCallBack, startPos) {
        var context = this;

        $(el).each(function () {
            context.destroySlimScroll(el);

            var height;

            if ($(this).attr("data-height")) {
                height = $(this).attr("data-height");
            } else {
                height = $(this).css('height');
            }

            var options = {
                allowPageScroll: true, // allow page scroll when the element scroll is ended
                size: '7px',
                color: ($(this).attr("data-handle-color") ? $(this).attr("data-handle-color") : '#bbb'),
                wrapperClass: ($(this).attr("data-wrapper-class") ? $(this).attr("data-wrapper-class") : 'slimScrollDiv'),
                railColor: ($(this).attr("data-rail-color") ? $(this).attr("data-rail-color") : '#eaeaea'),
                position: LMYFrameWork.Constants.IsRTL ? 'left' : 'right',
                height: height,
                alwaysVisible: ($(this).attr("data-always-visible") == "1" ? true : false),
                railVisible: ($(this).attr("data-rail-visible") == "1" ? true : false),
                disableFadeOut: true
            }

            if ($(this).attr("data-scroll-start")) {
                options.start = $(this).attr("data-scroll-start");
            } else if (startPos) {
                options.start = startPos;
            }

            $(this).slimScroll(options).bind('slimscrolling', function (event, pos) {
                if (scollingEventCallBack)
                    scollingEventCallBack(event, pos);
            });

            $(this).attr("data-scroll-initialized", "1");
        });
    },

    destroySlimScroll: function (el) {
        $(el).each(function () {
            if ($(this).attr("data-scroll-initialized") === "1") { // destroy existing instance before updating the height
                $(this).removeAttr("data-scroll-initialized");
                $(this).removeAttr("style");

                var attrList = {};

                // store the custom attribures so later we will reassign.
                if ($(this).attr("data-handle-color")) {
                    attrList["data-handle-color"] = $(this).attr("data-handle-color");
                }
                if ($(this).attr("data-wrapper-class")) {
                    attrList["data-wrapper-class"] = $(this).attr("data-wrapper-class");
                }
                if ($(this).attr("data-rail-color")) {
                    attrList["data-rail-color"] = $(this).attr("data-rail-color");
                }
                if ($(this).attr("data-always-visible")) {
                    attrList["data-always-visible"] = $(this).attr("data-always-visible");
                }
                if ($(this).attr("data-rail-visible")) {
                    attrList["data-rail-visible"] = $(this).attr("data-rail-visible");
                }

                $(this).slimScroll({
                    wrapperClass: ($(this).attr("data-wrapper-class") ? $(this).attr("data-wrapper-class") : 'slimScrollDiv'),
                    destroy: true
                });

                var the = $(this);

                // reassign custom attributes
                $.each(attrList, function (key, value) {
                    the.attr(key, value);
                });

            }
        });
    },

    scrollTo: function (el, offeset) {
        var pos = (el && el.size() > 0) ? el.offset().top : 0;

        if (el) {
            if ($('body').hasClass('page-header-fixed')) {
                pos = pos - $('.page-header').height();
            } else if ($('body').hasClass('page-header-top-fixed')) {
                pos = pos - $('.page-header-top').height();
            } else if ($('body').hasClass('page-header-menu-fixed')) {
                pos = pos - $('.page-header-menu').height();
            }
            pos = pos + (offeset ? offeset : -1 * el.height());
        }

        $('html,body').animate({
            scrollTop: pos
        }, 'slow');
    },

    scrollElemntTo: function (el, pos) {
        if (!pos)
            pos = 'bottom';

        if (pos == 'bottom')
            pos = $(el)[0].scrollHeight;
        else if (pos == 'start')
            pos = 0;

        $(el).scrollTop(pos);


        //$(el).animate({
        //    scrollTop: pos
        //}, 100);
    },
}


