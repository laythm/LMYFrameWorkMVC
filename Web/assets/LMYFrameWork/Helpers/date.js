//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    var LMYFrameWork = {};

//this is static class
LMYFrameWork.DateHelper = {
    getMomentDateFormat: function (dateFormat) {
        dateFormat = dateFormat.replace(/y/g, 'Y');
        dateFormat = dateFormat.replace(/d/g, 'D');
        dateFormat = dateFormat.replace(/H/g, 'h');
        dateFormat = dateFormat.replace(/tt/g, 'a');
        return dateFormat;
    },

    getDateFromString: function (dateString, dateFormat) {
        if (dateString) {
            dateFormat = this.getMomentDateFormat(dateFormat);
            return moment(dateString, dateFormat, true)._d;
        }
        return '';
    },

    getStringFromDate: function (date, dateFormat) {
        if (date) {
            dateFormat = this.getMomentDateFormat(dateFormat);
            return moment(date).format(dateFormat);
        }
        return '';
    },

    getDateFromJsonDate: function (jsonDate) {
        if (jsonDate) {
            if ((new Date(jsonDate)).getTime() > 0) {
                return new Date(jsonDate);
            }

            var pattern = /Date\(([^)]+)\)/;
            var results = pattern.exec(jsonDate);
            var dt = new Date(parseFloat(results[1]));
            return dt;
        }

        return '';
    },
    getDateFromUTCDate: function (jsonDate) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(jsonDate);
        var dt = new Date(parseFloat(results[1]));
        return dt;
    },
    getCurrentDate: function () {
        serverDateAtRender = this.getDateFromString(LMYFrameWork.Constants.ServerStringDateAtRender, LMYFrameWork.Constants.FullDateFormat);
        clientDateAtRender = LMYFrameWork.Constants.ClientDateAtRender;

        serverOffset = this.getDateOffsetInMilliseconds(serverDateAtRender, clientDateAtRender);

        return moment().add('milliseconds', serverOffset)._d;
    },
    getDateOffsetInMilliseconds: function (firstDate, secondDate) {
        return moment(firstDate).diff(secondDate);
    }
}