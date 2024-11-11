var datatable;
var pathname;
var userId;
var categoryId = 0;

var columnNames = ['CollectionId', 'Name', 'CategoryId'];
$(document).ready(function () {
    $('#categoryId').change(function () {
        $('#customFilter').val("all");
        categoryId = $('#categoryId').val();
        datatable.destroy();
        loadCollectionOfUser_Guest();
    });
    pathname = window.location.pathname;
    userId = pathname.split('/').pop();

});


function loadCollectionOfUser_Guest() {
    datatable = $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": `/collection/get-user-collection/${userId}?categoryId=${categoryId}`,
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
            { data: 'category.name', width: '10%' },
            {
                data: 'collectionId',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <button type="button" onclick="GoToItemList(${data})" class="btn btn-info"><i class="bi bi-list-ul"></i></button>
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
    window.location.href = "/item/collection/" + id;
}
