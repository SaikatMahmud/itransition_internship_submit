$(document).ready(function () {
    var attributeIndex = window.attributeIndex;
    var fieldList = window.fieldList;
    $('#add-attribute-btn').click(function () {
        console.log(attributeIndex);
        tableRow =
            `<tr>
           <td>
              <input type='hidden' name='collection.CustomAttributes.index' value="${attributeIndex}" />
           </td>
           <td>
              <label class="fw-bold text-muted">Field</label>
           </td>
           <td>
              <input name='collection.CustomAttributes[${attributeIndex}].FieldName' type='text' class='form-control' maxlength="25" required />
           </td>
           <td>
              <select name="collection.CustomAttributes[${attributeIndex}].FieldType" class="form-select" id="fieldTypeSelect${attributeIndex}" required>
                 <option value="" disabled selected>Select Type</option>
              </select>
           </td>
           <td>
              <button type="button" class="btn btn-danger btn-sm close-btn"><i class="bi bi-dash-circle"></i></button>
           </td>
        </tr>`;

        $('#add-attribute-row').append(tableRow);

        var fieldTypeSelect = $(`#fieldTypeSelect${attributeIndex}`);
        $.each(fieldList, function (index, field) {
            fieldTypeSelect.append($('<option>', { value: field.value, text: field.text }));
        });
        attributeIndex++;
    });

    $('#add-attribute-row').on('click', '.close-btn', function () {
        event.preventDefault();
        var btnId = $(this).attr('id'); //for edit page. if it has btnId means its saved in Db, so need to delete from Db
        if (btnId != null) {
            deleteAttribute(btnId);
        }
        else $(this).closest('tr').remove(); //otherwise just remove from UI
    });
});
function deleteAttribute(id) {
    Swal.fire({
        title: "Are you sure?",
        text: "All related data will be removed!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/collection/attribute/delete/' + id,
                type: 'DELETE',
                success: function (response) {
                    debugger;
                    if (response.success) {
                        $('#attribute-' + id).remove(); //also remove from UI
                    }
                    else {
                        toastr.error("Server side error!");
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

