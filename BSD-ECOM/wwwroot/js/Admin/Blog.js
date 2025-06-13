var filenameorginal = "";
$(document).ready(function () {
    $("#blogimg").change(function () {
        filenameorginal = $('#blogimg').val();
    })
})

function IUBlog() {
    var id = $('#id').val();
    var Title = $('#txttitle').val();
    var ShortDesc = CKEDITOR.instances["txtShortDesc"].getData();
    var LongDesc = CKEDITOR.instances["txtlongDesc"].getData();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var fdata = new FormData();
    var fileExtension = ['png', 'img', 'jpg', 'jpeg', 'PNG', 'IMG', 'JPG', 'JPEG'];
    var filename = $('#blogimg').val();
    if (filenameorginal == filename) {
        if (filename.length == 0) {
            alert("Please select a file.");
            return false;
        }
        else {
            var extension = filename.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("flg", "okg");
        var fileUpload = $("#blogimg").get(0);
        var files = fileUpload.files;
        fdata.append(files[0].name, files[0]);
    }
    else {
        fdata.append("flg", "ok");

    }
    fdata.append("id", id);
    fdata.append("Title", Title);
    fdata.append("ShortDesc", ShortDesc);
    fdata.append("LongDesc", LongDesc);
    fdata.append("Status", Status);
    fdata.append("filenmes", filenameorginal);
    var msg = ValidationCreate();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IUBlog',
            dataType: "JSON",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: fdata,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.message == "This Title is already exit") {
                    alert("This Title is already exit.");
                }
                else if (result.message == "Blog added.") {
                    if (result.id == '0') {
                        alert("Blog added successfully");
                    }
                    else {
                        alert("Blog modify successfully.");
                    }
                    ShowBlog();
                    clearBlog();
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });
    }
    else {
        alert(msg);
    }
   
}

function ShowBlog() {
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    $.ajax({
        url: "/Admin/Admin/ShowBlog",
        //data: { Status: Status },
        dataType: 'JSON',
        type: 'GET',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.title + '</td>';
                //html += '<td>' + item.blogImage + '</td>';
                html += '<td><img src="/images/Blog/6/' + item.blogImage + '"  width="75" height="80" /></td>';
                html += '<td>' + item.shortDesc + '</td>';
                html += '<td>' + item.longDesc + '</td>';
                if ((item.status) == 1) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyblog(' + item.id + ',' + item.status+')"><i class="fa fa-edit"></i></a></td>';
                // html += '<td><button class="btnView"><a class="btn btn-sm" href="#"><i class="fa fa-view"></i></a></button></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Deletebyblog(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.blogbody').html(html);
        }
    });
}

function Deletebyblog(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteBlog",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            // contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    alert("Delete Success");
                    ShowBlog();
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

function Editbyblog(id,status) {
    var PetObj = JSON.stringify({ id: id, status: status });
    $.ajax({
        url: "/Admin/Admin/EditBlog",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.id);
            $('#txttitle').val(result.title);
            $('#img').attr('src', "/images/Blog/6/" + result.blogImage + "");
            CKEDITOR.instances["txtShortDesc"].setData(result.shortDesc);
            CKEDITOR.instances["txtlongDesc"].setData(result.longDesc);
            if ((status) == true) {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
            filenameorginal = result.blogImage;
            //ShowBlog();
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}


blogimg.onchange = evt => {
    const [file] = blogimg.files
    if (file) {
        img.src = URL.createObjectURL(file)
    }
}

function ValidationCreate() {
    var msg = "";
    if ($('#txttitle').val() == "") { msg += "Tital can not  Blank !! \n"; }
    else if ($('#txttitle').val().length > 197) { msg += "Tital character should be less than 197 !! \n"; }
    if (CKEDITOR.instances["txtShortDesc"].getData() == "") { msg += "ShortDesc can not  Blank !! \n"; }
    if (CKEDITOR.instances["txtlongDesc"].getData() == "") { msg += "longDesc can not  Blank !! \n"; }
    //if (msg != "") { alert(msg); }
    return msg;
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'Blog');
}
function clearBlog() {
    $('#id').val("0");
    $('#img').attr('src', "/images/dummy-pic.png");
    $('#txttitle').val("");
    $('#btnSave').val("Save");
    $('#blogimg').val("");
    CKEDITOR.instances["txtShortDesc"].setData("");
    CKEDITOR.instances["txtlongDesc"].setData("");
}
