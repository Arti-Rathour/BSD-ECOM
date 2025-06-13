function CustomerList() {

    var searchitem = $("#txtsearch").val();
    $.ajax({
        url: "/Admin/Admin/ShowCustomerList",
        dataType: 'JSON',
        type: 'POST',
        data: { searchitem: searchitem },
       // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.reg_id + '</td>';
                html += '<td>' + item.first_name + '</td>';
                html += '<td>' + item.email + '</td>';
                html += '<td>' + item.password + '</td>';
                html += '<td>' + item.mobileNo + '</td>';
                html += '<td>' + item.stateID + '</td>';
                html += '<td>' + item.cityID + '</td>';
                html += '<td style="display:none">' + item.dob + '</td>';
                //html += '<td><a class="btn btn-sm" href="/Admin/Admin/Approve?id=' + item.id + '">View</a></td>';
                //html += '<td><a class="btn btn-sm" href="#" onclick="return Save(' + item.id + ')">Approve</a></td>';
                //html += '<td style="width:15%;"><a class="btn btn-sm" href="#" onclick="return Notapprove(' + item.id + ')">Not Approve</a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
} 
$(document).ready(function () {
    $('#LoadExcel').on('click', function () {
        var buttonValue = $(this).val();
        $.ajax({
            type: "POST",
            data: {buttonValue: buttonValue },
            url: "/UploadExcel/Datadownaload",
            success: function (response) {
                if (response.length == 0)
                    alert('Some error occured while downaload Data');
                else {
                    $('#divPrint').html(response);
                    exportexcel('xlsx');
                }
            },
            error: function (e) {
                $('#divPrint').html(e.responseText);
            }
        });
        // document.theForm.submit();

    });
});

function exportexcel(type, fn, dl) {
    var pagename = $("#customerlist").text();
    var elt = document.getElementById('data-table');
    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }):
        XLSX.writeFile(wb, fn || (pagename + '.' + (type || 'xlsx')));
    // XLSX.writeFile(wb, fn || (ddd+ (type+ '.' || 'xlsx')));
}
function exportexceldata(type, fn, dl) {
    //var ddlval = $("#ddlType option:selected").text();
    //var reportname = $("#txtreporttype").val();
    var pagename = $("#customerlist").text();
    //var fname = '@ViewBag.ReportType';
    //var fname = reportname + ' Report';
    //var ddd = ddlval.trim() == "Select Reports" ? fname : ddlval;
    var elt = document.getElementById('data-table');
    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        XLSX.writeFile(wb, fn || (pagename + '.' + (type || 'xlsx')));
    // XLSX.writeFile(wb, fn || (ddd+ (type+ '.' || 'xlsx')));
}
CustomerList();