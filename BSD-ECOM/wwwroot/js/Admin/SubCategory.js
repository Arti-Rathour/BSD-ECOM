var filenameorginal = "";
function Savesubcategory() {
    var id = $('#SubCategoryid').val();
    var SuperCategoryId = $('#ddlSuCategory').find("option:selected").val();
    var SuperCategoryText = $('#ddlSuCategory').find("option:selected").text();
    var CategoryId = $('#ddlCategory').find("option:selected").val();
    var CategoryText = $('#ddlCategory').find("option:selected").text();
    var SubCategory = $('#txtCategory').val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var fdata = new FormData();
    var fileExtension = ['png', 'img', 'jpg'];
    var filename = $('#imagefile').val();
    if (filenameorginal == filename) {
        if (filename.length == 0) {
            alert("Please select a file.");
            return false;
        }
        else {
            var extension = filename.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("flg", "okg");
        //var fdata = new FormData();
        var fileUpload = $("#imagefile").get(0);
        var files = fileUpload.files;
        fdata.append(files[0].name, files[0]);
    }
    else {
        fdata.append("flg", "ok");

    }
    fdata.append("id", id);
    fdata.append("SuperCategoryId", SuperCategoryId);
    fdata.append("CategoryId", CategoryId);
    fdata.append("SubCategory", SubCategory);
    fdata.append("Status", Status);
    fdata.append("filenmes", filenameorginal);
    var msg = validationsubcategory();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/SaveSubCategory',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: fdata,
            contentType: false,
            processData: false,
            // data: { id: id, SubCategory: SubCategory, SuperCategoryId: SuperCategoryId, CategoryId: CategoryId, Category: Category, Status: Status },
            //dataType: "JSON",
            success: function (result) {
                if (result.message == "Sub Category added.") {
                    if (result.id == 0) {
                        alert("Sub Category added successfully.");
                    }
                    else {
                        alert("Sub Category modify successfully.");
                    }
                    cleardata();
                    ShowDataInTable();
                }
            },
            error: function () {
                alert("Some thing Error.");
            }
        });
    }
    else {
        alert(msg);
    }
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'SubCategory');
}

$("#ddlSuCategory").change(function () {
    BindCategory();
});
function BindCategory() {
    var selected_val = $('#ddlSuCategory').find(":selected").attr('value');
    $("#ddlSuCategory").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Admin/Admin/BindCategory', //url to be called
        data: "id=" + selected_val, //data to be send
        success: function (json, result) {
            $("#ddlCategory").empty();
            json = json || {};
            $("#ddlCategory").append('<option value="0">Select Category</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlCategory").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlCategory").prop("disabled", false);
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}

function ShowDataInTable() {
    var SuperCategoryId = $('#ddlSuCategory').find("option:selected").val();
    var CategoryId = $('#ddlCategory').find("option:selected").val();
    $.ajax({
        url: "/Admin/Admin/ShowSubCategory",
        dataType: 'JSON',
        type: 'POST',
        data: { CategoryId: CategoryId, MainCategoryId: SuperCategoryId },
        // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.cat_id + '</td>';
                html += '<td style="display:none">' + item.main_cat_id + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                html += '<td style="display:none">' + item.category_id + '</td>';
                html += '<td>' + item.category_name + '</td>';
                html += '<td>' + item.cat_name + '</td>';
                //html += '<td>' + item.company_name + '</td>';
                if ((item.cat_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                //html += '<td>' + item.cat_status + '</td>';
                html += '<td><img src="/images/SubCategory/6/' + item.image + '" alt="your image" width="75" height="80" /></td>';
              //  html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbysubcategory(' + item.cat_id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Deletebysubcategory(' + item.cat_id + ')"><i class="fa fa-trash-o"></i></a></td>';
                // html += '<td><a class="btn btn-sm" href="#" onclick="return ViewSubCategory(' + item.cat_id + ')"><i class="fa fa-eye"></i></a></td>';
                html += '<td><a href="/Admin/Admin/SubCategoryReportFile?cat_id=' + item.cat_id + '"><i class="fa fa-eye"></i></a></td>';
                html += '</tr>';
                index++;
            });
            if (html != "") {
                $('.tbodyData').html(html);
            }
            else {
                $(".tbodyData").html("No data found.");
            }
            //            $('.tbodyData').html(html);
        }
    });
}

$("#example1").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var Sub_cat_id = currentRow.find("td:eq(0)").html();
    var Sub_cat_status = currentRow.find("td:eq(6)").html();
    var main_cat_id = currentRow.find("td:eq(1)").html();
    Categorybymainid(main_cat_id);
    Editbyid(Sub_cat_id, Sub_cat_status, main_cat_id);
});

function Categorybymainid(id) {
    //var selected_val = $('#ddlSuCategory').find(":selected").attr('value');
    $("#ddlSuCategory").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Admin/Admin/BindCategory', //url to be called
        data: "id=" + id, //data to be send
        async: false,
        success: function (json, result) {
            $("#ddlCategory").empty();
            json = json || {};
           //$("#ddlCategory").append('<option value="0">Select Category</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlCategory").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlCategory").prop("disabled", false);
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}

function Editbysubcategory(Sub_cat_id, Sub_cat_status, main_cat_id) {
    //Categorybymainid(main_cat_id);
    var Obj = JSON.stringify({ Sub_cat_id: Sub_cat_id });
    $.ajax({
        url: "/Admin/Admin/EditSubCategory",
        data: JSON.parse(Obj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#SubCategoryid').val(result.cat_id); 
            $('#ddlSuCategory').val(result.main_cat_id);
            Categorybymainid(result.main_cat_id);
              //$('#ddlCategory').val(result.category_id).trigger('change'); 
          //  $('#ddlCategory').val(result.category_id).trigger('select');
           $('#ddlCategory').val(result.category_id);
            $('#txtCategory').val(result.cat_name);
            if ((result.Sub_cat_status) ==1) {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
            $('#img').attr('src', "/images/SubCategory/6/" + result.image + "");
            filenameorginal = result.image;
            //$('#chkStatus').val(result.cat_status);

            //$('#imagname').val(result.image);
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}

$(document).ready(function () {
    $("#imagefile").change(function () {
        filenameorginal = $('#imagefile').val();
    })
})
function Deletebysubcategory(cat_id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var Obj = JSON.stringify({ cat_id: cat_id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteSubCategory",
            data: JSON.parse(Obj),
            dataType: 'JSON',
            type: 'POST',
            success: function (result) {
                if (result.msg == "Delete Successfull") {
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

function ViewSubCategory(Sub_cat_id) {

}

function cleardata() {
    $("#btnSave").val("Save");
    $('#SubCategoryid').val("0");
    $('#ddlSuCategory').val("0");
    $('#ddlCategory').val("0");
    $('#txtCategory').val("");
    $('#chkStatus').prop('checked', true);
    $('#btnSave').val = "SAVE";
    $('#imagefile').val("");
    $('#img').attr('src', "/images/dummy-pic.png");
    filenameorginal = "";
}

function validationsubcategory() {
    var msg = "";
    if ($('#ddlSuCategory').val() == 0) { msg += "Super Category can not left Blank !! \n"; }
    if ($('#ddlCategory').val() == 0) { msg += "Category can not left Blank !! \n"; }
    if ($('#txtCategory').val() == "") { msg += "Sub Category can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;
}