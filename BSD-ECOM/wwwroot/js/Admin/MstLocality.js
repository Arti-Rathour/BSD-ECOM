var filenameorginal = "";
$(document).ready(function () {
    $("#imagefile").change(function () {
        filenameorginal = $('#imagefile').val();
    })
})
$(document).ready(function () {
    CKEDITOR.replace('#txtContent');
});
imagefile.onchange = evt => {
    const [file] = imagefile.files
    if (file) {
        img.src = URL.createObjectURL(file)
    }
}
function Save() {
    var Loc_id = $('#Loc_id').val();
    var Name = $('#txtName').val();
    var Img = $('#imagefile').val();
    var content = CKEDITOR.instances["txtContent"].getData();
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
    Validation();
    fdata.append("Loc_id", Loc_id);
    fdata.append("Name", Name);
    fdata.append("Img", Img);
    fdata.append("Status", Status);
    fdata.append("content", content);
    fdata.append("filenmes", filenameorginal);
    $.ajax({
        type: "POST",
        url: '/Admin/Admin/SaveLocality',
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
            if (result.message == "Locality added") {
                if (result.id == 0) {
                    alert("Locality  successfully.");
                }
                else {
                    alert("Locality modify successfully.");
                }
                cleardata();
                ShowDataInTable();
            }
        },
        error: function () {
            alert("Locality not added.");
        }
    });
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'Locality');
}
function ShowDataInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowLocality",
        dataType: 'JSON',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.loc_id + '</td>';
                html += '<td>' + item.name + '</td>';
                html += '<td><img src="/images/Locality_Img/' + item.img + '"  width="75" height="80" /></td>';
                //html += '<td>' + item.loc_status + '</td>';
                if ((item.loc_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                //html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyid(' + item.loc_id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.loc_id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}
$("#example1").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var loc_id = currentRow.find("td:eq(0)").html();
    var loc_status = currentRow.find("td:eq(3)").html();
    //alert(loc_status);
    Editbyid(loc_id, loc_status);
});
function Editbyid(loc_id, loc_status) {
    var PetObj = JSON.stringify({ loc_id: loc_id });
    $.ajax({
        url: "/Admin/Admin/EditLocality",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val("Update");
            $('#Loc_id').val(result.loc_id);
            $('#txtName').val(result.name);
            $('#img').attr('src', "/images/Locality_Img/" + result.img + "");
           // $('#chkStatus').val(result.brand_status);
           CKEDITOR.instances["txtContent"].setData(result.content);
            if ((loc_status) == "Active") {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
            filenameorginal = result.img;
        },
        error: function () {
            alert("Data Not Founf !!");
        }
    });
}
function DeletebyId(Loc_id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ Loc_id: Loc_id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteLocality",
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
    if ($('#txtName').val() == "") { msg += "Name can not left Blank !! \n"; }
    // var Loc_id = $('#ddlLocalType').find("option:selected").val();
    //if (msg != "") { alert(msg); return false; }
    return msg
}
function cleardata() {
    $('#Loc_id').val("0");
    $('#txtName').val("");
    $('#imagefile').val("");
    CKEDITOR.instances["txtContent"].setData("");
    $('#chkStatus').prop('checked', true);
    $('#btnSave').val("Save");
    $('#imagefile').val("");
    $('#img').attr('src', "/images/dummy-pic.png");
    filenameorginal = "";
}