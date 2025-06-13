
//-------------------------Orders----------------------------

/*const { ajax } = require("jquery");*/

function ShowDataInTabletemp() {
    var UserName = $('#ddlUserNames').find("option:selected").val();
    var from_date = $('#txtfdate').val();
    var to_date = $('#txttdate').val();
    $.ajax({
        url: "/Admin/Admin/ShowOrders",
        data: { from_date: from_date, to_date: to_date, UserName: UserName },
        dataType: 'JSON',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                html += '<td>' + index + '</td>';
                html += '<td>' + item.order_no + '</td>';
                html += '<td style="display:none;">' + item.order_id + '</td>';
                html += '<td style="display:none;">' + item.user_id + '</td>';
                html += '<td>' + item.userName + '</td>';
                html += '<td>' + item.mobileno + '</td>';
                html += '<td>' + item.itemName + '</td>';

                html += '<td style="display:none;">' + item.email + '</td>';
                html += '<td style="display:none;">' + item.amount + '</td>';
                html += '<td style="display:none;">' + item.payment_mode + '</td>';
                html += '<td style="display:none;">' + item.unit_qty + '</td>';
                
                //html += '<td><i  data-toggle="modal" data-target="#orderlist" class="fa fa-eye"></i></td>';
               // html += '<td><a onclick="return OrdersReport(' + item.order_id + ')"><i class="fa fa-eye"></i></a></td>';
               // html += '<td><a onclick="return fillInquiryDetails(' + item.order_id + ')"><i class="fa fa-eye"></i></a></td>';
                html += `
                   <td>
              <a onclick="return fillInquiryDetails(${item.order_id})" style="text-decoration: none; cursor: pointer; display: flex; flex-direction: column; align-items: center; color: black;" 
               onmouseover="this.style.color='#007BFF';" 
               onmouseout="this.style.color='black';">
                <span style="font-size: 14px;">Reconfirm</span>
              <i class="fa fa-eye" style="font-size: 24px; margin-top: 5px;"></i>
            </a>
             </td>`;
            //    html += `
            //       <td>
            //  <a onclick="return fillInquiryDetailsf(${item.order_id})" style="text-decoration: none; cursor: pointer; display: flex; flex-direction: column; align-items: center; color: black;"
            //   onmouseover="this.style.color='#007BFF';" 
            //   onmouseout="this.style.color='black';">
            //    <span style="font-size: 14px;">Final</span>
            //  <i class="fa fa-eye" style="font-size: 24px; margin-top: 5px;"></i>
            //</a>
            // </td>`;

               // html += '<td><a onclick="return fillInquiryDetails(' + item.order_id + ')"><i class="fa fa-eye"></i></a></td>';
                //html += '<td><input type = "button" data-toggle="modal" data-target="#btnconfirm" onclick="fillInquiryDetails(' + item.order_id + ')" class = "btn btn-primary"  value = "CONVERT TEMPORARY ORDER INTO CONFIRM ORDER"></td>';
                // html += '<td><a class="btn btn-sm" href="#" onclick="return EditState(' + item.id + ')"></td>';
                // html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Ordersbody').html(html);
        }
    });
}








function fillInquiryDetails(id) {
    $("#hidid").val(id);
    BindPopup(id);
    //window.location.href = "/Admin/Admin/Inquiry_Page?id=" + id + "";
}






function BindPopup(id) {
    $.ajax({
        url: "/Admin/Admin/BindPopup11",
        dataType: 'JSON',
        type: 'GET',
        data: { id: id },
        success: function (data) {
            //$('#orderlist').modal('show');
            data = JSON.parse(data);
            $("#hididd").val(data[0].order_id);
            $("#hiduserid").val(data[0].user_id);
            $("#txtFullname").val(data[0].UserName);
            $("#txtEmail").val(data[0].email);
            $("#txtMobile").val(data[0].mobileno);


            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                html += '<td>' + index + '</td>';
                html += '<td style="display:none;">' + item.ID + '</td>';

                html += '<td>' + item.ItemName + '</td>';
                //html += '<td>' + item.amount + '</td>';
                //html += '<td>' + item.UnitQty + '</td>';


                html += '<td><input type="text" id="txtamount" value="' + item.rate + '" /></td>';
                html += '<td><input type="text" id="txtquantity" value="' + item.UnitQty + '" /></td>';
                html += '<td><input type="text" id="txtcourior" value="' + item.Courior + '" /></td>';
                html += '<td><input type="text" oninput="totalrate()" id="txttax" value="' + item.Tax + '" /></td>';
                //html += '<td><input type="text" id="txtTotal" value="' + 0 + '" /></td>';

                html += '<td>' + item.amount + '</td>';
                html += '</tr>';
                index++;
            });
            if (html != null) {
                $('.itemdetails_body').html(html);
                $('#btnconfirm').modal("show");
            }
        }
    });

}




function totalrate() {
    var total = 0; // Initialize the total value
    $(".itemdetails_body TR").each(function () {
        var item = {};
        var row = $(this);

        // Get the values for item_id, rate, quantity, and tax
        var item_id = row.find('td:eq(1)').html();
        if (item_id != '0') {

            var rate = parseFloat(row.find('td:eq(3) input').val()) || 0;
            var quantity = parseFloat(row.find('td:eq(4) input').val()) || 0;
            var courior = parseFloat(row.find('td:eq(5) input').val()) || 0;
            var tax = parseFloat(row.find('td:eq(6) input').val()) || 0;

            // Calculate total for this row: rate * quantity * (1 + tax / 100)
            /*var rowTotal = rate * quantity * (1 + tax / 100);*/
            /* var rowTotal =((rate * quantity) * tax / 100);*/
            /* var rowTotal = ((rate * quantity) * tax / 100 + (rate * quantity));*/
            var rowTotal = (((rate * quantity) + courior) * tax / 100 + ((rate * quantity) + courior));
            total += rowTotal; // Add to overall total

            // Update a cell in the row with the calculated row total (optional)
            row.find('td:eq(7)').html(rowTotal.toFixed(2)); // Assuming column 6 is for the row total


            $("#hidrate").val(rate);
            $("#hidquantity").val(quantity);
            $("#hidcourior").val(courior);
            $("#hidtax").val(tax);
            $("#hidtotal").val(rowTotal);



        }


    });

}

// Set the total in the Total textbox
//$('#totalTextbox').val(total.toFixed(2)); // Replace '#totalTextbox' with the actual ID of your Total textbox




function Saveconfrimorderemail(type) {
    var id = $("#hididd").val();

    
    var dear = $("#dear").html();
    var cust = $("#cust").html();
    var thank = $("#thank").html();
    var order = $("#order").html();
    var orderno = $("#orderno").html();
    var pleasereview = $("#pleasereview").html();
    var total = $("#total").html();
    var payment = $("#payamount").html();
    //var accountdetail = $("#accountdetail").html();
    //var bank = $("#bank").html();
    //var account = $("#account").html();
    //var beneficiary = $("#beneficiary").html();
    //var branch = $("#branch").html();
    //var ifsc = $("#ifsc").html();
    //var paymentterm = $("#paymentterm").html();
    //var advance = $("#advance").html();
    //var kindly = $("#kindly").html();
    //var email = $("#email").html();
    //var once = $("#once").html();
    //var choosing = $("#choosing").html();
    var ifeverything = $("#ifeverything").html();
    var willwe = $("#willwe").html();
    var shopping = $("#shopping").html();
    var best = $("#best").html();
    var team = $("#team").html();

    var sr = $("#sr").html();
    var itemid = $("#itemid").html();
    var itemname = $("#itemname").html();
    var rate = $("#rate").html();
    var quantity = $("#quantity").html();
    var courior = $("#courior").html();
    var tax = $("#tax").html();
    var totalid = $("#totalid").html();


    var th1 = $("#th1").html();
    var th2 = $("#th2").html();
    var th3 = $("#th3").html();
    var th4 = $("#th4").html();
    var th5 = $("#th5").html();
    var th6 = $("#th6").html();
    var th7 = $("#th7").html();



    var ButtonValue = $('#btnSumit').val();
    var data = new Array();



    //$(".itemdetails_body TR").each(function () {
    //    var item = {};
    //    var row = $(this);
    //    var item_id = row.find('td:eq(1)').html();


    //    if (item_id != '0') {
    //        item.item_id = item_id;

    //        item.ItemName = row.find('td:eq(2)').html();
    //        item.rate = row.find('td:eq(3) input').val();
    //        item.quantity = row.find('td:eq(4) input').val();
    //        item.courior = row.find('td:eq(5) input').val();
    //        item.tax = row.find('td:eq(6) input').val();
    //        item.total = row.find('td:eq(7)').html();
    //        data.push(item);
    //    }
    //});

    $.ajax({
        type: "POST",
        url: '/Admin/Admin/Confirm_orderemail',
        data: { id: id, dear: dear, cust: cust, thank: thank, order: order, orderno: orderno, pleasereview: pleasereview, total: total, payment: payment, /*accountdetail: accountdetail, bank: bank, account: account, beneficiary: beneficiary, branch: branch, ifsc: ifsc, paymentterm: paymentterm, advance: advance, kindly: kindly, email: email, once: once, choosing: choosing,*/ ifeverything: ifeverything, willwe: willwe, shopping: shopping, best: best, team: team, sr: sr, itemid: itemid, itemname: itemname, rate: rate, quantity: quantity, courior: courior, tax: tax, totalid: totalid, th1: th1, th2: th2, th3: th3, th4: th4, th5: th5, th6: th6, th7: th7, jsondetailsdata: JSON.stringify(data), type: type },
        dataType: "JSON",
        success: function (result) {
            if (result.message == "temporary converted to confirm order.") {

                $("#orderModal").modal("hide");

                alert("Temporary converted to confirm order.");

            }
            window.location.href = "/Admin/Admin/Orders"
        },
        error: function () {
            alert("Enquiry not converted.");
        }
    });

}




function Saveconfrimorder(type) {

    //  $('#btnAddtoquery').modal("hide");
    var id = $("#hidid").val();
    var useridid = $("#hiduserid").val();
    var full_name = $('#txtFullname').val();
    var mobile = $('#txtMobile').val();
    var email = $('#txtEmail').val();
    var payment_mode = $('#ddlpayment_mode').val();
    var shipping_add = $('#txtaddress').val();
    var ButtonValue = $('#btnSumit').val();
    var data = new Array();

    $(".itemdetails_body TR").each(function () {
        var item = {};
        var row = $(this);
        var item_id = row.find('td:eq(1)').html();
        if (item_id != '0') {
            item.item_id = row.find('td:eq(1)').html();
            item.rate = row.find('td:eq(3) input').val();
            item.quantity = row.find('td:eq(4) input').val();
            item.tax = row.find('td:eq(4) input').val();
            data.push(item);
        }


    });

    $.ajax({
        type: "POST",
        url: '/Admin/Admin/Confirm_order',
        data: { id: id, useridid: useridid, full_name: full_name, mobile: mobile, email: email, payment_mode: payment_mode, shipping_add, jsondetailsdata: JSON.stringify(data), type: type },
        dataType: "JSON",
        success: function (result) {
            if (result.message == "Enquiry converted to confirm order.") {

                $("#btnconfirm").modal("hide");
                // Example data for dynamic fields

                //var orderNumber = "12345";
              
                $('#cust').html($('#txtFullname').val());
                
                //var rate = $("#hidrate").val();
                //var quantity = $("#hidquantity").val();
                //var courior = $("#hidcourior").val();
                //var tax = $("#hidtax").val();
                var total = $("#hidtotal").val();
                $("#payamount").html(total);
                $("#orderno").html(result.orderno);

                //var bankDetails = `
                //Bank Name: HDFC Bank Ltd
                //Account No.: 05512000003074
                //Beneficiary Name: BSD InfoTech
                //Branch: Mayapuri
                //IFSC Code: HDFC0000551`;

                //var orderContent = `Dear Sir,

                //Thank you for placing your order with us. Your Order Number is ${orderNumber}.

                //Order Details:
                //------------------------------------------------------------------------------
                //| S.No | Item    |     Rate   |     Qty    | Courier   | Tax(%) | Amount     |
                //|------|---------|------------|------------|-----------|--------|------------|
                //| 1    | ${rate} | ${rate}    | ${quantity}| ${courior}| ${tax} | ${total}   |
                //------------------------------------------------------------------------------

                //Total Amount Payable: ${total}

                //Bank Account Details:
                //${bankDetails}

                //Payment Terms:
                //100% advance payment required.
                //Kindly share the payment screenshot with us at info@bsdinfotech.com. Once the payment is confirmed, we will provide the dispatch details.

                //Thank you for choosing BSD InfoTech.

                //Best Regards,
                //Team BSD`;

                //// Insert content into the textarea
                //document.getElementById("orderDetailsTextarea").value = orderContent;

                itemeditshow();


                $('#orderModal').modal("show");

            }
        },
        error: function () {
            alert("Enquiry not converted.");
        }
    });

}




function itemeditshow() {

    var data = [];


    $(".itemdetails_body TR").each(function () {
        var item = {};
        var row = $(this);
        var item_id = row.find('td:eq(1)').html();


        if (item_id != '0') {
            item.item_id = item_id;

            item.ItemName = row.find('td:eq(2)').html();
            item.rate = row.find('td:eq(3) input').val();
            item.quantity = row.find('td:eq(4) input').val();
            item.courior = row.find('td:eq(5) input').val();
            item.tax = row.find('td:eq(6) input').val();
            item.total = row.find('td:eq(7)').html();
            data.push(item);
        }
    });



    var html = '';
    var index = 1;
    var i = 1;
    data.forEach(function (item, index) {
        html += '<tr class="hover-primary">';
        html += '<td id="sr"contenteditable="true" >' + i + '</td>';
        html += '<td  id="itemid"style="display:none;">' + item.item_id + '</td>';
        html += '<td id="itemname" contenteditable="true">' + item.ItemName + '</td>';

        html += '<td id="rate" contenteditable="true">' + item.rate + '</td>';
        html += '<td id="quantity" contenteditable="true">' + item.quantity + '</td>';
        html += '<td id="courior" contenteditable="true">' + item.courior + '</td>';
        html += '<td id="tax" contenteditable="true">' + item.tax + '</td>';
        html += '<td id="totalid" contenteditable="true">' + item.total + '</td>';
        html += '</tr>';
        index++;
        i++;


    });

    if (html != null) {
        $('.itemdetailsemail').html(html);
        // $('#btnAddtoquery').modal("show");
    }

}





function Todaydate() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    return today;
}

function SearchOrders() {
    var order_id = $('#order_id').val();
    var user_id = $('#ddlUserNames').find("option:selected").val();
    var from_date = $('#txtfdate').val();
    var to_date = $('#txttdate').val();
    if (from_date <= to_date) {
        $.ajax({
            url: "/Admin/Admin/SearchOrders",
            data: { order_id: order_id, user_id: user_id, from_date: from_date, to_date: to_date },
            dataType: 'JSON',
            type: 'GET',
            // contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var html = '';
                var index = 1;
                $.each(data, function (key, item) {
                    html += '<tr class="hover-primary">';
                    html += '<td>' + index + '</td>';
                    //html += '<td>' + item.order_id + '</td>';
                    html += '<td>' + item.userName + '</td>';
                    html += '<td>' + item.order_no + '</td>';
                    html += '<td>' + item.amount + '</td>';
                    html += '<td>' + item.payment_mode + '</td>';
                    html += '<td><a onclick="return OrdersReport(' + item.order_id + ')"><i class="fa fa-eye"></i></a></td>';
                    //html += '<td><a href="/Admin/Admin/OrdersReport?order_id=' + item.order_id + '"><i class="fa fa-eye"></i></a></td>';
                    //html += '<td><a class="btn btn-sm" href="#" onclick="return EditState(' + item.order_id + ')"><i class="fa fa-eye"></i></a></td>';
                    // html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.order_id + ')"><i class="fa fa-trash-o"></i></a></td>';
                    html += '</tr>';
                    index++;
                });
                if (html != "") {
                    $('.Ordersbody').html(html);
                }
                else {
                    $(".Ordersbody").html("No data found for this Date.");
                }
            }
        });
    }
    else {
        message = "from_date is lesser than to_date.";
    }
}


$("#ddlUserNames").change(function () {
    SearchOrders(); 
});

function OrdersReport(order_id) {
    $.ajax({
        url: "/Admin/Admin/OrdersReport",
        data: { order_id: order_id },
        dataType: 'JSON',
        type: 'GET',
        success: function (data) {
            $('#orderlist').modal('show');
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                //html += '<td>' + item.order_id + '</td>';
                html += '<td>' + item.order_no + '</td>';
                html += '<td>' + item.order_date + '</td>';
                html += '<td>' + item.userName + '</td>';
                html += '<td>' + item.amount + '</td>';
                html += '<td>' + item.subtotal + '</td>';
                html += '<td>' + item.itemName + '</td>';
                html += '<td>' + item.unit_rate + '</td>';
                html += '<td>' + item.quantity + '</td>';
                html += '<td>' + item.payment_mode + '</td>';
                html += '</tr>';
                index++;
            });
            $('.OrdersReportsbody').html(html);

        }
    });
}


//----------------------------Cancelled_Orders--------------------------------------------
function ShowCancelled_Orders() {
    var UserName = $('#ddlUserNames').val();
    var from_date = $('#txtfdate').val();
    var to_date = $('#txttdate').val();
    $.ajax({
        url: "/Admin/Admin/ShowCancelled_Orders",
        dataType: 'JSON',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        data: { from_date: from_date, to_date: to_date, UserName: UserName },
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
                html += '<td><a class="btn btn-sm" href="#" onclick="return ApproveCancelPopPup(' + item.order_id + ',' + item.amount + ')"><i class="fa fa-eye"></i></a></td>';
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
function SearchCancelled_Orders() {
    var user_id = $('#ddlUserNames').find("option:selected").val();
    var from_date = $('#txtfdate').val();
    var to_date = $('#txttdate').val();
    if (from_date <= to_date) {
        $.ajax({
            url: "/Admin/Admin/SearchCancelled_Orders",
            data: { user_id: user_id, from_date: from_date, to_date: to_date },
            dataType: 'JSON',
            type: 'GET',
            success: function (data) {
                var html = '';
                var index = 1;
                $.each(data, function (key, item) {
                    html += '<tr class="hover-primary">';
                    html += '<td>' + index + '</td>';
                    // html += '<td style="display:none">' + item.order_id + '</td>';
                    html += '<td>' + item.order_no + '</td>';
                    html += '<td>' + item.userName + '</td>';
                    html += '<td>' + item.amount + '</td>';
                    html += '<td>' + item.order_date + '</td>';
                    html += '<td>' + item.cal_date + '</td>';
                    /*html += '<td>' + item.cancelled_User + '</td>';*/
                    html += '<td>' + item.cancel_type + '</td>';
                    html += '<td>' + item.payment_mode + '</td>'
                    // html += '<td><a class="btn btn-sm" href="/Admin/Admin/Approve?id=' + item.id + '">View</a></td>';
                    html += '<td><a href="/Admin/Admin/Cancelled_Orders_Report?order_id=' + item.order_id + '"><i class="fa fa-eye"></i></a></td>';
                    html += '<td><a class="btn btn-sm" href="#" onclick="return ApproveCancelPopPup(' + item.order_id + ',' + item.amount + ')"><i class="fa fa-eye"></i></a></td>';
                    // html += '<td><a class="btn btn-sm" href="#" onclick="return EditState(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';
                    // html += '<td><button class="btnView"><a class="btn btn-sm" href="#"><i class="fa fa-view"></i></a></button></td>';
                    // html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                    html += '</tr>';
                    index++;
                });
                if (html != "") {
                    $('.Cancelled_Ordersbody').html(html);
                }
                else {
                    $(".Cancelled_Ordersbody").html("No data found for this Date.");
                }
            }
        });
    }

    else {
        message = "from_date is lesser than to_date.";
    }

}
function cancelPaymentmodechange(paymentmode) {
    if (paymentmode == 1) {
        $("#divTransaction").css("display", "block");
        $("#onlineupdatebtn").css("display", "block");
    }
    else {
        $("#divTransaction").css("display", "none");
        $("#onlineupdatebtn").css("display", "block");
    }
}
function UpdateCancel() {
    var orderid = $("#hiddenorderid").val();
    var amount = $("#txtamount").val();
    var Transactionnumber = $("#txttransactionNo").val();
    var remarks = $("#txtremarks").val();
    var paymentmode = $('#ddlpayemtmode').find("option:selected").val();
    var paymentmodeTest = $('#ddlpayemtmode').find("option:selected").text();
    $.ajax({
        url: "/Admin/Admin/UpdateCancel",
        data: { orderid: orderid, amount: amount, Transactionnumber: Transactionnumber, remarks: remarks, paymentmodeTest: paymentmodeTest },
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            if (result.message == "Cancel approve updated.") {
                $("#hiddenorderid").val("");
                $("#txtamount").val("");
                $("#txttransactionNo").val("");
                $("#txtremarks").val("");
                $('#ddlpayemtmode').val(0);
                $("#btnclose").trigger('click');
                ShowCancelled_Orders();
                alert("Cancel approve updated.");
            } else {
                alert("Cancel approve not updated.");
            }
        }
    });
}

function ApproveCancelPopPup(order_id, amount) {
    $("#txtamount").val(amount);
    $("#hiddenorderid").val(order_id);
    $("#quickViewModal").modal("show");
}
//$("#ddlUserNames").change(function () {
//    SearchCancelled_Orders();
//});

//------------------------------------Returned Orders------------------------------------------------------

function ShowReturned_Orders() {
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    $.ajax({
        url: "/Admin/Admin/ShowReturned_Orders",
        data: { Status: Status },
        dataType: 'JSON',
        type: 'GET',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
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
                //html += '<td><input type="checkbox" id="chkStatus" class="status"></td>';
                //if ((item.chkStatus) == 1) {
                //    html += '<td class="active">Active</td>';
                //} else {
                //    html += '<td class="active">Inactive</td>';
                //}
                html += '<td><a href="/Admin/Admin/Returned_Orders_Report?order_id=' + item.order_id + '"><i class="fa fa-eye"></i></a></td>';
                html += '<td><a onclick="return ReturnPopup(' + item.order_id + ',' + item.amount + ')"><i class="fa fa-eye"></i></a></td>';
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
function SearchReturned_Orders() {
    var user_id = $('#ddlUserNames').val();
    var from_date = $('#txtfdate').val();
    var to_date = $('#txttdate').val();


    $.ajax({
        url: "/Admin/Admin/SearchReturned_Orders",
        data: { user_id: user_id, from_date: from_date, to_date: to_date },
        dataType: 'JSON',
        type: 'GET',
        // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
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
                //html += '<td><input type="checkbox" id="chkStatus" class="status"></td>';
                //if ((item.chkStatus) == 1) {
                //    html += '<td class="active">Active</td>';
                //} else {
                //    html += '<td class="active">Inactive</td>';
                //}
                html += '<td><a href="/Admin/Admin/Returned_Orders_Report?order_id=' + item.order_id + '"><i class="fa fa-eye"></i></a></td>';
                html += '<td><a onclick="return ReturnPopup(' + item.order_id + ',' + item.amount + ')"><i class="fa fa-eye"></i></a></td>';
                // html += '<td><a class="btn btn-sm" href="#" onclick="return EditState(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';
                // html += '<td><button class="btnView"><a class="btn btn-sm" href="#"><i class="fa fa-view"></i></a></button></td>';
                // html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            if (html != "") {
                $('.Returned_Ordersbody').html(html);
            }
            else {
                $(".Returned_Ordersbody").html("No data found for this Date.");
            }
        }
    });
}

//$("#ddlUserNames").change(function () {
//    SearchReturned_Orders();
//});
function ApproveReturn() {
    var returnorders = new Array();
    $("#data-table tbody tr").each(function () {
        var row = $(this);
        var returnorder = {};
        returnorder.User_id = row.find("span.user_id").text();
        returnorder.amount = row.find("span.amount").text();
        returnorder.EntryDate = row.find("span.EntryDate").text();
        returnorder.order_id = row.find("span.order_id").text();
        returnorder.chkStatus = row.find('input.status').is(':checked') ? 1 : 0
        // $('#chkStatus').is(':checked') ? 1 : 0;
        returnorders.push(returnorder);
    });
    var returnorderjson = JSON.stringify(returnorders);
    $.ajax({
        url: "/Admin/Admin/UpdateReturned_Orders",
        data: { returnorderjson: returnorderjson },
        dataType: "JSON",
        type: "POST",
        success: function (result) {
            if (result == "Return approve.") {
                alert("Successfully.");
            }
            else {
                alert("Not Successfully.");
            }
            ShowReturned_Orders();
        }
    })
}
function ReturnPopup(order_id, amount) {
    $("#txtamount").val(amount);
    $("#hiddenorderid").val(order_id);
    $('#returnPopup').modal('show');
}
function returnPaymentmodechange(paymentmode) {
    if (paymentmode == 1) {
        $("#divTransactionNumber").css("display", "block");
        $("#onlineupdatebtn").css("display", "block");
    }
    else {
        $("#divTransactionNumber").css("display", "none");
        $("#onlineupdatebtn").css("display", "block");
    }
}
function UpdateReturn() {
    var orderid = $("#hiddenorderid").val();
    var amount = $("#txtamount").val();
    var Transactionnumber = $("#txttransactionNo").val();
    var remarks = $("#txtremarks").val();
    var paymentmode = $('#ddlpayemtmode').find("option:selected").val();
    var paymentmodeTest = $('#ddlpayemtmode').find("option:selected").text();
    $.ajax({
        url: "/Admin/Admin/UpdateReturn",
        data: { orderid: orderid, amount: amount, Transactionnumber: Transactionnumber, remarks: remarks, paymentmodeTest: paymentmodeTest },
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            if (result.message == "Return approve updated") {
                $("#hiddenorderid").val("");
                $("#txtamount").val("");
                $("#txttransactionNo").val("");
                $("#txtremarks").val("");
                $('#ddlpayemtmode').val(0);
                $("#close").trigger("click");
                ShowReturned_Orders();
                alert("Return approve updated.");
            } else {
                alert("Return approve not updated.");
            }
        }
    });
}
//----------------------------Dispatch_Orders------------------------------------------------------------

function ShowORT_Dispatch() {

    var Type = $("#hidType").val();

    $.ajax({
        url: "/Admin/Admin/ShowORT_Dispatch",
        data: {
            Type: Type
        },
        dataType: 'JSON',
        type: 'GET',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.order_id + '</td>';
                html += '<td>' + item.order_no + '</td>';
                html += '<td>' + item.userName + '</td>';
                // html += '<td>' + item.amount + '</td>';
                html += '<td>' + item.address + '</td>';
                html += '<td>' + item.mobileNo + '</td>';
                html += '<td>' + item.order_date + '</td>';
                html += '<td style="display:none">' + item.dispatch_date + '</td>';
                html += '<td>' + item.payment_mode + '</td>';
                html += '<td>' + item.dispatch_flg + '</td>';
                html += '<td><a onclick="return DispatchView(' + item.order_id + ')"><i class="fa fa-eye"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return DispatchOrderModel(' + item.order_id + ')"><i class="fa fa-eye"></i></a></td>';
                html += '</tr>';
                index++;
            });
            if (html != null) {
                $('.ORT_Dispatchbody').html(html);
            }
            else {
                $('.ORT_Dispatchbody').html("Data not available.");
            }
        }
    });
}


function DispatchView(order_id) {
    $.ajax({
        url: "/Admin/Admin/Dispatchview",
        data: { order_id: order_id },
        dataType: 'JSON',
        type: 'GET',
        success: function (data) {
            $('#orderlist').modal('show');
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                 html += '<td>' + index + '</td>';
                //html += '<td>' + item.order_id + '</td>';
                html += '<td>' + item.order_no + '</td>';
                //  html += '<td>' + item.order_date + '</td>';
                //  html += '<td>' + item.userName + '</td>';

                //  html += '<td>' + item.subtotal + '</td>';
                html += '<td>' + item.itemName + '</td>';
                html += '<td>' + item.unit_rate + '</td>';
                html += '<td>' + item.quantity + '</td>';
                html += '<td>' + item.courior + '</td>';

                html += '<td>' + item.amount + '</td>';
                // html += '<td>' + item.payment_mode + '</td>';
                html += '</tr>';
                index++;
            });
            $('.OrdersReportsbody').html(html);

        }
    });
}

function SearchORT_Dispatch() {
    var from_date = $('#txtfdate').val();
    var to_date = $('#txttdate').val();
    if (from_date == "" && to_date == "") {
        ShowORT_Dispatch();
    }
    else {
        if (from_date <= to_date) {
            $.ajax({
                url: "/Admin/Admin/SearchORT_Dispatch",
                data: { from_date: from_date, to_date: to_date },
                dataType: 'JSON',
                type: 'GET',
                //contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    var html = '';
                    var index = 1;
                    $.each(data, function (key, item) {
                        html += '<tr class="hover-primary">';
                        html += '<td>' + index + '</td>';
                        html += '<td style="display:none">' + item.order_id + '</td>';
                        html += '<td>' + item.order_no + '</td>';
                        html += '<td>' + item.userName + '</td>';
                        html += '<td>' + item.amount + '</td>';
                        html += '<td>' + item.address + '</td>';
                        html += '<td>' + item.mobileNo + '</td>';
                        html += '<td>' + item.order_date + '</td>';
                        html += '<td style="display:none">' + item.dispatch_date + '</td>';
                        html += '<td>' + item.payment_mode + '</td>';
                        html += '<td>' + item.dispatch_flg + '</td>';
                        html += '<td style="display:none"><a href="/Admin/Admin/ORT_DispatchReport?order_id=' + item.order_id + '"><i class="fa fa-eye"></i></a></td>';
                        html += '<td><a class="btn btn-sm" href="#" onclick="return DispatchOrderModel(' + item.order_id + ')"><i class="fa fa-eye"></i></a></td>';
                        html += '</tr>';
                        index++;
                    });
                    if (html != "") {
                        $('.ORT_Dispatchbody').html(html);
                    }
                    else {
                        $(".ORT_Dispatchbody").html("Data not available for this date.");
                    }
                }
            });
        }
        else {
            message = "Form date will be always larger from Todate. ";
        }
    }
}
function DispatchOrderModel(order_id) {
    $('#orderDispatchlist').modal('show');
    $('#orderid').val(order_id);
}
function UpdateDispatchOrder() {
    var order_id = $('#orderid').val();
    var cour_id = $('#ddlcourier').find("option:selected").val();
    var cour_no = $('#txttrackno').val();
    var cour_remarks = $('#txtcr').val();
    $.ajax({
        url: "/Admin/Admin/UpdateDispatchOrder",
        data: { order_id: order_id, cour_id: cour_id, cour_no: cour_no, cour_remarks: cour_remarks },
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            if (result.message == "Table modify successfully.") {
                $('#orderid').val("");
                $('#ddlcourier').val(0);
                $('#txttrackno').val("");
                $('#txtcr').val("");
                $('#btnclose').trigger("click");
                ShowORT_Dispatch();
                alert("Dispatch successfully.");
            }
            else if (result.message == "Your Order Dispatch Successfully.") {
                $('#orderid').val("");
                $('#ddlcourier').val(0);
                $('#txttrackno').val("");
                $('#txtcr').val("");
                $('#btnclose').trigger("click");
                ShowORT_Dispatch();
                alert("Dispatch successfully.");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Cleartext() {
    $("#hiddenorderid").val("0");
    $('#orderid').val("0");
    $('#ddlcourier').val("0");
    $('#txttrackno').val("");
    $('#txtcr').val("");
    $('#btnclose').trigger("click");
}

function validatestate() {
    var msg = "";
    if ($('#ddlcourier').val() == 0) { msg += "courier  can not left Blank !! \n"; }
    if ($('#txttrackno').val() == "") { msg += "Tracker No can not left Blank !! \n"; }
    //if ($('#txtcr').val() == "") { msg += " can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;
}

//-------------------------------------------Diliver Order--------------------------------------------------

function ShowOR_to_Deliver() {
    $.ajax({
        url: "/Admin/Admin/ShowOR_to_Deliver",
        dataType: 'JSON',
        type: 'GET',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none"><span class="order_id">' + item.order_id + '</span></td>';
                html += '<td>' + item.order_no + '</td>';
                html += '<td>' + item.userName + '</td>';
                html += '<td>' + item.amount + '</td>';
                html += '<td>' + item.mobileNo + '</td>';
                html += '<td>' + item.order_date + '</td>';
                html += '<td>' + item.dispatch_date + '</td>';
                html += '<td>' + item.payment_mode + '</td>';
                html += '<td><a href="/Admin/Admin/OR_to_DeliverReport?order_id=' + item.order_id + '"><i class="fa fa-eye"></i></a></td>';
                if (item.chkStatus == "1") {
                    html += '<td><input type="checkbox" name="status_flg" id="chkStatus" checked  class="status"/></td>';
                }
                else {
                    html += '<td><input type="checkbox" name="status_flg" id="chkStatus"  class="status"/></td>';
                }
                //html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-eye"></i></a></button></td>';
                html += '</tr>';
                index++;
            });
            $('.Orderdiliverbody').html(html);
        }
    });
}

function SearchOR_to_Deliver() {
    var order_id = $('#order_id').val();
    var from_date = $('#txtfdate').val();
    var to_date = $('#txttdate').val();
    if (from_date <= to_date) {
        $.ajax({

            url: "/Admin/Admin/SearchOR_to_Deliver",
            data: { order_id: order_id, from_date: from_date, to_date: to_date },
            dataType: 'JSON',
            type: 'GET',
            //contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var html = '';
                var index = 1;
                $.each(data, function (key, item) {
                    html += '<tr class="hover-primary">';
                    // html += '<td>' + index + '</td>';
                    html += '<td style="display:none"><span class="order_id">' + item.order_id + '</span></td>';
                    html += '<td>' + item.order_no + '</td>';
                    html += '<td>' + item.userName + '</td>';
                    html += '<td>' + item.amount + '</td>';
                    html += '<td>' + item.mobileNo + '</td>';
                    html += '<td>' + item.order_date + '</td>';
                    html += '<td>' + item.dispatch_date + '</td>';
                    html += '<td>' + item.payment_mode + '</td>';
                    html += '<td><a href="/Admin/Admin/OR_to_DeliverReport?order_id=' + item.order_id + '"><i class="fa fa-eye"></i></a></td>';
                    html += '<td><input type="checkbox" name="status_flg" id="chkStatus"  class="status"/></td>';
                    html += '</tr>';
                    index++;
                });
                if (html != "") {
                    $('.Orderdiliverbody').html(html);
                }
                else {
                    $('.Orderdiliverbody').html("Data not found.");
                }
            }
        });
    }
    else {

        message = "from_date is lesser than to_date.";
    }
}

function ApproveOR_to_Deliver() {
    var Deliverdisplays = new Array();
    $("#data-table tbody tr").each(function () {
        var row = $(this);
        var Deliverdisplay = {};
        Deliverdisplay.order_id = row.find("span.order_id").text();
        Deliverdisplay.chkStatus = row.find('input.status').is(':checked') ? 1 : 0

        // $('#chkStatus').is(':checked') ? 1 : 0;
        Deliverdisplays.push(Deliverdisplay);
    });
    var Deliverdisplayjson = JSON.stringify(Deliverdisplays);
    $.ajax({
        url: "/Admin/Admin/UpdateOR_to_Deliver",
        data: { Deliverdisplayjson: Deliverdisplayjson },
        dataType: "JSON",
        type: "POST",
        success: function (result) {
            if (result == "Your Order Delivered Successfully.") {

                alert("Deliver Successfully.");
            }
            else {
                alert("Deliver not  Successfully.");
            }
            /* ShowDataInTable();*/
        }
    })
}



function UserNameBind() {
    $("#ddlUserName").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: "/Admin/Admin/OrdersBind", //url to be called
        success: function (json, result) {
            $("#ddlUserName").empty();
            json = json || {};
            $("#ddlUserName").append('<option value="0">Select UserName</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlUserName").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            // GetPatientData();
            $("#ddlUserName").prop("disabled", false);
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}