$(document).ready(function () {
    CKEDITOR.replace('#txtTerms');
});
function Clear() {
    $('#btnSave').val("SAVE");
    ReloadPageWithAreas('Admin', 'Admin', 'Terms');
}
function IUTerms() {
    var terms = CKEDITOR.instances["txtTerms"].getData();
    var id = $("#id").val();
    if (terms == "") {
        alert("Terms cann't left blank.");
    }
    else {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IUTerms',
            data: { terms: terms, id: id },
            dataType: "JSON",
            success: function (result) {
                if (result.message == "Terms added.") {
                    ShowTerms();
                    if (result.id == 0) {
                        alert("Terms added successfully.");
                    }
                    else {
                        alert("Terms modify successfully.");
                    }
                }
                Clearterms();
            },
            error: function () {
                alert("Terms not added.");
            }
        });
    }
    
}
function ShowTerms() {
    var item;
    $.ajax({
        url: '/Admin/Admin/ShowTerms',
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
                html += '<td>' + item.terms + '</td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyterms(' + item.id + ')"><i class="fa fa-edit"></i></a><a class="btn btn-sm" href="#" onclick="return Deletebyterms(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Termsbody').html(html);
        }
    });
}
function Clearterms() {
    CKEDITOR.instances["txtTerms"].setData("");
    $('#btnSaved').val("SAVE");
    $("#id").val("0");
}
function Deletebyterms(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteTerms",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    ShowTerms();
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
function Editbyterms(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditTerms",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',        
        success: function (result) {
            $('#btnSaved').val("UPDATE");
            $('#id').val(result.id);            
            CKEDITOR.instances["txtTerms"].setData(result.terms);
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
ShowTerms();
