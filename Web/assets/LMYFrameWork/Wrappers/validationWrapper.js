//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.ValidationWrapper = function () {

}

LMYFrameWork.ValidationWrapper.prototype = {
    init: function (container) {
        this.initValidation(container);
    },
    initValidation: function (container) {

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
}