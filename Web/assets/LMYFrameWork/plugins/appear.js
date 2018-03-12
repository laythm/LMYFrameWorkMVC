//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

(function ($) {
    $.fn.appear = function (options) {

        if (typeof options === 'object') {
            var defaults = {
                parentElement: window,
                shownCallback: function () { },
                hiddenCallback: function () { }
            };


        } else if (typeof options === 'string') {
            if (options == 'destroy') {
                $(el).removeAttr('data-appear');

                return this;
            }
        }

        var settings = $.extend({}, defaults, options);

        function isScrolledTo(el, parentElement) {
            var viewTop = $(parentElement).scrollTop();
            var viewBottom = viewTop + $(parentElement).height();

            var elemTop = $(el).offset().top;
            var elemBottom = elemTop + $(el).height();

            return ((elemBottom >= viewTop) && (elemTop <= viewBottom));
        }

        function onScrolledTo(el, parentElement, shownCallback, hiddenCallback) {
            var isVisible = false;

            $(parentElement).scroll(function () {
                if ($(el).attr('data-appear') == 'true') {
                    if ($(el).is(':visible')) {
                        if (isScrolledTo(el, parentElement)) {
                            if (!isVisible) {
                                isVisible = true;
                                if (shownCallback) (shownCallback(el));
                            }
                        } else {
                            if (isVisible) {
                                isVisible = false;
                                if (hiddenCallback) (hiddenCallback(el));
                            }
                        }
                    }
                }
            }).scroll();
        }

        return this.each(function () {
            el = this;
            $(el).attr('data-appear', 'true');

            //assign scroll event on parent elment to check if scroll will show the element then
            onScrolledTo(el, settings.parentElement, settings.shownCallback, settings.hiddenCallback);

            //if the element is shown and scrolled to then showncall back
            $(el).bind('show', function () {
                if (isScrolledTo(el, settings.parentElement)) {
                    if (settings.shownCallback) (settings.shownCallback(el));
                }
            });
        });
    };

}(jQuery));