
$(document).ready(function () {
    $("#ddlSuCategory").change(function () {
        BindCategory();
    });
    $("#ddlCategory").change(function () {
        BindSubCategory();
    });
    $("#ddlDupProduct").change(function () {
        GetProduct();
    });
});
function BindCategory() {
    var selected_val = $('#ddlSuCategory').find(":selected").attr('value');
    $("#ddlSuCategory").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Admin/Admin/BindCategory', //url to be called
        data: "id=" + selected_val, //data to be send
        success: function (json, result) {
            $("#ddlCategory").empty();
            json = json || {};
            $("#ddlCategory").append('<option value="0">Select Category</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlCategory").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlCategory").prop("disabled", false);
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}
function BindSubCategory() {
    var selected_val = $('#ddlCategory').find(":selected").attr('value');
    $("#ddlCategory").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Admin/Admin/BindSubCategory', //url to be called
        data: "id=" + selected_val, //data to be send
        success: function (json, result) {
            $("#ddlsubCategory").empty();
            json = json || {};
            $("#ddlsubCategory").append('<option value="0">Select SubCategory</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlsubCategory").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            $("#ddlsubCategory").prop("disabled", false);
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}
imagefile.onchange = evt => {
    const [file] = imagefile.files
    if (file) {
        img.src = URL.createObjectURL(file)
    }
}
backimage.onchange = evt => {
    const [file] = backimage.files
    if (file) {
        backimg.src = URL.createObjectURL(file)
    }
}
leftimage.onchange = evt => {
    const [file] = leftimage.files
    if (file) {
        leftimg.src = URL.createObjectURL(file)
    }
}
rightimage.onchange = evt => {
    const [file] = rightimage.files
    if (file) {
        rightimg.src = URL.createObjectURL(file)
    }
}
//function Save() {
//    var fileExtension = ['png', 'img', 'jpg'];
//    var id = $('#id').val();
//    var ItemName = $('#txtItemName').val();
//    var GroupID = $('#txtSuperCategory').val();
//    var CategoryID = $('#txtCategory').val();
//    var SubGroupID = $('#txtSubCategory').val();
//    var BrandID = $('#txtBrandID').val();
//    var HSNCode = $('#txtHSNCode').val();
//    var URLName = $('#txtUrl').val();
//    var SKUCode = $('#txtSkuCode').val();
//    var RegularPrice = $('#txtRegularPrice').val();
//    var SalePrice = $('#txtSalePrice').val();
//    var Stockstatus = $('#txtStockstatus').val();
//    var Weight = $('#txtWeight').val();
//    var D_Length = $('#txtLength').val();
//    var D_Width = $('#txtwidth').val();
//    var D_height = $('#txtheight').val();
//    var ShipCharges = $('#txtShipCharges').val();
//    var bal = $('#hdnBalance').val();
//    var status = $('#chkStatus').is(':checked') ? 1 : 0;
//    var flag = $('#chkApproval').is(':checked') ? 1 : 0;
//    var hdnStock = $('#hdnStock').val();

//    var filename = $('#imagefile').val();
//    if (filename.length == 0) {
//        alert("Please select a file.");
//        return false;
//    }
//    else {
//        var extension = filename.replace(/^.*\./, '');
//        if ($.inArray(extension, fileExtension) == -1) {
//            alert("Please select only PNG/IMG/JPG files.");
//            return false;
//        }
//    }

//    var backfilename = $('#backimage').val();
//    if (backfilename.length == 0) {
//        alert("Please select b file.");
//        return false;
//    }
//    else {
//        var extension1 = backfilename.replace(/^.*\./, '');
//        if ($.inArray(extension1, fileExtension) == -1) {
//            alert("Please select only PNG/IMG/JPG files.");
//            return false;
//        }
//    }

//    var leftfilename = $('#leftimage').val();
//    if (leftfilename.length == 0) {
//        alert("Please select a file.");
//        return false;
//    }
//    else {
//        var extension2 = leftfilename.replace(/^.*\./, '');
//        if ($.inArray(extension2, fileExtension) == -1) {
//            alert("Please select only PNG/IMG/JPG files.");
//            return false;
//        }
//    }

//    var rightfilename = $('#rightimage').val();
//    if (rightfilename.length == 0) {
//        alert("Please select f file.");
//        return false;
//    }
//    else {
//        var extension3 = rightfilename.replace(/^.*\./, '');
//        if ($.inArray(extension3, fileExtension) == -1) {
//            alert("Please select only PNG/IMG/JPG files.");
//            return false;
//        }
//    }
  
//    var ProductDesc = CKEDITOR.instances["txtProductDesc"].getData();
//    var AddInformation = CKEDITOR.instances["AddInformation"].getData();
//    var Ingredients = CKEDITOR.instances["Ingredients"].getData();
//    var ButtonValue = $('#btnSave').val();

//    var fdata = new FormData();
//    var fileUpload = $("#imagefile").get(0);
//    var file = fileUpload.files;
//    fdata.append(file[0].name, file[0]);

//    var fileUploadback = $("#backimage").get(0);
//    var backfiles = fileUploadback.files;
//    fdata.append(backfiles[0].name, backfiles[0]);

//    var leftfileUpload = $("#leftimage").get(0);
//    var leftfiles = leftfileUpload.files;
//    fdata.append(leftfiles[0].name, leftfiles[0]);

//    var rightfileUpload = $("#rightimage").get(0);
//    var rightfiles = rightfileUpload.files;
//    fdata.append(rightfiles[0].name, rightfiles[0]);

  
//    fdata.append("id", id);
//    fdata.append("ItemName", ItemName);
//    fdata.append("URLName", URLName);
//    fdata.append("GroupID", GroupID);
//    fdata.append("CategoryID", CategoryID);
//    fdata.append("SubGroupID", SubGroupID);
//    fdata.append("BrandID", BrandID);
//    fdata.append("HSNCode", HSNCode);
//    fdata.append("SKUCode", SKUCode);
//    fdata.append("RegularPrice", RegularPrice);
//    fdata.append("SalePrice", SalePrice);
//    fdata.append("Stockstatus", Stockstatus);
//    fdata.append("Weight", Weight);
//    fdata.append("D_Length", D_Length);
//    fdata.append("D_Width", D_Width);
//    fdata.append("D_height", D_height);
//    fdata.append("ShipCharges", ShipCharges);
//    fdata.append("ProductDesc", ProductDesc);
//    fdata.append("AddInformation", AddInformation);
//    fdata.append("Ingredients", Ingredients); 
//    fdata.append('balance', bal);
//    fdata.append('status', status);
//    fdata.append('flag', flag);
//    fdata.append('hdnStock', hdnStock);

//    $.ajax({
//        type: "POST",
//        url: '/Admin/Admin/SaveApprove',
//        beforeSend: function (xhr) {
//            xhr.setRequestHeader("XSRF-TOKEN",
//                $('input:hidden[name="__RequestVerificationToken"]').val());
//        },
//        data: fdata,
//        contentType: false,
//        processData: false,
//        // data: { id: id, SubCategory: SubCategory, SuperCategoryId: SuperCategoryId, CategoryId: CategoryId, Category: Category, Status: Status },
//        //dataType: "JSON",
//        success: function (result) {
//            if (result.message == "Item Approve added") {
//                if (result.id == 0) {
//                    alert("Item Approve added successfully.");
//                }
//                else {
//                    alert("Item Approve modify successfully.");
//                }
//                ClearField();
//                ProductList();
//            }
//        },
//        error: function () {
//            alert("Item Approve not added.");
//        }
//    });
//}
$(document).ready(function () {
    CKEDITOR.replace('#txtProductDesc');
    CKEDITOR.replace('#AddInformation');
    CKEDITOR.replace('#Ingredients');
});
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'NewProduct');
}

function Approve(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/Admin/UpdateApprove',
        data: { id: id },
        dataType:"JSON",
        success: function (result) {
            if (result.message == "Item Approve Successfully.") {
                alert("Item approve Successfully.");
                ItemApproveList();
            }
        },
        error: function () {
            alert("Item not approve.");
        }
    });
}
function Notapprove(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/Admin/NotApproveStatus',
        data: { id: id },
        dataType: "JSON",
        success: function (result) {
            if (result.message == "Item not Approve.") {
                alert("Item not Approve.");
                ClearField();
                ItemApproveList();
            }
        },
        error: function () {
            alert("some thing error.");
        }
    });
}
function ItemApproveList() {
    $.ajax({
        url: "/Admin/Admin/ShowItemApproveList",
        dataType: 'JSON',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td>' + item.id + '</td>';
                html += '<td>' + item.itemName + '</td>';
                //html += '<td style="display:none">' + item.stockStatus + '</td>';
                //html += '<td style="display:none">' + item.mrp + '</td>';
                if ((item.status) == true) {
                    html += '<td class="active">Approval</td>';
                } else {
                    html += '<td class="active">Waiting Approval</td>';
                }
                html += '<td><a class="btn btn-sm" style="color: white;background: #3bb77e;" href="/Admin/Admin/Approve?id=' + item.id + '">View</a></td>';
                html += '<td><a class="btn btn-sm" href="#" style="color: white;background: #3bb77e;" onclick="return Approve('+item.id+')">Approve</a></td>';
                html += '<td style="width:15%;"><a class="btn btn-sm" style="color: white;background: #3bb77e;" href="#" onclick="return Notapprove(' + item.id + ')">Not Approve</a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
        }
    });
} 
function Editbyid(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditProducr",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val = "UPDATE";
            $('#id').val(result.id);
            $('#txtProductName').val(result.itemName);
            $('#ddlDupProduct').find("option:selected").val();
            $('#txtUrl').val(result.uRLName);
            $('#ddlSuCategory').find("option:selected").text();
            $('#ddlCategory').find("option:selected").text();
            $('#img').attr('src', "/images/Productimage/" + result.image + "");
            $('#ddlsubCategory').find("option:selected").text();
            $('#txtProductTag').val(result.productTag);
            $('#ddlBrand').find("option:selected").text();
            $('#ddlHSN').find("option:selected").val();
            $('#ddlItemType').find("option:selected").val();
            $('#txtSkuCode').html(result.sKUCode);
            $('#txtRegularPrice').html(result.costPrice);
            $('#txtSalePrice').html(result.mRP);
            $('#txtStockstatus').html(result.stockStatus);
            $('#txtWeight').html(result.dimension);
            $('#txtLength').html(result.dimension);
            $('#txtwidth').html(result.dimension);
            $('#txtShipCharges').html(result.chipCharge);
            $('#Featuredimage').html(result.image);
            $('#BackImage').html(result.image1);
            $('#Leftimage').html(result.image2);
            $('#RightImage').html(result.image3);
            $('#txtProductDesc').html(result.description);
            $('#AddInformation').html(result.additional);
            $('#Ingredients').html(result.ingredients);
            $('#chkStatus').val(result.welfarestatus);
        },
        error: function () {
            alert("Data Not Founf !!");
        }
    });
}

function DeletebyId(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteProductList",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    alert("Delete Success");
                    ProductList();
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

function ClearField() {
    $('#id').val("0");
    $('#txtProductName').val("");
    $('#ddlDupProduct').find("option:selected").val(0);
    //$('#ddlDupProduct').find("option:selected").text();
    $('#Url').val("");
    $('#ddlSuCategory').find("option:selected").val(0);
    //$('#ddlSuCategory').find("option:selected").text();
    $('#ddlCategory').find("option:selected").val();
    //$('#ddlCategory').find("option:selected").text();
    $('#ddlsubCategory').find("option:selected").val(0);
    //$('#ddlsubCategory').find("option:selected").text();
    $('#txtProductTag').val("");
    $('#ddlBrand').find("option:selected").val(0);
    // var BrandText = $('#ddlBrand').find("option:selected").text();
    $('#ddlHSN').find("option:selected").val(0);
    //var HSNText = $('#ddlHSN').find("option:selected").text();
    var ItemTypeval = $('#ddlItemType').find("option:selected").val(0);
    //var ItemTypeText = $('#ddlItemType').find("option:selected").text();
    $('#txtSkuCode').val("");
    $('#txtRegularPrice').val("");
    $('#txtSalePrice').val("");
    $('#txtStockstatus').val("");
    $('#txtWeight').val("");
    $('#ddlweight').find("option:selected").val(0);
    //    var weightText = $('#ddlweight').find("option:selected").text();
    $('#txtLength').val("");
    $('#txtwidth').val("");
    $('#txtheight').val("");
    $('#txtShipCharges').val("");
    $('#Featuredimage').val("");
    $('#hdnBalance').val("0");
    $('#hdnStock').val("0");
}

function GetProduct() {
    var selected_val = $('#ddlDupProduct').find(":selected").attr('value');
    $("#ddlDupProduct").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: '/Admin/Admin/GetAllProduct', //url to be called
        data: "id=" + selected_val, //data to be send
        type: 'POST',
        success: function (result) {
            $('#id').val();
            $('#txtProductName').val(result.id);
            //$('#ddlDupProduct').find("option:selected").val();
            //$('#ddlDupProduct').find("option:selected").text();
            $('#Url').val(result.uRLName);
            $('#ddlSuCategory').find("option:selected").val(result.groupID);
            //$('#ddlSuCategory').find("option:selected").text();
            $('#ddlCategory').find("option:selected").val(result.categoryID);
            //$('#ddlCategory').find("option:selected").text();
            $('#ddlsubCategory').find("option:selected").val(result.subGroupID);
            //$('#ddlsubCategory').find("option:selected").text();
            $('#txtProductTag').val(result.productTag);
            $('#ddlBrand').find("option:selected").val(result.brandID);
            // var BrandText = $('#ddlBrand').find("option:selected").text();
            $('#ddlHSN').find("option:selected").val(result.hsnCode);
            //var HSNText = $('#ddlHSN').find("option:selected").text();
            $('#ddlItemType').find("option:selected").val(0);
            //var ItemTypeText = $('#ddlItemType').find("option:selected").text();
            $('#txtSkuCode').val(result.sKUCode);
            $('#txtRegularPrice').val(result.costPrice);
            $('#txtSalePrice').val(result.mrp);
            $('#txtStockstatus').val(result.stockStatus);
            $('#txtWeight').val(result.weight);
            //$('#ddlweight').find("option:selected").val();
            //    var weightText = $('#ddlweight').find("option:selected").text();
            $('#txtLength').val("");
            $('#txtwidth').val("");
            $('#txtheight').val("");
            $('#txtShipCharges').val(result.shipCharge);
            $('#Featuredimage').val("");
            $('#hdnBalance').val(0);
            $('#hdnStock').val(0);
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}
