$(document).ready(function () {
    ShowDataInTable();
});


function Savereplyemail(type) {

    var id = $("#hiduser").val();
    var dear = $("#dearuser").html();
    var cust = $("#custuser").html();
    var thank = $("#thankuser").html();
    var best = $("#bestuser").html();
    var team = $("#teamuser").html();
    

    $.ajax({
        type: "POST",
        url: '/Admin/Admin/Savereplyemail',
        data: {id:id, dear: dear, thank: thank,best: best, team: team,cust:cust},
        dataType: "JSON",
        success: function (result) {
            if (result.message == "Reply sent successfully.") {

                $("#orderModal2").modal("hide");

                alert("Reply sent successfully.");

            }
            else if (result.message == "Email sending failed.") {

                $("#orderModal2").modal("hide");

                alert("Reply sending failed.");
            }
            else {
                alert("No record found for the provided ID.");

            }
            window.location.href = "/Admin/Admin/Inquiry_Page"
        },
        error: function () {
            alert("Reply sending failed.");
        }
    });

}

function Saveconfrimorderemail(type) {
    var idd = $("#hidid").val();
    var id = $("#hidAdd").val();
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
            if (result.message == "Enquiry converted to confirm order.") {

                $("#orderModal").modal("hide");
              
                alert("Enquiry converted to confirm order.");
                
            }
            window.location.href = "/Admin/Admin/Inquiry_Page"
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
    var EnquiryNo = $('#hidEnquiryNo').val();
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
            data: { id: id, useridid: useridid, EnquiryNo: EnquiryNo, full_name: full_name, mobile: mobile, email: email, payment_mode: payment_mode, shipping_add, jsondetailsdata: JSON.stringify(data), type:type },
            dataType: "JSON",
            success: function (result) {
                if (result.message == "Enquiry converted to confirm order.") {
                    
                    $("#btnAddtoquery").modal("hide");
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

                    $("#hidid").val(result.id);
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


function confrimordercontct(type) {

    //  $('#btnAddtoquery').modal("hide");
    var id = $("#hidAdd").val();
    var useridid = $("#hiduseridAdd").val();
    var EnquiryNo = $('#hidEnquiryNoAdd').val();
    var full_name = $('#txtFullnameAdd').val();
    var mobile = $('#txtMobileAdd').val();
    var email = $('#txtEmailAdd').val();
    var payment_mode = $('#ddlpayment_modeAdd').val();
    var shipping_add = $('#txtaddressAdd').val();
    var ButtonValue = $('#btnSumitAdd').val();
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
        data: { id: id, useridid: useridid, EnquiryNo: EnquiryNo, full_name: full_name, mobile: mobile, email: email, payment_mode: payment_mode, shipping_add, jsondetailsdata: JSON.stringify(data), type: type },
        dataType: "JSON",
        success: function (result) {
            if (result.message == "Enquiry converted to confirm order.") {

                $("#btnAddtoquery2").modal("hide");
                // Example data for dynamic fields

                //var orderNumber = "12345";
                $('#cust').html($('#txtFullnameAdd').val());
                //var rate = $("#hidrate").val();
                //var quantity = $("#hidquantity").val();
                //var courior = $("#hidcourior").val();
                //var tax = $("#hidtax").val();
                var total = $("#hidtotal").val();
                $("#payamount").html(total);
                $("#orderno").html(result.orderno);

                $("#hidAdd").val(result.id);
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

                itemeditshowAdd();


                $('#orderModal').modal("show");

            }
        },
        error: function () {
            alert("Enquiry not converted.");
        }
    });

}


function ClearAllField() {
    $("#btnSumit").val("confirm order");
    $('#id').val("0");
    $('#txtFname').val("");
    $('#txtMobile').val("");
    $('#txtEmail').val("");
    $('#ddlpayment_mode').val("0");
    $('#txtaddress').val("");
}



function ShowDataInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowConfirmorder",
        type: 'POST',
        data: {},
        dataType: 'JSON',
        // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.order_no +  '</td>';
                html += '<td>' + item.full_name + '</td>';
                html += '<td>' + item.mobile + '</td>';
                html += '<td>' + item.item_name + '</td>';
                //html += '<td>' + item.email + '</td>';
                //html += '<td>' + item.payment_mode + '</td>';
                //html += '<td>' + item.shipping_add + '</td>';

                //html += '<td>' + item.rate + '</td>';
                //html += '<td>' + item.quantity + '</td>';
                //html += '<td>' + item.rate * item.quantity + '</td>';
              /*  html += '<td>';*/
                //if (item.status_flag != 5) {
                //    html += '<input type = "button" class = "btn btn-primary" onclick="FinalOrder(' + item.id + ')" value = "CONVERT INQUIRY INTO ORDER"></td>';
                //}

                if (item.paymentflagid == 1)  {

                    html += `<td> </td>`;

                   
                } else {
                    html += `
                   <td>
              <a onclick="return FinalOrder(${item.id})" style="text-decoration: none; cursor: pointer; display: flex; flex-direction: column; align-items: center; color: black;"
               onmouseover="this.style.color='#007BFF';" 
               onmouseout="this.style.color='black';">
                <span style="font-size: 14px;">Received</span>
              <i class="fa fa-eye" style="font-size: 24px; margin-top: 5px;"></i>
            </a>
             </td>`;
                }
               
                html += '<td>' + item.paymentflag + '</td>';
                

                html += `
                   <td>
              <a onclick="return fillInquiryDetailsf(${item.id})" style="text-decoration: none; cursor: pointer; display: flex; flex-direction: column; align-items: center; color: black;"
               onmouseover="this.style.color='#007BFF';" 
               onmouseout="this.style.color='black';">
                <span style="font-size: 14px;">Final</span>
              <i class="fa fa-eye" style="font-size: 24px; margin-top: 5px;"></i>
            </a>
             </td>`;
                html += '</td>';
                html += '</tr>';
                index++;
            });
            $('.Confirm_order_body').html(html);
            
        }
    });
}






function fillInquiryDetailsf(id) {
    $("#hididf").val(id);
    BindPopup1(id);
    //window.location.href = "/Admin/Admin/Inquiry_Page?id=" + id + "";
}

function BindPopup1(id) {
    $.ajax({
        url: "/Admin/Admin/BindPopup11",
        dataType: 'JSON',
        type: 'GET',
        data: { id: id },
        success: function (data) {
            //$('#orderlist').modal('show');
            data = JSON.parse(data);
            $("#hididdf").val(data[0].order_id);
            $("#hididdforder").val(data[0].order_no);
            $("#hididdpayamountf").val(data[0].amount);
            $("#txtFullnamef").val(data[0].UserName);
            $("#txtEmailf").val(data[0].email);
            $("#txtMobilef").val(data[0].mobileno);


            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                html += '<td>' + index + '</td>';
                html += '<td style="display:none;">' + item.ID + '</td>';

                html += '<td>' + item.ItemName + '</td>';
                html += '<td>' + item.rate + '</td>';
                html += '<td>' + item.UnitQty + '</td>';
                html += '<td>' + item.Courior + '</td>';
                html += '<td>' + item.Tax + '</td>';
                html += '<td>' + item.amount + '</td>';



                //html += '<td><input type="text" id="txtamount" value="' + item.rate + '" /></td>';
                //html += '<td><input type="text" id="txtquantity" value="' + item.UnitQty + '" /></td>';
                //html += '<td><input type="text" id="txtcourior" value="' + item.Courior + '" /></td>';
                //html += '<td><input type="text" oninput="totalrate()" id="txttax" value="' + item.Tax + '" /></td>';
                //html += '<td><input type="text" id="txtTotal" value="' +  item.amount + '" /></td>';

                /*html += '<td>' + 0 + '</td>';*/
                html += '</tr>';
                index++;
            });
            if (html != null) {
                $('.itemdetails_bodyf').html(html);
                $('#btnconfirmfinal').modal("show");
            }
        }
    });

}
$('#orderModal').modal("show");



function Replyconfrimuser(id) {

    $.ajax({
        type: "POST",
        url: '/Home/Replyconfrimuser',
        data: {Userid: id, },
        dataType: "JSON",
        success: function (result) {

            var data = JSON.parse(result);
            $("#custuser").html(data[0].Fullname);
            $("#hiduser").val(data[0].id)
         
             $('#orderModal2').modal("show");
       
           

        },
        error: function () {
            swal({
                title: "Please try again",
                text: "",
                icon: "danger",
                timer: 10000,
            });
        }
    });




}

function Saveconfrimorderfinal(type) {

    $("#btnconfirmfinal").modal("hide");
    
   
    $("#custemail").val($("#txtEmailf").val());
    $("#cust").html($("#txtFullnamef").val());
    $("#payamountf").html($("#hididdpayamountf").val());
    $("#ordernof").html($("#hididdforder").val());

    itemeditshowfinal();


    $('#orderModalFinal').modal("show");

}


function Saveconfrimorderemailfinal(type) {
    var id = $("#hididdf").val();
    var custemail = $("#custemail").val();
    var dear = $("#dearf").html();
    var cust = $("#custf").html();
    var thank = $("#thankf").html();
    var order = $("#orderfinal").html();
    var orderno = $("#ordernof").html();
    var pleasereview = $("#pleasereview").html();
    var total = $("#totalf").html();
    var payment = $("#payamountf").html();
    var accountdetail = $("#accountdetailf").html();
    var bank = $("#bankf").html();
    var account = $("#accountf").html();
    var beneficiary = $("#beneficiaryf").html();
    var branch = $("#branchf").html();
    var ifsc = $("#ifscf").html();
    var paymentterm = $("#paymenttermf").html();
    var advance = $("#advancef").html();
    var kindly = $("#kindlyf").html();
    var email = $("#emailf").html();
    var once = $("#oncef").html();
    var choosing = $("#choosingf").html();
    var ifeverything = $("#ifeverything").html();
    var willwe = $("#willwe").html();
    var shopping = $("#shopping").html();
    var best = $("#bestf").html();
    var team = $("#teamf").html();

    var sr = $("#srf").html();
    var itemid = $("#itemidf").html();
    var itemname = $("#itemnamef").html();
    var rate = $("#ratef").html();
    var quantity = $("#quantityf").html();
    var courior = $("#couriorf").html();
    var tax = $("#taxf").html();
    var totalid = $("#totalidf").html();


    var th1 = $("#th1f").html();
    var th2 = $("#th2f").html();
    var th3 = $("#th3f").html();
    var th4 = $("#th4f").html();
    var th5 = $("#th5f").html();
    var th6 = $("#th6f").html();
    var th7 = $("#th7f").html();



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
        url: '/Admin/Admin/Confirm_orderemailfinal',
        data: { id: id, custemail: custemail, dear: dear, cust: cust, thank: thank, order: order, orderno: orderno, pleasereview: pleasereview, total: total, payment: payment, accountdetail: accountdetail, bank: bank, account: account, beneficiary: beneficiary, branch: branch, ifsc: ifsc, paymentterm: paymentterm, advance: advance, kindly: kindly, email: email, once: once, choosing: choosing, ifeverything: ifeverything, willwe: willwe, shopping: shopping, best: best, team: team, sr: sr, itemid: itemid, itemname: itemname, rate: rate, quantity: quantity, courior: courior, tax: tax, totalid: totalid, th1: th1, th2: th2, th3: th3, th4: th4, th5: th5, th6: th6, th7: th7, jsondetailsdata: JSON.stringify(data), type: type },
        dataType: "JSON",
        success: function (result) {
            if (result.message == "Confirm Order converted to final order.") {

                $("#orderModalFinal").modal("hide");

                alert("Confirm Order converted to final order.");

            }
            window.location.href = "/Admin/Admin/Confirm_order_page"
        },
        error: function () {
            alert("Enquiry not converted.");
        }
    });

}


//function Saveconfrimorderfinal(type) {

//    //  $('#btnAddtoquery').modal("hide");
//    var id = $("#hididf").val();
//    var full_name = $('#txtFullnamef').val();
//    var mobile = $('#txtMobilef').val();
//    var email = $('#txtEmailf').val();
//    var payment_mode = $('#ddlpayment_modef').val();
//    var shipping_add = $('#txtaddressf').val();
//    var ButtonValue = $('#btnSumitf').val();
//    var data = new Array();

//    $(".itemdetails_bodyf TR").each(function () {
//        var item = {};
//        var row = $(this);
//        var item_id = row.find('td:eq(1)').html();
//        if (item_id != '0') {
//            item.item_id = row.find('td:eq(1)').html();
//            item.rate = row.find('td:eq(3) input').val();
//            item.quantity = row.find('td:eq(4) input').val();
//            item.tax = row.find('td:eq(4) input').val();
//            data.push(item);
//        }


//    });

//    $.ajax({
//        type: "POST",
//        url: '/Admin/Admin/Confirm_orderfinal',
//        data: { id: id, full_name: full_name, mobile: mobile, email: email, payment_mode: payment_mode, shipping_add, jsondetailsdata: JSON.stringify(data), type: type },
//        dataType: "JSON",
//        success: function (result) {
//            if (result.message == "Enquiry converted to confirm order.") {

//                $("#btnconfirmfinal").modal("hide");
//                // Example data for dynamic fields

//                //var orderNumber = "12345";

//                //var rate = $("#hidrate").val();
//                //var quantity = $("#hidquantity").val();
//                //var courior = $("#hidcourior").val();
//                //var tax = $("#hidtax").val();
//                var total = $("#hidtotalf").val();
//                $("#payamountf").html(total);
//                $("#ordernof").html(result.orderno);

//                //var bankDetails = `
//                //Bank Name: HDFC Bank Ltd
//                //Account No.: 05512000003074
//                //Beneficiary Name: BSD InfoTech
//                //Branch: Mayapuri
//                //IFSC Code: HDFC0000551`;

//                //var orderContent = `Dear Sir,

//                //Thank you for placing your order with us. Your Order Number is ${orderNumber}.

//                //Order Details:
//                //------------------------------------------------------------------------------
//                //| S.No | Item    |     Rate   |     Qty    | Courier   | Tax(%) | Amount     |
//                //|------|---------|------------|------------|-----------|--------|------------|
//                //| 1    | ${rate} | ${rate}    | ${quantity}| ${courior}| ${tax} | ${total}   |
//                //------------------------------------------------------------------------------

//                //Total Amount Payable: ${total}

//                //Bank Account Details:
//                //${bankDetails}

//                //Payment Terms:
//                //100% advance payment required.
//                //Kindly share the payment screenshot with us at info@bsdinfotech.com. Once the payment is confirmed, we will provide the dispatch details.

//                //Thank you for choosing BSD InfoTech.

//                //Best Regards,
//                //Team BSD`;

//                //// Insert content into the textarea
//                //document.getElementById("orderDetailsTextarea").value = orderContent;

//                itemeditshowfinal();


//                $('#orderModalFinal').modal("show");

//            }
//        },
//        error: function () {
//            alert("Enquiry not converted.");
//        }
//    });

//}

function itemeditshowfinal() {

    var data = [];


    $(".itemdetails_bodyf TR").each(function () {
        var item = {};
        var row = $(this);
        var item_id = row.find('td:eq(1)').html();


        if (item_id != '0') {
            item.item_id = item_id;

            item.ItemName = row.find('td:eq(2)').html();
            item.rate = row.find('td:eq(3)').html();
            item.quantity = row.find('td:eq(4)').html();
            item.courior = row.find('td:eq(5)').html();
            item.tax = row.find('td:eq(6)').html();
            item.total = row.find('td:eq(7)').html();
            data.push(item);
        }
    });

   


    var html = '';
    var index = 1;
    var i = 1;
    data.forEach(function (item, index) {
        html += '<tr class="hover-primary">';
        html += '<td id="srf"contenteditable="true" >' + i + '</td>';
        html += '<td  id="itemidf"style="display:none;">' + item.item_id + '</td>';
        html += '<td id="itemnamef" contenteditable="true">' + item.ItemName + '</td>';

        html += '<td id="ratef" contenteditable="true">' + item.rate + '</td>';
        html += '<td id="quantityf" contenteditable="true">' + item.quantity + '</td>';
        html += '<td id="couriorf" contenteditable="true">' + item.courior + '</td>';
        html += '<td id="taxf" contenteditable="true">' + item.tax + '</td>';
        html += '<td id="totalidf" contenteditable="true">' + item.total + '</td>';
        html += '</tr>';
        index++;
        i++;


    });

    if (html != null) {
        $('.itemdetailsemailfinal').html(html);
        // $('#btnAddtoquery').modal("show");
    }

}

function ShowDatadash() {
    var orderno = $('#txtsearch').val();
    $.ajax({
        url: "/Admin/Admin/ShowDatadash",
        type: 'POST',
        data: { orderno: orderno },
        dataType: 'JSON',
        // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {

                /*$('#txtsearch').val('');*/
                if (item.status == 1) {
                    window.location.replace("/Admin/Admin/Confirm_order_page");

                }
                else {

                    //window.location.replace("/CustomerAccount");
                    window.location.replace("/Admin/Admin/Orders");

                }

                //html += '<tr class="hover-primary">';
                //// html += '<td>' + index + '</td>';
                //html += '<td style="display:none">' + item.id + '</td>';
                //html += '<td>' + item.order_no + '</td>';
                //html += '<td>' + item.full_name + '</td>';
                //html += '<td>' + item.mobile + '</td>';
                //html += '<td>' + item.email + '</td>';
                //html += '<td>' + item.payment_mode + '</td>';
                //html += '<td>' + item.shipping_add + '</td>';
                //html += '<td>' + item.item_name + '</td>';
                //html += '<td>' + item.rate + '</td>';
                //html += '<td>' + item.quantity + '</td>';
                //html += '<td>' + item.rate * item.quantity + '</td>';
                //html += '<td>';
                //if (item.status_flag != 5) {
                //    html += '<input type = "button" class = "btn btn-primary" onclick="FinalOrder(' + item.id + ')" value = "CONVERT INQUIRY INTO ORDER"></td>';
                //}
                //html += '</td>';
                //html += '</tr>';
                //index++;
            });
           // $('.Confirm_order_body').html(html);

        }
    });
}

function FinalOrder(order_id) {
    $.ajax({
        type: "POST",
        url: '/Admin/Admin/finalorder',
        data: { order_id: order_id },
        dataType: "JSON",
        success: function (result) {
            if (result.message == "Payment Recieved") {

                alert(result.message);
                ShowDataInTable();

            }
        },
        error: function () {
            alert("Payment not Recieved");
        }
    });

}
