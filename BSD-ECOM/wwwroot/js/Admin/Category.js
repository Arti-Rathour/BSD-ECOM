function Savecategory() {
    var id = $('#Categoryid').val();
    var Category = $('#txtCategory').val();
    var SuperCategoryId = $('#ddlSuCategory').find("option:selected").val();
    var SuperCategoryText = $('#ddlSuCategory').find("option:selected").text();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var msg = ValidationCategory();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/Category',
            data: { id: id, Category: Category, SuperCategoryId: SuperCategoryId, Status: Status },
            dataType: "JSON",
            success: function (result) {
                if (result.message == "Category added.") {
                    if (result.id == 0) {
                        alert("Category added successfully.");
                    }
                    else {
                        alert("Category modify successfully.");
                    }
                    clearAllField();
                    ShowDataInTable();
                }
            },
            error: function () {
                alert("Some thing error.");
            }
        });
    }
    else {
        alert(msg);
    }
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'Category');
}
function Clear1() {
    ReloadPageWithAreas('Admin', 'Admin', 'CategoryReport');
}
function clearAllField() {
    $('#Categoryid').val("0");
    $("#btnSave").val("Save");
    $('#Categoryid').val("0");
    $('#txtCategory').val("");
    $('#ddlSuCategory').val("0");
    //$('#ddlSuCategory').find("option:selected").text();
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
    var SuperCategoryId = $('#ddlSuCategory').find("option:selected").val();
    $.ajax({
        url: "/Admin/Admin/ShowAllCategory",
        dataType: 'JSON',
        type: 'POST',
        data: { SuperCategoryId: SuperCategoryId},
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.category_id + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                html += '<td>' + item.category_name + '</td>';
                if ((item.cat_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                //html += '<td>' + item.cat_status + '</td>';
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                //html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyid(' + item.category_id + ')"><i class="fa fa-edit"></i></a></td>';
             /*   html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyIId(' + item.category_id + ')"><i class="fa fa-trash-o"></i></a></td>';*/
                html += '<td><a href="/Admin/Admin/CategoryReportFile?category_id=' + item.category_id + '"><i class="fa fa-eye"></i></a></td>';
                html += '</tr>';
                index++;
            });
            if (html != "") {
                $('.tbodyData').html(html);
            }
            else {
                $(".tbodyData").html("No data found for this Super Category.");
            }
        }
    });
}
$("#example1").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var category_id = currentRow.find("td:eq(0)").html();
    var cat_status = currentRow.find("td:eq(3)").html();
    EditCategorybyid(category_id, cat_status);
});
function EditCategorybyid(category_id, cat_status) {
    var PetObj = JSON.stringify({ category_id: category_id });
    $.ajax({
        url: "/Admin/Admin/EditCategory",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#Categoryid').val(result.category_id);
            $('#ddlSuCategory').val(result.main_cat_id);
            $('#txtCategory').val(result.category_name);
            if ((cat_status) == "Active") {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
            //$('#chkStatus').val(result.cat_status);
        },
        error: function () {
            alert("Data Not Founf !!");
        }
    });
}
function DeletebyIId(category_id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ category_id: category_id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteCategory",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    alert("Delete Success");
                   
                }
                ShowDataInTable();
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
function ValidationCategory() {
    var msg = "";
    if ($('#txtCategory').val() == "") { msg += "Category can not left Blank !! \n"; }
    if ($('#ddlSuCategory').val() == 0) { msg += "Super category can not left Blank !! \n"; }
    //var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    //if (Status == 0) { msg += "Status can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;
}



//make by kamni

function ShowDataTableReport() {
    $.ajax({
        url: "/Admin/Admin/ShowCategoryReport",
        dataType: 'JSON',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td>' + item.category_id + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                html += '<td>' + item.category_name + '</td>';
                if ((item.cat_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">AnActive</td>';
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
    var Main_cat_id = $('#ddlSuCategory').find("option:selected").val();
    //var main_cat_name = $('#ddlSuCategory').find("option:selected").text();
    var cat_id = $('#SubCategoryid').val();
    var category_name = $('#txtCategory').val();
    $.ajax({
        url: "/Admin/Admin/SearchCategoryReport",
        data: { cat_id: cat_id, Main_cat_id: Main_cat_id, category_name: category_name },
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td>' + item.category_id + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                html += '<td>' + item.category_name + '</td>';
                if ((item.cat_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">InActive</td>';
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
    var category_name = $('#txtCategory').val();
    if (category_name == "") {
        ShowDataTableReport();
    }
}