
function IULocation() {
    var ID = $('#id').val();
    //var state_id = $('#ddlstate').find("option:selected").val();
    var LocationName = $('#txtlocation').val();
    var PinCode = $('#txtpincode').val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();

    //$('#txtpincode').keypress(function (e) {
    //    var charCode = (e.which) ? e.which : event.keyCode
    //    if (String.fromCharCode(charCode).match(/[^0-6]/g))
    //        return false;
    //});
    var msg = validatelocation();
    if (msg == "")
    {
       $.ajax({
            type: "POST",
            url: '/Admin/Admin/IULocation',
            data: { ID: ID, LocationName: LocationName, PinCode: PinCode, Status: Status },
            dataType: "JSON",
           success: function (result) {
               alert(result.message);
                //if (result.message == "This Location is already exit.")
                //{
                //    alert("This Location is already exit.");
                //}
                //else if (result.message == "Location added.")
                //{
                //    if (result.id == 0)
                //    {
                //            alert("Location added.");
                //    }
                //    else
                //    {
                //            alert("Location modify successfully.");
                //    }
                       
                //}
               ClearLocation();

               ShowDataInTable();
                
            },
            error: function () {
                alert("Location not added.");
            }
        });
    }

    else
    {
        alert(msg);

    }
    
}
function ClearLocation() {
    $('#btnSave').val("SAVE");
    $('#id').val("0");
    $('#txtlocation').val("");
    $('#txtpincode').val("");
    $('#chkStatus').prop("checked", true);
}

function OnlyNumberKey(e) {
    var charCode = (e.which) ? e.which : event.keyCode
    if (String.fromCharCode(charCode).match(/[^0-6]/g))
        return false;
}

function ShowDataInTable() {

    var searchitem = $("#txtsearch").val();
    $.ajax({
        url: "/Admin/Admin/ShowLocation",
        dataType: 'JSON',
        type: 'GET',
        data: { searchitem: searchitem },
      //  contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td>' + item.id + '</td>';
                html += '<td>' + item.locationName + '</td>';
                html += '<td>' + item.pinCode + '</td>';
                if ((item.status) == 1) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
              // html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbylocation(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Deletebylocation(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Locationbody').html(html);
        }
    });
}

function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'Location');
}

function validatelocation() {
    var msg = "";
    if ($('#txtlocation').val() == "") { msg += "Location Name  can not left Blank !! \n"; }
    if ($('#txtpincode').val() == "") { msg += "Pin Code can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;
}

$("#data-table").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var id = currentRow.find("td:eq(0)").html();s
    var status = currentRow.find("td:eq(3)").html();
    //alert(comp_status);
    Editbyid(id, status);
});

function Editbylocation(id, status) {
    var PetObj = JSON.stringify({ id: id });
    // var item;
    $.ajax({
        url: "/Admin/Admin/EditLocation",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result)
        {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.id);
            $('#txtlocation').val(result.locationName);
            $('#txtpincode').val(result.pinCode);
            if ((item.status) == 1) {
                html += '<td class="active">Active</td>';
            } else {
                html += '<td class="active">Inactive</td>';
            }

            ShowDataInTable();
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
function Deletebylocation(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteLocation",
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

