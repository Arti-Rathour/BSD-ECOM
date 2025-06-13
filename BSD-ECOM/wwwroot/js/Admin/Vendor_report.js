


function ProductList98() {
    $.ajax({
        url: "/Admin/Admin/Showvendoreport",
        dataType: 'JSON',
        data: { vendor_id: $("#ddldistrict").val() },
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var data = JSON.parse(data.dt);

            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';


              //  html += '<td><input type="checkbox" class="checked" id="txtcheckbox' + item.ID + '" value = "1" /></td>';
                //  html += '<td>' + item.ID + '</td>';
                html += '<td>' + item.vendar_name + '</td>';
                html += '<td>' + item.ItemName + '</td>';
                html += '<td>' + item.Unit_Rate + '</td>';
                html += '<td>' + item.vendor_price + '</td>';



                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);

           

        }
    });
}
