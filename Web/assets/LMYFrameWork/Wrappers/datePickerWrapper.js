//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.DatePickerWrapper = function () {

}

LMYFrameWork.DatePickerWrapper.prototype = {
    init: function (container) {
        //for future datepicker and past use mvc validation see max filesize validation
        var context = this;
        $('.datePicker', container).each(function () {
            context.initDatePicker(this);
        });
    },
    getDatePickerDateFormat: function (dateFormat) {
        dateFormat = dateFormat.replace(/M/g, 'm');

        return dateFormat;
    },
    initDatePicker: function (element, options) {
        var context = this;
        var datepicker = $(element).datepicker({
            autoclose:true,
            format: context.getDatePickerDateFormat(LMYFrameWork.Constants.DateFormat)
        });
    }
}