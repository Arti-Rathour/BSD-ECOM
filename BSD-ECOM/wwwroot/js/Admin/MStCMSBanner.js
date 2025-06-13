var filenameorginal = "";
var bfilenameorginal = "";
var tilenameorginal = "";
var ffilenameorginal = "";
var mfilenameorginal = "";
var cilenameorginal = "";
$(document).ready(function () {
    $("#imagefile").change(function () {
        filenameorginal = $('#imagefile').val();
    })
    $("#bimagefile").change(function () {
        bfilenameorginal = $('#bimagefile').val();
    })
    $("#timagefile").change(function () {
        tfilenameorginal = $('#timagefile').val();
    })
    $("#fimagefile").change(function () {
        ffilenameorginal = $('#fimagefile').val();
    })
    $("#mimagefile").change(function () {
        mfilenameorginal = $('#mimagefile').val();
    })
    $("#cimagefile").change(function () {
        cfilenameorginal = $('#cimagefile').val();
    })
})
imagefile.onchange = evt => {
    const [file] = imagefile.files
    if (file) {
        img.src = URL.createObjectURL(file)
    }
}
bimagefile.onchange = evt => {
    const [file] = bimagefile.files
    if (file) {
        bimg.src = URL.createObjectURL(file)
    }
}
timagefile.onchange = evt => {
    const [file] = timagefile.files
    if (file) {
        timg.src = URL.createObjectURL(file)
    }
}
fimagefile.onchange = evt => {
    const [file] = fimagefile.files
    if (file) {
        fimg.src = URL.createObjectURL(file)
    }
}
mimagefile.onchange = evt => {
    const [file] = mimagefile.files
    if (file) {
        mimg.src = URL.createObjectURL(file)
    }
}
cimagefile.onchange = evt => {
    const [file] = cimagefile.files
    if (file) {
        cimg.src = URL.createObjectURL(file)
    }
}
function Save() {
    var ID = $('#ID').val();
    var fileExtension = ['png', 'img', 'jpg'];
    var TUrl1 = $('#txtTUrl1').val();
    var TUrl2 = $('#txtTUrl2').val();
    var TUrl3 = $('#txtTUrl3').val();
    var FUrl2 = $('#txtFUrl2').val();
    var MUrl = $('#txtMUrl').val();
    var category_bannerUrl = $('#txtcategory_bannerUrl').val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var fdata = new FormData();
    var filename = $('#imagefile').val();
    if (filenameorginal == filename) {
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
        fdata.append("flg", "okg");
        var fileUpload = $("#imagefile").get(0);
        var files = fileUpload.files;
        fdata.append(files[0].name, files[0]);
    }
    else {
        fdata.append("flg", "ok");
    }

    var bfilename = $('#bimagefile').val();
    if (bfilenameorginal == bfilename) {
        if (bfilename.length == 0) {
            alert("Please select b file.");
            return false;
        }
        else {
            var extension1 = bfilename.replace(/^.*\./, '');
            if ($.inArray(extension1, fileExtension) == -1) {
                alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("bflg", "okg");
        var fileUploadb = $("#bimagefile").get(0);
        var bfiles = fileUploadb.files;
        fdata.append(bfiles[0].name, bfiles[0]);
    }
    else {
        fdata.append("bflg", "ok");
    }


    var tilename = $('#timagefile').val();
    if (tfilenameorginal == tilename) {
        if (tilename.length == 0) {
            alert("Please select a file.");
            return false;
        }
        else {
            var extension2 = tilename.replace(/^.*\./, '');
            if ($.inArray(extension2, fileExtension) == -1) {
                alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("tflg", "okg");
        var tfileUpload = $("#timagefile").get(0);
        var tfiles = tfileUpload.files;
        fdata.append(tfiles[0].name, tfiles[0]);
    }
    else {
        fdata.append("tflg", "ok");
    }


    var ffilename = $('#fimagefile').val();
    if (ffilenameorginal == ffilename) {
        if (ffilename.length == 0) {
            alert("Please select f file.");
            return false;
        }
        else {
            var extension3 = ffilename.replace(/^.*\./, '');
            if ($.inArray(extension3, fileExtension) == -1) {
                alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("fflg", "okg");
        var FfileUpload = $("#fimagefile").get(0);
        var ffiles = FfileUpload.files;
        fdata.append(ffiles[0].name, ffiles[0]);

    }
    else {
        fdata.append("fflg", "ok");
    }


    var mfilename = $('#mimagefile').val();
    if (mfilenameorginal == mfilename) {
        if (mfilename.length == 0) {
            alert("Please select m file.");
            return false;
        }
        else {
            var extension4 = mfilename.replace(/^.*\./, '');
            if ($.inArray(extension4, fileExtension) == -1) {
                alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("mflg", "okg");
        var mfileUpload = $("#mimagefile").get(0);
        var mfiles = mfileUpload.files;
        fdata.append(mfiles[0].name, mfiles[0]);
    }
    else {
        fdata.append("mflg", "ok");
    }

    var cfilename = $('#cimagefile').val();
    if (cfilenameorginal == cfilename) {
        if (cfilename.length == 0) {
            alert("Please select c file.");
            return false;
        }
        else {
            var extension5 = cfilename.replace(/^.*\./, '');
            if ($.inArray(extension5, fileExtension) == -1) {
                alert("Please select only PNG/IMG/JPG files.");
                return false;
            }
        }
        fdata.append("cflg", "okg");
        var CfileUpload = $("#cimagefile").get(0);
        var cfiles = CfileUpload.files;
        fdata.append(cfiles[0].name, cfiles[0]);
    }
    else {
        fdata.append("cflg", "ok");
    }

    fdata.append("ID", ID);
    fdata.append("TUrl1", TUrl1);
    fdata.append("TUrl2", TUrl2);
    fdata.append("TUrl3", TUrl3);
    fdata.append("FUrl2", FUrl2);
    fdata.append("MUrl", MUrl);
    fdata.append("category_bannerUrl", category_bannerUrl);
    fdata.append("Status", Status);
    fdata.append("filenmes", filenameorginal);
    fdata.append("bfilenmes", bfilenameorginal);
    fdata.append("tfilenmes", tfilenameorginal);
    fdata.append("ffilenmes", ffilenameorginal);
    fdata.append("mfilenmes", mfilenameorginal);
    fdata.append("cfilenmes", cfilenameorginal);
    $.ajax({
        type: "POST",
        url: '/Admin/Admin/SaveMStCMSBanner_srv',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: fdata,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result.message == "Banner added") {
                if (result.id == 0) {
                    alert("Banner added successfully.");
                }
                else {
                    alert("Banner modify successfully.");
                }
            }
            $('#ID').val("0");
        },
        error: function () {
            alert("Some thing Error.");
        }
    });
}
function Clear() {
    ReloadPageWithAreas('Admin', 'Admin', 'Locality');
}
function ShowDataInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowBannerService",
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td style="display:none">' + item.companyId + '</td>';
                html += '<td>' + item.turl1 + '</td>';
                html += '<td><img src="/images/Banner/BSD INFOTECH/' + item.tbanner1 + '"  width="75" height="80" /></td>';
                html += '<td>' + item.turl2 + '</td>';
                html += '<td><img src="/images/Banner/BSD INFOTECH/' + item.tbanner2 + '"  width="75" height="80" /></td>';
                html += '<td>' + item.turl3 + '</td>';
                html += '<td><img src="/images/Banner/BSD INFOTECH/' + item.tbanner3 + '"  width="75" height="80" /></td>';
                if ((item.brand_status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                //html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyid(' + item.brand_id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            if (html != null) {
                $('.tbodyData').html(html);
            }
            else {
                $('.tbodyData').html("Data Not Available.");
            }
        }
    });
}
function ShowFooterDatatInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowFooterBanner",
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
                html += '<td><img src="/images/Banner/BSD INFOTECH/' + item.fbanner2 + '"  width="75" height="80" /></td>';
                html += '<td>' + item.furl2 + '</td>';
                if ((item.status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '</tr>';
                index++;
            });
            $('.tbodyfooterData').html(html);
        }
    });
}
function ShowMainDataInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowMainBanner",
        dataType: 'JSON',
        type: 'POST',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td><img src="/images/Banner/BSD INFOTECH/' + item.hbanner + '"  width="75" height="80" /></td>';
                html += '<td>' + item.murl + '</td>';
                if ((item.status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '</tr>';
                index++;
            });
            $('.tbodymainData').html(html);
        }
    });
}
function ShowCategoryDataInTable() {
    $.ajax({
        url: "/Admin/Admin/ShowCategoryBanner",
        dataType: 'JSON',
        type: 'POST',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.companyId + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td><img src="/images/Banner/BSD INFOTECH/' + item.category_banner + '"  width="75" height="80" /></td>';
                html += '<td>' + item.category_bannerUrl + '</td>';
                if ((item.status) == true) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"  onclick="return Editbyid(' + item.id + ')"><i class="fa fa-edit"></i></a></button></td>';
                /*   html += '<td><a class="btn btn-sm" href="#" onclick="return Editbyid(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';*/
                html += '<td><a class="btn btn-sm" href="#" onclick="return DeletebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.tbodycategoryData').html(html);
        }
    });
}
function Editbyid(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditBanner",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val = "UPDATE";
            $('#ID').val(result.id);
            $('#img').attr('src', "/images/Banner/BSD INFOTECH/" + result.tBanner1 + "");
            $('#txtTUrl1').val(result.tUrl1);
            $('#bimg').attr('src', "/images/Banner/BSD INFOTECH/" + result.tBanner2 + "");
            $('#txtTUrl2').val(result.tUrl2);
            $('#timg').attr('src', "/images/Banner/BSD INFOTECH/" + result.tBanner3 + "");
            $('#txtTUrl3').val(result.TUrl3);
            $('#fimg').attr('src', "/images/Banner/BSD INFOTECH/" + result.fBanner2 + "");
            $('#txtFUrl2').val(result.fUrl2);
            $('#mimg').attr('src', "/images/Banner/BSD INFOTECH/" + result.hBanner + "");
            $('#txtMUrl').val(result.mUrl);
            $('#cimg').attr('src', "/images/Banner/BSD INFOTECH/" + result.category_banner + "");
            $('#txtcategory_bannerUrl').val(result.category_bannerUrl);
            if ((status) == "Active") {
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
