var datatable;
var pageNumber = 1;
var viewportHeight;
var scrolltop;
var bottomOfTable;
var gotToServer = true;

var region = "";
var errorValue = 0;
var seedValue = 1;

$(document).ready(function () {
    $('#regionId').change(function () {
        region = $('#regionId').val();
        datatable.destroy();
        LoadUsers();
    });
    $('#seedInput').on('input', function () {
        seedValue = $('#seedInput').val();
        if (seedValue == '') {
            seedValue = 1;
        }
        datatable.destroy();
        LoadUsers();
    });

    $('#errorSlide').change(function () {
        errorValue = $('#errorSlide').val();
        $('#errorInput').val(errorValue);
        datatable.destroy();
        LoadUsers();
    });

    $('#errorInput').on('input', function () {
        errorValue = $(this).val();
        if (errorValue == '') {
            errorValue = 0;
        }
        if (errorValue < 0 || errorValue > 1000) {
            errorValue = errorValue < 0 ? 0 : 1000;
            $(this).val(errorValue);
        }
        $('#errorSlide').val(errorValue);
        datatable.destroy();
        LoadUsers();
    });
    $('#errorInput').blur(function () {
        $('#errorInput').val(errorValue);
    });
    $('#seedInput').blur(function () {
        $('#seedInput').val(seedValue);
    });
    $('#btnRandomSeed').on('click', function () {
        GenerateSeed();
        datatable.destroy();
        LoadUsers();
    });
    $('#btnExport').on('click', function () {
        datatable.button('.buttons-csv').trigger();
    })

    GenerateSeed();
    LoadUsers();
});

function GenerateSeed() {
    //seedValue = Math.floor(Math.random() * (Number.MAX_SAFE_INTEGER - 1)) + 1;
    seedValue = Math.floor(Math.random() * (999999 - 1)) + 1;
    $('#seedInput').val(seedValue);
}
function LoadUsers() {
    pageNumber = 1;
    gotToServer = true;
    datatable = $('#tblData').DataTable({
        "processing": true,
        "searching": false,
        "cache": true,
        "lengthChange": false,
        "info": false,
        "paging": false,
        "ordering": false,
        "ajax": {
            "url": `/Home/GetUserData?page=${pageNumber}&region=${region}&seed=${seedValue}&error=${errorValue}`,
            "type": "GET",
        },
        "columnDefs": [
            {
                "targets": 0,
                "render": function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { "className": "dt-center", "targets": "_all" }
        ],
        "order": [[1, 'asc']],
        "columns": [
            { data: null, width: '2%' },
            { data: 'userId', width: '13%' },
            { data: 'name', width: '10%' },
            { data: 'address', width: '12%' },
            { data: 'phone', width: '7%' },
        ],
        buttons: [
            {
                extend: 'csv',
                className: 'csv',
                filename: 'RandomUser_data',
            }
        ],
    });
};
function LoadUserPagination() {
    if (gotToServer === true) {
        gotToServer = false;
        pageNumber++;
        console.log("Page no: " + pageNumber);
        $.ajax({
            "url": `/Home/GetUserData?page=${pageNumber}&region=${region}&seed=${seedValue}&error=${errorValue}`,
            "type": "GET",
            success: function (data) {
                datatable.rows.add(data.data);
                datatable.draw();
                $(window).trigger("newfetch");
                gotToServer = true;
            }
        });
    }
}

$(window).bind('newfetch', function () {
    viewportHeight = window.innerHeight;
    bottomOfTable = $('#tblData').offset().top + $('#tblData').outerHeight();
});

$(window).bind('scroll', function () {
    $(window).trigger("newfetch");
    scrolltop = $(window).scrollTop();
    if ((scrolltop + viewportHeight) >= (bottomOfTable - 100)) {
        LoadUserPagination();
    }
});