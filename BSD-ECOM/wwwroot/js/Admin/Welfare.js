var filenameorginal = "";

$(document).ready(function () {
    CKEDITOR.replace('#txtContent');

});

$(document).ready(function () {
    $("#imagefile").change(function () {
        filenameorginal = $("#imagefile").val();
    })
})
function clearwelfarefield() {
    $('#id').val("0");
    $('#txtWelfare_Name').val("");
    $('#ddlwelfaremaster').val(0);
    $('#txtcontactNo').val("");
    $('#txtEmail').val("");
    $('#imagefile').val("");
    $('#img').attr('src', "~/images/dummy - pic.png");
    CKEDITOR.instances["txtContent"].setData("");
    $('#btnSave').val("SAVE");
}
function Save() {
    var id = $('#id').val();
    var Welfare_Name = $('#txtWelfare_Name').val();
    var welmaster_id = $('#ddlwelfaremaster').find(":selected").attr('value');
    var contactNo = $('#txtcontactNo').val();
    var Email = $('#txtEmail').val();
    //var content = $('#txtContent').val();
    var content = CKEDITOR.instances["txtContent"].getData();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var fdata = new FormData();
    var fileExtension = ['png', 'img', 'jpg', 'jpeg', 'PNG', 'IMG', 'JPG', 'JPEG'];
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
        var fileUpload = $("#imagefile").get(0);
        var files = fileUpload.files;
        fdata.append(files[0].name, files[0]);
    }
    else {
        fdata.append("flg", "ok");
    }
    fdata.append("id", id);
    fdata.append("Welfare_Name", Welfare_Name);
    fdata.append("contactNo", contactNo);
    fdata.append("Email", Email);
    fdata.append("content", content);
    fdata.append("Status", Status);
    fdata.append("filenmes", filenameorginal);
    fdata.append("welmaster_id", welmaster_id);
    var msg = Validation();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/SaveWelfare',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: fdata,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.message == "Welfare added") {
                    if (result.id == 0) {
                        alert("Welfare added successfully.");
                    }
                    else {
                        alert("Welfare modify successfully.");
                    }
                    clearwelfarefield();
                    ShowDataInTable();
                }
            },
            error: function () {
                alert("Welfare not added.");
            }
        });
    }
    else {
        alert(msg);
       // Validation();
    }
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'Welfaredetails');
    $('#btnSave').val("SAVE");
}
function ShowDataInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowWelfare",
        dataType: 'JSON',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.welfaremaster_name+'</td>'
                html += '<td>' + item.welfare_Name + '</td>';
                html += '<td><img src="/images/Welfare/6/' + item.img + '"  width="75" height="80" /></td>';
                html += '<td>' + item.contactNo + '</td>';
                html += '<td>' + item.email + '</td>';
                html += '<td>' + item.content + '</td>';
                if ((item.welfarestatus) == true) {
                    html += '<td>Active</td>';
                } else {
                    html += '<td>Inactive</td>';
                }
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                //html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyid(' + item.id + ')"><i class="fa fa-edit"></i></a><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}
$("#example1").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var id = currentRow.find("td:eq(0)").html();
    var welfarestatus = currentRow.find("td:eq(7)").html();
    Editbyid(id, welfarestatus);
});
function Editbyid(id, welfarestatus) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditWelfare",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.id);
            $('#ddlwelfaremaster').val(result.welmaster_id);
            $('#txtWelfare_Name').val(result.welfare_Name);
            $('#img').attr('src', "/images/Welfare/6/" + result.img + "");
            filenameorginal = result.img;
            $('#txtcontactNo').val(result.contactNo);
            $('#txtEmail').val(result.email);
            CKEDITOR.instances["txtContent"].setData(result.content);
            if ((welfarestatus) == "Active") {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
function DeletebyId(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteWelfare",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
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
    if ($('#ddlwelfaremaster').val() == 0) { msg += "Please select valid welfare !! \n"; }
    if ($('#txtWelfare_Name').val() == "") { msg += "Name can not left Blank !! \n"; }
    //if ($('#img').val() == "") { msg += "img can not left Blank !! \n"; }
    if ($('#txtcontactNo').val() == "") { msg += "ContactNo can not left Blank !! \n"; }
    if ($('#txtEmail').val() == "") { msg += "Email can not left Blank !! \n"; }
    if (CKEDITOR.instances["txtContent"].getData() == "") { msg += "Content can not left Blank !! \n"; }
  
    return msg;
}

//function Editbyid(id, loc_status) {
//    var PetObj = JSON.stringify({ id: id });
//    // var chkStatus = result.loc_status;
//    $.ajax({
//        url: "/Admin/Admin/EditLocalityDetails",
//        data: JSON.parse(PetObj),
//        dataType: 'JSON',
//        type: 'POST',
//        //contentType: 'application/json; charset=utf-8',
//        success: function (result) {
//            $('#btnSave').val = "UPDATE";
//            $('#id').val(result.id);
//            $('#txtName').val(result.name);
//            $('#txtcontact_name').val(result.contact_name);
//            $('#txtmobile_no').val(result.mobile_no);
//            $('#txtContent').html(result.content);
//            $('#txtpincode').val(result.pincode);
//            $('#txtLoc_address').val(result.loc_address);
//            $('#txtphone').val(result.phone);
//            $('#ddlLocalType').val(result.loc_id);
//            $('#img').attr('src', "/images/" + result.img + "");
//            if ((loc_status) == "Active") {
//                $("#chkStatus").prop("checked", true);
//            }
//            else {
//                $('#chkStatus').prop('checked', false);
//            }
//        },
//        error: function () {
//            alert("Data Not Founf !!");
//        }
//    });
//}


/*----------- Start WelfareMaster--------- */

var wmfilenameorginal = "";
$(document).ready(function () {
    $("#wmimagefile").change(function () {
        wmfilenameorginal = $("#wmimagefile").val();
    })
})

function IU_WelfareMaster()
{
    var id = $('#hiddenid').val();
    var Welfare_Name = $('#txtWelfare_Name').val();
    var contactNo = $('#txtcontactNo').val();
    var Email = $('#txtEmail').val();
    var content = CKEDITOR.instances["txtContent"].getData();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var fdata = new FormData();
    var fileExtension = ['png', 'img', 'jpg','jpeg','PNG','IMG','JPG','JPEG'];
    var filename = $('#wmimagefile').val();
    if (wmfilenameorginal == filename) {
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
        var fileUpload = $("#wmimagefile").get(0);
        var files = fileUpload.files;
        fdata.append(files[0].name, files[0]);
    }
    else {
        fdata.append("flg", "ok");
    }

    fdata.append("id", id);
    fdata.append("Welfare_Name", Welfare_Name);
    fdata.append("contactNo", contactNo);
    fdata.append("Email", Email);
    fdata.append("content", content);
    fdata.append("Status", Status);
    fdata.append("filenmes", wmfilenameorginal);

    var msg = Validationwelfaremaster();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IU_welfaremaster',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: fdata,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.message == "Welfare added") {
                    if (result.id == 0) {
                        alert("Welfare added successfully.");
                    }
                    else {
                        alert("Welfare modify successfully.");
                    }
                    clearWelmaster();
                    showwelfareMasterdata();
                }
            },
            error: function () {
                alert("Welfare not added.");
            }
        });
    }
    else {

        alert(msg);
        //Validation();
    }
}
function showwelfareMasterdata() {
    $.ajax({
        url: "/Admin/Admin/ShowWelfaremster",
        dataType: 'JSON',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.welfare_Name + '</td>';
                html += '<td><img src="/images/Welfare/6/' + item.img + '"  width="75" height="80" /></td>';
                html += '<td>' + item.contactNo + '</td>';
                html += '<td>' + item.email + '</td>';
                html += '<td>' + item.content + '</td>';
                if ((item.welfarestatus) == true) {
                    html += '<td>Active</td >';
                } else {
                    html += '<td>Inactive</td >';
                }
                html += '<td><button class="btnSelectWM"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button><a class="btn btn-sm" href="#" onclick="return DeletewelfaremasterbyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                //html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyid(' + item.id + ')"><i class="fa fa-edit"></i></a><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}
$("#example1").on('click', '.btnSelectWM', function () {
    var currentRow = $(this).closest("tr");
    var id = currentRow.find("td:eq(0)").html();
    var welfarestatus = currentRow.find("td:eq(6)").html();
    Editwelfaremasterbyid(id, welfarestatus);
});
function Editwelfaremasterbyid(id, welfarestatus){
    var Obj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditWelfaremaster",
        data: JSON.parse(Obj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#hiddenid').val(result.id);
            $('#txtWelfare_Name').val(result.welfare_Name);
            $('#img').attr('src', "/images/Welfare/6/" + result.img + "");
            wmfilenameorginal = result.img;
            $('#txtcontactNo').val(result.contactNo);
            $('#txtEmail').val(result.email);
            CKEDITOR.instances["txtContent"].setData(result.content);
            if ((welfarestatus) == "Active") {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
function DeletewelfaremasterbyId(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var Obj = JSON.stringify({ id: id });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteWelfaremaster",
            data: JSON.parse(Obj),
            dataType: 'JSON',
            type: 'POST',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    alert("Delete Success");
                    showwelfareMasterdata();
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

function Validationwelfaremaster() {
    var msg = "";
    if ($('#txtWelfare_Name').val() == "") { msg += "Welfare Name can not left Blank !! \n"; }
    if ($('#txtcontactNo').val() == "") { msg += "contactNo can not left Blank !! \n"; }
    if ($('#txtEmail').val() == "") { msg += "Email can not left Blank !! \n"; }
    if (CKEDITOR.instances["txtContent"].getData() == "") { msg += "Content can not left Blank !! \n"; }
    
    return msg;
}
function clearwelfareMasterfield() {
    $('#hiddenid').val("");
    $('#txtWelfare_Name').val("");
    $('#txtcontactNo').val("");
    $('#txtEmail').val("");
    CKEDITOR.instances["txtContent"].setData("");
    $("#chkStatus").prop("checked", true);
    $('#wmimagefile').val("");
    $('#img').attr('src', "~/images/dummy - pic.png");
    $('#btnSave').val("SAVE");
}
function clearWelmaster() {
    ReloadPageWithAreas('Admin', 'Admin', 'Welfaremaster');
    $('#btnSave').val("SAVE");
}