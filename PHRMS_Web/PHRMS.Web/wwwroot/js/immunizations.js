 
$(function () {
        $("#grid").jqGrid({
        url: "/Immunization/GetImmunizationList",
        datatype: "json",
        mtype: "Get",
        colNames: ["ImmunizationsTypeId", 'Immunization', 'Entered By','UserId', 'Taken On', 'Modified On'],
        colModel: [
            { key: true, hidden: true, name: 'ImmunizationsTypeId', index: 'ImmunizationsTypeId' },
            { key: false, name: 'ImmunizationName', index: 'ImmunizationName', editable: false },
            { key: false, name: 'EnteredBy', index: 'EnteredBy', editable: true },
            { key: false, name: 'UserId', index: 'UserId', hidden:true },
            { key: false, name: 'CreatedDate', index: 'CreatedDate', editable: true },
            { key: false,hidden:true, name: 'ModifiedDate', index: 'ModifiedDate', editable: true }
        ],
        pager: jQuery('#pager'),
        rowNum: 10,
        rowList: [10, 20, 30, 40],
        height: 'auto',
        viewrecords: true,
        caption: 'Current Immunizations',
        jsonReader: {
            root:"rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            id:"0"
},
        autowidth: true,
        multiselect: false
        }).navGrid("#pager", { edit: false, add:false, del: false, search: false, refresh: false },
        

    //To Edit the selected data into the grid
    {
        //zIndex: 400,
        //width: 'auto',
        //top: 300,
        //left:200,
        //url: '/Immunization/Edit',
        //closeOnEscape: true,
        //closeAfterEdit: true,
        //recreateForm:true,
        //afterComplete: function (response) {
        //    if (response.responseText) {
        //        alert(response.responseText);
        //    }
        //}
    },
    //To Add the data into the grid and then ultimately into the databases
    {
        zIndex: 400,        
        width: 'auto',
        top: 300,
        left: 200,
        url: '/Immunization/ImmunizationsList',
        //closeOnEscape: true,
        //afterComplete: function (response)
        //{
        //    if (response.responseText) {
        //        alert(response.responseText);
        //    }
        //}

    },
    {//delete options
        zIndex: 600,
        width:"auto",
        top: 300,
        left: 200,
        url: "/Immunization/Delete",
        closeOnEscape: true,
        recreateForm: true,
        msg: "Are you sure you want to delete this hierarchy?",
        afterComplete: function (response) {
            if (response.responseText) {
                alert(response.responseText);
            }
        }


    }, {
        //search
        zIndex: 400,
        top: 300,
        left: 200,
        width:"auto"
    }
    ).navButtonAdd('#pager', {
        caption: "",
        title: "Edit Component",
        id: "Edit",
        modal: true,
        zIndex: 800,
        title:"testing modal",
        buttonicon: "ui-icon-pencil",
        onClickButton: function () {
               
                        alert('testing this worked');
                    $("#popup1").dialog();
                
            
        }
    });
//    $("#divModal").dialog({
//    autoOpen: false,
//    width: 700,
//    modal: true,
//    zIndex: 800,
//    title: "testing modal"
//});
//    $("#btnPopUp").click(function () {
//        $("#divModal").dialog('open');
//    });
       
     

});



