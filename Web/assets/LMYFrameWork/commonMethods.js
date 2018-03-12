//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

//this is a static class 
LMYFrameWork.CommonMethods = {
    handleAlerts: function (data, alertsContainer) {
        alertsContainer = $($(alertsContainer).children("ul")[0]);
        var hasAlert;

        if (data)
            if (data.ErrorsList && data.ErrorsList.length > 0) {
                for (i = 0; i < data.ErrorsList.length; i++) {
                    alertsContainer.append('<li class="alert list-group-item list-group-item-danger"><button class="close pull-right" data-dismiss="alert" aria-label="close"></button>' + data.ErrorsList[i].Message + '</li>');
                }
                hasAlert = true;
            }

        if (data)
            if (data.SuccessesList && data.SuccessesList.length > 0) {
                for (i = 0; i < data.SuccessesList.length; i++) {
                    alertsContainer.append('<li class="alert list-group-item list-group-item-success"><button class="close pull-right" data-dismiss="alert" aria-label="close"></button>' + data.SuccessesList[i].Message + '</li>');
                }
                hasAlert = true;
            }

        if (data)
            if (data.InfoList && data.InfoList.length > 0) {
                for (i = 0; i < data.InfoList.length; i++) {
                    alertsContainer.append('<li class="alert list-group-item list-group-item-info"><button class="close pull-right" data-dismiss="alert" aria-label="close"></button>' + data.InfoList[i].Message + '</li>');
                }
                hasAlert = true;
            }

        if (data)
            if (data.WarningList && data.WarningList.length > 0) {
                for (i = 0; i < data.WarningList.length; i++) {
                    alertsContainer.append('<li class="alert list-group-item list-group-item-warning"><button class="close pull-right" data-dismiss="alert" aria-label="close"></button>' + data.WarningList[i].Message + '</li>');
                }
                hasAlert = true;
            }

        //data.statusText.replace(/\s/g, '') to remove spaces if any 
        if (data) {
            if (data.statusText) {
                var txt = Resources[data.statusText.replace(/\s/g, '')];
                if (!txt)
                    txt = data.statusText;

                alertsContainer.append('<li class="alert list-group-item list-group-item-danger"><button class="close pull-right" data-dismiss="alert" aria-label="close"></button>' + txt + '</li>');
                hasAlert = true;
            } else if (data.message) {
                alertsContainer.append('<li class="alert list-group-item list-group-item-danger"><button class="close pull-right" data-dismiss="alert" aria-label="close"></button>' + data.message + '</li>');
                hasAlert = true;
            }
        }

        if (alertsContainer.length > 0 && hasAlert)
            $(window).scrollTop(alertsContainer.offset().top - 150);
    },
    addFormSubmitEvent: function (form, callBack) {
        $(form).submit(function (event) {
            if ($(this).valid())
                callBack(event, this);
        });
    },
    isFormValid: function (form) {
        return $(form).valid();
    },
    checkBootStrapCheckBox: function (element, checked) {
        if (checked)
            $(element).parent().addClass('checked');
        else
            $(element).parent().removeClass('checked');

        element.checked = checked;
        return element;
    },
    redirectToUrl: function (url) {
        window.location = url;
    },
    alertOnExit: function (enable, message) {
        if (!message)
            message = Resources.ExitWarningMessage;

        function bindExitEvents() {
            $(window).on('beforeunload.exitHandler', function () {
                return 'you are about to loose the data';
            });

            //$('a').bind('click.exitHandler', function (e) {
            //    e.preventDefault();
            //    var el = this;

            //    if (el.href != '#' && !LMYFrameWork.EmptyHelper.isNullOrEmpty(el.href) && el.href != 'javascript:;' && el.href != '#') {
            //        var r = confirm(message);
            //        if (r == true) {
            //            unbindExitEvents();
            //            $(el)[0].click();
            //        }
            //    }
            //});
        }

        function unbindExitEvents() {
            $(window).unbind('beforeunload.exitHandler');
            // $('a').unbind('click.exitHandler');
        }

        if (enable) {
            bindExitEvents();
        }
        else {
            unbindExitEvents();
        }
    },
    printElemnt: function (el) {
        var cloned = $(el).clone();
        var printSection = $('#printSection');
        if (printSection.length == 0) {
            printSection = $('<div id="printSection"></div>')
            $('body').append(printSection);
        }
        printSection.append(cloned);
        var toggleBody = $('body *:visible');
        toggleBody.hide();
        $('#printSection, #printSection *').show();
        window.print();
        printSection.remove();
        toggleBody.show();
    }
}

