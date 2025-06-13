$(document).ready(function () {
    CKEDITOR.replace('#txtFAQ');

});
function IUFAQ() {
    var FAQ = CKEDITOR.instances["txtFAQ"].getData();
    var id = $("#id").val();
    if (FAQ == "") {
        alert("FAQ can not left blank.")
    }
    else {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IUFAQ',
            data: { FAQ: FAQ, id: id },
            dataType: "JSON",
            success: function (result) {
                if (result.message == "FAQ added.") {
                    ShowFAQ();
                    if (result.id == 0) {
                        alert("FAQ added successfully.");
                    }
                    else {
                        alert("FAQ modify successfully.");
                    }
                }
                $("#id").val("");
                $('#btnSave').val("SAVE");
                CKEDITOR.instances["txtFAQ"].setData("");
            },
            error: function () {
                alert("FAQ not added.");
            }
        });
    }
}
function ShowFAQ() {
    var item;
    $.ajax({
        url: '/Admin/Admin/ShowFAQ',
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
                html += '<td>' + item.faq + '</td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return EditbyFAQ(' + item.id + ')"><i class="fa fa-edit"></i></a><a class="btn btn-sm" href="#" onclick="return DeletebyFAQ(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.FAQbody').html(html);
        }
    });
}
function Clear() {
    $('#btnSave').val("SAVE");
    ReloadPageWithAreas('Admin', 'Admin', 'FAQ');
    
}


function clearFAQ() {
    CKEDITOR.instances["txtFAQ"].setData("");
    $('#btnSave').val("SAVE");
}
function DeletebyFAQ(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteFAQ",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            // contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    ShowFAQ();
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
function EditbyFAQ(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditFAQ",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#id').val(result.id);
            $('#btnSave').val("UPDATE");
            CKEDITOR.instances["txtFAQ"].setData(result.faq);
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
ShowFAQ();
