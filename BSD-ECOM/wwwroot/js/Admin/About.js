$(document).ready(function () {
    CKEDITOR.replace('#txtabout');
});
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'About');
    $('#btnSave').val("SAVE");
}
function InsertUpdateAbout() {
    var About = CKEDITOR.instances["txtabout"].getData();
    var id = $("#id").val();
    if (About == "") {
        alert("Please Enter About.");
    }
    else {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/InsertUpdateAbout',
            data: { About: About, id: id },
            dataType: "JSON",
            success: function (result) {
                //ShowAbout();
                if (result.message == "About added.") {
                    if (result.id == 0) {
                        alert("About added successfully.");
                    }
                    else {
                        alert("About modify successfully.");
                    }
                }
                ClearData();
                ShowAbout();
            },
            error: function () {
                alert("About not added.");
            }
        });
    }
}
function ClearData() {
    CKEDITOR.instances["txtabout"].setData("");
    //$("#txtabout").innerhtml("");
    $('#btnSave').val("SAVE");
   $("#id").val("0");
}
function ShowAbout() {
    var item;
    $.ajax({
        url: '/Admin/Admin/ShowAbout',
        data: JSON.stringify(item),
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
                html += '<td>' + item.about + '</td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyabout(' + item.id + ')"><i class="fa fa-edit"></i></a><a class="btn btn-sm" href="#" onclick="return Deletebyabout(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Aboutbody').html(html);
        }
    });
}
function Deletebyabout(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteAbout",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    alert("Delete Success");
                    ShowAbout();
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
function Editbyabout(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditAbout",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.id);
            //$('#txtabout').val(result.About);
            CKEDITOR.instances["txtabout"].setData(result.about);
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
ShowAbout();