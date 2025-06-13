imagefile.onchange = evt => {
    const [file] = imagefile.files
    if (file) {
        img.src = URL.createObjectURL(file)
    }
}
function Save() {
    var Comp_Id = $('#Comp_Id').val();
    var CompanyName = $('#txtCompName').val();
    var ShortName = $('#txtshortName').val();
    var EmailID = $('#txtEmail').val();
    var PhoneNo = $('#txtPhone').val();
    var Comp_Address = $('#txtAddress').val();
    var MobileNo = $('#txtMobile').val();
    var CityName = $('#txtCity').val();
    var FaxNo = $('#txtFax').val();
    var GstNo = $('#txtGst').val();
    var PinCode = $('#txtPinCode').val();
    var webs = $('#txtWebsite').val();
    var TinNo = $('#txtTinNo').val();
    var domainname = $('#txtdomainname').val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var fileExtension = ['png', 'img', 'jpg'];
    var filename = $('#imagefile').val();
    if (filename.length == 0) {
        alert("Please select a file.");
        return false;
    }
    else {
        var extension = filename.replace(/^.*\./, '');
        if ($.inArray(extension, fileExtension) == -1) {
            alert("Please select only PNG/IMG/JPG files.");
            return false;
        }
    }
    var fdata = new FormData();
    var fileUpload = $("#imagefile").get(0);
    var files = fileUpload.files;
    fdata.append(files[0].name, files[0]);
    fdata.append("Comp_Id", Comp_Id);
    fdata.append("CompanyName", CompanyName);
    fdata.append("ShortName", ShortName);
    fdata.append("TinNo", TinNo);
    fdata.append("EmailID", EmailID);
    fdata.append("PhoneNo", PhoneNo);
    fdata.append("Comp_Address", Comp_Address);
    fdata.append("MobileNo", MobileNo);
    fdata.append("CityName", CityName);
    fdata.append("FaxNo", FaxNo);
    fdata.append("GstNo", GstNo);
    fdata.append("PinCode", PinCode);
    fdata.append("webs", webs);
    fdata.append("domainname", domainname);
    fdata.append("Status", Status);
    $.ajax({
        type: "POST",
        url: '/Admin/SuperAdmin/SaveCompanyMasters',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: fdata,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result.message == "Company Master added") {
                if (result.id == 0) {
                    alert("Company Master added successfully.");
                }
                else {
                    alert("Company Master modify successfully.");
                }
                $('#Comp_Id').val("0");
                ShowDataInTable();
            }
        },
        error: function () {
            alert("Company Master not added.");
        }
    });
}
function Clear() {
    ReloadPageWithAreas('Admin', 'SuperAdmin', 'CompanyMasters');
}
function ShowDataInTable() {
    //  var item;
    $.ajax({
        url: "/Admin/SuperAdmin/AllCompany",
        //    data: JSON.stringify(item),
        dataType: 'JSON',
        type: 'GET',
        // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr>';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.comp_Id + '</td>';
                html += '<td>' + item.companyName + '</td>';
                html += '<td>' + item.shortName + '</td>';
                html += '<td>' + item.domainname + '</td>';
                html += '<td><img src="/images/CompanyLogo' + item.logo + '"  width="75" height="80" /></td>';
                html += '<td>' + item.emailID + '</td>';
                html += '<td>' + item.phoneNo + '</td>';
                html += '<td>' + item.comp_Address + '</td>';
                html += '<td>' + item.mobileNo + '</td>';
                html += '<td>' + item.webs + '</td>';
                //html += '<td>' + item.comp_status + '</td>';
                if ((item.comp_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                // html += '<td><a style="width:auto; padding: 10px 15px;" class="a" href="#" onclick="return EditCompany(' + item.comp_Id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a style="width:auto; padding: 10px 15px;" class="a" href="#" onclick="return DeleteCompany(' + item.comp_Id + ')"><i class="fa fa-trash"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Comptbody').html(html);
        }
    });
}
$("#example1").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var comp_Id = currentRow.find("td:eq(0)").html();
    var comp_status = currentRow.find("td:eq(10)").html();
    //alert(comp_status);
    Editbyid(comp_Id, comp_status);
});
function Editbyid(comp_Id, comp_status) {
    // var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ comp_Id: comp_Id });
    // var item;
    $.ajax({
        url: "/Admin/SuperAdmin/EditCompany",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#Comp_Id').val(result.comp_Id);
            $('#ddlPrintingId').val(result.printingType);
            $('#txtCompName').val(result.companyName);
            $('#txtshortName').val(result.shortName);
            $('#txtdomainname').val(result.domainname);
            $('#txtEmail').val(result.emailID);
            $('#txtPhone').val(result.phoneNo);
            $('#txtAddress').val(result.comp_Address);
            $('#txtMobile').val(result.mobileNo);
            $('#txtCity').val(result.cityName);
            $('#txtFax').val(result.faxNo);
            $('#txtGst').val(result.gstNo);
            $('#txtPinCode').val(result.pinCode);
            $('#txtWebsite').val(result.webs);
            $('#txtTinNo').val(result.tinNo);
            $('#img').attr('src', "/images/CompanyLogo" + result.logo + "");
            if ((comp_status) == "Active") {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
            $('#divUpdate').show();
            $('#divSave').hide();
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}
function DeleteCompany(comp_Id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: comp_Id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/SuperAdmin/DeleteCompany",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    alert("Delete Success");
                    ShowDataInTable();
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
    else {
        return false;
    }
}

