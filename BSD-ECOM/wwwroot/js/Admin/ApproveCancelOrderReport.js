function ShowCancelled_Orders() {
    var user_id = $('#ddlUserNames').find("option:selected").val();
    var from_date = $('#txtfdate').val();
    var to_date = $('#txttdate').val();
    $.ajax({
        url: "/Admin/Admin/ShowApproveCancelOrderReport",
        dataType: 'JSON',
        type: 'GET',
        data: { user_id: user_id, from_date: from_date, to_date: to_date },
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                html += '<td>' + index + '</td>';
                //html += '<td style="display:none">' + item.order_id + '</td>';
                html += '<td>' + item.order_no + '</td>';
                html += '<td>' + item.userName + '</td>';
                html += '<td>' + item.amount + '</td>';
                html += '<td>' + item.order_date + '</td>';
                html += '<td>' + item.cal_date + '</td>';
                /*html += '<td>' + item.cancelled_User + '</td>';*/
                html += '<td>' + item.cancel_type + '</td>';
                html += '<td>' + item.payment_mode + '</td>'
                html += '<td><a href="/Admin/Admin/Cancelled_Orders_Report?order_id=' + item.order_id + '"><i class="fa fa-eye"></i></a></td>';
                /*html += '<td><a class="btn btn-sm" href="#" onclick="return ApproveCancelPopPup(' + item.order_id + ',' + item.amount + ')"><i class="fa fa-eye"></i></a></td>';*/
                //html += '<td><a href="/Admin/Admin/Cancelled_Orders_Report"? class="btn btn-danger" value="view"> View</a></td>';

                // html += '<td><a class="btn btn-sm" href="#" onclick="return EditState(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';
                // html += '<td><button class="btnView"><a class="btn btn-sm" href="#"><i class="fa fa-view"></i></a></button></td>';
                // html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            if (html != null) {
                $('.Cancelled_Ordersbody').html(html);
            }
            else {
                $('.Cancelled_Ordersbody').html("Data not available.");
            }
        }
    });
}
function Cleartext() {
    $('#ddlUserNames').val("0");
    $('#txtfdate').val("");
    $('#txttdate').val("");
}