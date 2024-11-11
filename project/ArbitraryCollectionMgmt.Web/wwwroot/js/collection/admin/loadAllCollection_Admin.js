var datatable;
var categoryId = 0;
var columnNames = ['CollectionId', 'Name', 'CategoryId'];
$(document).ready(function () {
    $('#categoryId').change(function () {
        $('#customFilter').val("all");
        categoryId = $('#categoryId').val();
        datatable.destroy();
        loadAllCollection_Admin();
    });
});


function loadAllCollection_Admin() {
    datatable = $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": `/admin/collection/get-all-collection?categoryId=${categoryId}`,
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
            "searchPlaceholder": "search for collection"
        },
        "columns": [
            { data: 'collectionId', width: '2%', "className": "dt-right" },
            { data: 'name', width: '15%' },
            { data: 'category.name', width: '8%' },
            {
                data: 'user',
                render: function (data) {
                    return `<a href="/admin/collection/user/${data.userId}" style="text-decoration: none">${data.name}</a>`
                },
                width: '4%',
                orderable: false
            },
            {
                data: 'collectionId',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <button type="button" onclick="GoToItemList(${data})" class="btn btn-info mx-1"><i class="bi bi-list-ul"></i></button>
                    <button type="button" onclick="EditCollection(${data})" class="btn btn-warning mx-1"><i class="bi bi-pencil-square"></i></button>
                    <button type="button" onclick="DeleteCollection(${data})" class="btn btn-danger mx-1"><i class="bi bi-trash"></i></button>
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


function GoToItemList(id) {
    window.location.href = "/admin/item/collection/" + id;
}

function EditCollection(id) {
    window.location.href = "/admin/collection/edit/" + id;
}

function DeleteCollection(id) {
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
                url: '/collection/delete/' + id,
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
