$(document).ready(function () {
    showInquiry();

});



function showInquiry() {
    $.ajax({
        url: "/Admin/Admin/showCustomerInquiry",
        dataType: 'JSON',
        type: 'GET',
        data: {},
        success: function (data) {
            //$('#orderlist').modal('show');
            data = JSON.parse(data);
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                html += '<td style="display:none;">' + item.id + '</td>';
               // html += '<td>' + item.EnquiryNo + '</td>';
                html += '<td style="display:none;">' + (item.EnquiryNo ? item.EnquiryNo : '') + '</td>';

                html += '<td>' + item.Fullname + '</td>';
              
                html += '<td>' + item.Email + '</td>';
                html += '<td>' + item.Mobile + '</td>';
                html += '<td>' + item.Createdate + '</td>';
                html += '<td>' + item.Comments + '</td>';

                if (item.Itemid != null) {



                    html += `
                   <td>
              <a onclick="return fillInquiryDetails(${item.id})" style="text-decoration: none; cursor: pointer; display: flex; flex-direction: column; align-items: center; color: black;"
               onmouseover="this.style.color='#007BFF';"
               onmouseout="this.style.color='black';">
                <span style="font-size: 14px;">Reconfirm</span>
              <i class="fa fa-eye" style="font-size: 24px; margin-top: 5px;"></i>
            </a>
                     </td>`;





                } else {
                    
//                    html += `
//<td>
//  <a onclick="return AddInquiryProduct(${item.id})"
//     style="text-decoration: none; cursor: pointer; display: flex; flex-direction: column; align-items: center; color: black;"
//     onmouseover="this.style.color='#007BFF';"
//     onmouseout="this.style.color='black';">
//    <span style="font-size: 14px;">Add Product</span>
//    <i class="fa fa-eye" style="font-size: 24px; margin-top: 5px;"></i>
//  </a>
                    //</td>`;



                    html += `
<td>
  <a onclick="return Replyconfrimuser(${item.id})"
     style="text-decoration: none; cursor: pointer; display: flex; flex-direction: column; align-items: center; color: black;"
     onmouseover="this.style.color='#007BFF';" 
     onmouseout="this.style.color='black';">
    <span style="font-size: 14px;">Reply</span>
    <i class="fa fa-eye" style="font-size: 24px; margin-top: 5px;"></i>
  </a>
</td>`;

                }

               
                /*html += '<td><input type = "button" data-toggle="modal" data-target="#btnAddtoquery" onclick="fillInquiryDetails('+item.id+')" class = "btn btn-primary"  value = "CONVERT ENQUIRY INTO CONFIRM ORDER"></td>';*/
                html += '</tr>';
                index++;
            });
            if (html != null) {
                $('.Inquiry_detalisbody').html(html);
            }

        }
    });
  
}





function totalrateAdd() {
    var total = 0; // Initialize the total value
    $(".itemdetails_bodyAdd TR").each(function () {
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

    $('.itemdetailsemail').empty();
  
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

function itemeditshowAdd() {

    var data = [];


    $(".itemdetails_bodyAdd TR").each(function () {
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

    $('.itemdetailsemail').empty();

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





function openpopupedit() {

    var dear = $("#dear").html();
    var thank = $("#thank").html();
    var order = $("#order").html();
    var total = $("#total").html();
    var accountdetail = $("#accountdetail").html();
    var bank = $("#bank").html();
    var account = $("#account").html();
    var beneficiary = $("#beneficiary").html();
    var branch = $("#branch").html();
    var ifsc = $("#ifsc").html();
    var paymentterm = $("#paymentterm").html();
    var advance = $("#advance").html();
    var kindly = $("#kindly").html();
    var email = $("#email").html();
    var once = $("#once").html();
    var choosing = $("#choosing").html();
    var best = $("#best").html();
    var team = $("#team").html();


    $("#dearedit").val(dear);
    $("#thankedit").val(thank);
    $("#orderedit").val(order);
    $("#totaledit").val(total);
    $("#accountdetailedit").val(accountdetail);
    $("#bankedit").val(bank);
    $("#accountedit").val(account);
    $("#beneficiaryedit").val(beneficiary);
    $("#branchedit").val(branch);
    $("#ifscedit").val(ifsc);
    $("#paymenttermedit").val(paymentterm);
    $("#advanceedit").val(advance);
    $("#kindlyedit").val(kindly);
    $("#emailedit").val(email);
    $("#onceedit").val(once);
    $("#choosingedit").val(choosing);
    $("#bestedit").val(best);
    $("#teamedit").val(team);
 
    $('#orderModal').modal("hide");
    $('#orderModaledit').modal("show");

}



function Updateemail() {

    var dear = $("#dearedit").val();
    var thank = $("#thankedit").val();
    var order = $("#orderedit").val();
    var total = $("#totaledit").val();
    var accountdetail = $("#accountdetailedit").val();
    var bank = $("#bankedit").val();
    var account = $("#accountedit").val();
    var dear = $("#dearedit").val();
    var dear = $("#dearedit").val();
    var beneficiary = $("#beneficiaryedit").val();
    var branch = $("#branchedit").val();
    var ifsc = $("#ifscedit").val();
    var paymentterm = $("#paymenttermedit").val();
    var advance  = $("#advanceedit").val();
    var kindly  = $("#kindlyedit").val();
    var email = $("#emailedit").val();
    var once = $("#onceedit").val();
    var choosing  = $("#choosingedit").val();
    var best = $("#bestedit").val();
    var team = $("#teamedit").val();



     $("#dear").html(dear);
     $("#thank").html(thank);
     $("#order").html(order);
     $("#total").html(total);
     $("#accountdetail").html(accountdetail);
     $("#bank").html(bank);
     $("#account").html(account);
     $("#beneficiary").html(beneficiary);
     $("#branch").html(branch);
     $("#ifsc").html(ifsc);
     $("#paymentterm").html(paymentterm);
     $("#advance").html(advance);
     $("#kindly").html(kindly);
     $("#email").html(email);
     document.getElementById('email').href = "mailto:"+email;
     $("#once").html(once);
     $("#choosing").html(choosing);
     $("#best").html(best);
     $("#team").html(team);



    $('#orderModaledit').modal("hide");
    $('#orderModal').modal("show");
    
}



function fillInquiryDetails(id) {
    $("#hidid").val(id);
    BindPopup(id);
    //window.location.href = "/Admin/Admin/Inquiry_Page?id=" + id + "";
}


function AddInquiryProduct(id) {
    $("#hidAdd").val(id);
    BindPopupAdd(id);
    //window.location.href = "/Admin/Admin/Inquiry_Page?id=" + id + "";
}



function BindPopupAdd(id) {
    $.ajax({
        url: "/Admin/Admin/BindPopupAdd",
        dataType: 'JSON',
        type: 'GET',
        data: { id: id },
        success: function (data) {
           
            data1 = JSON.parse(data.dt1);
            data2 = JSON.parse(data.enquirydt);

            
            $("#hiduseridAdd").val(data1[0].createuser);
            $("#hidEnquiryNoAdd").val(data1[0].EnquiryNo);
            $("#txtFullnameAdd").val(data1[0].Fullname);
            $("#txtEmailAdd").val(data1[0].Email);
            $("#txtMobileAdd").val(data1[0].Mobile);

          
            $("#ddlProductNames").empty().append('<option value="0">Select Product Name</option>');

            for (var i = 0; i < data2.length; i++) {
                $("#ddlProductNames").append('<option value="' + data2[i].ID + '">' + data2[i].ItemName + '</option>');
            }

           
            $('#btnAddtoquery2').modal("show");
        }

    });

} 



function bindtableid(id) {
    $('#itemidtable').hide();
    $.ajax({
        url: "/Admin/Admin/bindtableid",
        dataType: 'JSON',
        type: 'GET',
        data: { id: id },
        success: function (data) {
            //$('#orderlist').modal('show');
            data = JSON.parse(data);
            
            $('#itemidtable').show();
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                html += '<td>' + index + '</td>';
                html += '<td style="display:none;">' + item.ID + '</td>';
                html += '<td>' + item.ItemName + '</td>';

                html += '<td><input type="text" id="txtamount" value="' + item.Amount + '" /></td>';
                html += '<td><input type="text" id="txtquantity" value="' + item.Unit_Qty + '" /></td>';
                html += '<td><input type="text" id="txtcourior" value="' + 0 + '" /></td>';
                html += '<td><input type="text" oninput="totalrateAdd()" id="txttax" value="' + 0 + '" /></td>';
                //html += '<td><input type="text" id="txtTotal" value="' + 0 + '" /></td>';

                html += '<td>' + 0 + '</td>';

                /*html += '<td><input type = "button" data-toggle="modal" data-target="#btnAddtoquery" onclick="fillInquiryDetails(' + item.id + ')" class = "btn btn-primary"  value = "CONVERT INQUIRY INTO ORDER"></td>';*/
                html += '</tr>';
                index++;
            });
            if (html != null) {
                $('.itemdetails_bodyAdd').html(html);
               
            }
        }
    });

}




function BindPopup(id) {
    $.ajax({
        url: "/Admin/Admin/BindPopup",
        dataType: 'JSON',
        type: 'GET',
        data: {id:id},
        success: function (data) {
            //$('#orderlist').modal('show');
            data = JSON.parse(data);
            $("#hidEnquiryNo").val(data[0].EnquiryNo);
            $("#hiduserid").val(data[0].createuser);
            $("#txtFullname").val(data[0].Fullname);
            $("#txtEmail").val(data[0].Email);
            $("#txtMobile").val(data[0].Mobile);


            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                html += '<td>' + index + '</td>';
                html += '<td style="display:none;">' + item.ID+ '</td>';
                html += '<td>' + item.ItemName + '</td>';
           
                html += '<td><input type="text" id="txtamount" value="' + item.Amount + '" /></td>';
                html += '<td><input type="text" id="txtquantity" value="' + item.Unit_Qty + '" /></td>';
                html += '<td><input type="text" id="txtcourior" value="' + 0 + '" /></td>';
                html += '<td><input type="text" oninput="totalrate()" id="txttax" value="' + 0 + '" /></td>';
                //html += '<td><input type="text" id="txtTotal" value="' + 0 + '" /></td>';

                html += '<td>' + 0 + '</td>';
              
                /*html += '<td><input type = "button" data-toggle="modal" data-target="#btnAddtoquery" onclick="fillInquiryDetails(' + item.id + ')" class = "btn btn-primary"  value = "CONVERT INQUIRY INTO ORDER"></td>';*/
                html += '</tr>';
                index++;
            });
            if (html != null) {
                $('.itemdetails_body').html(html);
                $('#btnAddtoquery').modal("show");
            }
        }
    });

}






