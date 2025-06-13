function ShowDataInTableBrandReport() {
    $.ajax({
        url: "/Admin/Admin/ShowBrandReport",
        dataType: 'JSON',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.order_id + '</td>';
                html += '<td>' + item.first_name + '</td>';
                html += '<td>' + item.address1 + '</td>';
                html += '<td>' + item.mobile + '</td>';
                html += '<td>' + item.itemName + '</td>';
                html += '<td>' + item.brand_name + '</td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}
function exportexcel(type, fn, dl) {
    var pagename = $("#brandreport").text();
    var elt = document.getElementById('data-table');
    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        XLSX.writeFile(wb, fn || (pagename + '.' + (type || 'xlsx')));
        //XLSX.writeFile(wb, fn || (ddd+ (type+ '.' || 'xlsx')));
}
function Searchbyname() {
    var mobile = $('#txtMobile').val();
    var first_name = $('#txtName').val();
    $.ajax({
        url: "/Admin/Admin/SearchBrandReport",
        data: { mobile: mobile, first_name: first_name},
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.order_id + '</td>';
                html += '<td>' + item.first_name + '</td>';
                html += '<td>' + item.address1 + '</td>';
                html += '<td>' + item.mobile + '</td>';
                html += '<td>' + item.itemName + '</td>';
                html += '<td>' + item.brand_name + '</td>';
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
    var first_name = $('#txtName').val();
    if (first_name == "") {
        ShowDataInTableBrandReport();
    }
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'BrandReport');
}
function ClearData() {
   // $("#txtName").innerhtml("");
    $("#txtName").val("");
    $("#txtMobile").val("");
}
ShowDataInTableBrandReport();