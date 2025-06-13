var filenameorginal = "";

    $("#imagefile").change(function () {
        filenameorginal = $('#imagefile').val();
    })


function SaveBrand() {
    var brand_id = $('#brand_id').val();
    var brand_name = $('#txtBrand').val();
    var brandImage = $('#imagefile').val();
    var item_cat_id = $('#item_cat_id').val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var fileExtension = ['png', 'img', 'jpg'];
    var filename = $('#imagefile').val();
    var fdata = new FormData();
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
        var fileUpload = $("#imagefile").get(0);
        var files = fileUpload.files;
        fdata.append(files[0].name, files[0]);
    }
    else {
        fdata.append("flg", "ok");
    }

    fdata.append("brand_id", brand_id);
    fdata.append("brand_name", brand_name);
    fdata.append("item_cat_id", item_cat_id);
    fdata.append("Status", Status);
    fdata.append("filenmes", filenameorginal);
    var msg = Validation();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/SaveBrand',
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
                if (result.message == "Brand added") {
                    if (result.brand_id == 0) {
                        alert("Brand added successfully.");
                    }
                    else {
                       
                        //$('#btnSave').val("SAVE");
                        alert("Brand modify successfully.");
                    }
                    ShowDataInTable();
                    ClearTextBox();
                }
            },
            error: function () {
                alert("Brand not added.");
            }
        });
    }
    else {
        alert(msg);
    }
}
function ClearTextBox() {
    $("#btnSave").val("Save");
    $('#txtBrand').val('');
    $('#imagefile').val('');
    $('#img').attr('src', "/images/dummy-pic.png");
    $('#chkStatus').prop('checked', false);
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'Brand');
}
function ShowDataInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowAllBrand",
        dataType: 'JSON',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.brand_id + '</td>';
                html += '<td>' + item.brand_name + '</td>';
                html += '<td><img src="/images/Brand/6/' + item.brandImage + '"  width="75" height="80" /></td>';
                //html += '<td>' + item.brand_status + '</td>';
                if ((item.brand_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                //html += '<td><a class="btn btn-sm" href="#" onclick="EditBrandbyid(' + item.brand_id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="DeleteBrandbyId(' + item.brand_id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}
$("#example1").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var brand_id = currentRow.find("td:eq(0)").html();
    var brand_status = currentRow.find("td:eq(3)").html();
    //alert(loc_status);
    EditBrandbyid(brand_id, brand_status);
});
function EditBrandbyid(brand_id ,brand_status) {
    var PetObj = JSON.stringify({ brand_id: brand_id });
    $.ajax({
        url: "/Admin/Admin/EditBrand",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#brand_id').val(result.brand_id);
            $('#txtBrand').val(result.brand_name);
            //$('#chkStatus').val(result.brand_status);
            if ((brand_status) == "Active") {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
            $('#img').attr('src', "/images/Brand/6/" + result.brandImage + "");
            //$('#imagname').val(result.brandImage);
            filenameorginal = result.brandImage;
        },
        error: function () {
            alert("Data Not Found !!");
        }        
    });
}
function DeleteBrandbyId(brand_id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ brand_id: brand_id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteBrand",
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
function Validation() {
    var msg = "";
    if ($('#txtBrand').val() == "") { msg += "Brand can not left Blank !! \n"; }
    // var Loc_id = $('#ddlLocalType').find("option:selected").val();
    //if (msg != "") { alert(msg); return false; }
    return msg;
}