$(document).ready(function () {
    $('#btnviewdetails').click(function () {
        var Loc_detailsId = $('#Loc_detailsId').val();
        var LocalityId = $('#LocalityId').val();
        var locdetails_Img = $('#locdetails_Img').attr('src');
        var Locality_detailsName = $('#LocName').text();
        $('#exampleModalToggle').modal('show');
        $('#hidlocdetailsId').val(Loc_detailsId);
        $('#hidLocalityId').val(LocalityId);
        $('#locDetailsimg').attr('src', "" + locdetails_Img + "");
        $('#Locality_detailsName').text(Locality_detailsName);
    });
});

$(document).ready(function () {
    $('#btnSubmit').click(function () {
        var LocalityId = $('#hidLocalityId').val();
        var locdetailsId = $('#hidlocdetailsId').val();
        var Fullname = $('#txtfullname').val();
        var phone = $('#txtPhone').val();
        var email = $('#txtEmail').val();
        var description = $('#txtdescription').val();
        var msdg = Validation();
        if (msdg == "") {
            $.ajax({
                type: "POST",
                url: '/Home/LocalityServicesEnquiry',
                data: { LocalityId: LocalityId, locdetailsId: locdetailsId, Fullname: Fullname, phone: phone, email: email, description: description },
                dataType: "JSON",
                success: function (result) {
                    if (result.message == "send enquiry.") {
                        ClearAllField();
                        $("#close").trigger('click');
                        //alert("Enquiry send successfully.");
                        swal({
                            title: "Thank You for sending the enquiry.",
                            text: "",
                            icon: "success",
                            timer: 10000,
                        });
                    }
                    else {
                        swal({
                            title: "Please try again",
                            text: "",
                            icon: "",
                            timer: 10000,
                        });
                    }
                },
                error: function () {
                    swal({
                        title: "Please try again",
                        text: "",
                        icon: "",
                        timer: 10000,
                    });
                }
            });
        }
        else {
            swal({
                title: msdg,
                text: "",
                icon: "",
                timer: 10000,
            });
        }
        });

});

function Validation() {
    var msg = "";
    if ($('#txtfullname').val() == "") { msg = "Full Name can not left Blank !! \n"; }
    if ($('#txtPhone').val() == "") { msg = "Phone can not left Blank !! \n"; }
    if ($('#txtEmail').val() == "") { msg = "Email can not left Blank !! \n"; }
    if ($('#txtdescription').val() == "") { msg = "Description can not left Blank !! \n"; }
    var email1 = $("#txtEmail").val();
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    var isValid = true;

    if ($('#txtEmail').val().trim() == "") {
      
        msg = "Please Enter Email Id.";
        // isValid = false;
    }
    else if (!emailReg.test(email1)) {
        msg = "Please Enter Valid Email.";
        //  isValid = false;
    }
    else {
      
    }

    var mobile = document.getElementById('txtPhone');
    if ($('#txtPhone').val().trim() == "") {
        //$("#txtMobileNoerror").text("Please Enter Mobile Number").show();
        // isValid = false;
        msg = "Please Enter Mobile Number";
    }
    else if (mobile.value.length > 10 || mobile.value.length < 10) {
        //$("#txtMobileNoerror").text("required 10 digits, match requested format!").show();
        // $("#txtMobileNoerror").show();
        // isValid = false;
        msg = "Required 10 digits, match requested format!";
    }
    else {
      
    }
    return msg;
}
$('.numberone').keypress(function (e) {
    var charCode = (e.which) ? e.which : event.keyCode
    if (charCode != "") {
        if (String.fromCharCode(charCode).match(/[^0-9]/g)) {
            // $("#txtMobileNoerror").text("Entered only Number").show();
            return false;
        }
        else {
            // $("#txtMobileNoerror").text("Entered only Number").hide();
        }
    }

});
function ClearAllField() {
    //$('#hidLocalityId').val("");
    //$('#hidlocdetailsId').val("");
    $('#txtfullname').val('');
    $('#txtPhone').val('');
    $('#txtEmail').val('');
    $('#txtdescription').val('');
}

$(document).ready(function () {
    $("#txtsearch").keyup(function () {
        //$("#txtsearch").change(function () {
        var len = $("#txtsearch").val().length;
        if (len == 6) {
            BindLocality();
        }
        else {
        }
    });
});
function BindLocality() {

    // var ff = $("#maindivid").html();
    // var ff=  $("p").text();

    // alert(ff);
    // $("#maindivid").html('');
    var pincode = $('#txtsearch').val();
    $.ajax({
        url: "/Home/LocalityBindByPincode",
        data: { pincode: pincode },
        dataType: 'JSON',
        type: 'GET',
        success: function (json) {
            var html = '';
            $("#maindivid").html('');
            json = json["locality"] || {};
            for (var i = 0; i < json.length; i++) {
                html += '<div class="col-sm-6 col-lg-3" id="divlocalitydetils">'
                html += '<a data-bs-toggle="modal" href="#exampleModalToggle">'
                html += '<div class="donation-item">'
                html += '<div class="img">'
                html += '<input type="hidden" value=' + json[i].loc_detailsId + ' id="Loc_detailsId" />'
                html += ' <input type="hidden" value=' + json[i].id + ' id="LocalityId" />'
                html += ' <img src="/images/Locality_Img/6/' + json[i].locdetails_Img + '" id="locdetails_Img" alt="Donation">'
                html += ' <div class="donate-btn">'
                html += ' <a class="common-btn" id="btnviewdetails">View Details</a>'
                html += '</div>'
                html += ' </div>'
                html += '<div class="inner" >'
                html += '    <div class="top">'
                html += '     <h3 id="LocName">' + json[i].locDetails_name + '</h3>'
                html += '      <h6 id="address">Address: ' + json[i].loc_address + '</h6>'
                html += '      <h6 id="pincode">Pin code: ' + json[i].pincode + '</h6>'
                html += '     <hr>'
                html += '         <h6 class="phone" id="contactno">Contact No.: ' + json[i].mobile_no + '</h6>'
                html += '         <hr />'
                html += '          <h6 class="phone" id="phoneno">Phone No.: ' + json[i].phone + '</h6>'
                html += '                  </div>'
                html += '  </div>'
                html += '  </div >'
                html += '       </a >'
                html += '    </div >'

                // alert(html);
                //$("#Loc_detailsId").val(json[i].loc_detailsId);
                //$("#LocalityId").val(json[i].id);
                //$("#locdetails_Img").attr('src', "/images/Locality_Img/" + json[i].locdetails_Img + "");
                //$('#LocName').text(json[i].locDetails_name);
                //$('#address').text(json[i].loc_address);
                //$('#pincode').text(json[i].pincode);
                //$('#contactno').text(json[i].mobile_no);
                //$('#phoneno').text(json[i].phone);
            }
            if (html != "") {
                $("#maindivid").html(html);
            }
            else {
                $("#maindivid").html("No data found for this pincode");
            }
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}