function clearcourierfield() {
   $('#ID').val("0");
   $('#txtName').val("");
   $('#txtContactPerson').val("");
   $('#txtEmail').val("");
   $('#txtAddress').val("");
   $('#txtMobileNumber').val("");
    $("#chkStatus").prop("checked", true);
    $('#btnSave').val("SAVE");
}
function Savecourier() {
    var ID = $('#ID').val();
    var Name = $('#txtName').val();
    var Contact_Person = $('#txtContactPerson').val();
    var Email = $('#txtEmail').val();
    var Address = $('#txtAddress').val();
    var MobileNumber = $('#txtMobileNumber').val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var fdata = new FormData();
    fdata.append("ID", ID);
    fdata.append("Name", Name);
    fdata.append("Contact_Person", Contact_Person);
    fdata.append("Email", Email);
    fdata.append("Address", Address);
    fdata.append("MobileNumber", MobileNumber);
    fdata.append("Status", Status);
    var msg = Validation();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/SaveUpdateCourier',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },

            data: fdata,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.message == "Courier added") {
                    if (result.id == 0) {
                        alert("Courier added successfully.");
                    }
                    else {
                        alert("Courier modify successfully.");
                    }
                    clearcourierfield();
                    ShowDataInTable();
                }
            },
            error: function () {
                alert("Courier not added.");
            }
        });
    }
    else {
        alert(msg);
    }
}
function Clear() {
    $('#btnSave').val("SAVE");
    ReloadPageWithAreas('Admin', 'Admin', 'Courier');
}
function ShowDataInTable() {   
    $.ajax({
        url: "/Admin/Admin/ShowCourier",      
        dataType: 'JSON',
        type: 'POST',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
               // html += '<td style="display:none">' + CompanyId + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.name + '</td>';
                html += '<td>' + item.contact_Person + '</td>';
                html += '<td>' + item.email + '</td>';
                html += '<td>' + item.address + '</td>';
                html += '<td>' + item.mobileNumber + '</td>';
                if ((item.status) == true) {
                    html += '<td>Active</td >';
                } else {
                    html += '<td>Inactive</td >';
                }
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#" onclick="return Editbycourier(' + item.id + ')"><i class="fa fa-edit"></i></a></button></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Deletebycourier(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                //html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
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
    var status = currentRow.find("td:eq(6)").html();
    Editbyid(id, status);
});
function Editbycourier(id, status) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditCourier",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',        
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#ID').val(result.id);
            $('#txtName').val(result.name);
            $('#txtContactPerson').val(result.contact_Person);
            $('#txtEmail').val(result.email);
            $('#txtAddress').val(result.address);
            $('#txtMobileNumber').val(result.mobileNumber);
            if ((result.status) == true) {
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
function Deletebycourier(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id:id });    
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteCourier",
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
    if ($('#txtName').val() == "") { msg += "Name can not left Blank !! \n"; }
    if ($('#txtContactPerson').val() == "") { msg += "ContactPerson can not left Blank !! \n"; }
    if ($('#txtMobileNumber').val() == "") { msg += "Mobile No can not left Blank !! \n"; }
    if ($('#txtEmail').val() == "") { msg += "Email can not left Blank !! \n"; }
    if ($('#txtAddress').val() == "") { msg += "Address can not left Blank !! \n"; }    
    if (msg != "") { alert(msg); return false; }
    return msg;
}