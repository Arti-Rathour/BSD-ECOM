function Savemaincategory() {
    var id = $('#id').val();
    var SuperCategory = $('#txtSuperCategory').val();
    var ServicesId = $('#ddlServices').find("option:selected").val();
    var ServicesText = $('#ddlServices').find("option:selected").text();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var msg = ValidationMainCategory();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/MainCategory',
            data: { id: id, SuperCategory: SuperCategory, ServicesId: ServicesId, Status: Status },
            dataType: "JSON",
            success: function (result) {
                if (result.message == "Main Category added.") {
                    if (result.id == 0) {
                        alert("Main Category added successfully.");
                    }
                    else {
                        alert("Main Category modify successfully.");
                    }
                    ClearAllField();
                    ShowDataInTable();
                }
            },
            error: function () {
                alert("Main Category not added.");
            }
        });
    }
    else {
        alert(msg);
    }
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'MainCategory');
}
function Clear1() {
    ReloadPageWithAreas('Admin', 'Admin', 'MainCategoryReport');
}
function ClearAllField() {
    $("#btnSave").val("Save");
    $('#id').val("0");
    $('#txtSuperCategory').val("");
    $('#ddlServices').val(0);
    //$('#ddlServices').find("option:selected").text();
    $('#chkStatus').is(':checked',true);
}
function ServicesBind() {
    $("#ddlServices").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: "/Admin/Admin/ServiceBind", //url to be called
        success: function (json, result) {
            $("#ddlServices").empty();
            json = json || {};
            $("#ddlServices").append('<option value="0">Select Service</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlServices").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            // GetPatientData();
            $("#ddlServices").prop("disabled", false);
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}
function ShowDataInTable() {
    var ServicesId = $('#ddlServices').find("option:selected").val();
    $.ajax({
        url: "/Admin/Admin/ShowAllMainCategory",
        type: 'POST',
        data: { ServicesId: ServicesId },
        dataType: 'JSON',
       // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.main_cat_id + '</td>';
                html += '<td>' + item.service + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                if ((item.main_cat_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                // html += '<td>' + item.main_cat_status + '</td>';
               // html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbymaincategory(' + item.main_cat_id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Deletebymaincategory(' + item.main_cat_id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '<td><a href="/Admin/Admin/MainCategoryReportFile?main_cat_id=' + item.main_cat_id + '"><i class="fa fa-eye"></i></a></td>';
                html += '</tr>';
                index++;
            });
            if (html != "") {
                $('.tbodyData').html(html);
            }
            else {
                $('.tbodyData').html("No data found for this services");
            }
        }
    });
}
$("#example1").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var main_cat_id = currentRow.find("td:eq(0)").html();
    var main_cat_status = currentRow.find("td:eq(3)").html();
    Editbyid(main_cat_id, main_cat_status);
});

function Editbymaincategory(main_cat_id, main_cat_status) {
    var PetObj = JSON.stringify({ main_cat_id: main_cat_id });
    $.ajax({
        url: "/Admin/Admin/EditMainCategory",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.main_cat_id);
            $('#ddlServices').val(result.services);
            $('#txtSuperCategory').val(result.main_cat_name);
            if ((result.main_cat_status) == 1) {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
           // $('#chkStatus').val(result.main_cat_status);
        },
        error: function () {
            alert("Data Not Founf !!");
        }
    });
}
function Deletebymaincategory(main_cat_id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ main_cat_id: main_cat_id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteMainCategory",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    alert("Delete Success");
                    ShowDataInTable();
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
    else {
        return false;
    }
}
function ValidationMainCategory() {
    var msg = "";
    if ($('#txtSuperCategory').val() == "") { msg += "Super Category can not left Blank !! \n"; }
    if ($('#ddlServices').val() == 0) { msg += "Services can not left Blank !! \n"; }
    //var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    //if (Status == 0) { msg += "Status can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;
}


//make by kamni 
function ShowDataInTableReport() {
    $.ajax({
        url: "/Admin/Admin/ShowMainCategoryReport",

        dataType: 'JSON',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.main_cat_id + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                if ((item.main_cat_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '<td>' + item.entry_date + '</td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}

function Searchbyname() {
    var main_cat_name = $('#txtSuperCategory').val();
    $.ajax({
        url: "/Admin/Admin/SearchMainCategoryReport",
        data: { main_cat_name: main_cat_name },
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.main_cat_id + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                /*  html += '<td>' + item.services + '</td>';*/
                if ((item.main_cat_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '<td>' + item.entry_date + '</td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        },
        error: function () {
            alert("Data Not Founf !!");
        }
    });
}
function ReTableBind() {
    var main_cat_name = $('#txtSuperCategory').val();
    if (main_cat_name == "") {
        ShowDataInTableReport();
    }
}