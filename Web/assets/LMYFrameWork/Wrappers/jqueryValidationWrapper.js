//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};


LMYFrameWork.JqueryValidationWrapper = {
    init: function () {

        if (!this.initlized) {
            this.intiSizeLimit();
            this.initDateValidation();
            this.initPastDateValidation();
        }

        this.initlized = true;
    },
    initValidation: function () {

        function initValidations(form) {
            form = $(form);
            //jquery validate only forms 
            //if you parse div this will disable all the jquery validatorrs in the page
            if (form.is("form")) {

                //disable remove validation for disablevalidation elements
                //$("*[data-disableValidation='true']").removeAttr('data-val-required');
                //$("*[data-disableValidation='true']").removeAttr('data-val-number');
                //$("*[data-disableValidation='true']").removeAttr('data-val-range');
                //$("*[data-disableValidation='true']").removeAttr('data-val-range-max');
                //$("*[data-disableValidation='true']").removeAttr('data-val-range-min');
                //$("*[data-disableValidation='true']").removeAttr('data-val-regex');
                //$("*[data-disableValidation='true']").removeAttr('data-val-regex-pattern');
                //$("*[data-disableValidation='true']").removeAttr('data-val-maxlength-max');
                //$("*[data-disableValidation='true']").removeAttr('data-val-maxlength');

                // $(form).data('validator', null);
                $(form).removeData('validator');
                $(form).removeData('unobtrusiveValidation');

                $("*[data-disableValidation='true']").removeAttr('data-val', '');

                if ($(form).data("validator"))
                    $(form).data("validator").settings.ignore = "*[data-disableValidation='true']";

                $(form).find("*[data-disableVRequiredalidation='true']").each(function (i, e) {
                    //$(e).removeAttr("data-val-required");
                    $(e).rules("remove", "required");
                });

                $.validator.unobtrusive.parse(form);

                //$(form).data('validator', {
                //    ignore: '',
                //    // any other options and/or rules
                //});
                //$.validator.unobtrusive.parse(form);
            }
        }


        //1 init the container  if any
        initValidations(container);

        //2 init each form in container  if any
        $('form', container).each(function () {
            var form = this;
            initValidations(form);
        });

        //3 init first parent form if any
        initValidations($(container).parents("form"));
    },
    intiSizeLimit: function () {
        $.validator.unobtrusive.adapters.addSingleVal("sizelimitation", "maxfilesize");

        $.validator.addMethod("sizelimitation", function (value, element, maxfilesize) {
            if (element && element.files[0]) {
                return element.files[0].size <= maxfilesize;
            }
            return true;
        });

        $.validator.unobtrusive.adapters.addSingleVal("allowdtypesvalidation", "allowdtypes");

        $.validator.addMethod("allowdtypesvalidation", function (value, element, allowdtypes) {

            if (element && element.files[0]) {
                return allowdtypes.indexOf(element.files[0].name.substr(element.files[0].name.lastIndexOf('.') + 1)) > -1;
            }

            return true;
        });

    },
    initDateValidation: function () {
        $.validator.unobtrusive.adapters.addSingleVal("datevalidation", "dateparameter");

        $.validator.addMethod("datevalidation", function (value, element, maxfilesize) {

            if (!value) {
                return true;
            }

            if (isNaN(LMYFrameWork.DateHelper.getDateFromString(value, LMYFrameWork.Constants.DateFormat).getTime())) {  // d.valueOf() could also work
                return false;
            }

            return true;
        });

        jQuery.validator.methods.date = function (value, element) {

            if (!value) {
                return true;
            }

            if (isNaN(LMYFrameWork.DateHelper.getDateFromString(value, LMYFrameWork.Constants.DateFormat).getTime())) {  // d.valueOf() could also work
                return false;
            }

            return true;
        };
    },
    initPastDateValidation: function () {
        $.validator.unobtrusive.adapters.addSingleVal("pastdatevalidation", "pastdateparameter");

        $.validator.addMethod("pastdatevalidation", function (value, element, maxfilesize) {
            if (!value) {
                return true;
            }
            var date = LMYFrameWork.DateHelper.getDateFromString(value, LMYFrameWork.Constants.DateFormat);

            if (isNaN(date.getTime())) {
                return true;
            }

            if (date > LMYFrameWork.DateHelper.getCurrentDate()) {
                return false;
            }

            return true;
        });

        jQuery.validator.methods.date = function (value, element) {
            if (!value) {
                return true;
            }

            if (isNaN(LMYFrameWork.DateHelper.getDateFromString(value, LMYFrameWork.Constants.DateFormat).getTime())) {  // d.valueOf() could also work
                return false;
            }

            return true;
        };
    }
}