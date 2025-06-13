$(document).ready(function () {
    StockReport();
});




function StockReport() {
    var productname = $("#txtSuperCategory").val();
    $.ajax({
        
        url: "/Admin/Admin/StockReportList",
        data: {
            productname: productname
        },
        dataType: 'JSON',
        type: 'POST',
       // contentType: 'application/json; charset=utf-8',
       
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.itemName + '</td>';
                html += '<td>' + item.unit_Rate + '</td>';
                html += '<td>' + item.stock_qty + '</td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
}



function exportToExcel(type, fn, dl) {


    var ddd = "Abstract";
    var elt = document.getElementById('example');
    console.log(elt);
    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });


    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        XLSX.writeFile(wb, fn || (ddd + '.' + (type || 'xlsx')));

    // XLSX.writeFile(wb, fn || (ddd+ (type+ '.' || 'xlsx')));
}