//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.ModalWrapper = function () {
    this.ajaxWrapper = new LMYFrameWork.AjaxWrapper();
}

LMYFrameWork.ModalWrapper.prototype = {
    init: function (container) {
        var context = this;

        $("a[data-modal]", container).unbind("click.modal");
        $("a[data-modal]", container).on("click.modal", function (e) {
            var button = this;

            var modelContainer = button.getAttribute('data-modalContainer');
            var modelalertsContainer = button.getAttribute('data-alertsContainer');

            context.showModal({
                modelContainer: modelContainer,
                modelalertsContainer: modelalertsContainer,
                url: button.href,
                successCallBack: function (jsonObject) {
                    $(button).trigger('success', [{ successData: jsonObject }]);
                }
            });

            return false;
        });
    },
    showModal: function (settings) {
        var context = this;

        var options = {
            modelContainer: 'body',
            modelalertsContainer: LMYFrameWork.Constants.alertsContainer,
            url: '',
            isPost: false,
            data: {},
            successCallBack: function () { }
        }

        $.extend(options, settings);

        if (!options.modelContainer)
            options.modelContainer = 'body';
        if (!options.modelalertsContainer)
            options.modelalertsContainer = LMYFrameWork.Constants.alertsContainer;

        function onReturnSuccess(html) {
            $(options.modelContainer).append(html);
            var dialogId = $(html)[0].id;

            context.bindForm(options.modelContainer, options.modelalertsContainer, dialogId, options.successCallBack);
        }

        if (options.isPost) {
            context.ajaxWrapper.post({
                url: options.url,
                async: true,
                data: options.data,
                alertsContainer: options.modelalertsContainer,
                container: options.modelContainer,
                successCallBack: onReturnSuccess
            });
        } else {
            context.ajaxWrapper.get({
                url: options.url,
                async: true,
                data: options.data,
                alertsContainer: options.modelalertsContainer,
                container: options.modelContainer,
                successCallBack: onReturnSuccess
            });
        }
    },
    bindForm: function (modelContainer, modelalertsContainer, dialogId, successCallBack) {
        var context = this;

        //$($("#" + dialogId)).on('LMYFrameWorkModal.hide', function () {
            
        //});

        //below to remove any attached submit event (because this submit is will be handled below )
        $('form', "#" + dialogId).each(function (i, formToBind) {
            if ($(formToBind).attr('data-ajaxSubmit') == 'true') {
                LMYFrameWork.CommonMethods.addFormSubmitEvent(formToBind, function (event, form) {
                    event.preventDefault();

                    var formData = new FormData(form);

                    context.ajaxWrapper.post({
                        url: form.action,
                        async: true,
                        data: formData,
                        processData: false,
                        contentType: false,
                        container: $($("#" + dialogId)[0]).attr('data-container'), //$(dialogId).id,
                        successCallBack: function (data, returnType) {

                            $('#' + dialogId).trigger('LMYFrameWorkModal.hide');

                            if (returnType == LMYFrameWork.Constants.htmlType) {
                                //you can see below attributes using in delete user model

                                var hassuccess;
                                if ($(data).find('[data-hassuccess]').length > 0) {
                                    hassuccess = $(data).find('[data-hassuccess]')[0].getAttribute('data-hassuccess');
                                    hassuccess = JSON.parse(hassuccess);
                                }

                                var hidemodal;
                                if ($(data).find('[data-hidemodal]').length > 0) {
                                    hidemodal = $(data).find('[data-hidemodal]')[0].getAttribute('data-hidemodal');
                                    hidemodal = JSON.parse(hidemodal);
                                }

                                var jsonObject;
                                if ($(data).find('[data-successjson]').length > 0) {
                                    try {
                                        jsonObject = JSON.parse($(data).find('[data-successjson]')[0].getAttribute('data-successjson'));
                                    } catch (ex) {
                                    }
                                }

                                //if has success then trigger succes 
                                if (hassuccess) {
                                    successCallBack(jsonObject);
                                }

                                //if hide modal then handle alerts on container alerts div 
                                if (hidemodal) {
                                    LMYFrameWork.CommonMethods.handleAlerts(jsonObject, modelalertsContainer);
                                } else {
                                    $(modelContainer).append(data);

                                    var newDialogId = $(data)[0].id;

                                    context.bindForm(modelContainer, modelalertsContainer, newDialogId, successCallBack);
                                }

                            }

                            if (returnType == LMYFrameWork.Constants.jsonType) {
                                //here handle errors for the model container alerts div
                                LMYFrameWork.CommonMethods.handleAlerts(data.baseModel, modelalertsContainer);

                                // handle json here
                                successCallBack(jsonObject);
                                // $(button).trigger('success', [{ successData: data }]);
                            }
                        },
                        errorCallBack: function (data, returnType) {

                            if (returnType == LMYFrameWork.Constants.htmlType) {
                                $($("#" + dialogId).find('.modalbody')[0]).html(data.responseText);
                            }
                            if (returnType == LMYFrameWork.Constants.jsonType) {
                                $('#' + dialogId).trigger('LMYFrameWorkModal.hide'); //$("#" + dialogId).modal('hide');

                                //here handle errors for the model container alerts div
                                if (data.baseModel)
                                    LMYFrameWork.CommonMethods.handleAlerts(data.baseModel, modelalertsContainer);
                                else
                                    LMYFrameWork.CommonMethods.handleAlerts(data, modelalertsContainer);
                            }
                        }
                    });

                    return false;
                });
            }
        });
        //if ($('form', "#" + dialogId).attr('data-ajaxSubmit') == 'true') {
        //    $('form', "#" + dialogId).each(function () {

        //    })
        //}
    },


}