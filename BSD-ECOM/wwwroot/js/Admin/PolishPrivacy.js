
$(document).ready(function () {
    CKEDITOR.replace('#txtPrivacyPolish');
});

function Clear() {
    $('#btnSave').val("SAVE");
    ReloadPageWithAreas('Admin', 'Admin', 'PrivacyPolish');
}

function IUPrivacyPolish() {
    var privacypolish = CKEDITOR.instances["txtPrivacyPolish"].getData();
    var id = $("#id").val();
    if (privacypolish == "") {
        alert("Privacy Polish cann't left blank.");
    }
    else {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IUPrivacyPolish',
            data: { privacypolish: privacypolish, id: id },
            dataType: "JSON",
            success: function (result) {
                if (result.message == "PrivacyPolish added.") {
                    ShowPrivacyPolish();
                    if (result.id == 0) {
                        alert("PrivacyPolish added successfully.");
                    }
                    else {
                        alert("PrivacyPolish modify successfully.");
                    }
                }
                Clearpolicy();
            },
            error: function () {
                alert("PrivacyPolish not added.");
            }
        });
    }
}

function ShowPrivacyPolish() {
    var item;
    $.ajax({
        url: '/Admin/Admin/ShowPrivacyPolish',
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
                html += '<td>' + item.privacypolish + '</td>';
                html += '<td><a sclass="btn btn-sm" href="#" onclick="return Editbypolicy(' + item.id + ')"><i class="fa fa-edit"></i></a><a class="btn btn-sm" href="#" onclick="return Deletebypolicy(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.PrivacyPolishbody').html(html);
        }
    });
}

function Clearpolicy() {
    $('#btnSave').val("SAVE");
    CKEDITOR.instances["txtPrivacyPolish"].setData("");
    //$("#txtPrivacyPolish").innerhtml("");
    $("#id").val("0");
}

function Deletebypolicy(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeletePrivacyPolish",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    ShowPrivacyPolish();
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

function Editbypolicy(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditPrivacyPolish",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.id);
            CKEDITOR.instances["txtPrivacyPolish"].setData(result.privacypolish);
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
