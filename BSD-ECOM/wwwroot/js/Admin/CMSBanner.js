var filenameorginal1 = "";
var filenameorginal2 = "";
var filenameorginal3 = "";
var filenameorginal4 = "";
var filenameorginal5 = "";
var filenameorginal6 = "";
$(document).ready(function () {

    MainBannerShow();
    Footer3BannerShow();
    CatagroyeBannerShow();

    filenameorginal6 = $('#imgfile6name').val();


    $("#imgfile1").change(function () {
        filenameorginal1 = $('#imgfile1').val();
    })
    $('#imgfile1').bind('change', function () {
        var fileExtension = ['png', 'img', 'jpg'];
        var filename1 = $('#imgfile1').val();
        var extension = filename1.replace(/^.*\./, '');
        if ($.inArray(extension, fileExtension) == -1) {
            alert("Please select only PNG/IMG/JPG files.");
            return false;
            $(this).val('');
        }
        var a = (this.files[0].size);
        //alert(a);
        //if (a > 1000000) {
        //    alert('Please Upload Photo1 Less Than 100kb');
        //    $(this).val('');
        //};
    });
    $("#imgfile2").change(function () {
        filenameorginal2 = $('#imgfile2').val();
    })
    $('#imgfile2').bind('change', function () {
        var fileExtension = ['png', 'img', 'jpg'];
        var filename2 = $('#imgfile2').val();
        var extension = filename2.replace(/^.*\./, '');
        if ($.inArray(extension, fileExtension) == -1) {
            alert("Please select only PNG/IMG/JPG files.");
            return false;
            $(this).val('');
        }
        var a = (this.files[0].size);
        //alert(a);
        //if (a > 1000000) {
        //    alert('Please Upload Photo2 Less Than 100kb');
        //    $(this).val('');
        //};
    });
    $("#imgfile3").change(function () {
        filenameorginal3 = $('#imgfile3').val();
    })
    $('#imgfile3').bind('change', function () {
        var fileExtension = ['png', 'img', 'jpg'];
        var filename3 = $('#imgfile3').val();
        var extension = filename3.replace(/^.*\./, '');
        if ($.inArray(extension, fileExtension) == -1) {
            alert("Please select only PNG/IMG/JPG files.");
            return false;
            $(this).val('');
        }
        var a = (this.files[0].size);
        //alert(a);
        //if (a > 1000000) {
        //    alert('Please Upload Photo3 Less Than 100kb');
        //    $(this).val('');
        //};
    });
    $("#imgfile4").change(function () {
        filenameorginal4 = $('#imgfile4').val();
    })
    $('#imgfile4').bind('change', function () {
        var fileExtension = ['png', 'img', 'jpg'];
        var filename4 = $('#imgfile4').val();
        var extension = filename4.replace(/^.*\./, '');
        if ($.inArray(extension, fileExtension) == -1) {
            alert("Please select only PNG/IMG/JPG files.");
            return false;
            $(this).val('');
        }
        var a = (this.files[0].size);
        //alert(a);
        //if (a > 1000000) {
        //    alert('Please Upload Photo4 Less Than 100kb');
        //    $(this).val('');
        //};
    });
    $("#imgfile5").change(function () {
        filenameorginal5 = $('#imgfile5').val();
    })
    $('#imgfile5').bind('change', function () {
        var fileExtension = ['png', 'img', 'jpg'];
        var filename5 = $('#imgfile5').val();
        var extension = filename5.replace(/^.*\./, '');
        if ($.inArray(extension, fileExtension) == -1) {
            alert("Please select only PNG/IMG/JPG files.");
            return false;
            $(this).val('');
        }
        var a = (this.files[0].size);
        //alert(a);
        //if (a > 1000000) {
        //    alert('Please Upload Photo5 Less Than 100kb');
        //    $(this).val('');
        //};
    });


    $("#imgfile6").change(function () {
        filenameorginal6 = $('#imgfile6').val();
    })
    $('#imgfile6').bind('change', function () {
        var fileExtension = ['png', 'img', 'jpg'];
        var filename6 = $('#imgfile6').val();
        var extension = filename6.replace(/^.*\./, '');
        if ($.inArray(extension, fileExtension) == -1) {
            alert("Please select only PNG/IMG/JPG files.");
            return false;
            $(this).val('');
        }
        var a = (this.files[0].size);
        //alert(a);
        //if (a > 1000000) {
        //    alert('Please Upload Category Photo Less Than 100kb');
        //    $(this).val('');
        //};
    });
})


function Savebanner() {
    if (window.FormData !== undefined) {
        var fileExtension = ['png', 'img', 'jpg', 'jpeg', 'PNG', 'IMG', 'JPG', 'JPEG',];
        var item = new Array();
        var data1 = {};
        var data2 = {};
        var data3 = {};
        var data4 = {};
        var filename1 = $('#imgfile1').val();
        var filename2 = $('#imgfile2').val();
        var filename3 = $('#imgfile3').val();
        var filename4 = $('#imgfile4').val();
        var filename5 = $('#imgfile5').val();
        var filename6 = $('#imgfile6').val();
        var filenameorginal6 = $('#imgfile6name').val();
        var filenameorginal5 = $('#imgfile5name').val();
        var filenameorginal1 = $('#imgfile1name').val();
        data1.Status = $('#chkStatus').is(":checked") ? 1 : 0;
        data1.id = $('#txtHiddenId').val();
        data1.id = $('#txtHiddenfooterId').val();
        data1.Url1 = $('#txturl1').val();
        data1.Url2 = $('#txturl2').val();
        data1.Url3 = $('#txturl3').val();
        if ($('#txturl1').val() != "" || $('#txturl2').val() != "" || $('#txturl3').val() != "") {
            data1.typeid = 2;
            
            item.push(data1);
        }
        data2.Status = $('#chkStatus').is(":checked") ? 1 : 0;
        data2.id = $('#txtHiddenId').val();
        data2.Url4 = $('#txturl4').val();
        if ($('#txturl4').val() != "") {
            data2.typeid = 4;
            data2.imag = "";
            item.push(data2);
        }
        data3.Status = $('#chkStatus').is(":checked") ? 1 : 0;
        data3.id = $('#txtHiddenId').val();
        data3.id = $('#txtHiddenmainId').val();
        data3.Url5 = $('#txturl5').val();
        if ($('#txturl5').val() != "") {
            data3.typeid = 1;
            data3.imag = "";
            item.push(data3);
        }
        data4.Status = $('#chkStatus').is(":checked") ? 1 : 0;
        data4.id = $('#txtHiddenId').val();
        data4.id = $('#txtHiddencatagroyeId').val();
        data4.Url6 = $('#txturl6').val();
        if ($('#txturl6').val() != "") {
            data4.typeid = 3;
            data4.imag = "";
            item.push(data4);
        }
        
        var fdata = new FormData();
        /*------------------------------------------*/
        if (filenameorginal1 == filename1) {
            fdata.append("flg1", "okg1");
            var fileUpload1 = $("#imgfile1").get(0);
            var files1 = fileUpload1.files;
            fdata.append('filenmes1', files1[0]);
        }
        else {
            fdata.append("flg1", "ok1");
        }
        /*------------------------------------------*/
        if (filenameorginal2 == filename2) {
            fdata.append("flg2", "okg2");
            var fileUpload2 = $("#imgfile2").get(0);
            var files2 = fileUpload2.files;
            fdata.append('filenmes2', files2[0]);
        }
        else {
            fdata.append("flg2", "ok2");
        }
        /*------------------------------------------*/
        if (filenameorginal3 == filename3) {
            fdata.append("flg3", "okg3");
            var fileUpload3 = $("#imgfile3").get(0);
            var files3 = fileUpload3.files;
            fdata.append('filenmes3', files3[0]);
        }
        else {
            fdata.append("flg3", "ok3");
        }
        /*------------------------------------------*/
        if (filenameorginal4 == filename4) {
            fdata.append("flg4", "okg4");
            var fileUpload4 = $("#imgfile4").get(0);
            var files4 = fileUpload4.files;
            fdata.append('filenmes4', files4[0]);
        }
        else {
            fdata.append("flg4", "ok4");
        }
        /*------------------------------------------*/
        if (filenameorginal5 == filename5) {
            fdata.append("flg5", "okg5");
            var fileUpload5 = $("#imgfile5").get(0);
            var files5 = fileUpload5.files;
            fdata.append('filenmes5', files5[0]);
        }
        else {
            fdata.append("flg5", "ok5");
        }

        /*------------------------------------------*/
        if (filenameorginal6 == filename6) {
            fdata.append("flg6", "okg6");
            var fileUpload5 = $("#imgfile6").get(0);
            var files5 = fileUpload5.files;
            fdata.append('filenmes6', files5[0]);
        }
        else {
            fdata.append("flg6", "ok6");
        }

        

        fdata.append("filenmes1", filenameorginal1);
        fdata.append("filenmes2", filenameorginal2);
        fdata.append("filenmes3", filenameorginal3);
        fdata.append("filenmes4", filenameorginal4);
        fdata.append("filenmes5", filenameorginal5);
        fdata.append("filenmes6", filenameorginal6);
        fdata.append("item", JSON.stringify(item));
        //var vali = validation();
        //if (vali == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/SaveCMSBanner',
            dataType: "JSON",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: fdata,
            contentType: false,
            processData: false,
            success: function (result) {
                alert(result.message);
                //clear();
                //ShowDataInTable();

                MainBannerShow();
                Footer3BannerShow();
                CatagroyeBannerShow();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
        //}
        //else {
        //    alert(vali);
        //}
    }
    else {
        alert("Your browser doesn support FormData");
    }
}




function MainBannerShow() {
   
    $.ajax({
        url: "/Admin/Admin/MainBannerShowid",
        data: { },
        dataType: 'JSON',
        type: 'POST',
        
        success: function (data) {

            



                var html = '';
                var index = 1;


           
            $.each(data, function (key, item) {

              

                    html += '<tr class="hover-primary">';

                    html += '<td style="display:none">' + item.id + '</td>';

                   /* html += '<td>' + item.hBanner + '</td>';*/

                html += '<td><img src="/images/Banner/6/' + item.hBanner + '" alt="your image" width="75" height="80" /></td>';


                    html += '<td>' + item.mUrl + '</td>';

                    html += '<td  style="display:none">' + item.typeid + '</td>';

                    if (item.active == 1) {
                        html += '<td>Active</td>';
                    }
                    else {
                        html += '<td>InActive</td>';
                    }
                html += '<td><a class="btn btn-sm" href="#"  onclick="return EditMainBannerShow(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';

                html += '<td><a class="btn btn-sm" href="#" onclick="return DeleteMainBannerShowid(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                    html += '</tr>';
                    index++;
             });
                $('.tbodymainData').html(html);


        }



        

    });
}



function EditMainBannerShow(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditMainBannerShow",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#txtHiddenmainId').val(result[0].id);
            $('#txturl5').val(result[0].mUrl);
            $('#imgfile5name').val(result[0].hBanner);
            if ((result[0].active) == 1) {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }


        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}

function DeleteMainBannerShowid(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteMainBannerShowid",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    alert("Delete Success");
                    MainBannerShow();
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





function Footer3BannerShow() {

    $.ajax({
        url: "/Admin/Admin/Footer3BannerShow",
        data: {},
        dataType: 'JSON',
        type: 'POST',

        success: function (data) {





            var html = '';
            var index = 1;



            $.each(data, function (key, item) {



                html += '<tr class="hover-primary">';

                html += '<td style="display:none">' + item.id + '</td>';

                html += '<td><img src="/images/Banner/6/' + item.hBanner + '" alt="your image" width="75" height="80" /></td>';



                html += '<td>' + item.mUrl + '</td>';

                html += '<td  style="display:none">' + item.typeid + '</td>';

                if (item.active == 1) {
                    html += '<td>Active</td>';
                }
                else {
                    html += '<td>InActive</td>';
                }
                html += '<td><a class="btn btn-sm" href="#" onclick="return EditFooter3BannerShow(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';

                html += '<td><a class="btn btn-sm" href="#" onclick="return DeleteFooter3BannerShow(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodyData').html(html);


        }





    });
}



function EditFooter3BannerShow(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditFooter3BannerShow",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#txtHiddenfooterId').val(result[0].id);
            $('#txturl1').val(result[0].mUrl);
            $('#imgfile1name').val(result[0].hBanner);
            if ((result[0].active) == 1) {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }

           

        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}


function DeleteFooter3BannerShow(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteFooter3BannerShow",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    alert("Delete Success");
                    CatagroyeBannerShow();
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






function CatagroyeBannerShow() {

    $.ajax({
        url: "/Admin/Admin/CatagroyeBannerShow",
        data: {},
        dataType: 'JSON',
        type: 'POST',

        success: function (data) {





            var html = '';
            var index = 1;



            $.each(data, function (key, item) {



                html += '<tr class="hover-primary">';

                html += '<td style="display:none">' + item.id + '</td>';

                html += '<td><img src="/images/Banner/6/' + item.hBanner + '" alt="your image" width="75" height="80" /></td>';



                html += '<td>' + item.mUrl + '</td>';

                html += '<td  style="display:none">' + item.typeid + '</td>';

                if (item.active == 1) {
                    html += '<td>Active</td>';
                }
                else {
                    html += '<td>InActive</td>';
                }
                html += '<td><a class="btn btn-sm" href="#" onclick="return EditCatagroyeBannerShow(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';

                html += '<td><a class="btn btn-sm" href="#" onclick="return DeleteCatagroyeBannerShow(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodycategoryData').html(html);


        }





    });
}




function EditCatagroyeBannerShow(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditCatagroyeBannerShow",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#txtHiddencatagroyeId').val(result[0].id);
            $('#txturl6').val(result[0].mUrl);
            /*$('#imgfile6').val(result[0].hBanner);*/
            $('#imgfile6name').val(result[0].hBanner);
            if ((result[0].active) == 1) {
                $("#chkStatus").prop("checked", true);
            }
            else {
                $('#chkStatus').prop('checked', false);
            }
            
            
        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}



function DeleteCatagroyeBannerShow(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    // var item;
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteCatagroyeBannerShow",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    alert("Delete Success");
                    CatagroyeBannerShow();
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
