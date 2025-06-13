

function ProductList56() {
    $.ajax({
        url: "/Admin/Admin/ShowProductList11",
        dataType: 'JSON',
        data: { vendor_id: $("#ddldistrict").val() },
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var data1 = JSON.parse(data.dt);
            var data2 = JSON.parse(data.dt1);

            var html = '';
            var index = 1;
            $.each(data1, function (key, item) {
                html += '<tr class="hover-primary">';


                html += '<td><input type="checkbox" class="checked" id="txtcheckbox' + item.ID + '" value = "1" /></td>';
                html += '<td>' + item.ID + '</td>';
                html += '<td>' + item.ItemName + '</td>';
                html += '<td>' + item.unit_rate + '</td>';
                html += '<td><input type="text" id="vendorprice' + item.ID + '" /></td>';


                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);

            $(".tbodyData TR").each(function () {

                var row = $(this);
                var id = row.find('td:eq(1)').html();
                for (var j = 0; j < data2.length; j++) {
                    if (id == data2[j].item_id) {
                        $("#txtcheckbox" + id + "").prop("checked", true);
                        $("#vendorprice" + id + "").val();
                        break;
                    }
                }
            });

        }
    });
}


function VendorMapping() {
    var data = new Array();
    $(".tbodyData TR").each(function () {
        var item = {};
        var row = $(this);
        var check = row.find('input.checked:checked').eq(0).val();
        if (check == 1) {

            item.vendor_id = $("#ddldistrict").val();
            item.id = row.find('td:eq(1)').html();
            item.unit_rate = row.find('td:eq(3)').html();
            item.vendor_price = row.find('td:eq(4) input').val();
            item.IsChecked = check;
            data.push(item);
        }
    });

    $.ajax({
        url: "/Admin/Admin/VendorMapping",
        dataType: 'JSON',
        data: { jsondata: JSON.stringify(data) },
        type: 'POST',
        success: function (result) {
            console.log(result);
            swal({
                title: "",
                text: result,
                icon: "success",
                timer: 10000,
            });
            clear();
        },
        error: function (er) {
            swal({
                title: "",
                text: er.statusText,
                icon: "error",
                timer: 10000,
            });
        }
    });
}




function exportexcel(type, fn, dl)
{


    var ddd = "Abstract";
    var elt = document.getElementById('example1');
    console.log(elt);
    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });


    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        XLSX.writeFile(wb, fn || (ddd + '.' + (type || 'xlsx')));

    // XLSX.writeFile(wb, fn || (ddd+ (type+ '.' || 'xlsx')));
}


