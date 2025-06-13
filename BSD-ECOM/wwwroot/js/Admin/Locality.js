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
    var id = $('#id').val();
    var Name = $('#txtName').val();
    var contact_name = $('#txtcontact_name').val(); 
    var mobile_no = $('#txtmobile_no').val();
    var content = CKEDITOR.instances["txtContent"].getData();
    var pincode = $('#txtpincode').val();
    var Loc_address = $('#txtLoc_address').val();
    var phone = $('#txtphone').val();
    var Loc_id = $('#ddlLocalType').find("option:selected").val();
    //var Loc_id = $('#ddlLocalType').find("option:selected").text();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var fdata = new FormData();
    var ButtonValue = $('#btnSave').val();
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
        var fileUpload = $("#imagefile").get(0);
        var files = fileUpload.files;
        fdata.append(files[0].name, files[0]);
    }
    else {
        fdata.append("flg", "ok");
    }
    
    fdata.append("id", id);
    fdata.append("Name",Name);
    fdata.append("contact_name", contact_name);
    fdata.append("mobile_no", mobile_no);
    fdata.append("pincode", pincode);
    fdata.append("content", content);
    fdata.append("Loc_address", Loc_address);
    fdata.append("phone", phone);
    fdata.append("Loc_id", Loc_id);
    fdata.append("Status", Status);
    fdata.append("filenmes", filenameorginal);
    var msg = Validation();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/SaveLocalityDetails',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: fdata,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.message == "Locality Details added") {
                    if (result.id == 0) {
                        alert("Locality Details added successfully.");
                    }
                    else {
                        alert("Locality Details modify successfully.");
                    }
                    cleardata();
                    ShowDataInTable();
                }
            },
            error: function () {
                alert("Locality Details not added.");
            }
        });
    }
    else {
        Validation();
    }
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'SaveLocalityDetails');
}
function ShowDataInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowLocalityDetails",
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
                html += '<td>' + item.name + '</td>';
                html += '<td>' + item.contact_name + '</td>';
                html += '<td><img src="/images/Locality_Img/6/' + item.img + '"  width="75" height="80" /></td>';
                html += '<td>' + item.content + '</td>';
                html += '<td>' + item.pin + '</td>';
                html += '<td>' + item.loc_address + '</td>';
                html += '<td>' + item.mobile_no1 + '</td>';
                html += '<td>' + item.phone1 + '</td>';
                html += '<td>' + item.loc_Name + '</td>';
               // html += '<td>' + item.loc_status + '</td>';
                if ((item.loc_status) == true) {
                    html += '<td>Active</td >';
                } else {
                    html += '<td>Inactive</td >';
                }
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';

                html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
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
    var loc_status = currentRow.find("td:eq(10)").html();
    Editbyid(id, loc_status);
});
function Editbyid(id, loc_status) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditLocalityDetails",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("Update");
            $('#txtName').val(result.name);
            $('#txtcontact_name').val(result.contact_name);
            $('#txtmobile_no').val(result.mobile_no1);
            CKEDITOR.instances["txtContent"].setData(result.content);
            $('#txtpincode').val(result.pincode);
            $('#txtLoc_address').val(result.loc_address);
            $('#txtphone').val(result.phone1);
            $('#ddlLocalType').val(result.loc_id);
            $('#img').attr('src', "/images/Locality_Img/6/" + result.img + "");
            if ((loc_status) == "Active") {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
            filenameorginal = result.img;
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
            url: "/Admin/Admin/DeleteLocalityDetails",
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
    if ($('#txtName').val() == "") { msg += "Name can not left Blank !! \n"; }
    if ($('#txtpincode').val() == "") { msg += "Pincode  can not left Blank !! \n"; }
    if ($('#txtLoc_address').val() == "") { msg += "Address can not left Blank !! \n"; }
    if ($('#txtphone').val() == "") { msg += "Phone can not left Blank !! \n"; }
    if ($('#ddlLocalType').val() ==0) { msg += "LocalType can not left Blank !! \n"; }
    
    return msg;
}
          
function cleardata() {
    $('#id').val("0");
    $('#txtName').val("");
    $('#txtcontact_name').val("");
    $('#txtmobile_no').val("");
    CKEDITOR.instances["txtContent"].setData("");
    $('#txtpincode').val("");
    $('#txtLoc_address').val("");
    $('#txtphone').val("");
    $('#ddlLocalType').find("option:selected").val(0);    
    $('#chkStatus').prop('checked', true);
    $('#btnSave').val("Save");
    $('#imagefile').val("");
    $('#img').attr('src', "/images/dummy-pic.png");
    filenameorginal = "";
}
