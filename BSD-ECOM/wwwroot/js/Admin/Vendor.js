


function Savevendor() {
    var id = $('#id').val();
    var vendor_name = $('#txtvendername').val();
    var vendor_emailid = $('#txtvenderemail').val();
    var vendor_mobileno = $('#txtvendermobile').val();
    var vendor_address = $('#txtvenderadd').val();
    var companyname = $('#txtcompanyname').val();
    var ButtonValue = $('#btnSave').val();
    var msg = ValidationMainCategory();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/Vendorreport',
            data: { id: id, vendor_name: vendor_name, vendor_emailid: vendor_emailid, vendor_mobileno: vendor_mobileno, vendor_address: vendor_address, companyname: companyname },
            dataType: "JSON",
            success: function (result) {
                if (result.message == "Vendor_Page added.") {
                    if (result.id == 0) {
                        alert("Vendor_Page added successfully.");
                    }
                    else {
                        alert("Vendor_Page modify successfully.");
                    }
                    ClearAllField();
                    ShowDataInTable();
                }
            },
            error: function () {
                alert("Vendor not added.");
            }
        });
    }
    else {
        alert(msg);
    }
}




function ClearAllField() {
    $("#btnSave").val("Save");
    $('#id').val("0");
    $('#txtvendername').val("");
    $('#txtvenderemail').val("");
    $('#txtvendermobile').val("");
    $('#txtvenderadd').val("");
    $('#txtcompanyname').val("");
}



function ShowDataInTable() {

    var searchitem = $("#txtsearch").val();
   $.ajax({
        url: "/Admin/Admin/ShowVendorReport",
        type: 'POST',
        data: { searchitem: searchitem },
        dataType: 'JSON',
        // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.vendor_name + '</td>';
                html += '<td>' + item.vendor_emailid + '</td>';
                html += '<td>' + item.vendor_mobileno + '</td>';
                html += '<td>' + item.vendor_address + '</td>';
                html += '<td>' + item.companyname + '</td>';
              //  html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyvendor(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Deletebyvendor(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            if (html != "") {
                $('.tbodyData').html(html);
            }
            else {
                $('.tbodyData').html("No data found for this services");
            }
        }
    });
}

function ValidationMainCategory() {
    var msg = "";
    if ($('#txtvendername').val() == "") { msg += "vender name can not left Blank !! \n"; }
    if ($('#txtvenderemail').val() == "") { msg += "vender email can not left Blank !! \n"; }
    if ($('#txtvendermobile').val() == "") { msg += "vender mobile can not left Blank !! \n"; }
    if ($('#txtvenderadd').val() == "") { msg += "vender address can not left Blank !! \n"; }
    if ($('#txtcompanyname').val() == "") { msg += "company name can not left Blank !! \n"; }
    return msg;
}


function Editbyvendor(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditVendor",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.id);
            $('#txtvendername').val(result.vendor_name);
            $('#txtvenderemail').val(result.vendor_emailid);
            $('#txtvendermobile').val(result.vendor_mobileno);
            $('#txtvenderadd').val(result.vendor_address);
            $('#txtcompanyname').val(result.companyname);
        },
        error: function () {
            alert("Data Not Founf !!");
        }
    });
}


function Deletebyvendor(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id:id});
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteVendor",
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