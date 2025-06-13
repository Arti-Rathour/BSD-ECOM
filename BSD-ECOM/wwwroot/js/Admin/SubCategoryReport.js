$("#ddlSuCategory").change(function () {
    BindCategory();
});
function BindCategory() {
    var selected_val = $('#ddlSuCategory').find(":selected").attr('value');
    $("#ddlSuCategory").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Admin/Admin/BindCategory', //url to be called
        data: "id=" + selected_val, //data to be send
        success: function (json, result) {
            $("#ddlCategory").empty();
            json = json || {};
            $("#ddlCategory").append('<option value="0">Select Category</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlCategory").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlCategory").prop("disabled", false);
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}
function ShowDataInTableReport() {
    $.ajax({
        url: "/Admin/Admin/ShowSubCategoryReport",
        dataType: 'JSON',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td>' + item.cat_id + '</td>';
                
                html += '<td style="display:none">' + item.main_cat_id + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                html += '<td style="display:none">' + item.category_id + '</td>';
                html += '<td>' + item.category_name + '</td>';
                html += '<td>' + item.cat_name + '</td>';
                html += '<td>' + item.entry_date + '</td>';
                if ((item.cat_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">AnActive</td>';
                }
               
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'SubCategoryReport');
}
function Searchbyname() {

    var Main_cat_id = $('#ddlSuCategory').find("option:selected").val();
    var category_id = $('#ddlCategory').find("option:selected").val();
    //var Main_cat_name = $('#ddlSuCategory').find("option:selected").text();
    var cat_id = $('#cat_id').val();
    var cat_name = $('#txtCategory').val();
    $.ajax({
        url: "/Admin/Admin/SearchSubCategoryReport",
        data: { cat_name: cat_name, cat_id: cat_id, Main_cat_id: Main_cat_id, category_id: category_id},
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td>' + item.cat_id + '</td>';
                html += '<td>' + item.cat_name + '</td>';
                html += '<td style="display:none">' + item.main_cat_id + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                html += '<td style="display:none">' + item.category_id + '</td>';
                html += '<td>' + item.category_name + '</td>';
                if ((item.cat_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">AnActive</td>';
                }
                html += '<td>' + item.entry_date + '</td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        },
        error: function () {
            alert("Data Not Founf !!");
        }
    });
}

function ReTableBind() {
    var cat_name = $('#txtCategory').val();
    if (cat_name == "") {
        ShowDataInTableReport();
    }
}


function clearsubcategoryreport() {
    $('#ddlSuCategory').val("0");
    $('#ddlCategory').val("0");
    $('#txtCategory').val("");
}