//this frame work  is developed  by layth morrar --layth.morrar@gmail.com
// and solution architecture is also done by layth morrar--layth.morrar@gmail.com

if (typeof LMYFrameWork == "undefined")
    LMYFrameWork = {};

LMYFrameWork.DataTableWrapper = function () {
    this.blockUiWrapper = new LMYFrameWork.BlockUiWrapper();
    this.commonWrapper = new LMYFrameWork.CommonWrapper();
}

LMYFrameWork.DataTableWrapper.prototype = {
    initServerSide: function (options) {
        var context = this;

        var customOptions = {
            url: '',
            table: '',
            container: 'body',
            autoWidth: false,
            alertsContainer: LMYFrameWork.Constants.alertsContainer,
            order: [[0, "asc"]],
            columns: [],
            customButtons: [],
            functions: [],
            renderComplete: '',
              retriveOnInit: true,
            filter: true,
        }

        $.extend(customOptions, options);

        // if (!customOptions.container)
        //    customOptions.container = 'body';

        // if (!customOptions.autoWidth)
        // customOptions.autoWidth = false;

        var options = {

            "dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><' 't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

            ajax: {
                url: customOptions.url,
                type: 'post',
                dataFilter: function (data) {
                    context.blockUiWrapper.unblockUI(customOptions.container);

                    return context.PrepairDataTableData(data, customOptions.alertsContainer);
                },
                error: function (data) {
                    LMYFrameWork.CommonMethods.handleAlerts(data, customOptions.alertsContainer);
                    context.blockUiWrapper.unblockUI(customOptions.container);
                },
                data: customOptions.data
            },
            processing: true,
            serverSide: true,
            filter: customOptions.filter,
            autoWidth: customOptions.autoWidth,
            columns: customOptions.columns,
            order: customOptions.order,
            responsive: true,
            columnDefs: customOptions.functions,
            lengthMenu: [
               [5, 10, 15, 20, -1],
               [5, 10, 15, 20, "All"] // change per page values here
            ],
            language: {
                processing: "",
                search: Resources.DataTable_search,
                lengthMenu: Resources.DataTable_lengthMenu,
                info: Resources.DataTable_info,
                infoEmpty: Resources.DataTable_infoEmpty,
                infoFiltered: Resources.DataTable_infoFiltered,
                infoPostFix: "",
                loadingRecords: Resources.DataTable_loadingRecords,
                zeroRecords: Resources.DataTable_zeroRecords,
                emptyTable: Resources.DataTable_emptyTable,
                paginate: {
                    first: Resources.DataTable_first,
                    previous: Resources.DataTable_previous,
                    next: Resources.DataTable_next,
                    last: Resources.DataTable_last
                },
                buttons: {
                    copyTitle: Resources.CopyToClipboard,
                    copySuccess: {
                        _: Resources.CopyToClipboard_CopiedNRows,
                        1: Resources.CopyToClipboard_Copied1Rows
                    }
                }
            },
            initComplete: function (settings, json) {
                //LMYFrameWork.Common.init();
            },
            drawCallback: function (settings) {
                if (customOptions.renderComplete)
                    customOptions.renderComplete();

                context.commonWrapper.init(customOptions.table);
            }
            //buttons: [
            //      { extend: 'print', className: 'btn dark btn-outline' },
            //      { extend: 'copy', className: 'btn red btn-outline' },
            //      { extend: 'pdf', className: 'btn green btn-outline' },
            //      { extend: 'excel', className: 'btn yellow btn-outline ' },
            //      { extend: 'csv', className: 'btn purple btn-outline ' },
            //      { extend: 'colvis', className: 'btn dark btn-outline', text: 'Columns' }
            //],
            //,
            //"sScrollY": "200px",
            //"bPaginate": false,

            //"sScrollX": "100%",
            //"sScrollXInner": "120%",
            //"bScrollCollapse": true,
            //Note: if you are applying horizontal scrolling (sScrollX) on a ".table-bordered"
            //you may want to wrap the table inside a "div.dataTables_borderWrap" element

            //"iDisplayLength": 50
        };

        if (!customOptions.retriveOnInit)
            options.deferLoading = 0;

        $.fn.dataTable.Buttons.swfPath = 'assets/plugins/DataTables-1.10.12/extensions/Buttons/swf/flashExport.swf';

        var table = $(customOptions.table).on('preXhr.dt', function (e, settings, data) {
            context.blockUiWrapper.blockUI({
                target: customOptions.container,
                animate: true
            });
        }).DataTable(options);


        if (!customOptions.customButtons)
            customOptions.customButtons = [];

        customOptions.customButtons.push({
            extend: 'print',
            className: 'btn dark btn-outline', text: Resources.Print, exportOptions: {
                columns: function (idx, data, node) {
                    var isVisible = table.column(idx).visible();
                    var isExportable = customOptions.columns[idx].exportable != false;
                    return isVisible && isExportable ? true : false;
                }
            }
        });
        customOptions.customButtons.push({
            extend: 'copy',
            className: 'btn red btn-outline',
            text: Resources.Copy,
            exportOptions: {
                columns: function (idx, data, node) {
                    var isVisible = table.column(idx).visible();
                    var isExportable = customOptions.columns[idx].exportable != false;
                    return isVisible && isExportable ? true : false;
                }
            }
        });
        customOptions.customButtons.push({
            extend: 'pdf',
            className: 'btn green btn-outline',
            text: Resources.PDF,
            exportOptions: {
                columns: function (idx, data, node) {
                    var isVisible = table.column(idx).visible();
                    var isExportable = customOptions.columns[idx].exportable != false;
                    return isVisible && isExportable ? true : false;
                }
            }
        });
        customOptions.customButtons.push({
            extend: 'excel',
            className: 'btn yellow btn-outline ',
            text: Resources.Excel,
            exportOptions: {
                columns: function (idx, data, node) {
                    var isVisible = table.column(idx).visible();
                    var isExportable = customOptions.columns[idx].exportable != false;
                    return isVisible && isExportable ? true : false;
                }
            }
        });
        customOptions.customButtons.push({
            extend: 'csv',
            className: 'btn purple btn-outline ',
            text: Resources.CSV,
            exportOptions: {
                columns: function (idx, data, node) {
                    var isVisible = table.column(idx).visible();
                    var isExportable = customOptions.columns[idx].exportable != false;
                    return isVisible && isExportable ? true : false;
                }
            }
        });
        customOptions.customButtons.push({
            extend: 'colvis',
            className: 'btn dark btn-outline',
            text: Resources.Columns,
            exportOptions: {
                columns: function (idx, data, node) {
                    var isVisible = table.column(idx).visible();
                    var isExportable = customOptions.columns[idx].exportable != false;
                    return isVisible && isExportable ? true : false;
                }
            }
        });

        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: customOptions.customButtons
        });

        if (customOptions.buttonsContainer)
            $(customOptions.buttonsContainer).append(buttons.container().children());
    },
    PrepairDataTableData: function (data, alertsContainer) {
        var context = this;

        var json = JSON.parse(data);

        LMYFrameWork.CommonMethods.handleAlerts(json.baseModel, alertsContainer);

        var dtData = {};

        if (!json.baseModel.HasError) {
            dtData.recordsTotal = json.baseModel.Total;
            dtData.recordsFiltered = json.baseModel.Total;
            dtData.data = json.baseModel.List;
        } else {
            dtData.data = [];
        }

        return JSON.stringify(dtData);
    },
    refreshTable: function (table) {
        $(table).DataTable().ajax.reload();
    },
    initClientSide: function (options) {
        var context = this;

        var customOptions = {
            table: '',
            autoWidth: false,
            order: [[0, "asc"]],
            customButtons: [],
            renderComplete: ''
        }

        $.extend(customOptions, options);

        var options = {
            processing: false,
            serverSide: false,
            autoWidth: customOptions.autoWidth,
            order: customOptions.order,
            responsive: true,
            lengthMenu: [
               [5, 10, 15, 20, -1],
               [5, 10, 15, 20, "All"] // change per page values here
            ],
            language: {
                processing: "",
                search: Resources.DataTable_search,
                lengthMenu: Resources.DataTable_lengthMenu,
                info: Resources.DataTable_info,
                infoEmpty: Resources.DataTable_infoEmpty,
                infoFiltered: Resources.DataTable_infoFiltered,
                infoPostFix: "",
                loadingRecords: Resources.DataTable_loadingRecords,
                zeroRecords: Resources.DataTable_zeroRecords,
                emptyTable: Resources.DataTable_emptyTable,
                paginate: {
                    first: Resources.DataTable_first,
                    previous: Resources.DataTable_previous,
                    next: Resources.DataTable_next,
                    last: Resources.DataTable_last
                },
                buttons: {
                    copyTitle: Resources.CopyToClipboard,
                    copySuccess: {
                        _: Resources.CopyToClipboard_CopiedNRows,
                        1: Resources.CopyToClipboard_Copied1Rows
                    }
                }
            },
            initComplete: function (settings, json) {
                //context.commonWrapper.init(customOptions.container);
            },
        };

        $.fn.dataTable.Buttons.swfPath = 'assets/plugins/DataTables-1.10.12/extensions/Buttons/swf/flashExport.swf';

        table = $(customOptions.table).on('preXhr.dt', function (e, settings, data) {
            context.blockUiWrapper.blockUI({
                target: customOptions.container,
                animate: true
            });
        }).DataTable(options);


        if (!customOptions.customButtons)
            customOptions.customButtons = [];

        customOptions.customButtons.push({
            extend: 'print',
            className: 'md-btn buttons-print', text: Resources.Print, exportOptions: {
                columns: function (idx, data, node) {
                    var isVisible = table.column(idx).visible();
                    var isExportable = customOptions.columns[idx].exportable != false;
                    return isVisible && isExportable ? true : false;
                }
            }
        });
        customOptions.customButtons.push({
            extend: 'copy',
            className: 'md-btn buttons-copy buttons-html5',
            text: Resources.Copy,
            exportOptions: {
                columns: function (idx, data, node) {
                    var isVisible = table.column(idx).visible();
                    var isExportable = customOptions.columns[idx].exportable != false;
                    return isVisible && isExportable ? true : false;
                }
            }
        });
        //customOptions.customButtons.push({
        //    extend: 'pdf',
        //    className: 'md-btn buttons-pdf buttons-html5',
        //    text: Resources.PDF,
        //    exportOptions: {
        //        columns: function (idx, data, node) {
        //            var isVisible = table.column(idx).visible();
        //            var isExportable = customOptions.columns[idx].exportable != false;
        //            return isVisible && isExportable ? true : false;
        //        }
        //    }
        //});
        customOptions.customButtons.push({
            extend: 'excel',
            className: 'md-btn buttons-excel buttons-html5 ',
            text: Resources.Excel,
            exportOptions: {
                columns: function (idx, data, node) {
                    var isVisible = table.column(idx).visible();
                    var isExportable = customOptions.columns[idx].exportable != false;
                    return isVisible && isExportable ? true : false;
                }
            }
        });
        //customOptions.customButtons.push({
        //    extend: 'csv',
        //    className: 'md-btn buttons-csv buttons-html5 ',
        //    text: Resources.CSV,
        //    exportOptions: {
        //        columns: function (idx, data, node) {
        //            var isVisible = table.column(idx).visible();
        //            var isExportable = customOptions.columns[idx].exportable != false;
        //            return isVisible && isExportable ? true : false;
        //        }
        //    }
        //});
        customOptions.customButtons.push({
            extend: 'colvis',
            className: '',
            text: Resources.Columns,
            exportOptions: {
                columns: function (idx, data, node) {
                    var isVisible = table.column(idx).visible();
                    var isExportable = customOptions.columns[idx].exportable != false;
                    return isVisible && isExportable ? true : false;
                }
            }
        });

        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: customOptions.customButtons
        });

        if (customOptions.buttonsContainer) {
            var childrens = buttons.container().children();
            for (x = 0; x < childrens.length; x++) {
                $(customOptions.buttonsContainer).append(childrens[x]);
            }
        }

        //if (customOptions.buttonsContainer)
        //    $(customOptions.buttonsContainer).append(buttons.container());
    },
}
