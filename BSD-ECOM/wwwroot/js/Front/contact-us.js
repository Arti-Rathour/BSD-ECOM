function Save() {
    var id = $('#id').val();
    var Firstname = $('#txtFname').val();
    var Lastname = $('#txtLname').val();
    var Email = $('#txtEmail').val();
    var mobile = $('#txtMobile').val();
    var Message = $('#txtComments').val();
    var msgd = Validation();
    if (msgd == "") {
        $.ajax({
            type: "POST",
            url: '/Home/enquiry',
            data: { Firstname: Firstname, Lastname: Lastname, Email: Email, mobile: mobile, Message: Message },
            dataType: "JSON",
            success: function (result) {
                if (result.message == "send enquiry") {
                    //alert("Email Sent Succesfully..!");
                    ClearAllField();
                   
                    //$("#exampleModalToggle").trigger("click");
                    /*  $('#exampleModalToggle').modal('show');*/
                    swal({
                        title: "Thank You",
                        text: "We have received your message and would like to thank you for writing to us.If your inquiry is urgent, please use the telephone numberlisted below to talk to one of our staff members. Otherwise, we will reply by email as soon as possible.",
                        icon: "success",
                        timer: 10000,
                    });
                   
                }
                else {
                    swal({
                        title: "Please try again",
                        text: "",
                        icon: "danger",
                        timer: 10000,
                    });
                   // alert("Invalid Email.");
                }
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
    else
    {
        swal({
            title: msgd,
            text: "",
            icon: "",
            timer: 10000,
        });
       // alert(msgd);
        return false;
    }
}

//$('#txtEmail').on('keyup', function () {
//    if (this.value != "") {
//        var re = /([A-Z0-9a-z_-][^@])+?@[^$#<>?]+?\.[\w]{2,4}/.test(this.value);
//        if (!re) {
//            $("#txtEmailIDerror").text("Please Enter Valid Email").show();
//        }
//    }
//});
function ClearAllField() {
    $('#txtFname').val('');
    $('#txtLname').val('');
    $('#txtEmail').val('');
    $('#txtMobile').val('');
    $('#txtComments').val('');
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
function Validation() {
    var msg = "";
    if ($('#txtFname').val() == "") { msg = "First Name can not left Blank !! \n"; }
    if ($('#txtLname').val() == "") { msg = "Last Name can not left Blank !! \n"; }
    //if ($('#txtEmail').val() == "") { msg = "Email can not left Blank !! \n"; }
    //if ($('#txtMobile').val() == "") { msg = "Phone can not left Blank !! \n"; }
    if ($('#txtComments').val() == "") { msg = "Message can not left Blank !! \n"; }


    var email1 = $("#txtEmail").val();
    // var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    var emailReg = /^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$/;
    var isValid = true;

    if ($('#txtEmail').val().trim() == "") {

        msg = "Please Enter Email Id.";

    }
    else if (!emailReg.test(email1)) {
        msg = "Please Enter Valid Email.";

    }
    else {
        //  msg = "";
    }



    var mobregex = /[0-9]+$/;
    var mobileNo = $("#txtMobile").val();
    if ($("#txtMobile").val().trim() == '') {
        msg += "Please Fill MobileNo!! \n";
    }
    else if (!mobregex.test($("#txtMobile").val().trim())) {
        msg += "Mobileno Only In Numeric \n";
    }
    else if (mobileNo.length != 10) {
        msg += "MobileNo  must be 10 digit \n";
    }

    return msg;
}