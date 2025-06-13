$(document).ready(function () {
    if ($("#example1 .tbodyData td").length>0) {
        $("#btnSave").css("display", "");
    }
    else {
        $("#btnSave").css("display", "none");
    }
    
})
$("#ddlSuCategory").change(function () {
    
    BindCategory();
});
$("#ddlCategory").change(function () {
    BindSubCategory();
});
$("#ddlsubCategory").change(function () {
    ItemstockqtyList();
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
function BindSubCategory() {
    var selected_val = $('#ddlCategory').find(":selected").attr('value');
    $("#ddlCategory").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Admin/Admin/BindSubCategory', //url to be called
        data: "id=" + selected_val, //data to be send
        success: function (json, result) {
            $("#ddlsubCategory").empty();
            json = json || {};
            $("#ddlsubCategory").append('<option value="0">Select Category</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlsubCategory").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlsubCategory").prop("disabled", false);
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}
function ItemstockqtyList() {
    // var SuperCategory = $('#ddlSuCategory').find("option:selected").val();
    //var Category = $('#ddlCategory').find("option:selected").val();
    var SubCategory = $('#ddlsubCategory').find("option:selected").val();
    $.ajax({
        url: "/Admin/Admin/ShowItemstrock",
        data: { SubCategory: SubCategory },
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.length > 0) {
                $("#btnSave").css("display", "");
            }

            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none"><span class="itemdetailsid">' + item.itemdetailsid + '</span></td>';
                html += '<td>' + item.itemName + '</td>';
                html += '<td class = "oldqty">' + item.stockqty + '</td>';
                html += '<td><input type = "text" class="newqty" id="updateQty" value="0"/></td>';
                //html += '<td><input type = "button" class="stockquty" value="Update" onclick = "Updatebyid(' + item.itemdetailsid + ')"/></td>';
                //html += '<td><input type = "button" value="Update" onclick="Updatebyid(' + item.itemdetailsid + ')"/></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}
function Updatebyid() {
    var SubCategory = $('#ddlsubCategory').find("option:selected").val();
    //var itemdetailsid = $('#itemdetailsid').val();
    var stock = new Array();
    $("#example1 tbody tr").each(function () {
        var row = $(this);
        var stockq = {};
        var itemdetailsid = row.find("span.itemdetailsid").text();
        if (itemdetailsid != '') {
            stockq.itemdetailsid = row.find("span.itemdetailsid").text();
            var oldQty = row.find("td.oldqty").text();
            var newqty = row.find("input.newqty").val();
            stockq.stockqty = parseInt(oldQty) + parseInt(newqty);
            row.find("input.newqty").val(0);
            stock.push(stockq);
        }
    });
    var jsonInput = JSON.stringify(stock)
    $.ajax({
        url: "/Admin/Admin/Updatestockqty",
        data: { jsonInput: jsonInput },
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            if (result.message == "stockqty modify successfully.") {

                alert("stockqty  modify successfully.");

                ItemstockqtyList();
            }
        },
        error: function () {
            alert("stockqty not modify successfully.");
        }
    });
}