function Clear1() {
    ReloadPageWithAreas('Admin', 'Admin', 'Country');
}
function Clear2() {
    ReloadPageWithAreas('Admin', 'Admin', 'State');
}
function Clear3() {
    ReloadPageWithAreas('Admin', 'Admin', 'City');
}

//--------------country---JS----------------------------------------------------------------------

function CountryBind() {
    $("#ddlcountry").prop("disabled", false);
    $.ajax({  //ajax call
        type: "GET",      //method == GET
        url: "/Admin/Admin/OrdersBind", //url to be called
        success: function (json, result) {
            $("#ddlcountry").empty();
            json = json || {};
            $("#ddlcountry").append('<option value="0">Select Country</option>');
            for (var i = 0; i < json.length; i++) {
                $("#ddlcountry").append('<option value="' + json[i].value + '">' + json[i].text + '</option>');
            }
            // GetPatientData();
            $("#ddlcountry").prop("disabled", false);
        },
        error: function () {
            alert("Data Not Found");
        }
    });
}
function ClearCountry() {
    $("#id").val("0");
    $("#txtcountry").val("");
    $('#btnSave').val("SAVE");
}
function IUCountry() {

    var id = $('#id').val();
    var country = $('#txtcountry').val();
    var ButtonValue = $('#btnSave').val();
    var msg = validatecountry();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IUCountry',
            data: { id: id, Country: country },
            dataType: "JSON",
            success: function (result) {
                alert(result.message);
                //if (result.message == "Country added.") {
                //    if (result.id == 0) {
                //        alert("Country added.");
                //    }
                //    else {
                //        alert("Country modify successfully.");
                //    }
                //    ShowDataInTable();
                //}
                ClearCountry();
                ShowDataInTable();
            },
            error: function () {
                alert("Country not added.");
            }
        });

    }
    else
    {
        alert(msg);

    }
}

function ShowDataInTable() {
    var searchitem = $("#txtsearch").val();
    $.ajax({
        url: "/Admin/Admin/ShowCountry",
        dataType: 'JSON',
        type: 'GET',
        data: { searchitem: searchitem },
       // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td>' + item.id + '</td>';
                html += '<td>' + item.country + '</td>';                             
                html += '<td><a class="btn btn-sm" href="#" onclick="return Editbycountry(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return Deletebycountry(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Countrybody').html(html);
        }
    });
}

function Deletebycountry(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });    
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteCountry",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',            
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    ShowDataInTable();
                    alert("Delete Success");
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

function Editbycountry(id) {
    var PetObj = JSON.stringify({ id: id });
    $.ajax({
        url: "/Admin/Admin/EditCountry",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#id').val(result.id);
            $('#txtcountry').val(result.country);

        },
        error: function () {
            alert("Data Not Found !!");
        }
    });
}

function validatecountry() {
    var msg = "";
    if ($('#txtcountry').val() == 0) { msg += "Country Name  can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;
}

//--------------------------------------state------JS---------------------------------------------

function ClearState() {
    $("#stateid").val("0");
    $("#ddlcountry").val("0");
    $('#txtstate').val("");
    $('#btnSave').val("SAVE");
}
function IUState() {
    var id = $('#stateid').val();
    var country_id = $('#ddlcountry').find("option:selected").val();
    var state_name = $('#txtstate').val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();
    var msg = validatestate();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IUState',
            data: { id: id, country_id: country_id, state_name: state_name, status: Status },
            dataType: "JSON",
            success: function (result) {
                alert(result.message);
                //if (result.message == "Country added.") {
                //    if (result.id == 0) {
                //        alert("Country added.");
                //    }
                //    else {
                //        alert("Country modify successfully.");
                //    }
                //    ShowState();
                //}
                ClearState();
                ShowState();
            },
            error: function () {
                alert("Country not added.");
            }
        });

    }

    else {
        alert(msg);
    }
}

$("#data-table").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var ID = currentRow.find("td:eq(0)").html();  
    var status = currentRow.find("td:eq(3)").html();
    //alert(comp_status);
    EditState(ID, status);
});

function EditState(ID, status) {
    var PetObj = JSON.stringify({ ID: ID });
    $.ajax({
        url: "/Admin/Admin/EditState",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#stateid').val(result.id);
            $('#txtstate').val(result.state);
            $('#ddlcountry').val(result.countryname);
            if ((result.status) == 1) {
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

function ShowState() {
    var searchitem = $("#txtsearch").val();
    $.ajax({
        url: "/Admin/Admin/ShowState",
        dataType: 'JSON',
        type: 'GET',
        data: { searchitem: searchitem },
      //  contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.state + '</td>';
                html += '<td>' + item.countryname + '</td>';
                if ((item.status) == 1) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
               // html += '<td><a class="btn btn-sm" href="#" onclick="return EditState(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return DeleteStatebyId(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Statebody').html(html);
        }
    });
}

function DeleteStatebyId(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteState",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    //ShowDataInTable();
                    alert("Delete Success");
                    ShowState();
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
function validatestate() {
    var msg = "";
    if ($('#ddlcountry').val() == 0) { msg += "Country  can not left Blank !! \n"; }
    if ($('#txtstate').val() == "") { msg += "State can not left Blank !! \n"; }
    if ($('#chkStatus').val() == "") { msg += "status can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;
}


//------------------------------------------------city---JS--------------------------------------

function ClearCity() {
    $("#cityid").val("0");
    $("#txtcity").val("");
    $("#ddlstate").val("0");
    $('#btnSave').val("SAVE");
}
function IUCity() {
    var id = $('#cityid').val();
    var state_id = $('#ddlstate').find("option:selected").val();
    var city = $('#txtcity').val();
    var Status = $('#chkStatus').is(':checked') ? 1 : 0;
    var ButtonValue = $('#btnSave').val();


    var msg = validatecity();
    if (msg == "") {
        $.ajax({
            type: "POST",
            url: '/Admin/Admin/IUCity',
            data: { id: id, state_id: state_id, city: city, status: Status },
            dataType: "JSON",
            success: function (result) {
                alert(result.message);
                ShowCity();
                ClearCity();
                //if (result.message = "This City is already exit.") {
                //    alert("This City is already exit.")
                //}
                //else {
                //    if (result.message == "City added.") {
                //        if (result.id == 0) {
                //            alert("City added.");
                //        }
                //        else {
                //            alert("City modify successfully.");
                //        }
                //        ShowCity();
                //    }
                //}
            },
            error: function () {
                alert("City not added.");
            }
        });
    }
    else {
        alert(msg);
    }
}

$("#data-table").on('click', '.btnSelect', function () {
    var currentRow = $(this).closest("tr");
    var ID = currentRow.find("td:eq(0)").html();
    var status = currentRow.find("td:eq(4)").html();
    //alert(comp_status);
    EditCity(ID, status);
});

function EditCity(ID, status) {
    var PetObj = JSON.stringify({ ID: ID });
    // var item;
    $.ajax({
        url: "/Admin/Admin/EditCity",
        data: JSON.parse(PetObj),
        dataType: 'JSON',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $('#btnSave').val("UPDATE");
            $('#cityid').val(result.id);
            $('#txtcity').val(result.city);
            $('#ddlstate').val(result.sid);
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

function DeleteCityById(id) {
    var checkstr = confirm('Are You Sure You Want To Delete This?');
    var PetObj = JSON.stringify({ id: id });
    if (checkstr == true) {
        $.ajax({
            url: "/Admin/Admin/DeleteCity",
            data: JSON.parse(PetObj),
            dataType: 'JSON',
            type: 'POST',
            success: function (result) {
                if (result.msg == "Delete Successfull!!") {
                    //ShowDataInTable();
                    alert("Delete Success");
                    ShowCity();
                    ClearCity();
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
function ShowCity() {

    var searchitem = $("#txtsearch").val();
    $.ajax({
        url: "/Admin/Admin/ShowCity",
        dataType: 'JSON',
        type: 'GET',
        data: { searchitem: searchitem },
       // contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var html = '';
            var index = 1;
            $.each(data, function (key, item) {
                html += '<tr class="hover-primary">';
                // html += '<td>' + index + '</td>';
                html += '<td style="display:none">' + item.id + '</td>';
                html += '<td>' + item.city + '</td>';
                html += '<td>' + item.state + '</td>';
                html += '<td>' + item.countryname + '</td>';
                if ((item.status) == 1) {
                    html += '<td class="active">Active</td>';
                } else {
                    html += '<td class="active">Inactive</td>';
                }
                html += '<td><button class="btnSelect"><a class="btn btn-sm" href="#"><i class="fa fa-edit"></i></a></button></td>';
               // html += '<td><a class="btn btn-sm" href="#" onclick="return EditCity(' + item.id + ')"><i class="fa fa-edit"></i></a></td>';
                html += '<td><a class="btn btn-sm" href="#" onclick="return DeleteCityById(' + item.id + ')"><i class="fa fa-trash-o"></i></a></td>';
                html += '</tr>';
                index++;
            });
            $('.Citybody').html(html);
        }
    });
}

function validatecity() {
    var msg = "";
    if ($('#ddlstate').val() == 0) { msg += "State  can not left Blank !! \n"; }
    if ($('#txtcity').val() == "") { msg += "City can not left Blank !! \n"; }
    if ($('#chkStatus').val() == "") { msg += "status can not left Blank !! \n"; }
    //if (msg != "") { alert(msg); return false; }
    return msg;
}







