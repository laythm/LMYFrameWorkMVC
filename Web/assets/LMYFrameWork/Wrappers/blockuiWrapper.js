//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.BlockUiWrapper = function () {

}

LMYFrameWork.BlockUiWrapper.prototype = {
    // wrApper function to  block element(indicate loading)
    blockUI: function (settings) {
        var context = this;

       var options = $.extend(true, {
            animate: true,
            target: ''
        }, settings);

        if (options.target && $(options.target)[0] && $(options.target).data()["blockUI.isBlocked"] == 1)
            return;

        var html = '';
        if (options.animate) {
            if (options.target) {
                var elWidth = $(options.target).attr('data-blockuiwidth');
                if (!elWidth)
                    elWidth = $(options.target).width();

                if (elWidth < 125) {
                    elWidth = elWidth - 30;
                    html = '<div style="min-width:' + elWidth + 'px" class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '">' + ' <div style="width:21px" class="block-spinner-bar"><div class="bounce1"></div> </div>' + '</div>';
                } else {
                    html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '">' + '<div class="block-spinner-bar"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div> ' + '</div>';
                }
            }

        } else if (options.iconOnly) {
            html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + this.getGlobalImgPath() + 'loading-spinner-grey.gif" align=""></div>';
        } else if (options.textOnly) {
            html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
        } else {
            html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '"><img src="' + this.getGlobalImgPath() + 'loading-spinner-grey.gif" align=""><span>&nbsp;&nbsp;' + (options.message ? options.message : 'LOADING...') + '</span></div>';
        }

        if (options.target) { // element blocking
            var el = $(options.target);
            var hight = $(options.target).attr('data-blockuiheight');
            if (!hight)
                hight = el.height()
            if (hight <= ($(window).height())) {
                options.cenrerY = true;
            }
            el.block({
                message: html,
                //baseZ: el.css('z-index') + 1,
                baseZ: 100000,
                centerY: options.cenrerY !== undefined ? options.cenrerY : false,
                css: {
                    top: '50%',
                    border: '0',
                    padding: '0',
                    backgroundColor: 'none',
                    left: ''
                },
                overlayCSS: {
                    backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                    opacity: options.boxed ? 0.05 : 0.1,
                    cursor: 'wait'
                }
            });
        } else { // page blocking
            $.blockUI({
                message: html,
                //baseZ: el.css('z-index') + 1,
                baseZ: 100000,
                css: {
                    border: '0',
                    padding: '0',
                    backgroundColor: 'none'
                },
                overlayCSS: {
                    backgroundColor: options.overlayColor ? options.overlayColor : '#555',
                    opacity: options.boxed ? 0.05 : 0.1,
                    cursor: 'wait'
                }
            });
        }
    },

    // wrApper function to  un-block element(finish loading)
    unblockUI: function (target) {
        if (target) {
            $(target).unblock({
                onUnblock: function () {
                    $(target).css('position', '');
                    $(target).css('zoom', '');
                }
            });
        } else {
            $.unblockUI();
        }
    },
}