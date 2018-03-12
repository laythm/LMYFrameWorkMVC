//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.CommonWrapper = function () {
    this.blockUiWrapper = new LMYFrameWork.BlockUiWrapper();
    this.slimScrollWrapper = new LMYFrameWork.SlimScrollWrapper();
}

LMYFrameWork.CommonWrapper.prototype = {
    init: function (container) {
        if (!container)
            container = 'body';

        //container and alerts div mmust f=given as ids 
        var clockPickerWrapper = new LMYFrameWork.ClockPickerWrapper();
        clockPickerWrapper.init(container);

        var datePickerWrapper = new LMYFrameWork.DatePickerWrapper();
        datePickerWrapper.init(container);

        var select2Wrapper = new LMYFrameWork.Select2Wrapper();
        select2Wrapper.init(container);

        var modalWrapper = new LMYFrameWork.ModalWrapper();
        modalWrapper.init(container);

        var validationWrapper = new LMYFrameWork.ValidationWrapper();
        validationWrapper.init(container);

        var slimScrollWrapper = new LMYFrameWork.SlimScrollWrapper();
        slimScrollWrapper.init();

        this.initSubmit(container);
        this.initToolTip(container);
        this.initOnlineUsersStatus(container);
        //this.initPageRedirect();
        this.intiCheckBoxes(container);
    },
    initSubmit: function (container) {
        var context = this;
        $('form', container).each(function () {
            LMYFrameWork.CommonMethods.addFormSubmitEvent(this, function (event, form) {
                LMYFrameWork.CommonMethods.alertOnExit(false);
                if ($(form).attr('data-ajaxSubmit') != 'true') {                
                    context.blockUiWrapper.blockUI({
                        target: 'body',
                        animate: true
                    });
                }
            });
        });
    },
    initToolTip: function (container) {
        $('[data-toggle="tooltip"]', container).tooltip().click(function (e) {
            $(this).tooltip('hide');
        });
    },
    initOnlineUsersStatus: function (container) {
        LMYFrameWork.SignalRWrapper.addCallBackToUserIsOnline(function (userId, isOnline) {
            $('.userStatus[data-userid="' + userId + '"]', container).each(function (i, el) {
                if (isOnline) {
                    //$(el).removeClass('badge-danger');
                    $(el).addClass('badge-success');
                } else {
                    //$(el).addClass('badge-danger');
                    $(el).removeClass('badge-success');
                }
            });
        });

    },
    intiCheckBoxes: function (container) {
        $('input[type="checkbox"]', container).uniform();
        $('input[type="checkbox"]', container).tooltip().click(function (e) {
            var checkBox = this;
            $(checkBox).change(function () {
                $(checkBox).val($(checkBox).is(':checked'));
            });
        });
    },
    initMessagesNotifications: function (messagesUrl) {
        var context = this;

        function getMessageHtml(data, fromUserModel) {
            var date = LMYFrameWork.DateHelper.getStringFromDate(LMYFrameWork.DateHelper.getDateFromJsonDate(data.CreatedAt), LMYFrameWork.Constants.FullDateFormat);
            var message =
                '<li data-userid="' + fromUserModel.Id + '" data-notfrommeandnotread="' + (data.NotFromMeAndNotRead) + '">' +
                '<a href="#" ' + (data.NotFromMeAndNotRead ? 'class="bg-grey"' : '') + '>' +
                '<span class="subject">' +
                '<span class="from">' + fromUserModel.Name + ' </span>' +
                '<span class="time">' + date + ' </span>' +
                '</span>' +
                '   <span class="message"> ' + data.MessageText + ' </span>' +
                '</a>' +
                '</li>';
            return message;
        }

        function renderMessages(container, context) {
            $('#header_inbox_bar_usersLastMessages').empty();
            var ajaxWrapper = new LMYFrameWork.AjaxWrapper();

            ajaxWrapper.get({
                alertsContainer: LMYFrameWork.Constants.alertsContainer,
                container: container,
                showLoading: true,
                url: messagesUrl,
                data: {},
                successCallBack: function (data) {
                    $('#header_inbox_bar_count2').html(data.NotReadCount);
                    $('#header_inbox_bar_count1').html(data.NotReadCount);

                    for (i = 0; i < data.baseModel.List.length; i++) {
                        var message = getMessageHtml(data.baseModel.List[i].LastMessageModel, data.baseModel.List[i]);
                        $('#header_inbox_bar_usersLastMessages').append(message);
                    }

                    context.slimScrollWrapper.initScroller('#header_inbox_bar_usersLastMessages');
                }
            });
        }

        $('#header_inbox_bar_viewMessages').mouseenter(function () {
            renderMessages("#header_inbox_bar_usersLastMessages", context);
        }).click(function () {
            renderMessages("#header_inbox_bar_usersLastMessages", context);
        });

        renderMessages("#header_inbox_bar", context);

        LMYFrameWork.SignalRWrapper.addCallBackToUpdateNotReadCount(function (count) {
            $('#header_inbox_bar_count2').html(count);
            $('#header_inbox_bar_count1').html(count);
        });

        LMYFrameWork.SignalRWrapper.addCallBackToConnectionStatusChanged(function (isConnected) {
            if (isConnected) {
                LMYFrameWork.SignalRWrapper.fireUpdateNotReadCountForCurrentConnection({
                    successCallBack: function () {

                    },
                    errorCallBack: '',
                    alertsContainer: LMYFrameWork.Constants.alertsContainer
                });
            }
        });

    },
    initAutomaticLogOutOnIdle: function (timeOutinMinutes) {
        var box;

        $(document).idle({
            onIdle: function () {
                box = bootbox.alert({
                    message: Resources.Common_UnActiveSessionExpire,
                });
            },
            onActive: function () {
                if (box)
                    box.modal('hide');
            },
            idle: (timeOutinMinutes * 60000),
            keepTracking: true
        });

        //idle on timeOutinMinutes + 10  seconds 
        $(document).idle({
            onIdle: function () {
                window.location = 'Account/Logoff';
            },
            idle: (timeOutinMinutes * 60000) + 10000,
            keepTracking: true
        });
    },
    initAutomaticLogOut: function (url) {
        LMYFrameWork.SignalRWrapper.addCallBackAutomaticLogOut(function () {
            window.location.href = url;
        });
    },
    initPageRedirect: function () {

        //do not use this function becuse it conflicts with alertOnExit method
        var context = this;
        function goodbye(e) {
            context.blockUiWrapper.blockUI({ target: 'body' });
        }
        //window.onbeforeunload = goodbye;
    }
}