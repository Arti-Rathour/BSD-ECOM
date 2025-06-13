//const { iteratee } = require("lodash");

$(document).ready(function () {
    applybrandfilter();


    //GetItemName();
});
function addcart(itemid, type) {
    var productimg = $('#frontimg').attr('src');
    // var productname = $('#itemname').text();
    // var productId = $('#productId').val();
    // var ProductPrice = $('#price').text().split('₹');
    var itemdetid = $('#' + 'productdetId_' + itemid).val();
    var quantity = $('#' + 'quantity_' + itemid).text();
    if (quantity == 0) {
        quantity = 1;
    }
    $.ajax({
        type: "POST",
        url: '/Home/Addcart',
        data: { productId: itemid, quantity: quantity, itemdetid: itemdetid, type: type },
        dataType: "JSON",
        success: function (response) {
            var html = '';
            //debugger;
            var count = response.count;
            //   alert(count);
            $('#carttotal').html(count)
            // var fff = response.carts[0].productname;
            var total = "";
            // alert(response.carts[0].productname);
            // response = response["carts"] || {};
            if (response.message == "add cart added.")
                html += '<div class="cart-dropdown-wrap cart-dropdown-hm2">';
            for (var i = 0; i < count; i++) {
                //  alert(response[].carts[0].productname);
                total = (Number(total) + Number(response.carts[i].totalprice));
                html += '<ul>';
                html += '<li>';
                html += '<div class="shopping-cart-img">'
                html += '<a href="shop-product-right.html"><img alt="Nest" src="/images/Productimage/6/' + response.carts[i].productimage + '"></a>';
                html += '</div>';
                html += '<div class="shopping-cart-title">';
                html += '<h4><a href="shop-product-right.html">' + response.carts[i].productname + '</a></h4>';
                html += '<h4><span>' + response.carts[i].quantity + ' × </span>' + response.carts[i].productprice + '</h4>';
                html += '<h4>Quantity -<span>' + response.carts[i].quantity + '</span></h4>';
                // html += '<h4>Type -<span>' + response.carts[i].unitdesc + ' </span></h4>';
                //html += '<div class="detail-extralink"> '     
                //html += '<div class="detail-qty border radius">'
                //html += '<a href="#" class="qty-down" onclick="changepriceid(' + response.carts[i].productid + ',1)"><i class="fi-rs-angle-small-down"></i></a>'
                //html += '<span class="qty-val" id="qty_' + response.carts[i].productid + '">' + response.carts[i].quantity + '</span>'
                //html += '<a href="#" class="qty-up" onclick="changepriceid(' + response.carts[i].productid + ',2)"><i class="fi-rs-angle-small-up"></i></a>'

                //html += '</div>'
                //html += '</div>'
                
                html += '</div>';
                // html += '<div class="shopping-cart-delete">'
                // html += '<a href="#" class="deleterow"><i class="fi-rs-cross-small"></i></a>'
                //html += '</div>';
                html += '</li>';
                html += '</ul>';
            }
            html += '<div class="shopping-cart-footer">';
            html += '<div class="shopping-cart-total">';
            html += '<h4>Total <span id="totals">' + total + '</span></h4>';
            html += ' </div>';
            html += '<div class="shopping-cart-button">';
            html += '<a href="/cart" class="outline">View cart</a>';
            html += '<a href="/CheckOut">Checkout</a>';
            html += '</div>';
            html += '</div>';
            html += '</div >';
            if (html != "") {
                $("#divcart").html(html);
                if (response.displaymessage != "") {

                    const el = document.createElement('div')
                    el.innerHTML = "GO TO <a href='/cart'>CART</a>"
                    //swal({
                    //    title: response.displaymessage,
                    //    text: '',
                    //    content: el,
                    //    icon: "success",
                    //    timer: 10000,
                    //});
                    swal({
                        title: response.displaymessage,
                        text: '',
                        content: el,
                        icon: "success",
                        timer: 10000,
                    }).then(() => {
                        
                        if (response.type == "5") {

                            window.location.replace("/wishlist");
                        }
                    });

                }
                var rowCount = $("#cartdata tr").length + 1;
                if (rowCount > 0) {
                    setTimeout(sendmailafter15minutes, 60000);
                }
                

            }
            else {
                $("#divcart").html('');
                if (response.displaymessage != "") {
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                    // alert(response.displaymessage);
                }
            }
        },
        error: function (e) {
            //$('#divPrint').html(e.responseText);
        }
    });

}


//function changepriceidid(itemid, type) {
//    var qty = $("#quantityidid").val();
//    var quantity = $('#' + 'quantity_' + itemid).text();

//    if (qty > quantity) {
//        $('.detail-qty').each(function () {
//            var qtyval = parseInt($(this).find('.qty-val').text(), 10);
//            $('.qty-up').on('click', function (event) {
//                event.preventDefault();
//                qtyval = qtyval + 1;
//                $(this).prev().text(qtyval);
//            });
//            $('.qty-down').on('click', function (event) {
//                event.preventDefault();
//                qtyval = qtyval - 1;
//                if (qtyval > 1) {
//                    $(this).next().text(qtyval);
//                } else {
//                    qtyval = 1;
//                    $(this).next().text(qtyval);
//                }
//            });
//        });
//       /* changeprice(itemid, type)*/
//    } else {

//        alert("Only " + qty + " Stock aviable for this product")
//    }


//}




function changepriceidid(itemid, type) {
    var qty = parseInt($("#quantityidid").val(), 10);

 

    var qtyval = parseInt($('#' + 'qty_' + itemid).text(), 10);


    var msg = changepricvalidat(itemid, type);
    if (msg == "") {
        $('.qty-up').on('click', function (event) {
            event.preventDefault();
            if (qtyval < qty) {

                qtyval += 1;
                $('#' + 'qty_' + itemid).text(qtyval);
                //$(this).closest('.detail-qty').find('.qty-valid').text(qtyval);

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

        return false;
    }
}


function changepricvalidat(itemid, type) {

    var qty = parseInt($("#quantityidid").val(), 10);
    var qtyval = parseInt($('#' + 'qty_' + itemid).text(), 10);
    var msg = "";

    if (qtyval < qty) {
        msg = "";
    } else {

        msg = "Only " + qty + " stock available for this product";


    }

    return msg;
}


function changepriceiddownid(itemid, type) {
    var qtyval = parseInt($('#' + 'qty_' + itemid).text(), 10);
    //$('.qty-down').on('click', function (event) {
    //    event.preventDefault();
        qtyval = qtyval - 1;
        if (qtyval > 1) {
            $('#' + 'qty_' + itemid).text(qtyval);
           // $(this).next().text(qtyval);
        } else {
            qtyval = 1;
            $('#' + 'qty_' + itemid).text(qtyval);
            //$(this).next().text(qtyval);
        }
/*    });*/

}



function changepriceiddownqty(itemid, type) {
    var qtyval = parseInt($('#' + 'quantity_' + itemid).text(), 10);
   /* $('.qty-down').on('click', function (event) {*/
       /* event.preventDefault();*/
        qtyval = qtyval - 1;
        if (qtyval > 1) {
            $('#quantity_' + itemid).text(qtyval);
        } else {
            qtyval = 1;
            $('#quantity_' + itemid).text(qtyval);
        }
   /* });*/

}
function changepriceiddown(itemid, type) {
    var qtyval = parseInt($('#' + 'quantity_' + itemid).text(), 10);
    //$('.qty-down').on('click', function (event) {
    //    event.preventDefault();
        qtyval = qtyval - 1;
        if (qtyval > 1) {
          $('#quantity_' + itemid).text(qtyval);
            //$(this).next().text(qtyval);
        } else {
            qtyval = 1;
            $('#quantity_' + itemid).text(qtyval);
            //$(this).next().text(qtyval);
        }
  /*  });*/

}



function changepriceidqty(itemid, type) {
    //var qty = parseInt($("#unitqtyid").val(), 10);
    var qty = parseInt($('#' + 'unitqtyid_' + itemid).val(), 10);

    var qtyval = parseInt($('#' + 'quantity_' + itemid).text(), 10);


    var msg = changepricvaliqty(itemid, type);
    if (msg == "") {
        //$('.qty-up').on('click', function (event) {
        //    event.preventDefault();
            if (qtyval < qty) {

                qtyval += 1;
                $('#' + 'quantity_' + itemid).text(qtyval);
                //$(this).closest('.detail-qty').find('.qty-val').text(qtyval);

            }

        //});
    }
    else {
        swal({
            title: msg,
            text: "",
            icon: "success",
            timer: 10000,
        });

        return false;
    }
}




function qtychangepriceid(itemid, type) {
    var qty = parseInt($("#unitqttyid").val(), 10);
    var qtyval = parseInt($('#' + 'quantity_' + itemid).text(), 10);


    var msg = qtychangepricvali(itemid, type);
    if (msg == "") {
        //$('.qty-up').on('click', function (event) {
        //    event.preventDefault();
            if (qtyval < qty) {

                qtyval += 1;
                $('#' + 'quantity_' + itemid).text(qtyval);
              //  $(this).closest('.detail-qty').find('.qty-val').text(qtyval);

            }

     /*   });*/
    }
    else {
        swal({
            title: msg,
            text: "",
            icon: "success",
            timer: 10000,
        });

        return false;
    }
}



function changepriceid(itemid, type) {
    
    var qty = parseInt($('#' + 'unitqtyid_' + itemid).val(), 10);
    var qtyval = parseInt($('#' + 'quantity_' + itemid).text(), 10);

    var msg = changepricvali(itemid, type);
    if (msg == "") {
        //$('.qty-up').on('click', function (event) {
        //    event.preventDefault();
            if (qtyval < qty) {

                qtyval += 1;
                $('#' + 'quantity_' + itemid).text(qtyval);
                /*$(this).closest('.detail-qty').find('.qty-val').text(qtyval);*/

            }

       /* });*/
    }
    else {
        swal({
            title: msg,
            text: "",
            icon: "success",
            timer: 10000,
        });
     
        return false;
    }
}




function changepricvaliqty(itemid, type) {

    //var qty = parseInt($("#unitqtyid").val(), 10);
    var qty = parseInt($('#' + 'unitqtyid_' + itemid).val(), 10);
    var qtyval = parseInt($('#' + 'quantity_' + itemid).text(), 10);
    var msg = "";

    if (qtyval < qty) {
        msg = "";
    } else {

        msg = "Only " + qty + " stock available for this product";


    }

    return msg;
}






function qtychangepricvali(itemid, type) {

    var qty = parseInt($("#unitqttyid").val(), 10);
    var qtyval = parseInt($('#' + 'quantity_' + itemid).text(), 10);
    var msg = "";

    if (qtyval < qty) {
        msg = "";
    } else {

        msg = "Only " + qty + " stock available for this product";


    }

    return msg;
}
function changepricvali(itemid, type) {

    var qty = parseInt($('#' + 'unitqtyid_' + itemid).val(), 10);
    var qtyval = parseInt($('#' + 'quantity_' + itemid).text(), 10);
    var msg = "";

    if (qtyval < qty) {
        msg = "";
    } else {
     
        msg = "Only " + qty + " stock available for this product";

        
    }
  
    return msg;
}



function changepriceidpro(itemid, type) {
   
    var qty = parseInt($('#' + 'unitqtyidid_' + itemid).val(), 10);
    
    var qtyval = parseInt($('#' + 'quantity_' + itemid).text(), 10);


    var msg = changepricvalipro(itemid, type);
    if (msg == "") {
        //$('.qty-up').on('click', function (event) {
        //    event.preventDefault();
            if (qtyval < qty) {

                qtyval += 1;
                $('#' + 'quantity_' + itemid).text(qtyval);
               // $(this).closest('.detail-qty').find('.qty-val').text(qtyval);

            }

       /* });*/
    }
    else {
        swal({
            title: msg,
            text: "",
            icon: "success",
            timer: 10000,
        });

        return false;
    }
}


function changepricvalipro(itemid, type) {

    var qty = parseInt($('#' + 'unitqtyidid_' + itemid).val(), 10);
    var qtyval = parseInt($('#' + 'quantity_' + itemid).text(), 10);
    var msg = "";

    if (qtyval < qty) {
        msg = "";
    } else {

        msg = "Only " + qty + " stock available for this product";


    }

    return msg;
}

function changeprice(itemid, type) {

    //var   type="";

    var itemdetid = $('#' + 'productdetId_' + itemid).val();
    var quantity = $('#' + 'qty_' + itemid).text();

    if (quantity == 0) {
        quantity = 1;
    }


    $.ajax({
        type: "POST",
        url: '/Home/Addcart',
        data: { productId: itemid, quantity: quantity, itemdetid: itemdetid, type: type },
        dataType: "JSON",
        success: function (response) {
            var html = '';
            var count = 0;
            count = response.carts.length;
            //  debugger;
            //alert(count);
            $('#carttotal').html(count)
            //  var fff = response.carts[0].productname;
            var total = "0";
            var gtotal = "0";
            var shipchg = "0";
            //if (response.displaymessage != "") {
            //    alert(response.displaymessage);
            //    $("#quantity_" + itemid + "").html(quantity);
            //}
            if (response.message == "add cart added.")

                $('#cartdata').html('')

            for (var i = 0; i < count; i++) {
                total = (parseFloat(total) + parseFloat(response.carts[i].totalprice));

                shipchg = (parseFloat(shipchg) + parseFloat(response.carts[i].shipcharge));
                html += '<tr class="pt-10">'
                html += '<td class="custome-checkbox pl-30" style="display:none">'
                html += '<input class="form-check-input" type="checkbox" name="checkbox" id="exampleCheckbox1" value="">'
                html += '<label class="form-check-label" for="exampleCheckbox1"></label>'
                html += '</td>'
                html += '<td class="image product-thumbnail pt-10"><img src="/images/Productimage/6/' + response.carts[i].productimage + '" alt="#"></td>'
                html += '<td class="product-des product-name">'
                html += '<h6 class="mb-5"><a class="product-name mb-10 text-heading" href="/product/' + response.carts[i].productid + '/' + response.carts[i].itemdetid + '/' + response.carts[i].productname + '/' + response.carts[i].categoryId + '">' + response.carts[i].productname + '</a></h6>'
                //html += '<div class="product-rate-cover">'
                //html += ' <div class="product-rate d-inline-block">'
                //html += '<div class="product-rating" style="width:90%">'
                //html += '</div>'
                //html += '</div>'
                //html += '<span class="font-small ml-5 text-muted"> (4.0)</span>'
                //html += '</div>'
                html += '</td>'
                html += '<td class="price" data-title="Price">'
                html += '<h4 class="text-body unitprice" id="unitprice_' + response.carts[i].productid + '">' + response.carts[i].productprice + '</h4>'
                html += '<input type="hidden" value=' + response.carts[i].itemdetid + ' id="productdetId_' + response.carts[i].productid + '" />'
                html += ' </td>'


                html += '<td class="text-center detail-info" data-title="Stock">'
                html += '<div class="detail-extralink">'
                html += '<div class="detail-qty border radius">'
                html += '<a href="#" class="qty-down" onclick="changeprice(' + response.carts[i].productid + ',1)"><i class="fi-rs-angle-small-down"></i></a>'
                html += '<span class="qty-val" id="qty_' + response.carts[i].productid + '">' + response.carts[i].quantity + '</span>'
                html += '<a href="#" class="qty-up" onclick="changeprice(' + response.carts[i].productid + ',2)"><i class="fi-rs-angle-small-up"></i></a>'
                html += '</div>'
                html += '</div>'
                html += ' </td>'
                html += '<td class="price" data-title="Price">'
                html += ' <span id="total_' + response.carts[i].productid + '" class="text-brand total">' + response.carts[i].totalprice + '</span>'
                html += ' </td>'
                html += ' <td class="action text-center" data-title="Remove"><a href="#" class="text-body" onclick=changeprice(' + response.carts[i].productid + ',3)><i class="fi-rs-trash"></i></a></td>'
                html += ' </tr>'


            }



            $("#stotal").text(total);
            $('#shipcharge').text(shipchg);

            $("#gtotal").text(parseFloat(total) + parseFloat(shipchg));
            $("#grandtotal").text(total);

            if (html != "") {
                $("#cartdata").html(html);
                if (response.displaymessage != "") {
                    alert(response.displaymessage);
                }
            }
            else {
                $("#cartdata").html("Your Cart is Empty.");
            }

            // alert("Your product added in cart.");
        },
        error: function (e) {
            //$('#divPrint').html(e.responseText);
        }
    });
}
function chek(itemid) {
    alert("ddd");
}




var Addtocarts = new Array();
var Subtotal = 0;

var rowCount;
function viewcart() {
    $.ajax({
        url: "/Home/Viewcart",
        dataType: 'JSON',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            if (response.length == 0)
                alert('Some error occured.');
            else {
                $('#divPrint').html(response);
                rowCount = $("#cartView tr").length;
                $('#count').text(rowCount);
            }
        },
        error: function (e) {
            $('#divPrint').html(e.responseText);
            rowCount = $("#cartView tr").length - 1;
            $('#count').text(rowCount);
        }
    });
}
function DeleteProduct(productid, itemdetid) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ productid: productid, itemdetid: itemdetid });
    alert(productid);
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Home/DeleteViewCart",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    viewcart();
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



(function () {
    const quantityContainer = document.querySelector(".quantity");
    //const minusBtn = quantityContainer.querySelector(".minus");
    //const plusBtn = quantityContainer.querySelector(".plus");
    //const inputBox = quantityContainer.querySelector(".input-box");

    //updateButtonStates();

    //quantityContainer.addEventListener("click", handleButtonClick);
    //inputBox.addEventListener("input", handleQuantityChange);

    //function updateButtonStates() {
    //    const value = parseInt(inputBox.value);
    //    minusBtn.disabled = value <= 1;
    //    plusBtn.disabled = value >= parseInt(inputBox.max);
    //}

    function handleButtonClick(event) {
        if (event.target.classList.contains("minus")) {
            decreaseValue();
        } else if (event.target.classList.contains("plus")) {
            increaseValue();
        }
    }

    function decreaseValue() {
        let value = parseInt(inputBox.value);
        value = isNaN(value) ? 1 : Math.max(value - 1, 1);
        inputBox.value = value;
        updateButtonStates();
        handleQuantityChange();
    }

    function increaseValue() {
        let value = parseInt(inputBox.value);
        value = isNaN(value) ? 1 : Math.min(value + 1, parseInt(inputBox.max));
        inputBox.value = value;
        updateButtonStates();
        handleQuantityChange();
    }

    function handleQuantityChange() {
        let value = parseInt(inputBox.value);
        value = isNaN(value) ? 1 : value;

        // Execute your code here based on the updated quantity value
        console.log("Quantity changed:", value);
    }
})();







function sendmailafter15minutes() {
    $.ajax({
        type: "POST",
        url: '/Home/sendmailafter15minutes',
        data: {},
        dataType: "JSON",
        success: function (response) {
            console.log(response);
            //alert(response);
        },
        error: function (e) {
            //$('#divPrint').html(e.responseText);
        }
    });
}
$(document).on('click', 'a.deleterow', function () {
    $(this).parents('tr').remove();
    return false;
});

function addcartquickview() {


    var productId = $('#quickitemId').val();

    var itemdetid = $('#quickproductdetId').val();
    var quantity = $('#qtyquick').text();
    if (quantity == 0) {
        quantity = 1;
    }
    $.ajax({
        type: "POST",
        url: '/Home/Addcart',
        data: { productId: productId, quantity: quantity, itemdetid: itemdetid },
        dataType: "JSON",
        success: function (response) {
            var html = '';
            //debugger;
            var count = response.count;
            //   alert(count);
            $('#carttotal').html(count)
            // var fff = response.carts[0].productname;
            var total = "";
            // alert(response.carts[0].productname);
            // response = response["carts"] || {};
            if (response.message == "add cart added.")
                html += '<div class="cart-dropdown-wrap cart-dropdown-hm2">';
            for (var i = 0; i < count; i++) {
                //  alert(response[].carts[0].productname);
                total = (Number(total) + Number(response.carts[i].totalprice));
                html += '<ul>';
                html += '<li>';
                html += '<div class="shopping-cart-img">'
                html += '<a href="shop-product-right.html"><img alt="Nest" src="/images/Productimage/6/' + response.carts[i].productimage + '"></a>';
                html += '</div>';
                html += '<div class="shopping-cart-title">';
                html += '<h4><a href="shop-product-right.html">' + response.carts[i].productname + '</a></h4>';
                html += '<h4><span>' + response.carts[i].quantity + ' × </span>' + response.carts[i].productprice + '</h4>';
                html += '<h4>Type -<span>' + response.carts[i].unitdesc + ' </span></h4>';
                html += '</div>';
                // html += '<div class="shopping-cart-delete">'
                // html += '<a href="#" class="deleterow"><i class="fi-rs-cross-small"></i></a>'
                //html += '</div>';
                html += '</li>';
                html += '</ul>';
            }
            html += '<div class="shopping-cart-footer">';
            html += '<div class="shopping-cart-total">';
            html += '<h4>Total <span id="totals">' + total + '</span></h4>';
            html += ' </div>';
            html += '<div class="shopping-cart-button">';
            html += '<a href="/cart" class="outline">View cart</a>';
            html += '<a href="/CheckOut">Checkout</a>';
            html += '</div>';
            html += '</div>';
            html += '</div >';
            if (html != "") {
                $("#divcart").html(html);
                if (response.displaymessage != "") {
                    //alert(response.displaymessage);
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                }
                $(".btn-close").click();
            }
            else {
                $("#divcart").html('');
                if (response.displaymessage != "") {
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                    //alert(response.displaymessage);
                }
            }

        },
        error: function (e) {
            //$('#divPrint').html(e.responseText);
        }
    });
}
function ShowItemView(id) {
    var itemdetid = $('#' + 'productdetId_' + id).val();
    $.ajax({
        type: 'POST',
        url: "/Home/ShowItemView",
        data: { id: id, itemdetailsid: itemdetid },
        dataType: 'JSON',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#discount').text("");
            $('#unitrate').text("");
            $('#disamt').text("");
            $('#frontimg').attr('src', "/images/Productimage/6/" + result[0].frontimage + "").trigger('click');
            $('#frontimg1').attr('src', "/images/Productimage/6/" + result[0].frontimage + "").trigger('click');
            $('#Backimg').attr('src', "/images/Productimage/6/" + result[0].backimage + "").trigger('click');
            $('#Backimg1').attr('src', "/images/Productimage/6/" + result[0].backimage + "").trigger('click');
            $('#itemname').text(result[0].itemName);
            if (result[0].price != 0) {
                if (result[0].discount != 0) {
                    $('#discount').text(result[0].discount);
                    $('#unitrate').text(result[0].unit_Rate);
                }
                $('#disamt').text(result[0].disamt);
            }

            $('#quickitemId').val(id);

            const spanElement = document.querySelector('.qty-valid');

            // Ensure that the element exists
            if (spanElement) {
                // Dynamically set the ID
                const idValue = 'qty_' + id; // Get quickitemId value
                spanElement.setAttribute('id', idValue);

                // Optional: Log the new ID for debugging
                console.log('ID set for span:', idValue);
            }



            $('#quantityidid').val(result[0].quantity);
            //ViewBag.Id= $('#quickitemId').val(id);
            $('#quickproductdetId').val(itemdetid);
            



            $('#quickViewModal').modal('show');
            //$('#quickViewModal').show();
        }
    });
}








function Addwishlist(itemid) {

    //var Item_id = $("#productId").val();
    var itemdetid = $('#' + 'productdetId_' + itemid).val();

    $.ajax({
        url: "/Home/Savewishlist",
        type: 'POST',
        data: { Item_id: itemid, itemdetid: itemdetid },
        dataType: 'JSON',
        success: function (result) {

            if (result.message == "Item already in wishlist") {
                swal({
                    title: "Item already in wishlist",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
            }
            else {
                if (result.message == "Add Your item in Wishlist") {
                    swal({
                        title: "Add Your item in Wishlist",
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                    // alert("Add Your item in Wishlist");
                }
            }
        },
        error: function () {
            swal({
                title: "Wishlist Submission Failed",
                text: "",
                icon: "success",
                timer: 10000,
            });

        }
    });
}
function ClearReview() {
    $("#txtcomment").val("");
    $("#txtname").val("");
    $("#email").val("");
    $('#ddlreview').val(3);
}
$("#filter").change(function () {
    var selectSize = $(this).val();
    filter(selectSize);
});
function filter(e) {
    var regex = new RegExp(e), count = 0;
    $('.locality-details h4').each(function () {
        if ($(this).text().search(new RegExp(regex, "i")) < 0) {
            $(this).hide();
            $('.locality-details product-img product-img-zooms').hide();
            //$(this).hide();
            //$("#loc_img").hide();
        } else {
            $(this).show();
            $('.locality-details product-img product-img-zooms').show();
            count++;
        }
    });
}
function selectonchange(id, itemid) {
    $.ajax({
        type: 'POST',
        url: "/Home/itemrate",
        data: { id: id, itemid: itemid },
        dataType: 'JSON',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {

            $('#' + 'disamtS_' + itemid).html("₹" + result[0].disamt);
            $('#' + 'urate_' + itemid).text("₹" + result[0].unit_Rate)
            $('#' + 'productdetId_' + itemid).val(id)
        }
    });


}
function proceedcheckout() {
    $.ajax({
        url: "/Home/proceedcheckout",
        type: 'POST',
        dataType: 'JSON',
        success: function (result) {
            if (result.message == "Please Login.") {
                window.location.replace("/Login/1");
                // alert("Please login.");
            }
            else {
                // alert('Thanks for placing the order.Your order ID is   .Our team will contact you soon on this');
                //window.location.replace("/CheckOut");
            }
        }
    });
}


function allproductsort(cateid, typee, val) {

   $("#hidedropdown").hide();

    var minamt = $("#slider-range-value1").html().replace("₹", "");

    var maxamt = $("#slider-range-value2").html().replace("₹", "");

    $.ajax({
        type: "POST",
        url: '/Home/allproductcatewise',
        data: { cateid: cateid, typeid: typee, minamt: minamt, maxamt: maxamt },
        dataType: "JSON",
        success: function (response) {
            var html = '';

            var count = response.count;

            response.itemdetailsS.length
            var total = "";

            if (response.message == "ok")
                $("#allp").html('');


            for (var i = 0; i < response.itemStroes.length; i++) {


                var itemdetailsid
                for (var j = 0; j < response.itemdetailsS.length; j++) {
                    if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {

                        itemdetailsid = response.itemdetailsS[j].id;
                    }
                }

                html += '<div class="col-lg-1-3 col-md-4 col-12 col-sm-6">'
                html += '<div class="product-cart-wrap mb-30 list-products">'
                html += '<div class="product-img-action-wrap">'
                html += '<div class="product-img product-img-zoom">'
               

                html += '<a href="/Product/' + response.itemStroes[i].id + '/' + itemdetailsid + '/' + response.itemStroes[i].itemName.replace(" ", "-") + '/' + response.itemStroes[i].categoryId + '">'
                html += '<img class="default-img" src="/images/Productimage/6/' + response.itemStroes[i].frontimage + '" alt="">'
                html += '<img class="hover-img" src="/images/Productimage/6/' + response.itemStroes[i].backimage + '" alt="">'
                html += '</a>'
                html += '</div>'
                html += '<div class="product-action-1">'
                html += '<a aria-label="Add To Wishlist" class="action-btn" href="shop-wishlist.html"><i class="fi-rs-heart"></i></a>'
                html += '<a aria-label="Quick view" class="action-btn" data-bs-toggle="modal" data-bs-target="#quickViewModal"><i class="fi-rs-eye"></i></a>'
                html += '</div>'
                html += '<div class="product-badges product-badges-position product-badges-mrg">'
                html += '<span class="hot">' + response.itemStroes[i].productTag + '</span>'
                html += '</div>'
                html += '</div>'

                html += '<div class="product-content-wrap">'
                html += '<div class="product-category">'
                html += ' <a href="#">' + response.itemStroes[i].category_name + '</a>'
                html += '</div>'
                html += '<h2><a href="/Product/' + response.itemStroes[i].id + '/' + itemdetailsid + '/' + response.itemStroes[i].itemName.replace(" ", "-") + '/' + response.itemStroes[i].categoryId + '">' + response.itemStroes[i].itemName + '</a></h2>'
                html += '<div class="main-data-list">'
                // html += '<div class="product-rate-cover">'
                // //html += '<div class="product-rate d-inline-block">'
                // //html += ' <div class="product-rating" style="width: 90%"></div>'
                // //html += '</div>'
                ///* html += '<span class="font-small ml-5 text-muted"> (4.0)</span>'*/
                // html += '</div>'


                // html +=  '< div class="clearfix product-price-cover" >'
                if (response.itemStroes[i].price == 1) {
                    html += '<div class="product-price primary-color float-left" style="display:((' + response.itemStroes[i].price+' == 1)?"inline-block":"none") " >'
                    html += '<span style="font-weight: 700;" class="current-price text-brand" id="disamtS_' + response.itemStroes[i].id + '">₹' + response.itemStroes[i].disamt + '</span>'
                    html += '<span>'
                    if (response.itemStroes[i].discount != 0) {
                        html += '<span class="save-price font-md color3 ml-15" style="display:((' + response.itemStroes[i].price+' == 1)?"inline-block":"none")" id=dis_' + response.itemStroes[i].id + '>' + response.itemStroes[i].discount % +'</span>'
                        html += '<span class="old-price font-md ml-15" style="display:((' + response.itemStroes[i].price+' == 1)?"inline-block":"none")" id = "urate_' + response.itemStroes[i].id + '" >₹' + response.itemStroes[i].unit_Rate + '</span >'
                    }
                    else {
                        html += ' <span class="save-price font-md color3 ml-15" id=dis_' + response.itemStroes[i].id + '></span >'
                        html += ' <span class="old-price font-md ml-15" id="urate_' + response.itemStroes[i].id + '"></span>'
                    }
                    html += ' </span >'
                    html += ' </div >'

                } else {
                    /*<span class="current-price text-brand" id="disamtS_@item.Id">₹ Not Available</span> */
                }
                //html += ' </div >'





                var cc = 0;
                var unitids = 0;
                var tcount = 0;
                var unitidd = 0;
                var types = "";

                if (response.itemStroes[i].types != "3") {
                    html += '<div class="product-card-bottoms mt-2">'
                    html += '<div class="product-prices">'
                    html += '<select name="itemm" style="box-shadow: 2px 2px 0px 0px #3bb77e;" id="ddlItemunit" onchange="selectonchange(this.value,' + response.itemStroes[i].id + ')">'

                    for (var j = 0; j < response.itemdetailsS.length; j++) {
                        if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {

                            if (cc == 0) {
                                unitids = response.itemdetailsS[j].id;

                                html += '<option value=' + response.itemdetailsS[j].id + '>' + response.itemdetailsS[j].unit_Qty + '</option>'
                                cc++;

                            }
                            else {
                                html += '<option value=' + response.itemdetailsS[j].id + '>' + response.itemdetailsS[j].unit_Qty + '</option>'

                                cc++;
                            }

                        }


                    }
                    html += '</select>'
                    html += '</div>'
                    html += '</div>'
                }
                else {
                    for (var j = 0; j < response.itemdetailsS.length; j++) {
                        if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {
                            unitids = response.itemdetailsS[j].id
                        }

                    }
                }

                html += '<div class="product-card-bottom main-bottom">'
                //for (var j = 0; j < response.itemdetailsS.length; j++) {

                //    if (response.itemdetailsS[j].itemId == response.itemStroes[i].id && response.itemdetailsS[j].id == unitids) {
                //        html += '<div class="product-price mt-2">'
                //        html += '<span id=disamtS_' + response.itemStroes[i].id + '>₹' + response.itemdetailsS[j].disamt + '</span>'
                //        html += '<span class="old-price" id=urate_' + response.itemStroes[i].id + '>₹' + response.itemdetailsS[j].unit_Rate + '</span >'
                //        html += '<input type="hidden" value=' + response.itemdetailsS[j].id + ' id="productdetId_' + response.itemStroes[i].id + '" />'
                //        html += '</div>'
                //    }
                //}
                if (response.itemStroes[i].price == 1) {
                    html += '<div class="detail-qty border radius">'

                    html += '<span class="qty-val" id="qty_' + response.itemStroes[i].id + '">1</span>'

                    html += '<button type="button" style="top: 0px;" class="qty-down"  onclick="plusminus(1,' + response.itemStroes[i].id + ')"   ><i class="fi-rs-angle-small-up"></i></button>'

                    /*html += '<span class="qty-val" id="qty_' + response.itemStroes[i].id + '">1</span>'*/

                    html += '<button type="button" style="bottom: -5px;" class="qty-down" onclick="plusminus(2,' + response.itemStroes[i].id + ')"  ><i class="fi-rs-angle-small-down"></i></button>'
                    html += '</div>'

                }
                //html += '<div class="add-cart">'
                //html += '<a class="button button-add-to-cart" href="#" onclick="addcart(' + response.itemStroes[i].id + ')"><i class="fi-rs-shopping-cart mr-5"></i>Add </a>'

                if (response.itemStroes[i].price == 1) {
                    html += '<div class="add-cart" style="display: flex;gap: 5px;">'
                    html += '<a class="button button-add-to-cart" href="#" onclick="addcart(' + response.itemStroes[i].id + ')"><i class="fi-rs-shopping-cart mr-5"></i>Add </a>'
                    html += '<a class="button button-add-to-cart" href="#" onclick="bindpopup(' + response.itemStroes[i].id + ')"><i class="fi-rs-shopping-cart mr-5"></i>Send query </a>'
                } else {
                    html += '<a class="button button-add-to-cart" href="#" onclick="bindpopup(' + response.itemStroes[i].id + ')"><i class="fi-rs-shopping-cart mr-5"></i>Send query </a>'
                }


                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'


            }

            if (html != "") {
                $("#allp").html(html);
                //$("#allp").trigger('click');

                // location.reload();

                if (typee != 4) {
                    $(".fi-rs-angle-small-down").trigger("click");
                    $("#sortid").html(val);
                }

                if (response.displaymessage != "") {
                    // Add Your item in Wishlist
                    // alert(response.displaymessage);
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                }

            }
            else {
                $("#allp").html('');
                if (response.displaymessage != "") {
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                }
            }

        },
        error: function (e) {
            //$('#divPrint').html(e.responseText);
        }
    });
}




function allproductsortasc(cateid, typee, val,name) {

    $("#hidedropdown").hide();

    var minamt = 0;
     
    var maxamt = 0;
     

    $.ajax({
        type: "POST",
        url: '/Home/allproductcatewiseasc',
        data: { cateid: cateid, typeid: typee, minamt: minamt, maxamt: maxamt, val: val },
        dataType: "JSON",
        success: function (response) {
            var html = '';

            $("#sortid").html(name);
            var count = response.count;

            response.itemdetailsS.length
            var total = "";

            if (response.message == "ok")
                $("#allp").html('');


            for (var i = 0; i < response.itemStroes.length; i++) {


                var itemdetailsid
                for (var j = 0; j < response.itemdetailsS.length; j++) {
                    if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {

                        itemdetailsid = response.itemdetailsS[j].id;
                    }
                }

                html += '<div class="col-lg-1-3 col-md-4 col-12 col-sm-6">'
                html += '<div class="product-cart-wrap mb-30 list-products">'
                html += '<div class="product-img-action-wrap">'
                html += '<div class="product-img product-img-zoom">'
                html += '<input type = "hidden" value = ' + response.itemStroes[i].quantity + ' id="unitqtyid_' + response.itemStroes[i].id + '" />'
                html += '<input type="hidden" value=' + response.itemStroes[i].itemdetailsId + ' id="productdetId_' + response.itemStroes[i].id + '" />'
                html += '<a href="/Product/' + response.itemStroes[i].id + '/' + itemdetailsid + '/' + response.itemStroes[i].itemName.replace(" ", "-") + '/' + response.itemStroes[i].categoryId + '">'
                html += '<img class="default-img" src="/images/Productimage/6/' + response.itemStroes[i].frontimage + '" alt="">'
                html += '<img class="hover-img" src="/images/Productimage/6/' + response.itemStroes[i].backimage + '" alt="">'
                html += '</a>'
                html += '</div>'
                html += '<div class="product-action-1">'
                html += '<a aria-label="Add To Wishlist" class="action-btn" href="#" onclick="Addwishlist(' + response.itemStroes[i].id + ')"><i class="fi-rs-heart"></i></a>'
                    /*'<a aria-label="Add To Wishlist" class="action-btn" href="shop-wishlist.html"><i class="fi-rs-heart"></i></a>'*/
                //if (response.itemStroes[i].price == 1) {
                //    html += ' <a aria-label="Quick view" class="action-btn" onclick="ShowItemView(' + response.itemStroes[i].id + ')"><i class="fi-rs-eye"></i></a>'
                //        /*'<a aria-label="Quick view" class="action-btn" data-bs-      toggle="modal" data-bs-target="#quickViewModal"><i class="fi-rs-eye"></i></a>'*/
                //}
                html += '</div>'
                html += '<div class="product-badges product-badges-position product-badges-mrg">'
                html += '<span class="hot">' + response.itemStroes[i].productTag + '</span>'
                html += '</div>'
                html += '</div>'

                html += '<div class="product-content-wrap">'

                html += '<div class="col-lg-3" id="gifid_' + response.itemStroes[i].id + '" style="display:none;">';
                html += '  <div class="form-group">';
                html += '    <img src="/CompanyLogo/loading-waiting.gif" ';
                html += '      style="width: 68px; height: 57px; margin: -99rem 0px 0 5rem; ';
                html += '      border-radius: 100%; margin-bottom: -81px;" />';
                html += '  </div>';
                html += '</div>';



                html += '<div class="product-category">'
                html += ' <a href="#">' + response.itemStroes[i].category_name + '</a>'
                html += '</div>'
                html += '<h2><a href="/Product/' + response.itemStroes[i].id + '/' + itemdetailsid + '/' + response.itemStroes[i].itemName.replace(" ", "-") + '/' + response.itemStroes[i].categoryId + '">' + response.itemStroes[i].itemName + '</a></h2>'
                html += '<div class="main-data-list">'
                // html += '<div class="product-rate-cover">'
                // //html += '<div class="product-rate d-inline-block">'
                // //html += ' <div class="product-rating" style="width: 90%"></div>'
                // //html += '</div>'
                ///* html += '<span class="font-small ml-5 text-muted"> (4.0)</span>'*/
                // html += '</div>'


                // html +=  '< div class="clearfix product-price-cover" >'
                if (response.itemStroes[i].price == 1) {
                    html += '<div class="product-price primary-color float-left" style="display:((' + response.itemStroes[i].price + ' == 1)?"inline-block":"none") " >'
                    html += '<span style="font-weight: 700;" class="current-price text-brand" id="disamtS_' + response.itemStroes[i].id + '">₹' + response.itemStroes[i].disamt + '</span>'
                    html += '<span>'
                    if (response.itemStroes[i].discount != 0) {
                        html += '<span class="save-price font-md color3 ml-15" style="display:((' + response.itemStroes[i].price + ' == 1)?"inline-block":"none")" id=dis_' + response.itemStroes[i].id + '>' + response.itemStroes[i].discount % +'</span>'
                        html += '<span class="old-price font-md ml-15" style="display:((' + response.itemStroes[i].price + ' == 1)?"inline-block":"none")" id = "urate_' + response.itemStroes[i].id + '" >₹' + response.itemStroes[i].unit_Rate + '</span >'
                    }
                    else {
                        html += ' <span class="save-price font-md color3 ml-15" id=dis_' + response.itemStroes[i].id + '></span >'
                        html += ' <span class="old-price font-md ml-15" id="urate_' + response.itemStroes[i].id + '"></span>'
                    }
                    html += ' </span >'
                    html += ' </div >'

                } else {
                    /*<span class="current-price text-brand" id="disamtS_@item.Id">₹ Not Available</span> */
                }
                //html += ' </div >'                                      





                var cc = 0;
                var unitids = 0;
                var tcount = 0;
                var unitidd = 0;
                var types = "";

                if (response.itemStroes[i].types != "3") {
                    html += '<div class="product-card-bottoms mt-2">'
                    html += '<div class="product-prices">'
                    html += '<select name="itemm" style="box-shadow: 2px 2px 0px 0px #3bb77e;" id="ddlItemunit" onchange="selectonchange(this.value,' + response.itemStroes[i].id + ')">'

                    for (var j = 0; j < response.itemdetailsS.length; j++) {
                        if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {

                            if (cc == 0) {
                                unitids = response.itemdetailsS[j].id;

                                html += '<option value=' + response.itemdetailsS[j].id + '>' + response.itemdetailsS[j].unit_Qty + '</option>'
                                cc++;

                            }
                            else {
                                html += '<option value=' + response.itemdetailsS[j].id + '>' + response.itemdetailsS[j].unit_Qty + '</option>'

                                cc++;
                            }

                        }


                    }
                    html += '</select>'
                    html += '</div>'
                    html += '</div>'
                }
                else {
                    for (var j = 0; j < response.itemdetailsS.length; j++) {
                        if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {
                            unitids = response.itemdetailsS[j].id
                        }

                    }
                }

                html += '<div class="product-card-bottom main-bottom">'
                //for (var j = 0; j < response.itemdetailsS.length; j++) {

                //    if (response.itemdetailsS[j].itemId == response.itemStroes[i].id && response.itemdetailsS[j].id == unitids) {
                //        html += '<div class="product-price mt-2">'
                //        html += '<span id=disamtS_' + response.itemStroes[i].id + '>₹' + response.itemdetailsS[j].disamt + '</span>'
                //        html += '<span class="old-price" id=urate_' + response.itemStroes[i].id + '>₹' + response.itemdetailsS[j].unit_Rate + '</span >'
                //        html += '<input type="hidden" value=' + response.itemdetailsS[j].id + ' id="productdetId_' + response.itemStroes[i].id + '" />'
                //        html += '</div>'
                //    }
                //}

                if (response.itemStroes[i].price == 1) {
                    html += '<div class="detail-qty border radius">'

                    html += '<span class="qty-val" id="quantity_' + response.itemStroes[i].id + '">1</span>'
                    
                    html += '<a href="#" class="qty-up" onclick="changepriceidqty(' + response.itemStroes[i].id + ',2)">';
                    html += '<i class="fi-rs-angle-small-up"></i>';
                    html += '</a>';

                    html += '<a href="#" class="qty-down" onclick="changepriceiddownqty(' + response.itemStroes[i].id + ',1)">';
                    html += '<i class="fi-rs-angle-small-down"></i>';
                    html += '</a>';

                    /*html += '<button type="button" style="top: 0px;" class="qty-up"  onclick="changepriceidqty(' + response.itemStroes[i].id + ',2)"  ><i class="fi-rs-angle-small-up"></i></button>'*/

                   /* html += '<button type="button" style="bottom: -9px;" class="qty-down" onclick="changepriceiddown(' + response.itemStroes[i].id + ',1)"  ><i class="fi-rs-angle-small-down"></i></button>'*/
                   
                    html += '</div>'
                }

                //html += '<div class="add-cart">'
                //html += '<a class="button button-add-to-cart" href="#" onclick="addcart(' + response.itemStroes[i].id + ')"><i class="fi-rs-shopping-cart mr-5"></i>Add </a>'

                if (response.itemStroes[i].price == 1) {
                    html += '<div class="add-cart" style="display: flex;gap: 5px;">'
                    html += '<a class="button button-add-to-cart" href="#" onclick="addcart(' + response.itemStroes[i].id + ')"><i class="fi-rs-shopping-cart mr-5"></i>Add </a>'
                    html += '<a class="button button-add-to-cart" href="#" onclick="bindpopup(' + response.itemStroes[i].id + ')"><i></i>Send query </a>'
                } else {
                    html += '<a class="button button-add-to-cart" href="#" onclick="bindpopup(' + response.itemStroes[i].id + ')"><i></i>Send query </a>'
                }


                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'


            }

            if (html != "") {
                $("#allp").html(html);
                //$("#allp").trigger('click');

                // location.reload();

                //if (typee != 4) {
                //    $(".fi-rs-angle-small-down").trigger("click");
                //    $("#sortid").html(name);
                //}

                if (response.displaymessage != "") {
                    // Add Your item in Wishlist
                    // alert(response.displaymessage);
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                }

            }
            else {
                $("#allp").html('');
                if (response.displaymessage != "") {
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                }
            }

        },
        error: function (e) {
            //$('#divPrint').html(e.responseText);
        }
    });
}

function applybrandfilter() {
    $.ajax({
        url: '/Home/GetBrands',
        type: 'GET',
        datatype: "Json",
        success: function (data) {
            var checkboxes = '';
            $.each(data, function (i, brand) {
                checkboxes += '<input type="checkbox" name="brand" value="' + brand.brand_id + '">' + brand.brand_name + '<br>';
            });
            $('#brandCheckboxes').html(checkboxes);
        },
        error: function () {

        }
    });
}

function brandsearchfilter(cateid, typee, val) {

    var brandcheckbox = $('input[name="brand"]:checked').val();


    $.ajax({
        type: "POST",
        url: '/Home/brandproduct',
        data: { cateid: cateid, typeid: typee, brandcheckbox: brandcheckbox },
        dataType: "JSON",
        success: function (response) {
            var html = '';

            var count = response.count;

            response.itemdetailsS.length
            var total = "";

            if (response.message == "ok")
                $("#allp").html('');


            for (var i = 0; i < response.itemStroes.length; i++) {


                var itemdetailsid
                for (var j = 0; j < response.itemdetailsS.length; j++) {
                    if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {

                        itemdetailsid = response.itemdetailsS[j].id;
                    }
                }

                html += '<div class="col-lg-1-3 col-md-4 col-12 col-sm-6">'
                html += '<div class="product-cart-wrap mb-30 list-products">'
                html += '<div class="product-img-action-wrap">'
                html += '<div class="product-img product-img-zoom">'
                html += '<a href="/Product/' + response.itemStroes[i].id + '/' + itemdetailsid + '/' + response.itemStroes[i].itemName.replace(" ", "-") + '">'
                html += '<img class="default-img" src="/images/Productimage/6/' + response.itemStroes[i].frontimage + '" alt="">'
                html += '<img class="hover-img" src="/images/Productimage/6/' + response.itemStroes[i].backimage + '" alt="">'
                html += '</a>'
                html += '</div>'
                html += '<div class="product-action-1">'
                html += '<a aria-label="Add To Wishlist" class="action-btn" href="shop-wishlist.html"><i class="fi-rs-heart"></i></a>'
                if (response.itemStroes[i].price == 1) {
                    html += '<a aria-label="Quick view" class="action-btn" data-bs-toggle="modal" data-bs-target="#quickViewModal"><i class="fi-rs-eye"></i></a>'
                }
                html += '</div>'
                html += '<div class="product-badges product-badges-position product-badges-mrg">'
                html += '<span class="hot">' + response.itemStroes[i].productTag + '</span>'
                html += '</div>'
                html += '</div>'

                html += '<div class="product-content-wrap">'
                html += '<div class="product-category">'
                html += ' <a href="#">' + response.itemStroes[i].category_name + '</a>'
                html += '</div>'
                html += '<h2><a href="/Product/' + response.itemStroes[i].id + '/' + itemdetailsid + '/' + response.itemStroes[i].itemName.replace(" ", "-") + '">' + response.itemStroes[i].itemName + '</a></h2>'
                html += '<div class="main-data-list">'
                html += '<div class="product-rate-cover">'
                html += '<div class="product-rate d-inline-block">'
                html += ' <div class="product-rating" style="width: 90%"></div>'
                html += '</div>'
                html += '<span class="font-small ml-5 text-muted"> (4.0)</span>'
                html += '</div>'


                var cc = 0;
                var unitids = 0;
                var tcount = 0;
                var unitidd = 0;
                var types = "";

                if (response.itemStroes[i].types != "3") {
                    html += '<div class="product-card-bottoms mt-2">'
                    html += '<div class="product-prices">'
                    html += '<select name="itemm" style="box-shadow: 2px 2px 0px 0px #3bb77e;" id="ddlItemunit" onchange="selectonchange(this.value,' + response.itemStroes[i].id + ')">'

                    for (var j = 0; j < response.itemdetailsS.length; j++) {
                        if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {

                            if (cc == 0) {
                                unitids = response.itemdetailsS[j].id;

                                html += '<option value=' + response.itemdetailsS[j].id + '>' + response.itemdetailsS[j].unit_Qty + '</option>'
                                cc++;

                            }
                            else {
                                html += '<option value=' + response.itemdetailsS[j].id + '>' + response.itemdetailsS[j].unit_Qty + '</option>'

                                cc++;
                            }

                        }


                    }
                    html += '</select>'
                    html += '</div>'
                    html += '</div>'
                }
                else {
                    for (var j = 0; j < response.itemdetailsS.length; j++) {
                        if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {
                            unitids = response.itemdetailsS[j].id
                        }

                    }
                }
                html += '<div class="product-card-bottom main-bottom">'
                for (var j = 0; j < response.itemdetailsS.length; j++) {

                    if (response.itemdetailsS[j].itemId == response.itemStroes[i].id && response.itemdetailsS[j].id == unitids) {
                        html += '<div class="product-price mt-2">'
                        html += '<span id=disamtS_' + response.itemStroes[i].id + '>₹' + response.itemdetailsS[j].disamt + '</span>'
                        html += '<span class="old-price" id=urate_' + response.itemStroes[i].id + '>₹' + response.itemdetailsS[j].unit_Rate + '</span >'
                        html += '<input type="hidden" value=' + response.itemdetailsS[j].id + ' id="productdetId_' + response.itemStroes[i].id + '" />'
                        html += '</div>'
                    }
                }

                html += '<div class="detail-qty border radius">'

                html += '<span class="qty-val" id="qty_' + response.itemStroes[i].id + '">1</span>'

                html += '<button type="button" style="top: 0px;" class="qty-down"  onclick="plusminus(1,' + response.itemStroes[i].id + ')"  ><i class="fi-rs-angle-small-up"></i></button>'

                html += '<button type="button" style="bottom: -5px;" class="qty-down" onclick="plusminus(2,' + response.itemStroes[i].id + ')"  ><i class="fi-rs-angle-small-down"></i></button>'
                html += '</div>'


                //html += '<div class="add-cart">'
                //html += '<a class="button button-add-to-cart" href="#" onclick="addcart(' + response.itemStroes[i].id + ')"><i class="fi-rs-shopping-cart mr-5"></i>Add </a>'

                if (response.itemStroes[i].price == 1) {
                    html += '<div class="add-cart">'
                    html += '<a class="button button-add-to-cart" href="#" onclick="addcart(' + response.itemStroes[i].id + ')"><i class="fi-rs-shopping-cart mr-5"></i>Add </a>'
                    html += '<a class="button button-add-to-cart" href="#" onclick="bindpopup(' + response.itemStroes[i].id + ')"><i></i>Send query </a>'
                } else {
                    html += '<a class="button button-add-to-cart" href="#" onclick="bindpopup(' + response.itemStroes[i].id + ')"><i ></i>Send query </a>'
                }


                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'


            }

            if (html != "") {
                $("#allp").html(html);
                //$("#allp").trigger('click');

                // location.reload();

                if (typee != 4) {
                    $(".fi-rs-angle-small-down").trigger("click");
                    $("#sortid").html(val);
                }

                if (response.displaymessage != "") {
                    // Add Your item in Wishlist
                    // alert(response.displaymessage);
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                }

            }
            else {
                $("#allp").html('');
                if (response.displaymessage != "") {
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                }
            }

        },
        error: function (e) {
            //$('#divPrint').html(e.responseText);
        }
    });
}






function plusminus(qty, itemid) {


    if (qty == 1) {
        var qtyy = $("#qty_" + itemid).html();

        //  alert(qtyy);

        $("#qty_" + itemid).html(parseFloat(qtyy) + 1);
    }
    else {
        var qtyy = $("#qty_" + itemid).html();

        if (qtyy != "1") {
            $("#qty_" + itemid).html(parseFloat(qtyy) - 1);
        }

    }


}





function allproductsearchsort(cateid, typee, val) {


    var minamt = $("#slider-range-value1").html().replace("$", "₹");
    var maxamt = $("#slider-range-value2").html().replace("$", "₹");

    var data = $("#datasearch").val();

    $.ajax({
        type: "POST",
        url: '/Home/allproductsearchcatewise',
        data: { cateid: cateid, typeid: typee, minamt: minamt, maxamt: maxamt, data: data },
        dataType: "JSON",
        success: function (response) {
            var html = '';

            var count = response.count;

            response.itemdetailsS.length
            var total = "";

            if (response.message == "ok")
                $("#allp").html('');


            for (var i = 0; i < response.itemStroes.length; i++) {


                var itemdetailsid
                for (var j = 0; j < response.itemdetailsS.length; j++) {
                    if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {

                        itemdetailsid = response.itemdetailsS[j].id;
                    }
                }

                html += '<div class="col-lg-1-3 col-md-4 col-12 col-sm-6">'
                html += '<div class="product-cart-wrap mb-30 list-products">'
                html += '<div class="product-img-action-wrap">'
                html += '<div class="product-img product-img-zoom">'
                html += '<a href="/Product/' + response.itemStroes[i].id + '/' + itemdetailsid + '/' + response.itemStroes[i].itemName.replace(" ", "-") + '">'
                html += '<img class="default-img" src="/images/Productimage/6/' + response.itemStroes[i].frontimage + '" alt="">'
                html += '<img class="hover-img" src="/images/Productimage/6/' + response.itemStroes[i].backimage + '" alt="">'
                html += '</a>'
                html += '</div>'
                html += '<div class="product-action-1">'
                html += '<a aria-label="Add To Wishlist" class="action-btn" href="shop-wishlist.html"><i class="fi-rs-heart"></i></a>'
                html += '<a aria-label="Quick view" class="action-btn" data-bs-toggle="modal" data-bs-target="#quickViewModal"><i class="fi-rs-eye"></i></a>'
                html += '</div>'
                html += '<div class="product-badges product-badges-position product-badges-mrg">'
                html += '<span class="hot">' + response.itemStroes[i].productTag + '</span>'
                html += '</div>'
                html += '</div>'

                html += '<div class="product-content-wrap">'
                html += '<div class="product-category">'
                html += ' <a href="#">' + response.itemStroes[i].category_name + '</a>'
                html += '</div>'
                html += '<h2><a href="/Product/' + response.itemStroes[i].id + '/' + itemdetailsid + '/' + response.itemStroes[i].itemName.replace(" ", "-") + '">' + response.itemStroes[i].itemName + '</a></h2>'
                html += '<div class="main-data-list">'
                html += '<div class="product-rate-cover">'
                html += '<div class="product-rate d-inline-block">'
                html += ' <div class="product-rating" style="width: 90%"></div>'
                html += '</div>'
                html += '<span class="font-small ml-5 text-muted"> (4.0)</span>'
                html += '</div>'


                var cc = 0;
                var unitids = 0;
                var tcount = 0;
                var unitidd = 0;
                var types = "";

                if (response.itemStroes[i].types != "3") {
                    html += '<div class="product-card-bottoms mt-2">'
                    html += '<div class="product-prices">'
                    html += '<select name="itemm" style="box-shadow: 2px 2px 0px 0px #3bb77e;" id="ddlItemunit" onchange="selectonchange(this.value,' + response.itemStroes[i].id + ')">'

                    for (var j = 0; j < response.itemdetailsS.length; j++) {
                        if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {

                            if (cc == 0) {
                                unitids = response.itemdetailsS[j].id;

                                html += '<option value=' + response.itemdetailsS[j].id + '>' + response.itemdetailsS[j].unit_Qty + '</option>'
                                cc++;

                            }
                            else {
                                html += '<option value=' + response.itemdetailsS[j].id + '>' + response.itemdetailsS[j].unit_Qty + '</option>'

                                cc++;
                            }

                        }


                    }
                    html += '</select>'
                    html += '</div>'
                    html += '</div>'
                }
                else {
                    for (var j = 0; j < response.itemdetailsS.length; j++) {
                        if (response.itemdetailsS[j].itemId == response.itemStroes[i].id) {
                            unitids = response.itemdetailsS[j].id
                        }

                    }
                }
                html += '<div class="product-card-bottom main-bottom">'
                for (var j = 0; j < response.itemdetailsS.length; j++) {

                    if (response.itemdetailsS[j].itemId == response.itemStroes[i].id && response.itemdetailsS[j].id == unitids) {
                        html += '<div class="product-price mt-2">'
                        html += '<span id=disamtS_' + response.itemStroes[i].id + '>₹' + response.itemdetailsS[j].disamt + '</span>'
                        html += '<span class="old-price" id=urate_' + response.itemStroes[i].id + '>₹' + response.itemdetailsS[j].unit_Rate + '</span >'
                        html += '<input type="hidden" value=' + response.itemdetailsS[j].id + ' id="productdetId_' + response.itemStroes[i].id + '" />'
                        html += '</div>'
                    }
                }

                html += '<div class="detail-qty border radius">'

                html += '<span class="qty-val" id="qty_' + response.itemStroes[i].id + '">1</span>'
                html += '<button type="button" style="top: 0px;" class="qty-down"  onclick="plusminus(1,' + response.itemStroes[i].id + ')"  ><i class="fi-rs-angle-small-up"></i></button>'
                html += '<button type="button" style="bottom: -5px;" class="qty-down" onclick="plusminus(2,' + response.itemStroes[i].id + ')"  ><i class="fi-rs-angle-small-down"></i></button>'

                html += '</div>'


                html += '<div class="add-cart">'
                html += '<a class="button button-add-to-cart" href="#" onclick="addcart(' + response.itemStroes[i].id + ')"><i class="fi-rs-shopping-cart mr-5"></i>Add </a>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'
                html += '</div>'


            }

            if (html != "") {
                $("#allp").html(html);
                //$("#allp").trigger('click');

                // location.reload();

                if (typee != 4) {
                    $(".fi-rs-angle-small-down").trigger("click");
                    $("#sortid").html(val);
                }

                if (response.displaymessage != "") {
                    //  alert(response.displaymessage);
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                }

            }
            else {
                $("#allp").html('');
                if (response.displaymessage != "") {
                    //alert(response.displaymessage);
                    swal({
                        title: response.displaymessage,
                        text: "",
                        icon: "success",
                        timer: 10000,
                    });
                }
            }

        },
        error: function (e) {
            //$('#divPrint').html(e.responseText);
        }
    });
}

function AddReview() {
    var Customer_review = $("#txtcomment").val();
    var Customer_name = $("#txtname").val();
    var Customer_Email = $("#email").val();
    var Item_id = $("#productId").val();
    var rating = $('#ddlreview').find("option:selected").val();
    $.ajax({
        type: 'POST',
        url: "/Home/SaveReview",
        data: { Item_id: Item_id, Customer_review: Customer_review, Customer_name: Customer_name, Customer_Email: Customer_Email, rating: rating },
        dataType: 'JSON',
        success: function (result) {
            if (result.message == "Review Submitted") {
                ClearReview();
                // alert("Review Submitted");
                swal({
                    title: "Review Submitted",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
            }
        },
        error: function () {
            ClearReview();
            // alert("Review Submission Failed")
            swal({
                title: "Review Submission Failed",
                text: "",
                icon: "success",
                timer: 10000,
            });
        }
    });
}

function delwishlist(id) {
    var itemdetid = $('#' + 'productdetId_' + id).val();

    $.ajax({
        type: 'POST',
        url: "/Home/delwishlist",
        data: { Item_id: id, itemdetid: itemdetid },
        dataType: 'JSON',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {

            if (result.message == "deleted") {
                swal({
                    title: "Delete Your item in Wishlist",
                    text: "",
                    icon: "success",
                    timer: 10000,
                });
                //alert("Delete Your item in Wishlist");
                location.reload();
            }
        }
    });
}

//---------------------rajesh-----------------

//function bindpopup(Id) {
//    $('#hidid').val(Id);
//    $('#btnAddtoquery').modal("show");
//}
//function Save() {
//    var id = $('#hidid').val();
//    var Firstname = $('#txtFname').val();
//    var ItemName = $('#ItemName_' + id + '').val();
//   var Lastname = $('#txtLname').val();
//    var Email = $('#txtEmail').val();
//    var mobile = $('#txtMobile').val();
//    var Message = $('#txtComments').val();
//    //var CustomerId = $('#hidcustomerid').val();
//    //var ItemId = $('#hiditemid').val();
//    //var Companyid = $('#hidcompanyid').val();
//    var msgd = Validation();
//    if (msgd == "") {
//        $.ajax({
//            type: "POST",
//            url: '/Home/enquiry1',
//            data: { Firstname: Firstname, Lastname: Lastname, Email: Email, mobile: mobile, Message: Message, ItemName: ItemName },
//            dataType: "JSON",
//            success: function (result) {
//                if (result.message == "send enquiry") {
//                    ClearAllField();

//                    swal({
//                        title: "Thank You",
//                        text: "We have received your message and would like to thank you for writing to us.If your inquiry is urgent, please use the telephone numberlisted below to talk to one of our staff members. Otherwise, we will reply by email as soon as possible.",
//                        icon: "success",
//                        timer: 100000,
//                    });

//                }
//                else {
//                    swal({
//                        title: "Please try again",
//                        text: "",
//                        icon: "danger",
//                        timer: 10000,
//                    });
//                }

//            },
//            error: function () {
//                swal({
//                    title: "Please try again",
//                    text: "",
//                    icon: "danger",
//                    timer: 10000,
//                });
//            }
//        });
//    }
//    else {
//        swal({
//            title: msgd,
//            text: "",
//            icon: "",
//            timer: 10000,
//        });
//        return false;
//    }
//}


function bindpopupnew(Id) {
    $('#hidid').val(Id);
    if ($("#hiddenuserid").val() != 0) {

        if ($("#hidstateid").val() != 0 && $("#hidcityid").val() != 0) {

            $("#gifidupnew").show();
            Save1();
            $("#btnAddtoquery").modal("hide");
        }
        else {

            $("#btnAddtoquery").modal("show");

        }
    }
    else {
        var hidcodeid = $("#hidcodeid").val();
        if ($("#hidcodeid").val() == "") {

            $("#btnAddtoquery").modal("show");
            $("#exampleModalCenter").modal("hide");

        }
        else {

            $("#gifidupnew").show();
            Save1();
            $("#btnAddtoquery").modal("hide");
        }

    }
}

function bindpopup(Id) {
    $('#hidid').val(Id);
    if ($("#hiddenuserid").val() != 0) {

        if ($("#hidstateid").val() != 0 && $("#hidcityid").val() != 0) {
            
            $('#gifid_' + Id).show();

            Save1();
            $("#btnAddtoquery").modal("hide");
        }
        else {
            
            $("#btnAddtoquery").modal("show");
          
        }
    }
    else {
        var hidcodeid = $("#hidcodeid").val();
        if ($("#hidcodeid").val() == "") {

            $("#btnAddtoquery").modal("show");
            $("#exampleModalCenter").modal("hide");

        }
        else {

            $('#gifid_' + Id).show();
            Save1();
            $("#btnAddtoquery").modal("hide");
        }
         
    }
}


function DATA() {
    var msgd = Validationpop();
    if (msgd == "") {

        var code1 = $('#code1').val();
        var code2 = $('#code2').val();
        var code3 = $('#code3').val();
        var code4 = $('#code4').val();
        var combinedCode = code1 + code2 + code3 + code4;
        var code = localStorage.getItem("key")

        if (combinedCode == code) {
            $("#gifiddd").show();
            $("#btnAddtoquery").modal("hide");
            Save2();
           


        } else {
            swal({
                title: "Please Enter Correct Verification Code.",
                text: "",
                icon: "danger",
                timer: 10000,
            });

        }

    }
    else {
        swal({
            title: msgd,
            text: "",
            icon: "",
            timer: 10000,
        });
        return false;
    }
}


//document.getElementById('btnSumit').addEventListener('click', function () {
//    $('#btnAddtooquery').modal("show");
//});

function ClearAllField(id) {
    $('#txtFullname').val('');
    $('#ItemName_' + id + '').val('');
   // $('#txtLname').val('');
    $('#txtEmail').val('');
    $('#txtMobile').val('');
    $('#ddlstate').val(0);
    $('#txtcity').val(0);
    $('#txtComments').val('');
    localStorage.removeItem("key");
}


function Save2() {


    var id = $('#hidid').val();
    var Fullname = $('#txtFullname').val();
    var ItemName = $('#ItemName_' + id + '').val();
    // var Lastname = $('#txtLname').val();
    var Email = $('#txtEmail').val();
    var mobile = $('#txtMobile').val();
    var state = $('#ddlstate').val();
    var city = $('#txtcity').val();
    var Message = $('#txtComments').val();
    var user_id = $('#hiddenuserid').val();
    //var CustomerId = $('#hidcustomerid').val();
    //var ItemId = $('#hiditemid').val();
    //var Companyid = $('#hidcompanyid').val();
    var code = localStorage.getItem("key")
    var msgd = Validationsendquery();
    if (msgd == "") {

        $.ajax({
            type: "POST",
            url: '/Home/enquiry1',
            data: { Fullname: Fullname, Email: Email, mobile: mobile, state: state, city: city, Message: Message, ItemName: ItemName, Itemid: id, user_id: user_id, code: code, status: 1 },
            dataType: "JSON",
            success: function (result) {

                if (result.message == "send enquiry") {
                    ClearAllField(id);


                    $('#gifid_' + id).hide();
                    $("#gifiddd").hide();
                    $("#exampleModalCenter").modal("hide");
                    swal({
                       // title: "/*Thank you for Registration .Please check your mail for login details.*/",
                        text: "Thank you for registering with us! We have received your query, and our team will get in touch with you shortly. Please check your email where your login details have been sent.",
                        icon: "success",
                        timer: 100000,
                    }).then(() => {

                        /*if (response.type == "5") {*/
                        location.reload();
                            //window.location.replace("/Login");
                        /*}*/
                    });
                    /*location.reload();*/

                }
                else {
                    swal({
                        title: "Please try again",
                        text: "",
                        icon: "danger",
                        timer: 10000,
                    });
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
    else {
        swal({
            title: msgd,
            text: "",
            icon: "",
            timer: 10000,
        });
        return false;
    }
}

function Save1() {


    var id = $('#hidid').val();
    var Fullname = $('#txtFullname').val();
    var ItemName = $('#ItemName_' + id + '').val();
    // var Lastname = $('#txtLname').val();
    var Email = $('#txtEmail').val();
    var mobile = $('#txtMobile').val();
    var state = $('#ddlstate').val();
    var city = $('#txtcity').val();
    var Message = $('#txtComments').val();
    var user_id = $('#hiddenuserid').val();
    //var CustomerId = $('#hidcustomerid').val();
    //var ItemId = $('#hiditemid').val();
    //var Companyid = $('#hidcompanyid').val();
    var code = localStorage.getItem("key")
    var msgd = Validationsendquery();
    if (msgd == "") {

        $.ajax({
            type: "POST",
            url: '/Home/enquiry1',
            data: { Fullname: Fullname, Email: Email, mobile: mobile, state: state, city: city, Message: Message, ItemName: ItemName, Itemid: id, user_id: user_id, code: code, status :1 },
            dataType: "JSON",
            success: function (result) {

                if (result.message == "send enquiry") {
                    // ClearAllField();


                    $('#gifid_' + id).hide();
                    $("#gifiddd").hide();
                    $("#gifidupnew").hide();
                    $("#exampleModalCenter").modal("hide");
                    swal({
                        title: "Thank You",
                        text: "We have received your query. Our team will get back to you soon.",
                        icon: "success",
                        timer: 100000,
                    });
                    location.reload();

                }
                else {
                    swal({
                        title: "Please try again",
                        text: "",
                        icon: "danger",
                        timer: 10000,
                    });
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
    else {
        swal({
            title: msgd,
            text: "",
            icon: "",
            timer: 10000,
        });
        return false;
    }
}

function verificationave() {
    //var msgd = Validationpop();
    //if (msgd == "") {
    $('#btnAddtoquery').modal("hide");
    $("#exampleModalCenter").modal("show");
    //}
    //else {
    //    swal({
    //        title: msgd,
    //        text: "",
    //        icon: "",
    //        timer: 10000,
    //    });
    //    return false;
    //}
}




function movetoNext(current, nextFieldID) {
    if (current.value.length >= current.maxLength) {
        document.getElementById(nextFieldID).focus();
    }
}

//function lastdata() {
//    document.getElementById("btnlogin").click();
//}



function openPopup() {

    var id = $('#hidid').val();
    var Email = $('#txtEmail').val();
    var mobile = $('#txtMobile').val();
    
        $.ajax({
            type: "POST",
            url: '/Home/openPopup',
            data: { Email: Email, mobile: mobile,Itemid: id,  },
            dataType: "JSON",
            success: function (result) {


                //var data = JSON.parse(result);
                if (result != '[]') {

                    $('#gifid_' + id).show();
                    Save1();
                    $("#btnAddtoquery").modal("hide");
                }
                else {

                    openPopupid();


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



function openPopupid() {

    var id = $('#hidid').val();
    var Fullname = $('#txtFullname').val();
    var ItemName = $('#ItemName_' + id + '').val();
    // var Lastname = $('#txtLname').val();
    var Email = $('#txtEmail').val();
    var mobile = $('#txtMobile').val();
    var state = $('#ddlstate').val();
    var city = $('#txtcity').val();
    var Message = $('#txtComments').val();
    var user_id = $('#hiddenuserid').val();

    var code = Math.floor(1000 + Math.random() * 9000);
    localStorage.setItem("key", code);
    var msgd = Validationsendquery();
    if (msgd == "") {
        $("#gif").show();
        $.ajax({
            type: "POST",
            url: '/Home/enquiry2',
            data: { Fullname: Fullname, Email: Email, mobile: mobile, state: state, city: city, Message: Message, ItemName: ItemName, Itemid: id, user_id: user_id, code: code },
            dataType: "JSON",
            success: function (result) {


                if (result.message == "send otp") {
                    //ClearAllField();
                    $("#gif").hide();
                    $('#btnAddtoquery').modal("hide");
                    $("#exampleModalCenter").modal("show");



                }
                else {
                    swal({
                        title: "Please try again",
                        text: "",
                        icon: "danger",
                        timer: 10000,
                    });
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
    else {
        swal({
            title: msgd,
            text: "",
            icon: "",
            timer: 10000,
        });
        return false;
    }


}




function Validationpop() {
    var msg = "";
    if ($('#code1').val() == "") { msg = "Please Enter Full Verificaton Code   \n"; }
    if ($('#code2').val() == "") { msg = "Please Enter Full Verificaton Code  \n"; }
    if ($('#code3').val() == "") { msg = "Please Enter Full Verificaton Code  \n"; }
    if ($('#code4').val() == "") { msg = "Please Enter Full Verificaton Code  \n"; }

    return msg;
}
function Validationsendquery() {
    var msg = "";
    if ($('#txtFullname').val() == "") { msg = "Full Name can not left Blank !! \n"; }
    //if ($('#txtEmail').val() == "") { msg = "Email can not left Blank !! \n"; }
    //if ($('#txtMobile').val() == "") { msg = "Phone can not left Blank !! \n"; }
    if ($('#txtcity').val() == "") { msg = "City can not left Blank !! \n"; }
    //if ($('#txtComments').val() == "") { msg = "Message can not left Blank !! \n"; }
    if ($('#ddlstate').val() == 0) {
         msg = "State can not left Blank !! \n";
        
    }

    //var Email = $("#txtEmail").val();
    //var emailregex = /^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$/;
    //if ($("#txtEmail").val() == '') {
    //    msg += "Please fill the email !! \n";
    //}
    //else if (!emailregex.test($("#txtEmail").val())) {
    //    msg += "Email is not valid \n";
    //}



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

    //var mobile = document.getElementById('txtMobile');
    //if ($('#txtMobile').val().trim() == "") {
    //    msg = "Please Enter Mobile Number";
    //}
    ////else if (mobile.value.length > 10 || mobile.value.length < 10) {
    ////    //$("#txtMobileNoerror").text("required 10 digits, match requested format!").show();
    ////    // $("#txtMobileNoerror").show();
    ////    // isValid = false;
    ////    msg = "Required 10 digits, match requested format!";
    ////}
    //else {
    //    //  msg = "";
    //}


/*     if (msg != "") { alert(msg); return false; }*/
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
function Validation() {
    var msg = "";
    if ($('#txtFname').val() == "") { msg = "First Name can not left Blank !! \n"; }
    if ($('#txtLname').val() == "") { msg = "Last Name can not left Blank !! \n"; }
    if ($('#txtEmail').val() == "") { msg = "Email can not left Blank !! \n"; }
    if ($('#txtMobile').val() == "") { msg = "Phone can not left Blank !! \n"; }
    //if ($('#txtComments').val() == "") { msg = "Message can not left Blank !! \n"; }
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
        //  msg = "";
    }
    var mobile = document.getElementById('txtMobile');
    if ($('#txtMobile').val().trim() == "") {
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
        //  msg = "";
    }


    // if (msg != "") { alert(msg); return false; }
    return msg;
}











//const data = [];
//function GetItemName() {
//    $.ajax({
//        url: '/Home/GetItemName',
//        data: {},
//        datatype: 'json',
//        type: 'post',
//        success: function (Data) {
//            Data = JSON.parse(Data);
//            console.log(Data);
//            for (var i = 0; i < Data.length; i++) {
//                data[i] = Data[i].Main_cat_id;
//                data[i] = Data[i].category_id;
//                data[i] = Data[i].category_name;

//            }
//        },
//        error: function (errormessage) {
//            alert(result.message);
//        }
//    });
//}
//function autoSearchItem() {

//    const input = document.getElementById('txtitemname').value.toLowerCase();


//    const results = document.getElementById('search-results');
//    results.innerHTML = '';

//    if (input.length === 0) {
//        return;
//    }

//    const filteredData = data.filter(item => item.toLowerCase().includes(input));




//    filteredData.forEach(item => {
//        const div = document.createElement('div');
//        div.textContent = item;
//        div.onclick = () => selectItem(item);
//        results.appendChild(div);

//    });
//}

//function selectItem(item) {
//    document.getElementById('txtitemname').value = item;

//    document.getElementById('search-results').innerHTML = '';
//}

//function FocusItemName() {
//    $("#txtitemname").focus();
//    Hdfc();
//}



//function Hdfc() {

//    var item = $('#txtitemname').val();
//    $.ajax({
//        url: '/Home/Hdfc',
//        data: { item: item },
//        datatype: 'json',
//        type: 'post',
//        success: function (Data) {
//            Data = JSON.parse(Data);
//            console.log(Data);

//            window.location.href = "/Category/" + Data[0].category_id + "/" + Data[0].category_name.replace(" ", "-") + "/2/" + Data[0].Main_cat_id + "";

//        },
//        error: function (errormessage) {
//            alert(result.message);
//        }
//    });
//}

$("#txtitemnameidon").keyup(function (event) {
    if (event.keyCode === 13) {
        PrOsearchid();
    }
});

//document.getElementById('txtitemnameidon').addEventListener('keydown', (e) => {
//    if (e.key === 'Enter') {
//        PrOsearchid(e);
//    }
//});
function PrOsearchid() {


    var cid = $('#dropcatid').val();
    var item = $('#txtitemnameidon').val();
    $.ajax({
        url: '/Home/PrOsearchid',
        data: { item: item, cid: cid },
        datatype: 'json',
        type: 'post',
        success: function (Data) {
            Data = JSON.parse(Data);
            console.log(Data);

            if (Data[0].Main_cat_name != undefined) {

                window.location.href = "/Category/" + Data[0].groupid + "/" + Data[0].Main_cat_name.replace(" ", "-") + "/1/" + Data[0].groupid + "?cat_id=" + cid + "&&itemname=" + item + "";
            } else {

                window.location.href = "/Category/" + Data[0].CategoryID + "/" + Data[0].Category_name.replace(" ", "-") + "/2/" + Data[0].groupid + "?cat_id=" + cid + "&&itemname=" + item + "";
            }


        },
        error: function (errormessage) {
            alert(result.message);
        }
    });
}








function Shortbynameasc(cateid, typee, val) {
    var cid = cateid;
    $.ajax({
        url: '/Home/Shortbynameasc',
        data: { cid: cid },
        datatype: 'json',
        type: 'post',
        success: function (Data) {
            Data = JSON.parse(Data);
            console.log(Data);

            if (Data[0].Main_cat_name != undefined) {

                window.location.href = "/Category/" + Data[0].groupid + "/" + Data[0].Main_cat_name.replace(" ", "-") + "/1/" + Data[0].groupid + "?cat_id=" + cid + "&&itemname=" + item + "";
            } else {

                window.location.href = "/Category/" + Data[0].CategoryID + "/" + Data[0].Category_name.replace(" ", "-") + "/2/" + Data[0].groupid + "?cat_id=" + cid + "&&itemname=" + item + "";
            }


        },
        error: function (errormessage) {
            alert(result.message);
        }
    });
}