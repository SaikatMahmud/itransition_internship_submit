$(document).ready(function () {
    $.ajax({
        url: '/tag/get-all',
        type: 'GET',
        success: function (response) {
            debugger;
            if (response.success) {
                var tagData = response.data;
                startCloud(tagData);
            } else {
                toastr.error("Server error!");
            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
});

function startCloud(tagData) {
    var tagGrid = document.getElementById('tag-grid');  
    var input = document.querySelector('input[name=tags]');


    var tagify = new Tagify(input, {
        enforceWhitelist: true,
        whitelist: tagData.map(tag => ({ value: tag.name, id: tag.tagId })),
        maxTags: 1,
        addTagOnBlur: false
    });
    tagify.on('add', function (e) {
        var selectedTagId = e.detail.data.id;
        var selectedTagName = e.detail.data.value;
        tagify.removeAllTags();
        window.location.href = `/Search/SearchResult?searchTag=${selectedTagId}_${selectedTagName}`;
    });

    tagData.forEach(tag => {
        var tagElement = document.createElement('div');
        tagElement.innerHTML = `<button class="badge bg-light text-dark"
                                style="border: 1px solid #ccc; padding: 0.4rem 0.7rem; margin: 2px; display: inline-block; border-radius: 3px; font-size: 0.8rem;">
                                ${tag.name}</button>`;
        tagElement.onclick = function () {
            window.location.href = `/Search/SearchResult?searchTag=${tag.tagId}_${tag.name}`;
        };
        tagGrid.appendChild(tagElement);
    });

};
