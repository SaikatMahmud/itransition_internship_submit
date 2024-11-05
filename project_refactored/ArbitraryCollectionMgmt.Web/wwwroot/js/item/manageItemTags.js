
$(document).ready(function () {
    var selectedTagIds = [];
    var unavailableTags = [];
    $(function () {
        $("#tagSearchField").autocomplete({
            source: function (request, response) {
                var userInput = request.term;
                $.ajax({
                    url: "/Tag/GetMatch",
                    data: {
                        search: userInput
                    },
                    success: function (data) {
                        var suggestions = $.map(data, function (tag) {
                            return {
                                label: tag.name,
                                tagId: tag.tagId
                            };
                        });
                        var exactMatch = $.grep(suggestions, function (suggestion) {
                            return suggestion.label.toLowerCase() === userInput.toLowerCase();
                        }).length > 0;
                        if (!exactMatch) {
                            suggestions.unshift({
                                label: userInput,
                                tagId: null,
                                isUnavailable: true
                            });
                        }
                        response(suggestions);
                    }
                });
            },
            minLength: 2,
            select: function (event, ui) {
                event.preventDefault();
                var tagId = ui.item.tagId;
                var tagName = ui.item.label;
                var isUnavailable = ui.item.isUnavailable;
                if (isUnavailable) {
                    unavailableTags.push(tagName);
                    console.log(unavailableTags);
                    $("#unavailable-tags").val(unavailableTags);
                }
                else {
                    selectedTagIds.push(tagId);
                    console.log(selectedTagIds);
                    $("#selected-tag-ids").val(selectedTagIds);
                }

                var tagHtml = $("<span>").text(tagName + " ").append($("<a>").text("x").attr("href", "#")
                    .addClass("remove-tag")).data("tag-id", tagId).data("tag-name", tagName)
                    .css({
                        "border": "1px solid #ccc",
                        "padding": "2px 5px",
                        "margin": "2px",
                        "display": "inline-block",
                        "border-radius": "3px"
                    }).addClass("badge bg-light text-dark");

                $("#selected-tags-name").append(tagHtml);
                $(this).val("");

                $(document).on("click", ".remove-tag", function (e) {
                    e.preventDefault();
                    var tagIdToRemove = $(this).parent().data("tag-id");
                    var index = selectedTagIds.indexOf(tagIdToRemove);
                    if (index !== -1) {
                        selectedTagIds.splice(index, 1);
                        $("#selected-tag-ids").val(selectedTagIds);
                        $(this).parent().remove();
                    }
                    else
                        var tagNameToRemove = $(this).parent().data("tag-name"); {
                        index = unavailableTags.indexOf(tagNameToRemove);
                        if (index !== -1) {
                            unavailableTags.splice(index, 1);
                            $("#unavailable-tags").val(unavailableTags);
                            $(this).parent().remove();
                            console.log(unavailableTags);
                        }
                    }

                });
            }
        });
    });
});

function DeleteItemTag(id, anchor) {
    event.preventDefault();
    Swal.fire({
        title: "Are you sure?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/ItemTag/Delete/' + id,
                type: 'DELETE',
                success: function (response) {
                    debugger;
                    if (response.success) {
                        $(anchor).parent('span').remove();
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

