$(document).ready(function () {
    CKEDITOR.replace('#txtDelivery_info');
});

function ShowDataintable() {
    var item;
    $.ajax({
        url: '/Admin/Admin/ShowDeliveryInformation',
        data: JSON.stringify(item),
        dataType: 'JSON',
        type: 'GET',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr>';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.delivery_Info + '</td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbydelivery(' + item.id + ')"><i class="fa fa-edit"></i></a><a class="btn btn-sm" href="#" onclick="return Deletebydelivery(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.DeliveryInformationbody').html(html);
        }
    });
}

function IUDeliveryInfo() {
    var Delivery_info = CKEDITOR.instances["txtDelivery_info"].getData();
    var id = $("#id").val();
    if (Delivery_info == "") {
        alert("Please Enter Delivery Information.");
    } else {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IUDeliveryInfo',
            data: { Delivery_info: Delivery_info, id: id },
            dataType: "JSON",
            success: function (result) {
                //ShowdeliveryInformation();
                if (result.message == "Delivery Information added.") {
                    if (result.id == 0) {
                        alert("Delivery Information added successfully.");
                    }
                    else {
                        alert("Delivery Information modify successfully.");
                    }
                }
                ClearData();
                ShowDataintable();
            },
            error: function () {
                alert("Delivery Information not added.");
            }
        });
    }
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'DeliveryInformation');
    $('#btnSave').val("SAVE");
}
function Editbydelivery(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditDeliveryInformation",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.id);
            CKEDITOR.instances["txtDelivery_info"].setData(result.delivery_Info);

        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
function ClearData() {
    CKEDITOR.instances["txtDelivery_info"].setData("");
    //$("#txtDelivery_info").innerhtml("");
    $("#id").val("0");
    $('#btnSave').val("SAVE");
}
function Deletebydelivery(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    // var item;
    if (checkstr == true)
    {
        $.ajax({
            url: "/Admin/Admin/DeleteDeliveryInformation",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            // contentType: 'application/json; charset=utf-8',
            success: function (result)
            {
                if (result.msg == "Delete Successfull!!")
                {
                    alert("Delete Success");
                    ShowDataintable();
                    //ShowDeliveryInformation();
                }
            },
            error: function (errormessage)
            {
                alert(errormessage.responseText);
            }
        });
    }
    else
    {

        return false;
    }
}
ShowDataintable();
