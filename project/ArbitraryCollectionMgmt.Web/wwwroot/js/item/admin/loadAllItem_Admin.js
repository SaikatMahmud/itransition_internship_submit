var datatable;
var columnNames = ['ItemId', 'Name'];

function loadAllItem_Admin() {
    datatable = $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": `/admin/item/get-all-item`,
            "type": "GET",
            "data": function (d) {            
                d.search = d.search.value;
                if (d.order && d.order[0] && d.order[0].column !== undefined) {
                    d.orderColumn = columnNames[d.order[0].column];
                    d.orderDirection = d.order[0].dir;
                } else {
                    d.orderColumn = columnNames[0];
                    d.orderDirection = 'asc';
                }
            }
        },
        "language": {
            "searchPlaceholder": "search for item"
        },
        "columns": [
            { data: 'itemId', width: '3%', "className": "dt-right" },
            { data: 'name', width: '10%' },
            {
                data: 'collection',
                render: function (data) {
                    return `<a href="/admin/item/collection/${data.collectionId}" style="text-decoration: none">${data.name}</a>`
                },
                width: '8%',
                orderable: false
            },
            {
                data: 'user',
                render: function (data) {
                    return `<a href="/admin/collection/user/${data.userId}" style="text-decoration: none">${data.name}</a>`
                },
                width: '8%',
                orderable: false
            },
            {
                data: 'itemId',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <button type="button" onclick="ViewItem(${data})" class="btn btn-info mx-1"><i class="bi bi-file-earmark-richtext"></i></button>
                    <button type="button" onclick="EditItem(${data})" class="btn btn-warning mx-1"><i class="bi bi-pencil-square"></i></button>
                    <button type="button" onclick="DeleteItem(${data})" class="btn btn-danger mx-1"><i class="bi bi-trash"></i></button>
                    </div>`
                },
                width: '2%',
                orderable: false
            }
        ],
        "order": [[0, "asc"]],
        "bInfo": false
    });
}
function EditItem(id) {
    window.location.href = "/admin/item/edit/" + id;
}
function ViewItem(id) {
    window.location.href = "/item/view/" + id;
}

function DeleteItem(id) {
    Swal.fire({
        title: "Are you sure?",
        text: "All related data will be deleted!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/item/delete/' + id,
                type: 'DELETE',
                success: function (response) {
                    debugger;
                    if (response.success) {
                        datatable.ajax.reload();
                        toastr.success(response.msg);
                    }
                    else {
                        toastr.error(response.msg);
                    }
                },
                error: function (error) {
                    debugger;
                    alert("Internal server error!");
                }
            });
        }
    });
}

