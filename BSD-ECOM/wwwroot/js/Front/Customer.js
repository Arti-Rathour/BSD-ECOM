/*const { debounce } = require("../../assets/vendor_components/fullcalendar/fullcalendar");*/

function validlogin()
{
    var msgs = "";
    var email = $('#txtemail').val();
    var password = $('#txtpassword').val();
    var securitycode = $('#security-code').text();
    var secruitycodevalue = $('#txtsecrutiycode').val();
    if (secruitycodevalue == "") {
        msgs = "Please enter secruity code.";
    }
    if (email == "") {
        msgs = "Please enter Email or Mobile no.";
        //return false;
    }
    if (password == "") {
        msgs = "Please enter the valid Password.";
        //return false;
    }
    if (secruitycodevalue.trim() != securitycode.trim())
    {
        msgs="Secruity code not match";
    }

    return msgs;
}
function Login() {

    localStorage.clear();

    var email = $('#txtemail').val();
    var mobile = "";
    var password = $('#txtpassword').val();
    var securitycode = $('#security-code').text();
    var secruitycodevalue = $('#txtsecrutiycode').val();

    var mgd = validlogin();

    if (mgd == "") {
        $.ajax({
            type: 'POST',
            url: "/Customer/CustomerLogin",
            data: { email: email, mobile: mobile, password: password },
            dataType: 'JSON',
            success: function (result) {

                if (result.message == "Success Login.") {
                    if ($('#hidtype').val() == 1) {
                        window.location.replace("/CheckOut");

                    }
                    else {

                        //window.location.replace("/CustomerAccount");
                        window.location.replace("/Home");

                    }

                }
                else {

                    swal({
                        title: "Invalid User.",
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });

                }
            }
        });
    }
    else {
        RefreshLoginSecruitcode();
        swal({
            title: mgd,
            text: "",
            icon: "success",
            timer: 10000,
        });
        return false;
        // alert("Secruity code does not match.")
    }
}
function Registration() {
    var id = $('#hidcustomerid').val();
    var firstname = $('#txtfirstname').val();
    var lastname = $('#txtlastname').val();
    var email = $('#txtEmail').val();
    var password = $('#newpassword').val();
    var confirmPassword = $('#confirmpassword').val();
    var Mobile = $('#txtmobilenumber').val();
    var securitycode = $('#security-code1').text();
    var secruitycodevalue = $('#txtsecrutiycode1').val();
    var companyid = $('#hiddencompanyid').val();
    var policy = $('#exampleCheckbox12').is(':checked') ? 1 : 0;
    var passwordlen = password.length;
   var msg = validationregistraion();
    if (msg == "") {
        if (password.trim() == confirmPassword.trim()) {
            if (password.length >= 6) {
                if (secruitycodevalue.trim() == securitycode.trim()) {
                    if (policy == 1) {
                        $.ajax({
                            type: 'POST',
                            url: "/Customer/CustomerRegistration",
                            data: { id: id, firstname: firstname, lastname: lastname, email: email, password: password, Mobile: Mobile, companyid: companyid },
                            dataType: 'JSON',
                            success: function (result) {
                                if (result.message == "Registration Success.") {
                                    clearregistraion();

                                    swal({
                                        title: "Thank you for Registration .Please check your mail for login details.",
                                        text: "",
                                        icon: "success",
                                        timer: 15000,
                                    });
                                    location.reload();
                                   // return RedirectToAction("Login", "Customer"/*, new { id = "1" }*/);
                                   // alert("Registration Success.\n your Email Id : " + result.email + "\n your password : " + result.password + "");
                                }
                                else if (result.message1 == "This Email id and Mobile number already exit.") {
                                   // alert("This Email id and Mobile number already exit.");
                                    swal({
                                        title: "This Email id and Mobile number already exit.",
                                        text: "",
                                        icon: "success",
                                        timer: 10000,
                                    });
                                }
                                else if (result.message1 == "This Mobile number already exit.") {
                                    swal({
                                        title: "This Mobile number already exit.",
                                        text: "",
                                        icon: "success",
                                        timer: 10000,
                                    });
                                    //alert("This Mobile number already exit.");
                                }
                                else if (result.message1 == "This Email Id already exit.") {
                                   // alert("This Email Id already exit.");
                                    swal({
                                        title: "This Email Id already exit.",
                                        text: "",
                                        icon: "success",
                                        timer: 10000,
                                    });
                                }
                                else {
                                    swal({
                                        title: "Some thing wrong..",
                                        text: "",
                                        icon: "success",
                                        timer: 10000,
                                    });
                                  //  alert("Some thing wrong.");
                                }
                            },
                        });
                    }
                    else {
                        swal({
                            title: "Please Check terms & Policy.",
                            text: "",
                            icon: "success",
                            timer: 10000,
                        });
                       // alert("Please Check terms & Policy.");
                    }
                }
                else {
                    swal({
                        title: "Security Code do not match.",
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                   // alert("Secutity Code do not match.");
                }
            }
            else {
                swal({
                    title: "Password should be minimum 6 characters with special characters..",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
               // alert("password should be minimum 6 characters with special characters.");
            }
        }
        else {
            $("#ErrorMessage").removeClass();
            $("#ErrorMessage").addClass('weak-password');
            $("#ErrorMessage").html("Passwords does not match!");
        }
    }
    else {

        swal({
            title: msg,
            text: "",
            icon: "success",
            timer: 10000,
        });
      //  validationregistraion();
    }
}
function clearregistraion() {
    $('#hidcustomerid').val(0);
    $('#txtfirstname').val("");
    $('#txtlastname').val("");
    $('#txtEmail').val("");
    $('#newpassword').val("");
    $('#confirmpassword').val("");
    $('#txtmobilenumber').val("");
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
function validationregistraion() {
    var msg = "";
    if ($('#txtfirstname').val() == "") { msg = "First name  can not  Blank !! \n"; }
    if ($('#txtlastname').val() == "") { msg = "Last name can not  Blank !! \n"; }
    if ($('#newpassword').val() == "") { msg = "Password  can not  Blank !! \n"; }
    if ($('#confirmpassword').val() == "") { msg = "Confirm Password can not  Blank !! \n"; }
    //if ($('#txtmobilenumber').val() == "") { msg = "Mobile can not  Blank !! \n"; }
    if ($('#txtsecrutiycode1').val() == "") { msg = "Secruity Code can not  Blank !! \n"; }


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
    var mobileNo = $("#txtmobilenumber").val();
    if ($("#txtmobilenumber").val().trim() == '') {
        msg += "Please Fill MobileNo!! \n";
    }
    else if (!mobregex.test($("#txtmobilenumber").val().trim())) {
        msg += "Mobileno Only In Numeric \n";
    }
    else if (mobileNo.length != 10) {
        msg += "MobileNo  must be 10 digit \n";
    }
    return msg;
}
function CustomerAccount() {

}
function ValidCheckout() {
   
    var msg = "";
    if ($('#txtfirstname').val() == "") { msg = "First name  can not  Blank !! \n"; }
    if ($('#txtlastname').val() == "") { msg = "Last name can not  Blank !! \n"; }
    if ($('#txtaddress1').val() == "") { msg = "Address Line 1 can not  Blank !! \n"; }
    if ($('#ddlcountry').val() == 0) { msg = "Country  can not  Blank !! \n"; }
    if ($('#ddlstate').val() == 0) { msg = "State can not  Blank !! \n"; }
    if ($('#ddlcity').val() == 0) { msg = "City can not  Blank !! \n"; }
    if ($('#txtpincode').val() == "") { msg = "Pincode can not  Blank !! \n"; }


    //if ($('#txtphone').val() == "") { msg = "Phone can not  Blank !! \n"; }
    //if ($('#txtemail').val() == "") { msg = "Email can not  Blank !! \n"; }

    var chkshipaddress = $('#chkshipaddress').is(':checked') ? 1 : 0;
    if (chkshipaddress == 1) {

        if ($('#txtSfirstname').val() == "") { msg = " Shipping First name  can not  Blank !! \n"; }
        //if ($('#txtlastname').val() == "") { msg += "Last name can not left Blank !! \n"; }
        if ($('#txtSaddress1').val() == "") { msg = " Shipping Address Line 1 can not  Blank !! \n"; }
        if ($('#ddlcountry1').val() == 0) { msg = " Shipping Country  can not  Blank !! \n"; }
        if ($('#ddlstate1').val() == 0) { msg = " Shipping State can not  Blank !! \n"; }
        if ($('#ddlstate1').val() == "") { msg = " Shipping State can not  Blank !! \n"; }
        if ($('#ddlcity1').val() == 0) { msg = " Shipping City can not  Blank !! \n"; }
        if ($('#ddlcity1').val() == "") { msg = " Shipping City can not  Blank !! \n"; }
        if ($('#txtSpincode').val() == "") { msg = " Shipping Pincode can not  Blank !! \n"; }
        //if ($('#txtSMobile').val() == "") { msg = " Shipping Phone can not  Blank !! \n"; }
        //if ($('#txtSemail').val() == "") { msg = " Shipping Email can not  Blank !! \n"; }

        var email1 = $("#txtSemail").val();
        // var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        var emailReg = /^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$/;
        var isValid = true;

        if ($('#txtSemail').val().trim() == "") {

            msg = "Please Enter Shipping Email Id.";

        }
        else if (!emailReg.test(email1)) {
            msg = "Please Enter Valid Shipping Email.";

        }
        else {
            //  msg = "";
        }



        var mobregex = /[0-9]+$/;
        var mobileNo = $("#txtSMobile").val();
        if ($("#txtSMobile").val().trim() == '') {
            msg += "Please Fill Shipping MobileNo!! \n";
        }
        else if (!mobregex.test($("#txtSMobile").val().trim())) {
            msg += "Shipping Mobileno Only In Numeric \n";
        }
        else if (mobileNo.length != 10) {
            msg += "Shipping MobileNo  must be 10 digit \n";
        }
       
    }


    var email1 = $("#txtemail").val();
    // var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    var emailReg = /^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$/;
    var isValid = true;

    if ($('#txtemail').val().trim() == "") {

        msg = "Please Enter Email Id.";

    }
    else if (!emailReg.test(email1)) {
        msg = "Please Enter Valid Email.";

    }
    else {
        //  msg = "";
    }



    var mobregex = /[0-9]+$/;
    var mobileNo = $("#txtphone").val();
    if ($("#txtphone").val().trim() == '') {
        msg += "Please Fill MobileNo!! \n";
    }
    else if (!mobregex.test($("#txtphone").val().trim())) {
        msg += "Mobileno Only In Numeric \n";
    }
    else if (mobileNo.length != 10) {
        msg += "MobileNo  must be 10 digit \n";
    }

    var ddd= $('#DeliveryErrormesage1').html();
    if (ddd != "") {
        msg = ddd;
    }

    var dddy = $('#DeliveryErrormesage').html();
    if (dddy != "") {
        msg = dddy;
    }
   // if (msg != "") { alert(msg); return false; }
    return msg;
}
function checkout() {
    clearTimeout(60000);
    var Sfirstname = "";
    var Slastname = "";
    var Saddress1 = "";
    var Saddress2 = "";
    var ScountryId = $('#ddlcountry1').find("option:selected").val();
    var SstateId = $('#ddlstate1').find("option:selected").val();
    var ScityId = $('#ddlcity1').find("option:selected").val();
    var Spincode = "";
    var smobile = "";
    var Semail = "";



    var paymentmode = "";
    var id = $('#customerid').val()
    var firstname = $('#txtfirstname').val();
    var lastname = $('#txtlastname').val();
    var address1 = $('#txtaddress1').val();
    var address2 = $('#txtaddress2').val();
    var countryId = $('#ddlcountry').find("option:selected").val();
    var stateId = $('#ddlstate').find("option:selected").val();
    var city = $('#ddlcity').find("option:selected").val();
    var pincode = $('#txtpincode').val();
    var phone = $('#txtphone').val();
    var email = $('#txtemail').val();
    var addtionalinfo = $('#txtadditionalinfo').val();
  //  showshipaddress();

    //if (typee == 1) {
    //    $('#DeliveryErrormesage1').text("This pincode not available for delivery");
    //}
    //else {
    //    $('#DeliveryErrormesage').text("This pincode not available for delivery");
    //}
    var chkshipaddress = $('#chkshipaddress').is(':checked') ? 1 : 0;
    if (chkshipaddress == 1) {
        var Sfirstname = $('#txtSfirstname').val();
        var Slastname = $('#txtSlastname').val();
        var Saddress1 = $('#txtSaddress1').val();
        var Saddress2 = $('#txtSaddress2').val();
        var ScountryId = $('#ddlcountry1').find("option:selected").val();
        var SstateId = $('#ddlstate1').find("option:selected").val();
        var ScityId = $('#ddlcity1').find("option:selected").val();
        var Spincode = $('#txtSpincode').val();
        var smobile = $('#txtSMobile').val();
        var Semail = $('#txtSemail').val();
    }
    else {

        var Sfirstname = firstname;
        var Slastname = lastname;
        var Saddress1 = address1;
        var Saddress2 = address2;
        var ScountryId = countryId;
        var SstateId = stateId
        var ScityId = city
        var Spincode = pincode;
        var smobile = phone;
        var Semail = email;

    }
    var rbtcashondelivery = $('#rbtcashondelivery').is(':checked') ? 1 : 0;
    var amount = $('#grandtotal').html();
    var shipamount = $('#shipcharged').html();
    if (rbtcashondelivery == 1) {
        paymentmode = "Cash on Delivery";
    }
    else {
        paymentmode = "Online Payment";
    }
    var productdetails = new Array();
    $("#productdetails tbody tr").each(function () {
        var row = $(this);
        var product = {};
        product.productid = row.find("span.itemid").text();
        product.productname = row.find('h6.productname').text();
        product.quantity = row.find('h6.quantity').text();
        product.productprice = row.find('h4.productprice').text();
        productdetails.push(product);
    });
  
    var productjson = JSON.stringify(productdetails)
    var msg = ValidCheckout();
    
    if (msg == "") {
        $("#pageloaddiv").show();
        $("#gifid").show();
        $.ajax({
            type: 'POST',
            url: "/Order/checkout",
            data: { id: id, amount: amount, firstname: firstname, lastname: lastname, address1: address1, address2: address2, countryId: countryId, stateId: stateId, city: city, pincode: pincode, phone: phone, email: email, addtionalinfo: addtionalinfo, Sfirstname: Sfirstname, Slastname: Slastname, Saddress1: Saddress1, Saddress2: Saddress2, ScountryId: ScountryId, SstateId: SstateId, ScityId: ScityId, Spincode: Spincode, paymentmode: paymentmode, smobile: smobile, Semail: Semail, shipc: shipamount, productjson: productjson },
            dataType: 'JSON',
            success: function (result) {
                $("#gifid").hide();
                if (result.message == "order place save.") {
                    window.location.replace("/thankyou");
                }
                else {

                }
            }
        });
    }
    else {
        swal({
            title: msg,
            text: "",
            icon: "success",
            timer: 10000,
        });
      //  alert(msg);
        //ValidCheckout();
        return false;
    }
}
function StateBind(selected_val) {
    //$("#ddlcountry").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Home/BindState', //url to be called
        data: "id=" + selected_val, //data to be send
        success: function (json, result) {
            $("#ddlstate").empty();
            json = json || {};
            $("#ddlstate").append('<option value="0">Select State</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlstate").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlstate").prop("disabled", false);
        },
        error: function () {
            swal({
                title: "Data Not Found",
                text: "",
                icon: "success",
                timer: 10000,
            });
            //alert("Data Not Found");
        }
    });
}
function StateBind1(selected_val) {
   /* var selected_val = $('#ddlcountry1').find(":selected").attr('value');*/
    //$("#ddlcountry1").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Home/BindState', //url to be called
        data: "id=" + selected_val, //data to be send
        success: function (json, result) {
            $("#ddlstate1").empty();
            json = json || {};
           // $("#ddlstate1").append('<option value="0">Select State</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlstate1").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlstate1").prop("disabled", false);
        },
        error: function () {
            swal({
                title: "Data Not Found",
                text: "",
                icon: "success",
                timer: 10000,
            });
        }
    });
}
function CityBind(selected_val) {
  //  var selected_val = $('#ddlstate').find(":selected").attr('value');
    //$("#ddlstate").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Home/BindCity', //url to be called
        data: "id=" + selected_val, //data to be send
        success: function (json, result) {
            $("#ddlcity").empty();
            json = json || {};
            $("#ddlcity").append('<option value="0">Select City</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlcity").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlcity").prop("disabled", false);
        },
        error: function () {
            //alert("Data Not Found");
            swal({
                title: "Data Not Found",
                text: "",
                icon: "success",
                timer: 10000,
            });
        }
    });
}
function CityBind1(selected_val) {
    //  var selected_val = $('#ddlstate').find(":selected").attr('value');
  //  $("#ddlstate1").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Home/BindCity', //url to be called
        data: "id=" + selected_val, //data to be send
        success: function (json, result) {
            $("#ddlcity1").empty();
            json = json || {};
            //$("#ddlcity1").append('<option value="0">Select City</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlcity1").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlcity1").prop("disabled", false);
        },
        error: function () {
           // alert("Data Not Found");
            swal({
                title: "Data Not Found",
                text: "",
                icon: "success",
                timer: 10000,
            });
        }
    });
}
function showshipaddress() {
  
    var chkshipaddress = $('#chkshipaddress').is(':checked') ? 1 : 0;
    if (chkshipaddress == 1) {
        var firstname = $('#txtfirstname').val();
        var lastname = $('#txtlastname').val();
        var address1 = $('#txtaddress1').val();
        var address2 = $('#txtaddress2').val();
        var countryId = $('#ddlcountry').find("option:selected").val();
        var stateId = $('#ddlstate').find("option:selected").val();
        var city = $('#ddlcity').find("option:selected").val();
        var pincode = $('#txtpincode').val();
        var phone = $('#txtphone').val();
        var email = $('#txtemail').val();
        var addtionalinfo = $('#txtadditionalinfo').val();
        $('#txtSfirstname').val(firstname);
        $('#txtSlastname').val(lastname);
        $('#txtSaddress1').val(address1);
        $('#txtSaddress2').val(address2);
        //$('#ddlcountry1').val(countryId);
       // $('#ddlcountry1').trigger('change');
        StateBind1(countryId);
        $('#ddlstate1').val(stateId);
        CityBind1(stateId);
        $('#ddlcity1').val(city);
        $('#txtSpincode').val(pincode);
        $('#txtSMobile').val(phone);
        $('#txtSemail').val(email);
    }
    else {
        $('#txtSfirstname').val("");
        $('#txtSlastname').val("");
        $('#txtSaddress1').val("");
        $('#txtSaddress2').val("");
        $('#ddlcountry1').val(0);
        $('#ddlstate1').val(0);
        $('#ddlcity1').val(0);
        $('#txtSpincode').val("");
        $('#txtSMobile').val("");
        $('#txtSemail').val("");
    }
}
function changePassword() {
    var currentpassword = $('#currentpassword').val();
    var newpassword = $('#newpassword').val();
    var confirmpassword = $('#confirmpassword').val();
    var msg = validatechangePassword();
    if (msg == "") {
        if (newpassword == confirmpassword) {
            if (newpassword.length >= 6) {
                $.ajax({
                    type: 'POST',
                    url: "/Customer/ChangePassword",
                    data: { currentpassword: currentpassword, newpassword: newpassword, confirmpassword: confirmpassword },
                    dataType: 'JSON',
                    success: function (result) {
                        if (result.message == "Password Update succesfully") {
                            clearchangePasswordfield();
                            window.location.replace("/Login");
                            alert("Password update succesfully.");
                        }
                        //else {
                        //    alert("Password and Confirm Password do not match.")
                        //}
                    },
                    error: function (e) {
                        swal({
                            title: "Old Password doest not exist.",
                            text: "",
                            icon: "success",
                            timer: 10000,
                        });
                       // alert("Old Password doest not exist.")
                    }
                });
            }
            else {
                swal({
                    title: "Password should be minimum 6 characters with special characters.",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
                //alert("password should be minimum 6 characters with special characters.");
            }
        }
        else {
            $("#ErrorMessage").removeClass();
            $("#ErrorMessage").addClass('weak-password');
            $("#ErrorMessage").html("Passwords does not match!");
        }
    }
}

function IChangeclear() {
    $('#txtnewpass').val("");
    $('#txtconfirmpass').val("");
    $('#hidemail').val("");
}





function IChangePassworddd() {
    var NewPassword = $('#txtnewpass').val();
    var ConfirmPassword = $('#txtconfirmpass').val();
    var email = $('#hidemail').val();

    

        $.ajax({
            type: "POST",
            url: '/Customer/IChangePassworddd',
            data: { NewPassword: NewPassword, ConfirmPassword: ConfirmPassword, email: email },
            dataType: "JSON",
            success: function (result) {
                if (result.message == "Password Changed Successfully") {
                    alert("Password Changed Successfully");
                } else {
                    alert("New Password nad Confirm Password do not match");
                }
                IChangeclear();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
   
}




function checkPasswordMatch() {
    var Password = $("#newpassword").val();
    var ConfirmPassword = $("#confirmpassword").val();
    if (Password != ConfirmPassword) {
        $("#ErrorMessage").removeClass();
        $("#ErrorMessage").addClass('weak-password');
        $("#ErrorMessage").html("Passwords does not match!");
    }
    else {
        $("#ErrorMessage").removeClass();
        $("#ErrorMessage").addClass('strong-password');
        $("#ErrorMessage").html("Passwords match.");
    }
}
function validatechangePassword() {
    var msg = "";
    if ($('#currentpassword').val() == "") { msg = "Current Password  can not  Blank !! \n"; }
    if ($('#newpassword').val() == "") { msg += "New Password can not  Blank !! \n"; }
    if ($('#confirmpassword').val() == "") { msg += "Confirm Password can not  Blank !! \n"; }
    if (msg != "")
    {
        swal({
            title: msg,
            text: "",
            icon: "success",
            timer: 10000,
        });
        //alert(msg);
        return false;
    }
    return msg;
}
function checknewPasswordStrength() {
    var number = /([0-9])/;
    var alphabets = /([a-zA-Z])/;
    var special_characters = /([~,!,@@,#,$,%,^,&,*,-,_,+,=,?,>,<])/;
    if ($('#newpassword').val().length < 6) {
        $('#passwords-strength-status').removeClass();
        $('#passwords-strength-status').addClass('weak-password');
        $('#passwords-strength-status').html("Weak (should be atleast 6 characters.)");
    } else {
        if ($('#newpassword').val().match(number) && $('#newpassword').val().match(alphabets) && $('#newpassword').val().match(special_characters)) {
            $('#passwords-strength-status').removeClass();
            $('#passwords-strength-status').addClass('strong-password');
            $('#passwords-strength-status').html("Strong");
        } else {
            $('#passwords-strength-status').removeClass();
            $('#passwords-strength-status').addClass('medium-password');
            $('#passwords-strength-status').html("Medium (should include alphabets, numbers and special characters.)");
        }
    }
}
function clearchangePasswordfield() {
    $('#currentpassword').val("");
    $('#newpassword').val("");
    $('#confirmpassword').val("");
}
function clearLoginField() {
    $('#txtemail').val("");
    $('#txtmobile').val("");
    $('#txtpassword').val("");
}
function UpdateAccount() {
    var firstname = $("#txtfirstname").val();
    var lastname = $("#txtLastname").val();
    var displayname = $("#txtdisplayname").val()
    var emailaddress = $("#txtemail").val()
    var mobilenumber = $("#txtmobile").val()
    $.ajax({
        type: "POST",
        url: "/Customer/UpdateAccount",
        data: { firstname: firstname, lastname: lastname, displayname: displayname, emailaddress: emailaddress, mobilenumber: mobilenumber },
        dataType: "JSON",
        success: function (result) {
            if (result.message == "Account update succesfully") {
                swal({
                    title: "Account update succesfully.",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
                window.location.replace("/Login");
                //alert("Account update succesfully.");
            }
        },
        error: function (e) {

           alert("Account update succesfully.")
        }
    });
}
function showdisplayname() {
    var firstname = $("#txtfirstname").val();
    var lastname = $("#txtLastname").val();
    var displayname = firstname + " " + lastname;
    $("#txtdisplayname").val(displayname);
}
function updatebillingaddress() {

    var BAddress1 = $('#txtbAddress1').val();
    var BAddress2 = $('#txtbAddress2').val();
    var bCountryid = $('#ddlbCountry').find("option:selected").val();
    var bCountrytext = $('#ddlbCountry').find("option:selected").text();
    var bstateid = $('#ddlstate').find("option:selected").val();
    var bstatetext = $('#ddlstate').find("option:selected").text();
    var bcityid = $('#ddlcity').find("option:selected").val();
    var bcitytext = $('#ddlcity').find("option:selected").text();
    var bmobile = $('#txtbmobile').val();
    var bpincode = $('#txtbpincode').val();
    $.ajax({
        type: "POST",
        url: "/Customer/Updatebillingaddress",
        data: { BAddress1: BAddress1.replace("Address Line1:", ""), BAddress2: BAddress2.replace("Address Line2:", ""), bCountryid: bCountryid, bstateid: bstateid, bcityid: bcityid, bmobile: bmobile, bpincode: bpincode},
        dataType: "JSON",
        success: function (result) {
            if (result.message == "update Billing address") {
                cleanbillingaddressfield();
                $("#btnbclose").trigger('click');
                swal({
                    title: "update Billing address succesfully.",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
                window.location.replace("/CustomerAccount");
                //alert("update Billing address succesfully.");
            }
        },
        error: function (e) {
            alert("Billing address not updated.")
        }
    });
}
function updateshippingaddress() {
    var SAddress1 = $('#txtsAddress12').val();
    var SAddress2 = $('#txtsAddress22').val();
    var SCountryid = $('#ddlsCountry1').find("option:selected").val();
    var SCountrytext = $('#ddlsCountry1').find("option:selected").text();
    var Sstateid = $('#ddlstate1').find("option:selected").val();
    var Sstatetext = $('#ddlstate1').find("option:selected").text();
    var scityid = $('#ddlcity1').find("option:selected").val();
    var Scitytext = $('#ddlcity1').find("option:selected").text();
    var sbmobile = $('#txtsmobile1').val();
    var spincode = $('#txtspincode1').val();
    $.ajax({
        type: "POST",
        url: "/Customer/UpdateShippingaddress",
        data: { SAddress1: SAddress1.replace("Address Line1:", ""), SAddress2: SAddress2.replace("Address Line2:",""), SCountryid: SCountryid, Sstateid: Sstateid, scityid: scityid, sbmobile: sbmobile, spincode: spincode},
        dataType: "JSON",
        success: function (result) {
            if (result.message == "update shipping address") {
                cleanshippingaddressfield();
                $("#btnsclose").trigger('click');
                swal({
                    title: "update shipping address succesfully.",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
                window.location.replace("/CustomerAccount");
                //alert("update shipping address succesfully.");
            }
        },
        error: function (e) {
            alert("shipping address not updated.")
        }
    });
}
function cleanshippingaddressfield() {
    $('#txtsAddress1').val("");
    $('#txtsAddress2').val("");
    $('#ddlsCountry').val(0);
    $('#ddlstate1').find("option:selected").val(0);
    $('#ddlcity1').find("option:selected").val(0);
    $('#txtsmobile').val("");
    $('#txtspincode').val("");
}
function cleanbillingaddressfield() {
    $('#txtbAddress1').val("");
    $('#txtbAddress2').val("");
    $('#ddlbCountry').val(0);
    $('#ddlstate').val(0);
    $('#ddlcity').val(0);    
    $('#txtbmobile').val("");
    $('#txtbpincode').val("");
}
function EditBillingaddress() {
   
    var baddress1 = $('#baddressline1').text();
    var baddress2 = $('#baddressline2').text();
    var bmobile = $('#bmobile').text();
    var bpincode = $('#bPinCode').text();
    var bcity = $('#bcity').text();
    var bstate = $('#bstate').text();
    var bcountry = $('#bcountry').text();
    $('#txtbAddress1').val(baddress1.replace("Address Line1:",""));
    $('#txtbAddress2').val(baddress2.replace("Address Line2:", ""));
    $('#ddlbCountry').find("option:selected").val();
    $('#ddlstate').find("option:selected").val();
    $('#ddlcity').find("option:selected").val();
    $('#txtbmobile').val(bmobile.replace("Mobile :", ""));
    $('#txtbpincode').val(bpincode.replace("Pincode : ", ""));
    $('#edit-baddress').modal('show');
}
function editshippingaddress()
{
    var saddress1 = $('#saddress1').text();
    var saddress2 = $('#saddress2').text();
    var smobile = $('#smobile').text();
    var spincode = $('#spincode').text();
    var scity = $('#scity').text();
    var sstate = $('#sstate').text();
    var scountry = $('#scountry').text();
    $('#txtsAddress12').val(saddress1.replace("Address Line1:", ""));
    $('#txtsAddress22').val(saddress2.replace("Address Line2:", ""));
    $('#ddlsCountry1').find("option:selected").val();
    $('#ddlstate1').find("option:selected").val();
    var scityid = $('#ddlcity1').find("option:selected").val();
    var sbmobile = $('#txtsmobile1').val(smobile.replace("Mobile : ",""));
    var spincode = $('#txtspincode1').val(spincode.replace("Pincode : ",""));

    $('#edit-saddress').modal('show');
}
function showaddress() {

    var chkshipaddress = $('#chkshipaddress').is(':checked') ? 1 : 0;
    if (chkshipaddress == 1) {
        $.ajax({
            url: "/Customer/showaddress",
            dataType: 'JSON',
            type: 'GET',
            //contentType: 'application/json; charset=utf-8',
            success: function (json) {
                var html = '';
                $("#maindivid").html('');
                json = json || {};
                for (var i = 0; i < json.length; i++) {
                    html += '<div class="col-lg-6">'
                    html += '<div class="card mb-3 mb-lg-0">'
                    html += '<div class="card-header">'
                    html += '<h5 class="text-uppercase" style="text-align:center">Billing Address</h3>'
                    html += '</div>'
                    html += '<div class="card-body" id="billingaddress">'
                    html += '<p id="baddressline1">'
                    html += 'Address Line1: ' + json[i].address + ''
                    //                html += '75 Business Spur,<br>'
                    //                html += 'Sault Ste. <br>Marie, MI 49783'
                    html += '</p>'
                    html += '<p id="baddressline2">'
                    html += 'Address Line2: ' + json[i].address2 + ''
                    html += '</p>'
                    html += '<p id="bmobile">Mobile : ' + json[i].mobileNo + '</p>'
                    html += '<p id="bcity">City : ' + json[i].bill_city + '</p>'
                    html += '<p id="bstate">State : ' + json[i].bill_state + '</p>'
                    html += '<p id="bPinCode">Pincode : ' + json[i].pinCode + '</p>'
                    html += '<p id="bcountry">Country : ' + json[i].bil_country + '</p>'
                    //  html += '<a aria-label="Quick view" class="btn-small" data-bs-toggle="modal" data-bs-target="#edit-baddress">Edit</a>'
                    html += '<a href="#" class="btn-small" onclick="EditBillingaddress()">Edit</a>'
                    html += '</div>'
                    html += '</div>'
                    html += '</div>'

                    html += '<div class="col-lg-6">'
                    html += '<div class="card">'
                    html += '<div class="card-header">'
                    html += '<h5 class="text-uppercase" style="text-align:center">Shipping Address</h5>'
                    html += '</div>'
                    html += '<div class="card-body" id="shippingaddress">'
                    html += '<p id="saddress1">'
                    html += 'Address Line1: ' + json[i].address + ''
                    //html += 'Sarasota, <br>FL 34249 USA <br>Phone: 1.941.227.4444'
                    html += '</p>'
                    html += '<p id="saddress2">'
                    html += 'Address Line2: ' + json[i].address2 + ''
                    html += '</p>'
                    html += '<p id="smobile">Mobile : ' + json[i].mobileNo + '</p>'
                    html += '<p id="scity">City : ' + json[i].bill_city + '</p>'
                    html += '<p id="sstate">State : ' + json[i].bill_state + '</p>'
                    html += '<p id="spincode">Pincode : ' + json[i].pinCode + '</p>'
                    html += '<p id="scountry">Country : ' + json[i].bil_country + '</p>'
                    //html += '<a aria-label="Quick view" class="btn-small" data-bs-toggle="modal" data-bs-target="#edit-saddress">Edit</a>'
                    html += '<a href="#" class="btn-small" onclick="editshippingaddress()">Edit</a>'
                    html += '</div>'
                    html += '</div>'
                    html += '</div>'

                    //html += '<div class="col-lg-6">'
                    //html += '<div class="card">'
                    //html += '<div class="card-header">'
                    //html += '<h5 class="text-uppercase" style="text-align:center">Shipping Address</h5>'
                    //html += '</div>'
                    //html += '<div class="card-body" id="shippingaddress">'
                    //html += '<p id="saddress1">'
                    //html += 'Address Line1: ' + json[i].shi_Address + ''
                    ////html += 'Sarasota, <br>FL 34249 USA <br>Phone: 1.941.227.4444'
                    //html += '</p>'
                    //html += '<p id="saddress2">'
                    //html += 'Address Line2: ' + json[i].s_address2 + ''
                    //html += '</p>'
                    //html += '<p id="smobile">Mobile : ' + json[i].shi_MobileNO+'</p>'
                    //html += '<p id="scity">City : ' + json[i].ship_city +'</p>'
                    //html += '<p id="sstate">State : ' + json[i].ship_state + '</p>'
                    //html += '<p id="spincode">Pincode : ' + json[i].shi_PinCode + '</p>'
                    //html += '<p id="scountry">Country : ' + json[i].ship_country +'</p>'
                    ////html += '<a aria-label="Quick view" class="btn-small" data-bs-toggle="modal" data-bs-target="#edit-saddress">Edit</a>'
                    //html += '<a href="#" class="btn-small" onclick="editshippingaddress()">Edit</a>'
                    //html += '</div>'
                    //html += '</div>'
                    //html += '</div>'


                    html += '<div class="form-group" >'
                    html += '<div class="custome-checkbox" >'
                    html += '<input class="form-check-input" type="checkbox" name="checkbox" id="chkshipaddress" onclick="showaddress()" checked>'
                    html += ' <label class="form-check-label label_info" data-bs-toggle="collapse" data- target="#collapseAddress" href="#collapseAddress" aria-controls="collapseAddress" for="chkshipaddress"><span>Ship to a different address?</span></label>'
                    html += '</div>'
                    html += '</div>'
                    html += '</div>'
                }
                if (html != "") {
                    $("#maindivid").html(html);
                }
                else {
                    $("#maindivid").html("No data found.");
                }
            },
            error: function () {
                alert("Data Not Found");
            }
        });
    } else {

        $.ajax({
            url: "/Customer/showaddress",
            dataType: 'JSON',
            type: 'GET',
            //contentType: 'application/json; charset=utf-8',
            success: function (json) {
                var html = '';
                $("#maindivid").html('');
                json = json || {};
                for (var i = 0; i < json.length; i++) {
                    html += '<div class="col-lg-6">'
                    html += '<div class="card mb-3 mb-lg-0">'
                    html += '<div class="card-header">'
                    html += '<h5 class="text-uppercase" style="text-align:center">Billing Address</h3>'
                    html += '</div>'
                    html += '<div class="card-body" id="billingaddress">'
                    html += '<p id="baddressline1">'
                    html += 'Address Line1: ' + json[i].address + ''
                    //                html += '75 Business Spur,<br>'
                    //                html += 'Sault Ste. <br>Marie, MI 49783'
                    html += '</p>'
                    html += '<p id="baddressline2">'
                    html += 'Address Line2: ' + json[i].address2 + ''
                    html += '</p>'
                    html += '<p id="bmobile">Mobile : ' + json[i].mobileNo + '</p>'
                    html += '<p id="bcity">City : ' + json[i].bill_city + '</p>'
                    html += '<p id="bstate">State : ' + json[i].bill_state + '</p>'
                    html += '<p id="bPinCode">Pincode : ' + json[i].pinCode + '</p>'
                    html += '<p id="bcountry">Country : ' + json[i].bil_country + '</p>'
                    //  html += '<a aria-label="Quick view" class="btn-small" data-bs-toggle="modal" data-bs-target="#edit-baddress">Edit</a>'
                    html += '<a href="#" class="btn-small" onclick="EditBillingaddress()">Edit</a>'
                    html += '</div>'
                    html += '</div>'
                    html += '</div>'



                    //html += '<div class="col-lg-6">'
                    //html += '<div class="card">'
                    //html += '<div class="card-header">'
                    //html += '<h5 class="text-uppercase" style="text-align:center">Shipping Address</h5>'
                    //html += '</div>'
                    //html += '<div class="card-body" id="shippingaddress">'
                    //html += '<p id="saddress1">'
                    //html += 'Address Line1: ' + json[i].shi_Address + ''
                    ////html += 'Sarasota, <br>FL 34249 USA <br>Phone: 1.941.227.4444'
                    //html += '</p>'
                    //html += '<p id="saddress2">'
                    //html += 'Address Line2: ' + json[i].s_address2 + ''
                    //html += '</p>'
                    //html += '<p id="smobile">Mobile : ' + json[i].shi_MobileNO+'</p>'
                    //html += '<p id="scity">City : ' + json[i].ship_city +'</p>'
                    //html += '<p id="sstate">State : ' + json[i].ship_state + '</p>'
                    //html += '<p id="spincode">Pincode : ' + json[i].shi_PinCode + '</p>'
                    //html += '<p id="scountry">Country : ' + json[i].ship_country +'</p>'
                    ////html += '<a aria-label="Quick view" class="btn-small" data-bs-toggle="modal" data-bs-target="#edit-saddress">Edit</a>'
                    //html += '<a href="#" class="btn-small" onclick="editshippingaddress()">Edit</a>'
                    //html += '</div>'
                    //html += '</div>'
                    //html += '</div>'

                    html += '<div class="form-group" >'
                    html += '<div class="custome-checkbox" >'
                    html += '<input class="form-check-input" type="checkbox" name="checkbox" id="chkshipaddress" onclick="showaddress()">'
                    html += ' <label class="form-check-label label_info" data-bs-toggle="collapse" data- target="#collapseAddress" href="#collapseAddress" aria-controls="collapseAddress" for="chkshipaddress"><span>Ship to a different address?</span></label>'
                    html += '</div>'
                    html += '</div>'
                    html += '</div>'
                }
                if (html != "") {
                    $("#maindivid").html(html);
                }
                else {
                    $("#maindivid").html("No data found.");
                }
            },
            error: function () {
                alert("Data Not Found");
            }
        });
    }
}






function showorder() {

    $.ajax({
        type: 'GET',
        url: '/Customer/showorder',
        dataType: 'JSON',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.order_no + '</td>';
                html += '<td>' + item.orderdate + '</td>';
                html += '<td>' + item.amount + '</td>';
                html += '<td>' + item.payment_mode + '</td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return showorderdetailsId(' + item.id + ')"><i class="fi-rs-eye"></i></a></td>';
                html += '<td>' + item.status + '</td>';
                if (item.status_flg != "5") {
//                    html += '<td><a class="btn btn-sm" href="#" onclick="return cancelorder(' + item.id + ')">Cancel</a></td>';
                    html += '<td><a class="btn btn-sm" href="#" onclick="return opencancelPopup(' + item.id + ')">Cancel</a></td>';
                }
                else if (item.status_flg == "5") {
                    if (parseInt(item.orderday) >= 7)
                    {

                        html += '<td></td>'
                    } else {
                      //  html += '<td><a class="btn btn-sm" href="#" onclick="return returnorder(' + item.id + ')">Return</a></td>'
                        html += '<td><a class="btn btn-sm" href="#" onclick="return openreturnpopup(' + item.id + ')">Return</a></td>'
                    }
                }
                else {
                    html += '<td></td>'
                }
                html += '</tr>';
                index++;
            });
            if (html != "") {
                $('.tbodyData').html(html);
            } else {
                $('.tbodyData').html("Your order is not available.");
            }
        }

    });
}
function opencancelPopup(id) {
    $('#order-cancel').modal('show');
    $('#cancelorderid').val(id);

}
function cancelorder() {
    var id = $('#cancelorderid').val();
    var cancel_type = $('#ddlReasionforcancel').find("option:selected").val();
    var cancel_Resion = $('#txtCancelcomment').val();
    $.ajax({
        url: "/Order/cancelorder",
        data: { id: id, cancel_type: cancel_type, cancel_Resion: cancel_Resion },
        dataType: 'JSON',
        type:'POST',
        success: function (result) {
            if (result.message == "Your Order Cancel Successfully.") {
               
                $('#order-cancel').modal('hide');
                //alert("Your Order Cancel Successfully.");
                swal({
                    title: "Your Order Cancel Successfully.",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
                showorder();
            }
            else {
                swal({
                    title: "Your Order not Cancel.",
                    text: "",
                    icon: "error",
                    timer: 10000,
                });
                //alert("Your Order not Cancel.");
            }
        },
    });
}
function openreturnpopup(id) {
    $('#order-return').modal('show');
    $('#returnorderid').val(id);
}
function returnorder() {
    var id = $('#returnorderid').val();
    var return_type = $('#ddlReasionforreturn').find("option:selected").val();
    var return_Resion = $('#txtreturncomment').val();
    var return_bank_name = $('#txtbname').val();
    var return_IFSC_code = $('#txtcode').val();
    var return_Account_no = $('#txtAccno').val();
    var return_Account_holder_name = $('#txtAccholdername').val();
    $.ajax({
        url: "/Order/returnorder",
        data: { id: id, return_type: return_type, return_Resion: return_Resion, return_bank_name: return_bank_name, return_IFSC_code: return_IFSC_code, return_Account_no: return_Account_no, return_Account_holder_name: return_Account_holder_name },
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            if (result.message == "Your Order Return Successfully.") {
                showorder();
                $('#order-return').modal('hide');
                //alert("Your Order Return Successfully.");
                swal({
                    title: "Your Order Return Successfully.",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
            }
            else {
                swal({
                    title: "Your Order not Return.",
                    text: "",
                    icon: "error",
                    timer: 10000,
                });
                //alert("Your Order not Return.");
            }
        },
    });
}
function walletbalance() {
    $.ajax({
        type: 'GET',
        url: '/Customer/walletbalance',
        dataType: 'JSON',
        success: function (data) {
            $('#lblCredit').text(data[0].credit);
            $('#lbldebit').text(data[0].debit);
            $('#lblbalance').text(data[0].totalbalance);
        }
    });
}
function AddMoney() {
    var rechargeamount = $('#txtrechargeamount').val();
    $.ajax({
        type: 'POST',
        data: { rechargeamount: rechargeamount},
        url: '/Customer/walletpayemnt',
        dataType: 'JSON',
        success: function (data) {
            if (data != null) {
                $('#My-walletpayemnt').modal('show');
                $("#Divdata").html(data);
            }
        }
    });
}
function PayNow() {
    var amt = $('#lblamt').text();
    $.ajax({
        type: 'POST',
        data: { amt: amt },
        url: '/Customer/rechargewallet',
        dataType: 'JSON',
        success: function (data) {
            if (data.message == "Recharge add Successfully.") {
                walletbalance();
                $('#btnMywalletpayemntclose').trigger("click");
                swal({
                    title: "Recharge add Successfully.",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
            }
            else {
                swal({
                    title: data.message,
                    text: "",
                    icon: "danger",
                    timer: 10000,
                });
            }
        }
    });
}
function showorderdetailsId(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Customer/showorderdetails",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'GET',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            console.log(data);
            var countcustomerOrders = data.customerOrders.length;
            var countcustomers = data.customers.length;
            var html = '';
            var index = 1;
            for (var i = 0; i < countcustomerOrders; i++) {
                html += '<tr class="hover-primary">';
                html += '<td style="display:none">' + data.customerOrders[i].orderdate + '</td>';
                html += '<td>' + data.customerOrders[i].itemname + '</td>';
                html += '<td><img src="/images/Productimage/6/' + data.customerOrders[i].itemimage + '" alt="your image" width="75" height="80" /></td>';
               /// html += '<td>' + data.customerOrders[i].taxable_Value + '</td>';
                //html += '<td>' + data.customerOrders[i].cgstAmt + '</td>';
               // html += '<td>' + data.customerOrders[i].sgstAmt + '</td>';
               // html += '<td>' + data.customerOrders[i].igstAmt + '</td>';
                html += '<td>' + data.customerOrders[i].subtotal + '</td>';
                html += '</tr>';
                index++;
            }
            if (html != "") {
                $('.tbodyOrderData').html(html);
            }
            else {
                $('.tbodyOrderData').html("Data Not Found.");
            }
            var html1 = "";
            html1 += '<div class="col-lg-6">'
            html1 += '<div class="card mb-3 mb-lg-0">'
            html1 += '<div class="card-header">'
            html1 += '<h5 class="text-uppercase" style="text-align:center">Billing Address</h3>'
            html1 += '</div>'
            html1 += '<div class="card-body" id="billingaddress">'
            html1 += '<p id="baddressline1">'
            html1 += 'Address Line1: ' + data.customers[0].address + ''
            html1 += '</p>'
            html1 += '<p id="baddressline2">'
            html1 += 'Address Line2: ' + data.customers[0].address2 + ''
            html1 += '</p>'
            html1 += '<p id="bmobile">Mobile : ' + data.customers[0].bill_MobileNO + '</p>'
            html1 += '<p id="bcity">City : ' + data.customers[0].bill_city + '</p>'
            html1 += '<p id="bstate">State : ' + data.customers[0].bill_state + '</p>'
            html1 += '<p id="bPinCode">Pincode : ' + data.customers[0].pinCode + '</p>'
            html1 += '<p id="bcountry">Country : ' + data.customers[0].bil_country + '</p>'            
            html1 += '</div>'
            html1 += '</div>'
            html1 += '</div>'
            html1 += '<div class="col-lg-6">'
            html1 += '<div class="card">'
            html1 += '<div class="card-header">'
            html1 += '<h5 class="text-uppercase" style="text-align:center">Shipping Address</h5>'
            html1 += '</div>'
            html1 += '<div class="card-body" id="shippingaddress">'
            html1 += '<p id="saddress1">'
            html1 += 'Address Line1: ' + data.customers[0].shi_Address + ''
            html1 += '</p>'
            html1 += '<p id="saddress2">'
            html1 += 'Address Line2: ' + data.customers[0].s_address2 + ''
            html1 += '</p>'
            html1 += '<p id="smobile">Mobile : ' + data.customers[0].shi_MobileNO + '</p>'
            html1 += '<p id="scity">City : ' + data.customers[0].ship_city + '</p>'
            html1 += '<p id="sstate">State : ' + data.customers[0].ship_state + '</p>'
            html1 += '<p id="spincode">Pincode : ' + data.customers[0].shi_PinCode + '</p>'
            html1 += '<p id="scountry">Country : ' + data.customers[0].ship_country + '</p>'
            html1 += '</div>'
            html1 += '</div>'
            html1 += '</div>'
            if (html1 != "") {
                $('#orderAddress').html(html1);
            }
            else {
                $('#orderAddress').html("Data not found.");
            }
            $('#Show-order-details').modal('show');
        },
        error: function () {
        }
    });
}
//function openInvoice(id) {
//    ReloadPageWithRouteandId("Invoice", id);
//}
function Genratesecruticode() {
    var charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    var password = '';
    var password_length = 5;
    for (var i = 0; i < password_length; i++) {
        var random_position = Math.floor(Math.random() * charset.length);
        password += charset[random_position,random_position+1];
    }
    if (password.length == password_length) {
        password = password.replace(/</g, "&lt;").replace(/>/g, "&gt;");
        return password;
        //$('#passwords').prepend(password + '<br/>');
    }
    //    else {
    //    console.log(password.length, password_length);
    //}
    //return password;
}
function RefreshLoginSecruitcode() {
    var password = Genratesecruticode();
    $('#security-code').text(password);
}
function RefreshRegSecruityCode() {
    var password = Genratesecruticode();
    $('#security-code1').text(password);
}
function check_contNo(mobile) {
    var valid = true;
    var phone = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    if (!(mobile).match(phone)) {
        $("#txtmobilenoerror").text("enter correct mobile number...!").show();
        valid = false;
    }
    else {
        $("#txtmobilenoerror").text("enter correct mobile number...!").hide();
        $("#txtmobilenoerror").hide();
    }
    return valid;
}
function check_Email(email) {
    var valid = true;
    var EmailFormate = /^([a-zA-Z0-9_.+-])+\@@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!(email).match(EmailFormate)) {
        $("#txtEmailerror").text("enter correct email...!").show();
        valid = false;
    }
    else {
        $("#txtEmailerror").text("enter correct email...!").hide();
        $("#txtEmailerror").hide();
    }
    return valid;
}
function checkplace(pincode,typee) {
    $.ajax({
        type: 'POST',
        url: "/Order/checkOrderplacebypincode",
        data: { pincode: pincode },
        dataType: 'JSON',
        success: function (result) {
            if (result.message == "This pincode available for delivery.") {
                if (typee == 1) {
                    $('#DeliveryErrormesage1').html('');
                   // $('#DeliveryErrormesage1').text("This pincode available for delivery.");
                }
                else {
                    $('#DeliveryErrormesage').html('');
                   // $('#DeliveryErrormesage').text("This pincode available for delivery.");
                }

            }
            else {
                if (typee == 1) {
                    $('#DeliveryErrormesage1').text("This pincode not available for delivery");
                }
                else {
                    $('#DeliveryErrormesage').text("This pincode not available for delivery");
                }
            }
        },
    });
}
function OTP() {

    var emailmobile = $('#txtemail').val();

    if (emailmobile != "") {
        $.ajax({
            type: 'POST',
            url: "/Customer/otp",
            data: { emailmobile: emailmobile },
            dataType: 'JSON',
            success: function (result) {
                if (result.message == "OTP") {

                    $("#OPTNOid").val(result.opt);
                    
                    $('#Registraion').css('display', 'none');
                    $('#Login-form').css('display', 'none');
                    $('#OTP').css('display', 'block');
                    //timer(60);
                }
                else {
                    swal({
                        title: "Please try again",
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                    //$("#OPTNOid").val(result.opt);
                    //$('#Registraion').css('display', 'none');
                    //$('#Login-form').css('display', 'none');
                    //$('#OTP').css('display', 'block');
                }
            },
        });
    }
    else {
        swal({
            title: "Please enter the email or mobile",
            text: "",
            icon: "success",
            timer: 10000,
        });
    }
}
function resenotp() {

    var emailmobile = $('#txtemail').val();
    $.ajax({
        type: 'POST',
        url: "/Customer/otp",
        data: { emailmobile: emailmobile },
        dataType: 'JSON',
        success: function (result) {
            if (result.message == "OTP") {

                $("#OPTNOid").val(result.opt);
                //$('#Registraion').css('display', 'none');
                //$('#Login-form').css('display', 'none');
                //$('#OTP').css('display', 'block');
            }
            else {

                swal({
                    title: "Please try again",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
                //$("#OPTNOid").val(result.opt);
                //$('#Registraion').css('display', 'none');
                //$('#Login-form').css('display', 'none');
                //$('#OTP').css('display', 'block');
            }
        },
    });

   

    
}

let timerOn = true;
function timer(remaining) {
    debugger;
    var m = Math.floor(remaining / 60);
    var s = remaining % 60;

    m = m < 10 ? '0' + m : m;
    s = s < 10 ? '0' + s : s;
    document.getElementById('timer').innerHTML = m + ':' + s;
    remaining -= 1;

    if (remaining >= 0 && timerOn) {
        setTimeout(function () {
            timer(remaining);
        }, 1000);
        return;
    }

    if (!timerOn) {
        // Do validate stuff here
        return;
    }

    
    //$("#resend").prop("disabled", false);
 //   $("#CheckOTPBtn").prop("disabled", true);
 //   $("#timer").css("display", "none");
    //  document.getElementById('timer').prop("display", "none");
}


function varfiy()
{
  
    var dd = $('#OPTNOid').val();
    var ddd = $('#OPTNO').val();
    var isValid = true;
    if ($('#OPTNO').val().trim() == "") {

        swal({
            title: "Please Enter your OTP",
            text: "",
            icon: "success",
            timer: 10000,
        });
       
      
    }
    else if (dd != ddd) {
       
        swal({
            title: "OTP entered in incorrect!",
            text: "",
            icon: "success",
            timer: 10000,
        });
        
    }
    else {
        $.ajax({
            type: 'POST',
            url: "/Customer/otpverfiy",
            data: { otp: ddd },
            dataType: 'JSON',
            success: function (result) {
                if (result.message == "Success Login.") {

                    window.location.replace("/CustomerAccount");
                }
                else {

                    swal({
                        title: "Please try again",
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                  
                }
            },
        });
       
       /* $("#OTPerror").text("Please Enter your OTP").hide();*/
    }

   
   
}