
function IUCreateuser() {
    var ID = $('#id').val();
    var Name = $('#txtuser').val();
    var Email = $('#txtemail').val();
    var MobileNo = $('#txtmob').val();
    var Password = $('#txtpass').val();  
    var Type = $('#ddltype').find("option:selected").val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var msg = validateuser();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IUCreateuser',
            data: { ID: ID, Name: Name, Email: Email, MobileNo: MobileNo, Password: Password, Type: Type, Status: Status },
            dataType: "JSON",
            success: function (result) {
                alert(result.message);
                //if (result.message == "User added.") {
                //    if (result.id == 0) {
                //        alert("User added.");
                //    }
                //    else {
                //        alert("User modify successfully.");
                //    }
                //
                //}
                ClearData();
                ShowDataInTable();
            },
            
            error: function () {
                alert("User not added.");
            }
        });
    }
    else
    {
        alert(msg);
    }
}

function ClearData() {
    $("#id").val("0");
    $("#txtuser").val("");
    $("#txtemail").val("");
    $("#txtmob").val("");
    $('#txtpass').val(""); 
    $("#ddltype").val("0");
    $("#chkStatus").prop("checked",true);
    $('#btnSave').val("SAVE");
}

function Clear() {
    $('#btnSave').val("SAVE");
    ReloadPageWithAreas('Admin', 'Admin', 'Createuser');
}

function ShowDataInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowCreateuser",
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
                html += '<td style="display:none">' + item.type + '</td>';
                if ((item.type) == 1) {
                    html += '<td class="admin">Admin</td>';
                } else {
                    html += '<td class="admin">Employee</td>';
                }
                html += '<td>' + item.name + '</td>';
                html += '<td style="display:none">' + item.email + '</td>';
                html += '<td style="display:none">' + item.password + '</td>';
                html += '<td style="display:none">' + item.mobileNo + '</td>';
                if ((item.status) == 1) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                /*html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyid(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';*/
                html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ',' + item.type+')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Createuserbody').html(html);
        }
    });
}

$("#data-table").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var ID = currentRow.find("td:eq(0)").html();
    var typeid = currentRow.find("td:eq(1)").html();
    var status = currentRow.find("td:eq(7)").html();
    //alert(comp_status);
    Editbyid(ID, status, typeid);
});

function Editbyid(ID, status, typeid) {
    var PetObj = JSON.stringify({ ID: ID, typeid: typeid });
    $.ajax({
        url: "/Admin/Admin/EditCreateuser",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',        
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#ddltype').val(result.type);
            $('#id').val(result.id);
            $('#txtuser').val(result.name);
            $('#txtemail').val(result.email);
            $('#txtpass').val(result.password);
            $('#txtmob').val(result.mobileNo);
            if ((status) == "Active") {
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


function validateuser()
{
    var msg = "";
    if ($('#ddltype').val() == 0) { msg += "Type can not left Blank !! \n"; }
    if ($('#txtuser').val() == "") { msg += "Name can not left Blank !! \n"; }
    if ($('#txtemail').val() == "") { msg += "Email can not left Blank !! \n"; }
    if ($('#txtpass').val() == "") { msg += "Password can not left Blank !! \n"; }
    if ($('#txtmob').val() == "") { msg += "Mobile No can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;


}

function DeletebyId(ID, typeid) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ ID: ID, typeid: typeid });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteCreateuser",
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

function clearAllField() {

    $('#btnSave').val("SAVE");
    $('#ddltype').find("option:selected").val(0);
    $('#id').val("0");
    $('#txtuser').val("");
    $('#txtemail').val("");
    $('#txtpass').val("");
    $('#txtmob').val("");
    $("#chkStatus").prop("checked", true);    
}

