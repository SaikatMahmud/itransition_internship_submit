function getSelectedIds() {
    var checkedIds = $('input[name="idSelection"]:checkbox:checked').map(function () {
        return $(this).val();
    }).toArray();
    return checkedIds;
}

function blockUsers() {
    var ids = getSelectedIds();
    if (ids.length == 0) {
        toastr.error("Select user(s) first!");
        return;
    }
    $.ajax({
        url: '/user/block',
        type: 'PUT',
        data: { Ids: ids },
        success: function (response) {
            debugger;
            if (response.success) {
                window.location.reload();
            }
            else {
            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
}

function unblockUsers() {
    var ids = getSelectedIds();
    if (ids.length == 0) {
        toastr.error("Select user(s) first!");
        return;
    }
    $.ajax({
        url: '/user/unblock',
        type: 'PUT',
        data: { Ids: ids },
        success: function (response) {
            debugger;
            if (response.success) {
                window.location.reload();

            }
            else {

            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
}
function deleteUsers() {
    var ids = getSelectedIds();
    if (ids.length == 0) {
        toastr.error("Select user(s) first!");
        return;
    }
    $.ajax({
        url: '/user/delete',
        type: 'PUT',
        data: { Ids: ids },
        success: function (response) {
            debugger;
            if (response.success) {
                window.location.reload();

            }
            else {

            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
}

function makeAdmin() {
    var ids = getSelectedIds();
    if (ids.length == 0) {
        toastr.error("Select user(s) first!");
        return;
    }
    $.ajax({
        url: '/user/make-admin',
        type: 'PUT',
        data: { Ids: ids },
        success: function (response) {
            debugger;
            if (response.success) {
                window.location.reload();

            }
            else {

            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
}
function removeAdmin() {
    var ids = getSelectedIds();
    if (ids.length == 0) {
        toastr.error("Select user(s) first!");
        return;
    }
    $.ajax({
        url: '/user/remove-admin',
        type: 'PUT',
        data: { Ids: ids },
        success: function (response) {
            debugger;
            if (response.success) {
                window.location.reload();

            }
            else {

            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
}