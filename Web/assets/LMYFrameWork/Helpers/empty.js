//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    var LMYFrameWork = {};

LMYFrameWork.EmptyHelper = {
    isNullOrEmpty: function (val) {
        if (typeof (val) == 'undefined' || val == null || val == '' || val == "" || val.trim() == '' || val.trim() == "")
            return true;

        return false;
    },
}