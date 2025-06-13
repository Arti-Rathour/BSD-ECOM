function ShowReturned_Orders() {
    var user_id = $('#ddlUserNames').find("option:selected").val();
    var from_date = $('#txtfdate').val();
    var to_date = $('#txttdate').val();    
    $.ajax({
        url: "/Admin/Admin/ShowReturnOrderReport",
        data: { user_id: user_id, from_date: from_date, to_date: to_date },
        dataType: 'JSON',
        type: 'GET',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                html += '<td>' + index + '</td>';
                html += '<td style="display:none"><span class="order_id">' + item.order_id + '</span></td>';
                html += '<td>' + item.order_no + '</td>';
                html += '<td>' + item.userName + '</td>';
                //html += '<td style="display:none"><span class="user_id">' + item.user_id + '</span></td>';
                //html += '<td>' + item.userName + '</td>';
                html += '<td><span class="amount">' + item.amount + '</td>';
                html += '<td>' + item.order_date + '</td>';
                html += '<td>' + item.return_date + '</td>';
                //html += '<td>' + item.return_User + '</td>';
                html += '<td>' + item.return_Type + '</td>';
                html += '<td>' + item.payment_mode + '</td>';
                html += '<td>' + item.approvestatus+'</td>'
                //html += '<td><input type="checkbox" id="chkStatus" class="status"></td>';
                //if ((item.chkStatus) == 1) {
                //    html += '<td class="active">Active</td>';
                //} else {
                //    html += '<td class="active">Inactive</td>';
                //}
                html += '<td><a href="/Admin/Admin/Returned_Orders_Report?order_id=' + item.order_id + '"><i class="fa fa-eye"></i></a></td>';
               // html += '<td><a onclick="return ReturnPopup(' + item.order_id + ',' + item.amount + ')"><i class="fa fa-eye"></i></a></td>';
                // html += '<td><a class="btn btn-sm" href="#" onclick="return EditState(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';
                // html += '<td><button class="btnView"><a class="btn btn-sm" href="#"><i class="fa fa-view"></i></a></button></td>';
                // html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            if (html != null) {
                $('.Returned_Ordersbody').html(html);
            }
            else {
                $('.Returned_Ordersbody').html("Data not available.");
            }
        }
    });
}

function Cleartext() {
    $('#ddlUserNames').val("0");
    $('#txtfdate').val("");
    $('#txttdate').val("");
    ShowReturned_Orders();
}