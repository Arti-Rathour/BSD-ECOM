function clearunitmasterfield() {
    $('#btnSave').val("Save");
    $('#unit_id').val("0");
    $('#txtunit_name').val("");
    $("#chkStatus").prop("checked", true);
}
function Saveunitmaster() {
    var unit_id = $('#unit_id').val();
    var unit_name = $('#txtunit_name').val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var fdata = new FormData();
    fdata.append("unit_id", unit_id);
    fdata.append("unit_name", unit_name);
    fdata.append("Status", Status);
    var msg = Validation();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/SaveUpdateunitmaster',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },

            data: fdata,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.message == "Unit name added") {
                    if (result.id == 0) {
                        alert("Unit name added successfully.");
                    }
                    else {
                        alert("Unit modify successfully.");
                    }
                    clearunitmasterfield();
                    ShowDataInTable();

                }
            },
            error: function () {
                alert("Unit name not added.");
            }
        });
    }
    else {
        alert(msg);
    }
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'unitmaster');
}
function ShowDataInTable() {
    $.ajax({
        url: "/Admin/Admin/Showunitmaster",
        dataType: 'JSON',
        type: 'POST',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.unit_id + '</td>';
                html += '<td>' + item.unit_name + '</td>';
                if ((item.unit_status) == true) {
                    html += '<td>Active</td >';
                } else {
                    html += '<td>Inactive</td >';
                }
                //html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.unit_id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#" onclick="return Editbyunitmaster(' + item.unit_id + ')"><i class="fa fa-edit"></i></a></button></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Deletebyunitmaster(' + item.unit_id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}
$("#example1").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var unit_id = currentRow.find("td:eq(0)").html();
    var status = currentRow.find("td:eq(2)").html();
    Editbyid(unit_id, status);
});
function Editbyunitmaster(unit_id, status) {
    var PetObj = JSON.stringify({ unit_id: unit_id });
    $.ajax({
        url: "/Admin/Admin/Editunitmaster",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("Update");
            $('#unit_id').val(result.unit_id);
            $('#txtunit_name').val(result.unit_name);
            if ((result.unit_status) ==1) {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
            //$('#chkStatus').val(result.unit_status);
        },
        error: function () {
            alert("Data Not Founf !!");
        }
    });
}
function Deletebyunitmaster(unit_id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ unit_id: unit_id });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/Deleteunitmaster",
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
    if ($('#txtunit_name').val() == "") { msg += "Unit Name can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;
}