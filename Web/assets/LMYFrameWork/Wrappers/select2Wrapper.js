//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.Select2Wrapper = function () {
    this.blockUiWrapper = new LMYFrameWork.BlockUiWrapper();
}

LMYFrameWork.Select2Wrapper.prototype = {
    init: function (container) {
        var context = this;
        $('.select2', container).each(function () {
            var element = this;
            var options = {};

            if ($(this).attr('placeholder')) {
                options.placeholder = $(element).attr('placeholder')
            }

            if ($(this).attr('data-url')) {
                options.url = $(element).attr('data-url');
            }

            if ($(this).attr('data-alertsContainer')) {
                options.alertsContainer = $(element).attr('data-alertsContainer');
            }

            if ($(this).attr('data-container')) {
                options.container = $(element).attr('data-container');
            } else {
                options.container = container;
            }

            if ($(this).attr('multiple')) {
                options.multiple = true;
            }

            if ($(this).attr('data-val-prop')) {
                options.dataValName = $(element).attr('data-val-prop');
            }

            if ($(this).attr('data-text-prop')) {
                options.dataTextName = $(element).attr('data-text-prop');
            }

            if ($(this).attr('data-data-function')) {
                options.data = window[$(element).attr('data-data-function')]();
            }

            if (!$(this).attr(''))
                context.initSelect2(this, options);
        });
    },
    initSelect2: function (element, options) {
        var context = this;

        var select2options = {
            width: '100%',
            language: LMYFrameWork.Constants.Language,
            allowClear: true,
            placeholder: '',
            multiple: false,
            url: '',
            dataValName: 'Value',
            dataTextName: 'Text',
            alertsContainer: LMYFrameWork.Constants.alertsContainer,
            data: {},
            container: 'body',
            templateSelection: function (data) {
                ///show parent text in selected text
                if (data.selected) {
                    var markup = data.text;
                    var parentOpt = $(data.element).parent('optgroup');
                    if (parentOpt.length > 0)
                        markup = parentOpt.attr('label') + ' - ' + data.text;

                    return markup;
                }

                return data.text;
            }
        }

        $.extend(select2options, options);

        if (select2options.url) {
            select2options.ajax = {
                url: select2options.url,
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var data = {};
                    data.text = params.term;
                    data.page = params.page || 1;
                    data.pageSize = 10;

                    if (select2options.data)
                        for (var prop in select2options.data) {
                            data[prop] = select2options.data[prop];
                        }

                    return data;
                },
                processResults: function (data, params) {
                    // parse the results into the format expected by Select2
                    // since we are using custom formatting functions we do not need to
                    // alter the remote JSON data, except to indicate that infinite
                    // scrolling can be used
                    params.page = params.page || 1;

                    if (select2options.alertsContainer)
                        LMYFrameWork.CommonMethods.handleAlerts(data.baseModel, select2options.alertsContainer);


                    var dtData = {
                        results: [],
                        pagination: { more: true }
                    }

                    if (!data.baseModel.HasError) {
                        var dtData = {
                            results: $.map(data.baseModel.List, function (obj) {
                                return { id: LMYFrameWork.JSONHelper.resolve(select2options.dataValName, obj), text: LMYFrameWork.JSONHelper.resolve(select2options.dataTextName, obj), data: obj };
                            }),
                            pagination: { more: true }
                        };

                        dtData.pagination.more = (params.page * select2options.data.pageSize) < data.baseModel.Total;
                    }

                    return dtData;

                },
                cache: false,
            }

            select2options.minimumInputLength = 1;

            select2options.initSelection = function (element, callback) {
                var data = {
                    id: ''//$(element).val()
                };

                if (!data.id) {
                    if (element.attr('data-selected-val'))
                        data.id = element.attr('data-selected-val');
                    if (element.attr('data-selected-text'))
                        data.text = element.attr('data-selected-text');
                }
                if (!data.id || data.id < 1) {
                    data.id = null;
                } else {
                    var option = document.createElement('option');
                    option.value = data.id;
                    option.innerHTML = data.text;
                    option.selected = true;
                    $(element, select2options.container).append(option)
                }

                callback(data);
            }
        }

        //var $select2 = $(element).select2(select2options);
        $(element, select2options.container).select2(select2options);
        $(element, select2options.container).attr('data-select2', true);

        $(element, select2options.container).on("select2:select", function (e) {
            $(element, select2options.container).trigger('lmyFrameWork.select', [{ event: e, data: e.params.data.data }]);
        });

        $(element, select2options.container).change(function (e, el) {

            var el = $(this);
            var textElement = $('#' + $(el).attr('data-save-text-in'), select2options.container);
       /*     var valueElement = $('#' + $(el).attr('data-save-value-in'))*/;

            if (textElement.length > 0)
                textElement.val(el.children(':selected').text());

            //if (valueElement.length > 0)
            //    valueElement.val(el.val());
        }).change();
    },
    getData: function (element) {
        if ($(element).select2('data'))
            return $(element).select2('data')[0].data;

        return null;
    },
    setSelectedValue: function (element, container, value, text) {
        if ($(element, container).find('option[value="' + value + '"]').length > 0) {
            $(element, container).val(value).trigger("change");
        } else {
            $(element, container)
                .empty() //empty select
                .append($("<option/>") //add option tag in select
                    .val(value) //set value for option to post it
                    .text(text)) //set a text for show in select
                .val(value) //select option of select2
                .trigger("change");
        }
    }
}


//sample 
            //ModelState.Remove(contactModel.nameof(x => x.Photo));
            //ModelState.ClearByModelName(contactModel.nameof(x => x.FatherModel));
//ModelState.ClearByModelName(contactModel.nameof(x => x.MotherModel));

//    <div class="uk-width-medium-1-3">
//                                    <div class="md-input-wrapper md-input-filled">
//                                        @Html.LabelFor(model => model.MotherID)
//                                        @Html.HiddenFor(model => model.MotherModel.Name)
//                                        @Html.DropDownListFor(model => model.MotherID, Enumerable.Empty<SelectListItem>(), " ",
//    htmlAttributes: new
//{
//                                               @class = "select2",
//                                               @data_url = Url.Action("GetContactsBySelectParameters", "Contact", new { gender = true }),
//                                               @data_val_prop = "ContactID",
//                                               @data_text_prop = "Name",
//                                               @data_selected_text = Model.MotherModel.Name,
//                                               @data_selected_val = Model.MotherID,
//                                               @data_save_text_in = Html.IdFor(model => model.MotherModel.Name),
//    })
//                                        <span class="md-input-bar"></span>
//                                    </div>
//                                    <div class="parsley-errors-list filled" id="parsley-id-5">
//                                        @Html.ValidationMessageFor(model => model.MotherID, "", new { @class = "parsley-required" })
//                                    </div>
//                                </div>





//                            <div class="uk-form-row">
//                                <div class="md-input-wrapper md-input-filled">
//                                    @Html.LabelFor(model => model.CityID)
//                                    @Html.HiddenFor(model => model.CityModel.Name)
//                                    @Html.DropDownListFor(model => model.CityID, Enumerable.Empty<SelectListItem>(), " ",
//    htmlAttributes: new
//{
//                                               @class = "select2",
//                                               @data_url = Url.Action("GetCitiesBySelectParameters", "Common"),
//                                               @data_val_prop = "CityID",
//                                               @data_text_prop = "Name",
//                                               @data_selected_text = Model.CityModel.Name,
//                                               @data_selected_val = Model.CityID,
//                                               @data_save_text_in = Html.IdFor(model => model.CityModel.Name),
//                                               @data_data_function = "getCountryData"
//                            })
//                                    <span class="md-input-bar"></span>
//                                </div>
//                                <div class="parsley-errors-list filled" id="parsley-id-5">
//                                    @Html.ValidationMessageFor(model => model.CityID, "", new { @class = "parsley-required" })
//                                </div>
//                            </div>


//function init() {
//    $("#" + "@Html.IdFor(model => model.CountryID)").change(function () {
//        $("#" + "@Html.IdFor(model => model.CityID)").val('').change()
//    });
//}
//function getCountryData() {
//    return {
//        CustomObject: function () {
//            return $("#" + "@Html.IdFor(model => model.CountryID)").val() ? $("#" + "@Html.IdFor(model => model.CountryID)").val() : null;
//        }
//    }
//}
//init();