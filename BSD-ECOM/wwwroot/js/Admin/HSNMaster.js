function clearHsnField() {
    $('#ID').val("0");
    $('#txthsnname').val("");
    $('#txthsncode').val("");
    $("#chkStatus").prop("checked", true);
    $('#btnSave').val("SAVE");
}
function SaveHSN() {
    var ID = $('#ID').val();
    var HSNName = $('#txthsnname').val();
    var HSNCode = $('#txthsncode').val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var fdata = new FormData();
    fdata.append("ID", ID);
    fdata.append("HSNName", HSNName);
    fdata.append("HSNCode", HSNCode);
    fdata.append("Status", Status);
    var msg = Validation();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/SaveUpdateHSNMaster',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },

            data: fdata,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.message == "Hsn added") {
                    if (result.id == 0) {
                        alert("Hsn added successfully.");
                    }
                    else {
                        alert("Hsn modify successfully.");
                    }
                    clearHsnField();
                    ShowDataInTable();
                }
            },
            error: function () {
                alert("Hsn not added.");
            }
        });
    }
    else {
        alert(msg);
    }
}
function Clear() {
    $('#btnSave').val("SAVE");
    ReloadPageWithAreas('Admin', 'Admin', 'HSNMaster');
}
function ShowDataInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowHSNMaster",
        dataType: 'JSON',
        type: 'POST',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.hsnName + '</td>';
                html += '<td>' + item.hsnCode + '</td>';
                if ((item.status) == true) {
                    html += '<td>Active</td >';
                } else {
                    html += '<td>Inactive</td >';
                }
               // html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button><a class="btn btn-sm" href="#" onclick="return DeletebyHSN(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#" onclick="return EditbyHSN(' + item.id + ')"><i class="fa fa-edit"></i></a></button></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyHSN(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                //html += '<td></td>';
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
    var status = currentRow.find("td:eq(3)").html();
    Editbyid(id, status);
});
function EditbyHSN(id, status) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditHSNMaster",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#ID').val(result.id);
            $('#txthsnname').val(result.hsnName);
            $('#txthsncode').val(result.hsnCode);
            if ((result.status) == 1) {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
            //$('#chkStatus').val(result.status);
        },
        error: function () {
            alert("Data Not Founf !!");
        }
    });
}
function DeletebyHSN(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteHSNMaster",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    ShowDataInTable();
                    alert("Delete Success");
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
    if ($('#txthsnname').val() == "") { msg += "Hsn Name can not left Blank !! \n"; }
    if ($('#txthsncode').val() == "") { msg += "Hsn Code can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;
}