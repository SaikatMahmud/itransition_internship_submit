var datatable;
var pathname;
var collectionId;
var columnNames = ['ItemId', 'Name'];
$(document).ready(function () {
    pathname = window.location.pathname;
    collectionId = pathname.split('/').pop();
});


function loadItemOfCollection_Guest() {
    datatable = $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": `/item/collection/get-list/${collectionId}`,
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
            { data: 'name', width: '15%' },
            {
                data: 'itemId',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <button type="button" onclick="ViewItem(${data})" class="btn btn-info"><i class="bi bi-file-earmark-richtext"></i></button>
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


function ViewItem(id) {
    window.location.href = "/item/view/" + id;
}


