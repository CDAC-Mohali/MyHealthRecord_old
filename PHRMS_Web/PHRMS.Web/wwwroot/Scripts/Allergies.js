$(function () {
    $("#grid").jqGrid({
        url: "/Allergies/GetAllergiesList",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['Id', 'Name', 'Still Have', 'Start Date'],
        colModel: [
            { key: true, hidden: true, name: 'Id', index: 'Id', editable: true },
            { key: false, name: 'Name', index: 'Name', editable: true },
            { key: false, name: 'till_Have', index: 'strStill_Have', editable: true },
            { key: false, name: 'StartDate', index: 'StartDate', editable: true, formatter: 'date', formatoptions: { newformat: 'd/m/Y' } }],
        pager: jQuery('#pager'),
        rowNum: 10,
        rowList: [10, 20, 30, 40],
        height: '100%',
        viewrecords: true,
        caption: 'Allergies List',
        emptyrecords: 'No records to display',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        autowidth: true,
        multiselect: true
    }).navGrid('#pager', { edit: false, add: false, del: false, search: false, refresh: false },
        {
            // edit options
            zIndex: 100,
            url: '/Allergies/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            // add options
            zIndex: 150,
            url: "/Allergies/Create",
            bSubmit:"Add",
            closeOnEscape: true,
            closeAfterAdd: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            // delete options
            zIndex: 100,
            url: "/Allergies/Delete",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Are you sure you want to delete this task?",
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        });
});