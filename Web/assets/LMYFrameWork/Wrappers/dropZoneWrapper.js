//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.DropZoneWrapper = function () {
    this.blockUiWrapper = new LMYFrameWork.BlockUiWrapper();
}
LMYFrameWork.DropZoneWrapper.prototype = {
    init: function (settings) {
        var context = this;
        var options = {
            element: '',
            container: 'body',
            successCallBack: '',
            errorCallBack: '',
            alertsContainer: '',
            saveUrl: '',
            deleteUrl: '',
            maxFiles: 10,
            alertsContainer: LMYFrameWork.Constants.alertsContainer,
            acceptedFileTypes: "",
            parallelUploads: 5,
            invalidFileTypeMessage: Resources.InvalidFileTypeMessage,
            maxFilesExceeded: Resources.MaxFilesExceeded,
            saveFilesHtmlModelToElement: '',
            htmlModelFileShortPathTemplate: 'Files[indexCounterReplacment].FileShortPath',
            htmlModelFileSizeTemplate: 'Files[indexCounterReplacment].FileSize',
            htmlModelFileNameTemplate: 'Files[indexCounterReplacment].FileName',
            preFiles: []
        }
        $.extend(options, settings);

        options.invalidFileTypeMessage = options.invalidFileTypeMessage.replace('{types}', options.acceptedFileTypes)

        Dropzone.autoDiscover = false;
        $(options.element).each(function (i, el) {

            var fileList = new Array;
            var i = 0;
            //or use  new Dropzone($(this).get(0), {url: "...."});
            //or  Dropzone.forElement("#DropzoneTarget_1").destroy();
            $(el).dropzone({
                addRemoveLinks: true,
                maxFiles: options.maxFiles, //change limit as per your requirements
                dictMaxFilesExceeded: options.maxFilesExceeded,
                acceptedFiles: options.acceptedFileTypes,
                dictInvalidFileType: options.invalidFileTypeMessage,
                init: function () {
                    if (options.preFiles) {
                        for (i = 0; i < options.preFiles.length; i++) {
                            var file = {
                                name: options.preFiles[i].FileName,
                                size: options.preFiles[i].Size,
                                status: Dropzone.ADDED,
                                accepted: true
                            };
                            this.emit("addedfile", file);
                            //  this.emit("thumbnail", file, value.path);
                            this.emit("complete", file);
                            this.files.push(file);
                            fileList.push(options.preFiles[i]);
                            //this.options.thumbnail.call(this, mockFile, "http://someserver.com/myimage.jpg");
                        }

                        context.renderHtmlModelOfFiles(fileList, options.saveFilesHtmlModelToElement, options.htmlModelFileShortPathTemplate, options.htmlModelFileSizeTemplate, options.htmlModelFileNameTemplate);
                    }

                    // Hack: Add the dropzone class to the element
                    //$(this.element).addClass("dropzone");
                    this.on("success", function (file, data) {
                        if (!data.baseModel.HasError) {
                            if (options.successCallBack)
                                options.successCallBack(data);

                            fileList[i] = data.baseModel;
                            i += 1;
                            $('.dz-message').show();

                            context.renderHtmlModelOfFiles(fileList, options.saveFilesHtmlModelToElement, options.htmlModelFileShortPathTemplate, options.htmlModelFileSizeTemplate, options.htmlModelFileNameTemplate);
                        } else {
                            if (options.errorCallBack)
                                options.errorCallBack(data);

                            this.removeFile(file);
                        }

                        if (options.alertsContainer)
                            LMYFrameWork.CommonMethods.handleAlerts(data.baseModel, options.alertsContainer);
                    });

                    this.on("error", function (file, error, response, v) {
                        if (options.errorCallBack) {
                            options.errorCallBack(response);
                        }

                        //if alertsContainer is not passed then thats mean the caller does not want to show the errors
                        if (options.alertsContainer)
                            LMYFrameWork.CommonMethods.handleAlerts(response, options.alertsContainer);
                        this.removeFile(file);
                    });

                    this.on("removedfile", function (file) {
                        var rmvFileIndex = context.getFileIndexFromArray(fileList, file);

                        if (rmvFileIndex > -1) {
                            LMYFrameWork.AjaxWrapper.get({
                                date: { fileid: fileList[rmvFileIndex].ID },
                                async: true,
                                showLoading: false,
                                container: settings.container,
                                alertsContainer: options.alertsContainer,
                                successCallBack: function () {
                                }
                            });

                            fileList.splice(rmvFileIndex, 1);

                            context.renderHtmlModelOfFiles(fileList, options.saveFilesHtmlModelToElement, options.htmlModelFileShortPathTemplate, options.htmlModelFileSizeTemplate, options.htmlModelFileNameTemplate);
                        }
                    });
                }
            });
        });
    },

    getFileIndexFromArray: function (array, file) {
        var foundFileIndex = -1;
        for (var f = 0; f < array.length; f++) {
            if (array[f].fileName == file.name) {
                foundFileIndex = f;
            }
        }
        return foundFileIndex;
    },

    renderHtmlModelOfFiles: function (array, saveFilesHtmlModelToElement, htmlModelFileShortPathTemplate, htmlModelFileSizeTemplate, htmlModelFileNameTemplate) {
        $(saveFilesHtmlModelToElement).empty();

        for (i = 0; i < array.length; i++) {
            $(saveFilesHtmlModelToElement).append('<input type="hidden" value="' + array[i].FileShortPathTemplate + '"  name="' + htmlModelFileShortPathTemplate.replace('indexCounterReplacment', i) + '"/>');
            $(saveFilesHtmlModelToElement).append('<input type="hidden" value="' + array[i].FileSize + '"  name="' + htmlModelFileSizeTemplate.replace('indexCounterReplacment', i) + '"/>');
            $(saveFilesHtmlModelToElement).append('<input type="hidden" value="' + array[i].FileName + '"  name="' + htmlModelFileNameTemplate.replace('indexCounterReplacment', i) + '"/>');
        }
    }
}