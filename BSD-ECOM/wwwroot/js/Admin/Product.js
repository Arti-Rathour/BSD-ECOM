var Featuredfilenameorginal = "";
var Backfilenameorginal = "";
var Leftfilenameorginal = "";
var Rightfilenameorginal = "";
var filename1orginal = "";
var pdf1orginal = "";
Featuredimage.onchange = evt => {
    const [file] = Featuredimage.files
    if (file) {
        imgfront.src = URL.createObjectURL(file);

    }
}

BackImage.onchange = evt => {
    const [file] = BackImage.files
    if (file) {
        imgBack.src = URL.createObjectURL(file);
    }
}

Leftimage.onchange = evt => {
    const [file] = Leftimage.files
    if (file) {
        imgLeft.src = URL.createObjectURL(file);
    }
}

RightImage.onchange = evt => {
    const [file] = RightImage.files
    if (file) {
        imgRight.src = URL.createObjectURL(file);
    }
}







$(document).ready(function () {


    filename1orginal = $('#imgfile1name').val();
    pdf1orginal = $('#pdf1name').val();
    Featuredfilenameorginal = $('#Featuredfilename').val();
    Backfilenameorginal = $('#Backfilename').val();
    Leftfilenameorginal = $('#Leftfilename').val();
    Rightfilenameorginal = $('#Rightfilename').val();
    ItemDetailsonEdits();

    $("#Featuredimage").change(function () {

        Featuredfilenameorginal = $("#Featuredimage").val();

        // alert(Featuredfilenameorginal);
    })
    $("#BackImage").change(function () {
        Backfilenameorginal = $("#BackImage").val();
    })
    $("#Leftimage").change(function () {
        Leftfilenameorginal = $("#Leftimage").val();
    })
    $("#RightImage").change(function () {
        Rightfilenameorginal = $("#RightImage").val();
        //alert(Rightfilenameorginal);
    })

    $("#imgfile1").change(function () {
        filename1orginal = $("#imgfile1").val();
        //alert(Rightfilenameorginal);
    })

    $("#pdf").change(function () {
        pdf1orginal = $("#pdf").val();
        
    })
});



var i = 0; currentRow = null;
$(document).ready(function () {
    CKEDITOR.replace('#txtProductDesc');
    CKEDITOR.replace('#AddInformation');
    CKEDITOR.replace('#Ingredients');
});
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'NewProduct');
}




function Save() {
    var fileExtension = ['png', 'img', 'jpg', 'mp4', 'pdf'];
    var id = $('#id').val();
    var ProductName = $('#txtProductName').val();
    var DupProductval = $('#ddlDupProduct').find("option:selected").val();
    var DupProductText = $('#ddlDupProduct').find("option:selected").text();
    var Url = $('#txtUrl').val();
    var SuperCategoryVal = $('#ddlSuCategory').find("option:selected").val();
    var SuperCategoryText = $('#ddlSuCategory').find("option:selected").text();
    var CategoryVal = $('#ddlCategory').find("option:selected").val();
    var CategoryText = $('#ddlCategory').find("option:selected").text();
    var SubCategoryVal = $('#ddlsubCategory').find("option:selected").val();
    var SubCategoryText = $('#ddlsubCategory').find("option:selected").text();
    var ProductTag = $('#txtProductTag').val();
    var BrandVal = $('#ddlBrand').find("option:selected").val();
    var BrandText = $('#ddlBrand').find("option:selected").text();
    var HSNVal = $('#ddlHSN').find("option:selected").val();
    var HSNText = $('#ddlHSN').find("option:selected").text();
    var ItemTypeval = $('#ddlItemType').find("option:selected").val();
    var ItemTypeText = $('#ddlItemType').find("option:selected").text();
    var SKUCode = $('#txtSkuCode').val();
    var RegularPrice = $('#txtRegularPrice').val();
    var SalePrice = $('#txtSalePrice').val();
    var Stockstatus = $('#txtStockstatus').val();
    var Weight = $('#txtWeight').val();
    var weightval = $('#ddlweight').find("option:selected").val();
    var weightText = $('#ddlweight').find("option:selected").text();
    var D_Length = $('#txtLength').val();
    var D_Width = $('#txtwidth').val();
    var D_height = $('#txtheight').val();
    var ShipCharges = $('#txtShipCharges').val();
    var Featuredimage = $('#Featuredimage').val();
    var BackImage = $('#BackImage').val();
    var Leftimage = $('#Leftimage').val();
    var RightImage = $('#RightImage').val();

    var filename1 = $('#imgfile1').val();
    var pdf1 = $('#pdf').val();
    //var filename1 = $('#imgfiles1').val();

    var bal = $('#hdnBalance').val();
    var hdnStock = $('#hdnStock').val();
    var Active = $('#txtcheckbox').is(':checked') ? 1 : 0;
    var Send_query = $('#txtsendcheck').is(':checked') ? 1 : 0;
    var Price = $('#txtpricecheck').is(':checked') ? 1 : 0;

    var ButtonValue = $('#btnSave').val();
    var fdata = new FormData();
    if (Featuredfilenameorginal == Featuredimage) {
        if (Featuredimage.length == 0) {
            swal({
                title: "Please select Featured image.",
                text: "",
                iicon: "success",
                timer: 10000,
            });
            //alert("Please select Featured image.");
            return false;
        }
        else {
            var extension = Featuredimage.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                swal({
                    title: "Please select only PNG/IMG/JPG files.",
                    text: "",
                    iicon: "success",
                    timer: 10000,
                });
                // alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("Featuredimageflg", "okg");
        var FeaturedimageUpd = $('#Featuredimage').get(0);
        var files = FeaturedimageUpd.files;
        fdata.append(files[0].name, files[0]);
    }
    else {
        fdata.append("Featuredimageflg", "ok");
    }

    if (Backfilenameorginal == BackImage) {
        if (BackImage.length == 0) {
            alert("Please select Back Image.");
            return false;
        }
        else {
            var extension = BackImage.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                swal({
                    title: "Please select only PNG/IMG/JPG files.",
                    text: "",
                    iicon: "success",
                    timer: 10000,
                });
                // alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("BackImageflg", "okg");
        var BackImageUpd = $('#BackImage').get(0);
        var bImage = BackImageUpd.files;
        fdata.append(bImage[0].name, bImage[0]);
    }
    else {
        fdata.append("BackImageflg", "ok");
    }

    if (Leftfilenameorginal == Leftimage) {
        if (Leftimage.length == 0) {
            alert("Please select Left image.");
            return false;
        }
        else {
            var extension = Leftimage.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                swal({
                    title: "Please select only PNG/IMG/JPG files.",
                    text: "",
                    iicon: "success",
                    timer: 10000,
                });
                //alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("Leftimageflg", "okg");
        var Leftimageupd = $('#Leftimage').get(0);
        var limage = Leftimageupd.files;
        fdata.append(limage[0].name, limage[0]);
    }
    else {
        fdata.append("Leftimageflg", "ok");
    }

    if (Rightfilenameorginal == RightImage) {
        if (RightImage.length == 0) {
            swal({
                title: "Please select Right Image.",
                text: "",
                iicon: "success",
                timer: 10000,
            });
            //alert("Please select Right Image.");
            return false;
        }
        else {
            var extension = RightImage.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                swal({
                    title: "Please select only PNG/IMG/JPG files.",
                    text: "",
                    iicon: "success",
                    timer: 10000,
                });
                // alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("RightImageflg", "okg");
        var RightImageupd = $('#RightImage').get(0);
        var rImage = RightImageupd.files;
        fdata.append(rImage[0].name, rImage[0]);
    }
    else {
        fdata.append("RightImageflg", "ok");
    }

    if (filename1orginal != '' || filename1 != '') {

        if (filename1orginal == filename1) {

            var extension = filename1.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                swal({
                    title: "Please select only MP4 files.",
                    text: "",
                    iicon: "success",
                    timer: 10000,
                });

                return false;
            }

            fdata.append("filename1Imageflg", "okg");
            var filename1upd = $('#imgfile1').get(0);
            var filename1Image = filename1upd.files;
            fdata.append(filename1Image[0].name, filename1Image[0]);
        }
        else {
            fdata.append("filename1Imageflg", "ok");
        }


    }
    else {
        fdata.append("filename1Imageflg", "ok");
    }



    if (pdf1orginal == pdf1) {

        var extension = pdf1.replace(/^.*\./, '');
        if ($.inArray(extension, fileExtension) == -1) {
            swal({
                title: "Please select only pdf files.",
                text: "",
                iicon: "success",
                timer: 10000,
            });

            return false;
        }

        fdata.append("pdf1Imageflg", "okg");
        var pdf1upd = $('#pdf').get(0);
        var pdf1Image = pdf1upd.files;
        fdata.append(pdf1Image[0].name, pdf1Image[0]);
    }
    else {
        fdata.append("pdf1Imageflg", "ok");
    }


    var ProductDesc = CKEDITOR.instances["txtProductDesc"].getData();
    var AddInformation = CKEDITOR.instances["AddInformation"].getData();
    var Ingredients = CKEDITOR.instances["Ingredients"].getData();
    fdata.append("id", id);
    fdata.append("ProductName", ProductName);
    fdata.append("DupProductval", DupProductval);
    fdata.append("Url", Url);
    fdata.append("SuperCategoryVal", SuperCategoryVal);
    fdata.append("CategoryVal", CategoryVal);
    fdata.append("SubCategoryVal", SubCategoryVal);
    fdata.append("ProductTag", ProductTag);
    fdata.append("BrandVal", BrandVal);
    fdata.append("HSNVal", HSNVal);
    fdata.append("ItemTypeval", ItemTypeval);
    fdata.append("SKUCode", SKUCode);
    fdata.append("RegularPrice", RegularPrice);
    fdata.append("SalePrice", SalePrice);
    fdata.append("Stockstatus", Stockstatus);
    fdata.append("Weight", Weight);
    fdata.append("weightval", weightval);
    fdata.append("D_Length", D_Length);
    fdata.append("D_Width", D_Width);
    fdata.append("D_height", D_height);
    fdata.append("ShipCharges", ShipCharges);
    fdata.append("ProductDesc", ProductDesc);
    fdata.append("AddInformation", AddInformation);
    fdata.append("Ingredients", Ingredients);
    fdata.append('balance', bal);
    fdata.append('hdnStock', hdnStock);



    fdata.append("Featuredfilename", Featuredfilenameorginal);
    fdata.append("Backfilename", Backfilenameorginal);
    fdata.append("Leftfilename", Leftfilenameorginal);
    fdata.append("Rightfilename", Rightfilenameorginal);

    fdata.append("filename1Image", filename1orginal);
    fdata.append("pdf1Image", pdf1orginal);

    fdata.append('Active', Active);
    fdata.append('Send_query', Send_query);
    fdata.append('Price', Price);

    var Units = new Array();
    $("#example1 tbody tr").each(function () {
        var row = $(this);
        var unit = {};
        unit.UnitId = row.find("span.unitId").text();
        unit.Qty = row.find('span.Quantity').text();
        unit.discount = row.find('span.Discount').text();
        unit.unitrate = row.find('span.unitrate').text();
        unit.stockQuantity = row.find('span.StockQuantity').text();
        unit.shippingCharge = row.find('span.ShippingCharge').text();
        unit.id = row.find('span.id').text();
        Units.push(unit);
    });
    var unitjson = JSON.stringify(Units)
    fdata.append('json', unitjson);
    var len = Units.length
    //for (len = 0; len > length; len++) {
    //    fdata.append('UnitId', Units[len]['UnitId']);
    //    fdata.append('Qty', Units[len]['Qty']);
    //    fdata.append('discount', Units[len]['discount']);
    //    fdata.append('unitrate', Units[len]['unitrate']);
    //}  
    var msg = ValidationProduct();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/SaveNewProduct',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: fdata,
            contentType: false,
            processData: false,
            // data: { id: id, SubCategory: SubCategory, SuperCategoryId: SuperCategoryId, CategoryId: CategoryId, Category: Category, Status: Status },
            //dataType: "JSON",
            success: function (result) {
                if (result.message == "NewProduct added.") {
                    if (result.id == 0) {
                        swal({
                            title: "New Product added successfully.",
                            text: "",
                            icon: "success",
                            timer: 10000,
                        });
                        /*window.reload();*/
                        // alert("New Product added successfully.");
                    }
                    else {
                        swal({
                            title: "Product modify successfully.",
                            text: "",
                            iicon: "success",
                            timer: 10000,
                        });
                        // alert("Product modify successfully.");
                    }
                    ClearField();
                    Clear();
                    //ProductList();
                }
                else if (result.message == "Name Already Exist") {
                    swal({
                        title: "Product Name Already Exist",
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                }
            },
            error: function () {
                swal({
                    title: "Product not Added.",
                    text: "",
                    icon: "",
                    timer: 10000,
                });
                // alert("Product not Added.");
            }
        });
    }
    else {
        swal({
            title: msg,
            text: "",
            icon: "",
            timer: 10000,
        });
        // alert(msg);
        // ValidationProduct();
    }
}
$(document).ready(function () {
    ProductList();
    clearItemType1();
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
function ProductList() {
    var searchitem = $("#txtsearch").val();
    var statusactive = $("#ddlactive").val();

    $.ajax({
        url: "/Admin/Admin/ShowProductList",
        data: { searchitem: searchitem, statusactive: statusactive },
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td><img src="/images/Productimage/6/' + item.image + '" alt="your image" width="75" height="80" /></td>';
                html += '<td>' + item.itemName + '</td>';
                //html += '<td>' + item.stockStatus + '</td>';

                html += '<td style="display:none">' + item.groupID + '</td>';
                html += '<td>' + item.main_cat_name + '</td>';
                html += '<td style="display:none">' + item.categoryID + '</td>';
                html += '<td>' + item.category_name + '</td>';
                html += '<td style="display:none">' + item.subGroupID + '</td>';
                html += '<td>' + item.subCategory_name + '</td>';
                html += '<td>' + item.productTag + '</td>';
                html += '<td>' + item.unit_Rate + '</td>';
                html += '<td>' + item.date + '</td>';
                //html += '<td>' + item.Active + '</td>';
                if (item.active == 1) {
                    html += '<td>Active</td>';
                }
                else {
                    html += '<td>InActive</td>';
                }
                html += '<td><a class="btn btn-sm" href="/Admin/Admin/EditProduct?id=' + item.id + '"><i class="fa fa-edit"></i></a></td>';
                /* html += '<td><a class="btn btn-sm" href="/Admin/Admin/NewProduct?id=' + item.id + '"><i class="fa fa-edit"></i></a></td>';*/
                //html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyid(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);
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
    $('#ddlDupProduct').val("0");
    //$('#ddlDupProduct').find("option:selected").text();
    $('#txtUrl').val("");
    $('#ddlSuCategory').val("0");
    //$('#ddlSuCategory').find("option:selected").text();
    $('#ddlCategory').val("0");
    //$('#ddlCategory').find("option:selected").text();
    $('#ddlsubCategory').val("0");
    //$('#ddlsubCategory').find("option:selected").text();
    $('#txtProductTag').val("");
    $('#ddlBrand').val("0");
    // var BrandText = $('#ddlBrand').find("option:selected").text();
    $('#ddlHSN').val("0");
    //var HSNText = $('#ddlHSN').find("option:selected").text();
    $('#ddlItemType').val("0");
    //var ItemTypeText = $('#ddlItemType').find("option:selected").text();
    $('#txtSkuCode').val("");
    $('#txtRegularPrice').val("");
    $('#txtSalePrice').val("");
    $('#txtStockstatus').val("");
    $('#txtWeight').val("");
    $('#ddlweight').val("0");
    //    var weightText = $('#ddlweight').find("option:selected").text();
    $('#txtLength').val("");
    $('#txtwidth').val("");
    $('#txtheight').val("");
    $('#txtShipCharges').val("");
    $('#Featuredimage').val("");
    $('#hdnBalance').val("0");
    $('#hdnStock').val("0");
    CKEDITOR.instances["txtProductDesc"].setData("");
    CKEDITOR.instances["AddInformation"].setData("")
    CKEDITOR.instances["Ingredients"].setData("");


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
function itemdetails() {
    var unitId = $('#ddlItemType').find("option:selected").val();
    if (unitId == 0) {
        alert("Please select Item Type.");
    }
    else {
        var UnitName = $('#ddlItemType').find("option:selected").text();
        $('#unitId').val(unitId);
        $('#txtUnitName').val(UnitName);
        $('#quickViewModal').modal('show');
    }
}

//$(document).ready(function () {

function saveitem() {


    //$("#savebutton").on("click", function () {
    var rowno = $('#Index').val();
    var unitId = $('#unitId').val();
    var unitName = $('#txtUnitName').val();
    var Quantity = $('#txtQuantity').val();
    var Discount = $('#txtDiscount').val();
    var unitrate = $('#txtunitrate').val();
    var StockQuantity = $('#txtStockQty').val();
    var ShippingCharge = $('#txtshipping').val();
    var msg = validitemdetails();
    if (msg == "") {
        i++;
        var new_row = "<tr id='row" + i + "'><td style='display: none'><span class='rowno'>" + i + "</span></td><td style='display: none'><span class='unitId'>" + unitId + "</span></td><td style='display: none'><span class='unitName'>" + unitName + "</span></td><td><span class='Quantity'>" + Quantity + "</span></td><td><span class='Discount'>" + Discount + "</span></td><td><span class='unitrate'>" + unitrate + "</span></td><td><span class='StockQuantity'>" + StockQuantity + "</span></td><td><span class='ShippingCharge'>" + ShippingCharge + "</span></td><td><a class='editrow' style='margin-right: 10px; margin-left: 10px'><i class='fa fa-edit'></i></a></td><td><a class='deleterow' style='margin-right: 10px; margin-left: 10px'><i class='fa fa-trash-o'></i></a></td></tr>";
        if (currentRow) {
            $("table tbody.tbodyUnit").find($(currentRow)).replaceWith(new_row);

        }
        else {
            $("table tbody.tbodyUnit").append(new_row);
            currentRow = null;
            clearItemType();
            $("#close").trigger('click');
        }
    }
    else {
        swal({
            title: msg,
            text: "",
            iicon: "",
            timer: 10000,
        });
    }
    var rowCount = document.getElementById("example1").rows.length - 1;
    //});
}
//});




function delrow(id) {

    var PetObj = JSON.stringify({ id: id });

    $.ajax({
        url: "/Admin/Admin/DeleteUNITTYPE",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.msg == "Delete Successfull!!") {

                ItemDetailsonEdit();
                //return false;
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}

function delrowbyid(id) {

    var PetObj = JSON.stringify({ id: id });

    $.ajax({
        url: "/Admin/Admin/DeleteUNITTYPE",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.msg == "Delete Successfull!!") {

                ItemDetailsonEdit();
                //return false;
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}
$(document).on('click', 'a.deleterow', function () {
    $(this).parents('tr').remove();
    return false;
});

$("#example1").on('click', '.editrow', function () {
    currentRow = $(this).parents('tr');
    var tablecurrentrow = $(this).closest('tr');
    var rowno = tablecurrentrow.find('span.rowno').text();
    var unitId = tablecurrentrow.find('span.unitId').text();
    var unitName = tablecurrentrow.find('span.unitName').text();
    var Quantity = tablecurrentrow.find('span.Quantity').text();
    var Discount = tablecurrentrow.find('span.Discount').text();
    var unitrate = tablecurrentrow.find('span.unitrate').text();
    var StockQuantity = tablecurrentrow.find('span.StockQuantity').text();
    var ShippingCharge = tablecurrentrow.find('span.ShippingCharge').text();
    var id = tablecurrentrow.find('span.id').text();

    $("#savebutton").hide();
    $("#Updatebutton").show();
    $('#Index').val(rowno);
    $('#unitId').val(unitId);
    $('#txtUnitName').val(unitName);
    $('#txtQuantity').val(Quantity);
    $('#txtDiscount').val(Discount);
    $('#txtunitrate').val(unitrate);
    $('#txtStockQty').val(StockQuantity);
    $('#txtshipping').val(ShippingCharge);
    $('#txthiddenitemid').val(id);
    //    edititemtype(rowno,unitId, unitName, Quantity, Discount, unitrate, StockQuantity, ShippingCharge);
});



$("#Updatebutton").click(function () {
    //$('#example1').empty();
    var rowno = $('#Index').val(); 
    var unitId = $('#unitId').val();
    var unitName = $('#txtUnitName').val();
    var Quantity = $('#txtQuantity').val();
    var Discount = $('#txtDiscount').val();
    var unitrate = $('#txtunitrate').val();
    var StockQuantity = $('#txtStockQty').val();
    var ShippingCharge = $('#txtshipping').val();
    var id = $('#txthiddenitemid').val();
    var msg = validationItemType1();
    if (msg == "") {
        i++;
        var new_row = "<tr id='row" + i + "'><td style='display: none'><span class='rowno'>" + i + "</span></td><td style='display: none'><span class='unitId'>" + unitId + "</span></td><td style='display: none'><span class='id'>" + id + "</span></td><td style='display: none'><span class='unitName'>" + unitName + "</span></td><td><span class='Quantity'>" + Quantity + "</span></td><td><span class='Discount'>" + Discount + "</span></td><td><span class='unitrate'>" + unitrate + "</span></td><td><span class='StockQuantity'>" + StockQuantity + "</span></td><td><span class='ShippingCharge'>" + ShippingCharge + "</span></td><td><a class='editrow' style='margin-right: 10px; margin-left: 10px'><i class='fa fa-edit'></i></a></td><td><a class='deleterow' style='margin-right: 10px; margin-left: 10px'><i class='fa fa-trash-o'></i></a></td></tr>";
        if (currentRow) {
            $("table tbody.tbodyUnit").find($(currentRow)).replaceWith(new_row);
        }
        else {
            $("table tbody.tbodyUnit").append(new_row);
            currentRow = null;
            clearItemType();
            $("#close").trigger('click');
        }
        //$("table tbody.tbodyUnit").find($(currentRow)).replaceWith(new_row);
        //currentRow = null;
    }
    else {
        swal({
            title: msg,
            text: "",
            iicon: "",
            timer: 10000,
        });
    }

   
    //$("#savebutton").show();
    $('#quickViewModal').modal('hide');
});
function ValidationProduct() {
    var msg = "";
    var Price = $('#txtpricecheck').prop('checked') ? 1 : 0;
    if (Price == 1) {
        if ($('#txtProductName').val() == "") { msg += "Product name can not left Blank !! \n"; }
        //if ($('#ddlDupProduct').val() == 0) { msg += "Duplicate Record can not left Blank !! \n"; }
        if ($('#txtUrl').val() == "") { msg += "URL can not left Blank !! \n"; }
        if ($('#ddlSuCategory').val() == 0) { msg += "Super category can not left Blank !! \n"; }
        if ($('#ddlCategory').val() == 0) { msg += "Category can not left Blank !! \n"; }
        //if ($('#ddlsubCategory').val() == 0) { msg += ""; }
        if ($('#txtProductTag').val() == "") { msg += "Product tag can not left Blank !! \n"; }
        if ($('#ddlBrand').val() == 0) { msg += "Brand can not left Blank !! \n"; }
        if ($('#ddlHSN').val() == 0) { msg += "HSN can not left Blank !! \n"; }
        if ($('#ddlItemType').val() == 0) { msg += "Item type can not left Blank !! \n"; }
        if ($("#example1 tbody tr").length == 0) { msg += "Item Details can not left Blank !! \n"; }
        if ($("#btnSave").val() == "Create Product") {
            var filePath = $("#Featuredimage").val();
            if (filePath.length == 0) {
                msg += "Featured Image can not be blank !! \n";
            }
        }
        if (CKEDITOR.instances["txtProductDesc"].getData() == "") { msg += "Product Description can not left Blank !! \n"; }
        if (CKEDITOR.instances["AddInformation"].getData() == "") { msg += "Product information  can not left Blank !! \n"; }
        if (CKEDITOR.instances["Ingredients"].getData() == "") { msg += "Ingredients  can not left Blank !! \n"; }
        //if (msg != "") 
    }
    return msg;
}
function clearItemType() {
    $('#unitId').val("");
    $('#Index').val("");
    $('#txtUnitName').val("");
    $('#txtQuantity').val("");
    $('#txtDiscount').val("");
    $('#txtunitrate').val("");
    $('#txtStockQty').val("");
    $('#txtshipping').val("");

}


function clearItemType1() {
    $('#unitId').val("");
    $('#Index').val("");
    $('#txtUnitName').val("");
    $('#txtQuantity').val("0");
    $('#txtDiscount').val("0");
    $('#txtunitrate').val("0");
    $('#txtStockQty').val("");
    $('#txtshipping').val("0");

}
function validitemdetails() {
    var msg = "";
    var unitname = $('#txtUnitName').val();
    var stockqty = $('#txtStockQty').val();
    if (unitname == "") { msg += "Unit name can not left Blank !! \n"; }
    //  if ($('#txtQuantity').val() == "") { msg += ""; }
    //  if ($('#txtDiscount').val() == 0) { msg += ""; }
    // if ($('#txtunitrate').val() == 0) { msg += "Unit rate can not left Blank !! \n"; }
    if (stockqty == 0) { msg += "Stock quantity  can not left Blank !! \n"; }
    //  if ($('#txtshipping').val() == "") { msg += ""; }

    return msg;
}


function validationItemType1() {
    var msg = "";
    var unitname = $('#txtUnitName').val();
    var stockqty = $('#txtStockQty').val();
    if (unitname == "") { msg += "Unit name can not left Blank !! \n"; }
    //  if ($('#txtQuantity').val() == "") { msg += ""; }
    //  if ($('#txtDiscount').val() == 0) { msg += ""; }
    // if ($('#txtunitrate').val() == 0) { msg += "Unit rate can not left Blank !! \n"; }
    if (stockqty == 0) { msg += "Stock quantity  can not left Blank !! \n"; }
    //  if ($('#txtshipping').val() == "") { msg += ""; }

    return msg;
}

function ItemDetailsonEdit() {
    var unitId = $('#ddlItemType').find("option:selected").val();
    if (unitId == 0) {
        alert("Please select Item Type.");
    }
    else {
        var UnitName = $('#ddlItemType').find("option:selected").text();
        $('#unitId').val(unitId);
        $('#txtUnitName').val(UnitName);
        var productid = $('#id').val();
        $.ajax({
            url: "/Admin/Admin/ShowItemdetailsOnEditProduct",
            data: { id: productid },
            dataType: "JSON",
            type: "POST",
            success: function (result) {
                console.log(result);
                var html = '';
                var index = 1;
                $.each(result, function (key, item) {
                    html += '<tr class="hover-primary">';
                    html += '<td style="display:none">' + index + '</td>';
                    html += '<td style="display:none"><span class="id">' + item.id + '</span></td>';

                    html += '<td style="display:none"><span class="unitId">' + item.unit_id + '</span></td>';
                    html += '<td style="display:none"><span class="unitName">' + item.unitname + '</span></td>';
                    html += '<td><span class="Quantity">' + item.quantity + '</span></td>';
                    html += '<td><span class="Discount">' + item.discount + '</span></td>';
                    html += '<td><span class="unitrate">' + item.unit_Rate + '</span></td>';
                    html += '<td><span class="StockQuantity">' + item.stockStatus + '</span></td>';
                    html += '<td><span class="ShippingCharge">' + item.shipCharge + '</span></td>';
                    html += '<td style="display:none"><span class="item_id">' + item.itemid + '</span></td>';
                    html += '<td><a class="editrow" style="margin-right: 10px; margin-left: 10px"><i class="fa fa-edit"></i></a></td><td><a class="deleterow" onclick=delrowbyid(' + item.id + ')  style="margin-right: 10px; margin-left: 10px"><i class="fa fa-trash-o"></i></a></td>';
                    html += '</tr>';
                    index++;
                });
                if (html != "") {
                    $(".tbodyUnit").html(html);
                }
                $('#quickViewModal').modal('show');
            }
        });
    }
}
function ItemDetailsonEdits() {
    var unitId = $('#ddlItemType').find("option:selected").val();
    if (unitId == 0) {
        // alert("Please select Item Type.");
    }
    else {
        var UnitName = $('#ddlItemType').find("option:selected").text();
        $('#unitId').val(unitId);
        $('#txtUnitName').val(UnitName);
        var productid = $('#id').val();
        $.ajax({
            url: "/Admin/Admin/ShowItemdetailsOnEditProduct",
            data: { id: productid },
            dataType: "JSON",
            type: "POST",
            success: function (result) {
                console.log(result);
                var html = '';
                var index = 1;
                $.each(result, function (key, item) {
                    html += '<tr class="hover-primary">';
                    html += '<td style="display:none">' + index + '</td>';
                    html += '<td style="display:none"><span class="id">' + item.id + '</span></td>';
                    html += '<td style="display:none"><span class="unitId">' + item.unit_id + '</span></td>';
                    html += '<td style="display:none"><span class="unitName">' + item.unitname + '</span></td>';
                    html += '<td><span class="Quantity">' + item.quantity + '</span></td>';
                    html += '<td><span class="Discount">' + item.discount + '</span></td>';
                    html += '<td><span class="unitrate">' + item.unit_Rate + '</span></td>';
                    html += '<td><span class="StockQuantity">' + item.stockStatus + '</span></td>';
                    html += '<td><span class="ShippingCharge">' + item.shipCharge + '</span></td>';
                    html += '<td><a class="editrow" style="margin-right: 10px; margin-left: 10px"><i class="fa fa-edit"></i></a></td><td><a class="deleterow" onclick=delrow(' + item.id + ')  style="margin-right: 10px; margin-left: 10px"><i class="fa fa-trash-o"></i></a></td>';
                    html += '</tr>';
                    index++;
                });
                if (html != "") {
                    $(".tbodyUnit").html(html);
                }
                //$('#quickViewModal').modal('show');
            }
        });
    }
}

function UpdateProduct() {
    var fileExtension = ['png', 'img', 'jpg', 'mp4', 'pdf'];

    var id = $('#id').val();
    var ProductName = $('#txtProductName').val();
    var DupProductval = $('#ddlDupProduct').find("option:selected").val();
    var DupProductText = $('#ddlDupProduct').find("option:selected").text();
    var Url = $('#txtUrl').val();
    var SuperCategoryVal = $('#ddlSuCategory').find("option:selected").val();
    var SuperCategoryText = $('#ddlSuCategory').find("option:selected").text();
    var CategoryVal = $('#ddlCategory').find("option:selected").val();
    var CategoryText = $('#ddlCategory').find("option:selected").text();
    var SubCategoryVal = $('#ddlsubCategory').find("option:selected").val();
    if (SubCategoryVal == 'Select') {
        SubCategoryVal = '0';
    }
    var SubCategoryText = $('#ddlsubCategory').find("option:selected").text();
    if (SubCategoryText == 'Select') {
        SubCategoryText = '0';
    }
    var ProductTag = $('#txtProductTag').val();
    var BrandVal = $('#ddlBrand').find("option:selected").val();
    var BrandText = $('#ddlBrand').find("option:selected").text();
    var HSNVal = $('#ddlHSN').find("option:selected").val();
    var HSNText = $('#ddlHSN').find("option:selected").text();
    var ItemTypeval = $('#ddlItemType').find("option:selected").val();
    var ItemTypeText = $('#ddlItemType').find("option:selected").text();
    var SKUCode = $('#txtSkuCode').val();
    var RegularPrice = $('#txtRegularPrice').val();
    var SalePrice = $('#txtSalePrice').val();
    var Stockstatus = $('#txtStockstatus').val();
    var Weight = $('#txtWeight').val();
    var weightval = $('#ddlweight').find("option:selected").val();
    var weightText = $('#ddlweight').find("option:selected").text();
    var D_Length = $('#txtLength').val();
    var D_Width = $('#txtwidth').val();
    var D_height = $('#txtheight').val();
    var ShipCharges = $('#txtShipCharges').val();
    var Featuredimage = $('#Featuredimage').val();
    var BackImage = $('#BackImage').val();
    var Leftimage = $('#Leftimage').val();
    var RightImage = $('#RightImage').val();
    var filename1 = $('#imgfile1').val();
    var pdf1 = $('#pdf').val();

    var bal = $('#hdnBalance').val();
    var hdnStock = $('#hdnStock').val();
    var ButtonValue = $('#btnSave').val();
    var Active = $('#txtcheckbox').prop('checked') ? 1 : 0;
    var Send_query = $('#txtsendcheck').prop('checked') ? 1 : 0;
    var Price = $('#txtpricecheck').prop('checked') ? 1 : 0;



    var fdata = new FormData();
    if (Featuredfilenameorginal == Featuredimage) {
        if (Featuredimage.length == 0) {
            alert("Please select Featured image.");
            return false;
        }
        else {
            var extension = Featuredimage.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("Featuredimageflg", "okg");
        var FeaturedimageUpd = $('#Featuredimage').get(0);
        var files = FeaturedimageUpd.files;
        fdata.append(files[0].name, files[0]);
    }
    else {
        fdata.append("Featuredimageflg", "ok");
    }

    // Function to handle image validation and appending
    //function handleImageValidation(imageSelector, filenameOriginal, flagName) {
    //    var imageInput = $(imageSelector).get(0);
    //    var imageFile = imageInput.files;

    //    // Check if the file input matches the original filename
    //    if (filenameOriginal == imageSelector) {
    //        // If no file is selected, simply set the flag and return
    //        if (imageFile.length == 0) {
    //            fdata.append(flagName + "flg", "ok");
    //            return; // Skip further checks if no file is selected
    //        }

    //        // Extract the file extension from the file name
    //        var extension = imageFile[0].name.replace(/^.*\./, '');

    //        // Validate the file extension
    //        if ($.inArray(extension, fileExtension) == -1) {
    //            alert("Please select only PNG/JPG/mp4 files.");
    //            return false;
    //        }

    //        // If file extension is valid, append the file to FormData
    //        fdata.append(flagName + "flg", "okg");
    //        fdata.append(imageFile[0].name, imageFile[0]);
    //    } else {
    //        // If the filename doesn't match, just set the flag and continue
    //        fdata.append(flagName + "flg", "ok");
    //    }
    //}


    //// Call the function for Back Image
    //handleImageValidation('#BackImage', Backfilenameorginal, "BackImage");

    //// Call the function for Left Image
    //handleImageValidation('#Leftimage', Leftfilenameorginal, "Leftimage");

    //// Call the function for Right Image
    //handleImageValidation('#RightImage', Rightfilenameorginal, "RightImage");

    //handleImageValidation('#imgfile1', filename1orginal, "imgfile1");
    if (Backfilenameorginal != '' || BackImage != '') {

       if (Backfilenameorginal == BackImage) {
        //if (BackImage.length == 0) {
        //    alert("Please select Back Image.");
        //    return false;
        //}
        //else {
            var extension = BackImage.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                swal({
                    title: "Please select only PNG/IMG/JPG files.",
                    text: "",
                    iicon: "success",
                    timer: 10000,
                });
                // alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        /*}*/
        fdata.append("BackImageflg", "okg");
        var BackImageUpd = $('#BackImage').get(0);
        var bImage = BackImageUpd.files;
        fdata.append(bImage[0].name, bImage[0]);
       }
        else {
        fdata.append("BackImageflg", "ok");
        }

    }
    else {
        fdata.append("BackImageflg", "ok");
    }

  if (Leftfilenameorginal != '' || Leftimage != '') {

    if (Leftfilenameorginal == Leftimage) {
        //if (Leftimage.length == 0) {
        //    alert("Please select Left image.");
        //    return false;
        //}
        //else {
            var extension = Leftimage.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                swal({
                    title: "Please select only PNG/IMG/JPG files.",
                    text: "",
                    iicon: "success",
                    timer: 10000,
                });
                //alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        /*}*/
        fdata.append("Leftimageflg", "okg");
        var Leftimageupd = $('#Leftimage').get(0);
        var limage = Leftimageupd.files;
        fdata.append(limage[0].name, limage[0]);
    }
    else {
        fdata.append("Leftimageflg", "ok");
    }
  }
  else {
      fdata.append("Leftimageflg", "ok");
  }

  if (Rightfilenameorginal != '' || RightImage != '') {

    if (Rightfilenameorginal == RightImage) {
        //if (RightImage.length == 0) {
        //    swal({
        //        title: "Please select Right Image.",
        //        text: "",
        //        iicon: "success",
        //        timer: 10000,
        //    });
        //    //alert("Please select Right Image.");
        //    return false;
        //}
        //else {
            var extension = RightImage.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                swal({
                    title: "Please select only PNG/IMG/JPG files.",
                    text: "",
                    iicon: "success",
                    timer: 10000,
                });
                // alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
       /* }*/
        fdata.append("RightImageflg", "okg");
        var RightImageupd = $('#RightImage').get(0);
        var rImage = RightImageupd.files;
        fdata.append(rImage[0].name, rImage[0]);
    }
    else {
        fdata.append("RightImageflg", "ok");
    }
  }
  else {
      fdata.append("RightImageflg", "ok");
  }


    if (filename1orginal != '' || filename1 != '') {

        if (filename1orginal == filename1) {

            var extension = filename1.replace(/^.*\./, '');
            if ($.inArray(extension, fileExtension) == -1) {
                swal({
                    title: "Please select only MP4 files.",
                    text: "",
                    iicon: "success",
                    timer: 10000,
                });

                return false;
            }

            fdata.append("filename1Imageflg", "okg");
            var filename1upd = $('#imgfile1').get(0);
            var filename1Image = filename1upd.files;
            fdata.append(filename1Image[0].name, filename1Image[0]);
        }
        else {
            fdata.append("filename1Imageflg", "ok");
        }


    }
    else {
        fdata.append("filename1Imageflg", "ok");
    }


  if (pdf1orginal != '' || pdf1 != '') {

    if (pdf1orginal == pdf1) {

        var extension = pdf1.replace(/^.*\./, '');
        if ($.inArray(extension, fileExtension) == -1) {
            swal({
                title: "Please select only pdf files.",
                text: "",
                iicon: "success",
                timer: 10000,
            });

            return false;
        }

        fdata.append("pdf1Imageflg", "okg");
        var pdf1upd = $('#pdf').get(0);
        var pdf1Image = pdf1upd.files;
        fdata.append(pdf1Image[0].name, pdf1Image[0]);
    }
    else {
        fdata.append("pdf1Imageflg", "ok");
      }

  }
  else {
      fdata.append("pdf1Imageflg", "ok");
  }

    var ProductDesc = CKEDITOR.instances["txtProductDesc"].getData();
    var AddInformation = CKEDITOR.instances["AddInformation"].getData();
    var Ingredients = CKEDITOR.instances["Ingredients"].getData();
    fdata.append("id", id);
    fdata.append("ProductName", ProductName);
    fdata.append("DupProductval", DupProductval);
    fdata.append("Url", Url);
    fdata.append("SuperCategoryVal", SuperCategoryVal);
    fdata.append("CategoryVal", CategoryVal);
    fdata.append("SubCategoryVal", SubCategoryVal);
    fdata.append("ProductTag", ProductTag);
    fdata.append("BrandVal", BrandVal);
    fdata.append("HSNVal", HSNVal);
    fdata.append("ItemTypeval", ItemTypeval);
    fdata.append("SKUCode", SKUCode);
    fdata.append("RegularPrice", RegularPrice);
    fdata.append("SalePrice", SalePrice);
    fdata.append("Stockstatus", Stockstatus);
    fdata.append("Weight", Weight);
    fdata.append("weightval", weightval);
    fdata.append("D_Length", D_Length);
    fdata.append("D_Width", D_Width);
    fdata.append("D_height", D_height);
    fdata.append("ShipCharges", ShipCharges);
    fdata.append("ProductDesc", ProductDesc);
    fdata.append("AddInformation", AddInformation);
    fdata.append("Ingredients", Ingredients);
    fdata.append('balance', bal);
    fdata.append('hdnStock', hdnStock);

    fdata.append("Featuredfilename", Featuredfilenameorginal);
    fdata.append("Backfilename", Backfilenameorginal);
    fdata.append("Leftfilename", Leftfilenameorginal);
    fdata.append("Rightfilename", Rightfilenameorginal);
    fdata.append("filename1Image", filename1orginal);
    fdata.append("pdf1Image", pdf1orginal);
    fdata.append("Active", Active);
    fdata.append("Send_query", Send_query);
    fdata.append("Price", Price);


    var Units = new Array();
    $("#example1 tbody tr").each(function () {
        var row = $(this);
        var unit = {};
        unit.UnitId = row.find("span.unitId").text();
        unit.Qty = row.find('span.Quantity').text();
        unit.discount = row.find('span.Discount').text();
        unit.unitrate = row.find('span.unitrate').text();
        unit.stockQuantity = row.find('span.StockQuantity').text();
        unit.shippingCharge = row.find('span.ShippingCharge').text();
        unit.id = row.find('span.id').text();
        unit.itemid = $('#txthiddenitemid').val();
        Units.push(unit);
    });
    var unitjson = JSON.stringify(Units)
    fdata.append('json', unitjson);
    var len = Units.length
    //for (len = 0; len > length; len++) {
    //    fdata.append('UnitId', Units[len]['UnitId']);
    //    fdata.append('Qty', Units[len]['Qty']);
    //    fdata.append('discount', Units[len]['discount']);
    //    fdata.append('unitrate', Units[len]['unitrate']);
    //}  
    //var msg = ValidationProduct();
    //if (msg == "") {

    $.ajax({
        type: "POST",
        url: '/Admin/Admin/SaveNewProduct',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: fdata,
        contentType: false,
        processData: false,
        // data: { id: id, SubCategory: SubCategory, SuperCategoryId: SuperCategoryId, CategoryId: CategoryId, Category: Category, Status: Status },
        //dataType: "JSON",
        success: function (result) {
            if (result.message == "NewProduct added.") {
                if (result.id == 0) {
                    swal({
                        title: "New Product added successfully.",
                        text: "",
                        iicon: "",
                        timer: 10000,
                    });
                    window.reload();
                    //   ProductList();
                    // alert("NewProduct added successfully.");
                }
                else {
                    swal({
                        title: "Product modify successfully.",
                        text: "",
                        iicon: "",
                        timer: 10000,
                    });
                    window.location.href = "/Admin/Admin/ProductList"
                    //alert("Product modify successfully.");
                }
                ClearField();
             

                //ProductList();
            }
            else if (result.message == "Name Already Exist") {
                swal({
                    title: "Product Name Already Exist",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
            }
        },
        error: function () {
            alert("New Product not added.");
        }
    });
    //}
    //else {
    //    alert(msg);
    //}
}