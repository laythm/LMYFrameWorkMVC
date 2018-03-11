//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.AjaxWrapper = function () {
    this.blockUiWrapper = new LMYFrameWork.BlockUiWrapper();
}

LMYFrameWork.AjaxWrapper.prototype = {
    get: function (settings) {
        var context = this;

        var options = {
            url:'',
            data:{},
            async: true,
            showLoading: true,
            containerId: 'body',
            cache: false,
            successCallBack: '',
            errorCallBack: '',
            alertsContainer: ''
        }
        $.extend(options, settings);

        //if (!options.alertsContainer)
        //    options.alertsContainer = LMYFrameWork.Constants.alertsContainer;

        if (options.async == true && options.showLoading) {
            context.blockUiWrapper.blockUI({
                target: options.container,
                animate: true
            });
        }

        $.ajax(
            {
                url: options.url,
                type: 'get',
                data: options.data,
                cache: options.cache,
                async: options.async,
                success: function (data, status, xhr) {
                    context.blockUiWrapper.unblockUI(options.container);

                    var ct = xhr.getResponseHeader("content-type") || "";
                    if (ct.indexOf('html') > -1) {
                        options.successCallBack(data, LMYFrameWork.Constants.htmlType);
                        //if alertsContainer is not passed then thats mean the caller does not want to show the errors
                        if (options.alertsContainer)
                            LMYFrameWork.CommonMethods.handleAlerts(data, options.alertsContainer);
                    }

                    if (ct.indexOf('json') > -1) {
                        if (!data.baseModel.HasError) {
                            if (options.successCallBack)
                                options.successCallBack(data, LMYFrameWork.Constants.jsonType);
                        } else {
                            if (options.errorCallBack)
                                options.errorCallBack(data, LMYFrameWork.Constants.jsonType);
                        }

                        //if alertsContainer is not passed then thats mean the caller does not want to show the errors
                        if (options.alertsContainer)
                            LMYFrameWork.CommonMethods.handleAlerts(data.baseModel, options.alertsContainer);
                    }
                },
                error: function (data, status, xhr) {
                    context.blockUiWrapper.unblockUI(options.container);
                    var ct = data.getResponseHeader("content-type") || "";

                    if (options.errorCallBack) {
                        if (ct.indexOf('html') > -1) {
                            options.errorCallBack(data, LMYFrameWork.Constants.htmlType);
                        }
                        if (ct.indexOf('json') > -1) {
                            options.errorCallBack(data, LMYFrameWork.Constants.jsonType);
                        }
                    }

                    //if alertsContainer is not passed then thats mean the caller does not want to show the errors
                    if (options.alertsContainer)
                        LMYFrameWork.CommonMethods.handleAlerts(data, options.alertsContainer);

                },
            });
    },
    post: function (settings) {
        var context = this;

        var options = {
            async: true,
            showLoading: true,
            container: 'body',
            cache: false,
            processData: true,
            contentType: true,
            alertsContainer: '',
            successCallBack: '',
            errorCallBack: ''
        }

        $.extend(options, settings);

        if (options.async == true) {
            context.blockUiWrapper.blockUI({
                target: options.container,
                animate: true
            });
        }

        $.ajax(
             {
                 url: options.url,
                 type: 'post',
                 data: options.data,
                 async: options.async,
                 cache: options.cache,
                 processData: options.processData,
                 contentType: options.contentType,
                 success: function (data, status, xhr) {
                     context.blockUiWrapper.unblockUI(options.container);

                     var ct = xhr.getResponseHeader("content-type") || "";
                     if (ct.indexOf('html') > -1) {
                         options.successCallBack(data, LMYFrameWork.Constants.htmlType);
                         //if alertsContainer is not passed then thats mean the caller does not want to show the errors
                         if (options.alertsContainer)
                             LMYFrameWork.CommonMethods.handleAlerts(data, options.alertsContainer);
                     }

                     if (ct.indexOf('json') > -1) {
                         if (!data.baseModel.HasError) {
                             if (options.successCallBack)
                                 options.successCallBack(data, LMYFrameWork.Constants.jsonType);
                         } else {
                             if (options.errorCallBack)
                                 options.errorCallBack(data, LMYFrameWork.Constants.jsonType);
                         }

                         //if alertsContainer is not passed then thats mean the caller does not want to show the errors
                         if (options.alertsContainer)
                             LMYFrameWork.CommonMethods.handleAlerts(data.baseModel, options.alertsContainer);
                     }
                 },
                 error: function (data, status, statusText) {
                     context.blockUiWrapper.unblockUI(options.container);
                     var ct = data.getResponseHeader("content-type") || "";

                     if (options.errorCallBack) {
                         if (ct.indexOf('html') > -1) {
                             options.errorCallBack(data, LMYFrameWork.Constants.htmlType);
                         }
                         if (ct.indexOf('json') > -1) {
                             options.errorCallBack(data, LMYFrameWork.Constants.jsonType);
                         }
                     }

                     //if alertsContainer is not passed then thats mean the caller does not want to show the errors
                     if (options.alertsContainer)
                         LMYFrameWork.CommonMethods.handleAlerts(data.baseModel, options.alertsContainer);
                 },
             });
    }
}