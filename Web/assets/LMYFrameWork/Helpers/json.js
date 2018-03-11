//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    var LMYFrameWork = {};

LMYFrameWork.JSONHelper = {
    stringifiyJSON: function (json) {
        return JSON.stringify(json).replaceAll("'", LMYFrameWork.Constants.SingleQuoteReplacement).replaceAll('"', LMYFrameWork.Constants.DoubleQuoteReplacement);
    },
    parseJSON: function (str) {
        return JSON.parse(str.replaceAll(LMYFrameWork.Constants.SingleQuoteReplacement, '\'').replaceAll(LMYFrameWork.Constants.DoubleQuoteReplacement, "\""));
    },
    resolve: function (path, obj) {
        return path.split('.').reduce(function (prev, curr) {
            return prev ? prev[curr] : undefined
        }, obj || self)
    }
}
