//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.MaskWrapper = function () {

}

LMYFrameWork.MaskWrapper.prototype = {
    init: function () {
        $.mask.definitions['T'] = "[AaPp]";
        $.mask.definitions['t'] = "[Mm]";
        //user mask and inputmaskplugin 
        $('.fullDateMask').mask(getMaskFormat(LMYFrameWork.Constants.FullDateFormat));
        $('.timeMask').mask(getMaskFormat(LMYFrameWork.Constants.TimeFormat));
        $('.dateMask').mask(getMaskFormat(LMYFrameWork.Constants.DateFormat));
        $('.decimalMask').inputmask('Regex', {
            regex: "^\[0-9]+(\.\[0-9]+)?$"
        });
 
    },
    getMaskFormat: function (dateFormat) {
        dateFormat = dateFormat.replace('y', '9');
        dateFormat = dateFormat.replace('m', '9');
        dateFormat = dateFormat.replace('d', '9');
        dateFormat = dateFormat.replace('H', '9');
        dateFormat = dateFormat.replace('h', '9');
        dateFormat = dateFormat.replace('M', '9');
        dateFormat = dateFormat.replace('tt', 'Tt');
        return dateFormat;
    },
}