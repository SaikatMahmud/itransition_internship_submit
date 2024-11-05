var datatable;
function loadMyTicket() {
    datatable = $('#tblData').DataTable({
        "processing": true,
        "serverSide": true,
        searching: false,
        "bInfo": false,
        "ajax": {
            "url": `/get-my-tickets`,
            "type": "GET",
        },
        "columns": [
            {
                data: 'fields.summary', width: '9%', orderable: false,
                render: function (data, type, row) {
                    return `<a href="https://saikatdev67-itransition.atlassian.net/browse/${row.key}" style="text-decoration: none">${data} <span class="text-muted fw-bold" style="font-size:0.65em"><i class="bi bi-box-arrow-up-right"></i></span></a>`
                }
            },
            {
                data: 'fields.customfield_10064', width: '8%', orderable: false,
                render: function (data, type, row) {
                    if (data !== null && data !== "") {
                        return `<a href="${data}" style="text-decoration: none">${data}</a>`
                    } else {
                        return "";
                    }
                }
            },
            { data: 'fields.status.name', width: '2%', orderable: false },
            { data: 'fields.created', width: '4%', orderable: false },
        ]
    });
}

