$(document).ready(function () {
    CKEDITOR.replace('#txtshipping');
});
function Clear() {
    $('#btnSave').val("SAVE");
    ReloadPageWithAreas('Admin', 'Admin', 'Shipping');
}
function IUShipping() {
    var Shipping = CKEDITOR.instances["txtshipping"].getData();
    var id = $("#id").val();
    $.ajax({
        type: "POST",
        url: '/Admin/Admin/IUShipping',
        data: { Shipping: Shipping, id: id },
        dataType: "JSON",
        success: function (result) {
            if (result.message == "Shipping added.") {
                ShowShipping();
                if (result.id == 0) {
                    alert("Shipping added successfully.");
                }
                else {
                    alert("Shipping modify successfully.");
                }
            }
            Clearshipping();
        },
        error: function () {
            alert("Shipping not added.");
        }
    });
}
function ShowShipping() {
    var item;
    $.ajax({
        url: '/Admin/Admin/ShowShipping',
        data: JSON.stringify(item),
        dataType: 'JSON',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr>';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.shipping + '</td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyshipping(' + item.id + ')"><i class="fa fa-edit"></i></a><a class="btn btn-sm" href="#" onclick="return Deletebyshipping(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Shippingbody').html(html);
        }
    });
}
function Editbyshipping(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditShippingDesc",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.id);
            CKEDITOR.instances["txtshipping"].setData(result.shipping);
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
function Clearshipping() {
    CKEDITOR.instances["txtshipping"].setData("");
    $('#btnSave').val("SAVE");
    $("#id").val("0");
}
function Deletebyshipping(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteShipping",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            // contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    ShowShipping();
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
ShowShipping();
