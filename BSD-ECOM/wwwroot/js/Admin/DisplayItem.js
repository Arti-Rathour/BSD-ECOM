$("#ddlSuCategory").change(function () {
    BindCategory();
});

$("#ddlCategory").change(function () {


    ShowDataInTable();
    //BindSubCategorys();


});

$("#ddlitem").change(function () {
    ShowDataInTable();

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

function BindSubCategorys() {
    var selected_val = $('#ddlCategory').find(":selected").attr('value');
    $("#ddlCategory").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Admin/Admin/BindSubCategory', //url to be called
        data: "id=" + selected_val, //data to be send
        success: function (json, result) {
            $("#ddlitem").empty();
            json = json || {};
            $("#ddlitem").append('<option value="0">Select Item</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlitem").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlitem").prop("disabled", false);
          
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}

function ApproveItem() {
    var itemdisplays = new Array();
    $("#example1 tbody tr").each(function () {
        var row = $(this);
        var itemdisplay = {};
        itemdisplay.id = row.find("span.id").text();
        itemdisplay.chkdisplay = row.find('input.status').is(':checked') ? 1 : 0

        // $('#chkStatus').is(':checked') ? 1 : 0;
        itemdisplays.push(itemdisplay);
    });
    var itemdisplayjson = JSON.stringify(itemdisplays);
    $.ajax({
        url: "/Admin/Admin/UpdateItemDisplay",
        data: { itemdisplayjson: itemdisplayjson },
        dataType: "JSON",
        type: "POST",
        success: function (result) {
            if (result.message == "Item approve.") {

                alert("Item approve  Successfully.");
            }
            else {
                alert("Item approve  Successfully.");
            }
            ShowDataInTable();
        }
    })
}

function ShowDataInTable() {
    var SuperCategory = $('#ddlSuCategory').find("option:selected").val();
    var Category = $('#ddlCategory').find("option:selected").val();
   // var Item = $('#ddlitem').find("option:selected").val();
    var Status = $('#chkdisplay').is(':checked') ? 1 : 0;
    $.ajax({
        type: 'POST',
        url: "/Admin/Admin/ShowItemDisplay",
        data: { Supercat: SuperCategory, Item: Category, Status: Status },
        //data: "Supercat=" + SuperCategory, "Cate=" + Category,
        dataType: 'JSON',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none"><span class="id">' + item.id + '</span></td>';
                html += '<td>' + item.itemName + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                html += '<td>' + item.cat_name + '</td>';
                html += '<td>' + item.status + '</td>';
                //html += '<td>' + item.display + '</td>';
                if (item.display == "1") {
                    html += '<td><input type="checkbox" name="display" id="chkdisplay" checked  class="status"/></td>';
                }
                else {
                    html += '<td><input type="checkbox" name="display" id="chkdisplay"  class="status"/></td>';
                }
                //html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyid(' + item.cat_id + ')"><i class="fa fa-checkbox"></i></a></td>';
                // html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyid(' + item.main_cat_id + ')"><i class="fa fa-edit"></i></a><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.main_cat_id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}
