$(document).ready(function () {
    Orders_delievery_Report();
});


function Orders_delievery_Report() {
    $.ajax({
        url: "/Admin/Admin/showOrder_delivery_Report",
        dataType: 'JSON',
        type: 'GET',
        data: {},
        success: function (data) {
            //$('#orderlist').modal('show');
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                //html += '<td>' + item.order_id + '</td>';
                html += '<td>' + item.order_no + '</td>';
                html += '<td>' + item.userName + '</td>';
                html += '<td>' + item.amount + '</td>';
                html += '<td>' + item.mobileNo + '</td>';
                html += '<td>' + item.order_date + '</td>';
                html += '<td>' + item.dispatch_date + '</td>';
                html += '<td>' + item.payment_mode + '</td>';
                html += '</tr>';
                index++;
            });
            if (html != null) {
                $('.Delivered_Ordersbody').html(html);
            }

        }
    });
}