



function ShowDataInTable() {
    var productStatus = $("#productStatus").val();
    var fromDate = $("#txtfdate").val();
    var toDate = $("#txttdate").val();
    $.ajax({
        url: "/Admin/Admin/ShowOrderReport",
        type: 'POST',
        data: { productStatus: productStatus, fromDate: fromDate, toDate: toDate },
        dataType: 'JSON',
        // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td>' + item.order_id + '</td>';
                html += '<td>' + item.order_no + '</td>';
                html += '<td>' + item.userName + '</td>';
                html += '<td>' + item.amount + '</td>';
                html += '<td>' + item.mobileNo + '</td>';
                html += '<td>' + item.order_date + '</td>';
                html += '</tr>';
                index++;
            });
            if (html != "") {
                $('.tbodyData').html(html);
            }
            else {
                $('.tbodyData').html("No data found for this services");
            }
        }
    });
}