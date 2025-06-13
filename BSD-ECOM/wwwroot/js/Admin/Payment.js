$(document).ready(function () {
    CKEDITOR.replace('#txtPayment');
});
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'Payment');
    $('#btnSave').val("SAVE");
}
function IUPayment() {
    var paymentcontent = CKEDITOR.instances["txtPayment"].getData();
    var id = $("#id").val();
    if (paymentcontent == "") {
        alert("Payment Information can not left blank.");
    }
    else {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IUPayment',
            data: { paymentcontent: paymentcontent, id: id },
            dataType: "JSON",
            success: function (result) {
                if (result.message == "Payments added.") {
                    ShowPayment();
                    if (result.id == 0) {
                        alert("Payments added successfully.");
                    }
                    else {
                        alert("Payments modify successfully.");
                    }
                }
                ClearData();
                ShowPayment();
            },
            error: function () {
                alert("Payments not added.");
            }
        });
    }
   
}
function ShowPayment() {
    var item;
    $.ajax({
        url: '/Admin/Admin/ShowPayment',
       // data: JSON.stringify(item),
        dataType: 'JSON',
        type: 'GET',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr>';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.payment + '</td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbypayment(' + item.id + ')"><i class="fa fa-edit"></i></a><a class="btn btn-sm" href="#" onclick="return Deletebypayment(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Paymentbody').html(html);
        }
    });
}
function Editbypayment(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditPayment",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.id);
            CKEDITOR.instances["txtPayment"].setData(result.payment);
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
function ClearData() {
     CKEDITOR.instances["txtPayment"].setData("");
    $('#btnSave').val("SAVE");
    $("#id").val("0");
}
function Deletebypayment(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeletePayment",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    ShowPayment();
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
ShowPayment();
