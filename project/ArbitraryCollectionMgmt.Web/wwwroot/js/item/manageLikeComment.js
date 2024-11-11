var pathname;
var itemId;
$(document).ready(function () {
    pathname = window.location.pathname;
    itemId = pathname.split('/').pop();

    $.ajax({
        url: '/Like/CheckUserLike/' + itemId,
        type: 'GET',
        success: function (response) {
            debugger;
            if (response.success) {
                if (response.isLiked) {
                    $("#btnRemoveLike").show();
                    $("#btnLike").hide();
                }
                else if (!response.isLiked) {
                    $("#btnRemoveLike").hide();
                    $("#btnLike").show();
                }
            }
            else {
                console.log("check like returned false for user access");
            }
        },
        error: function (error) {
            debugger;
            console.log("Internal server error!");
        },
        complete: function (data) {
            GetComments(itemId);
        }
    });
});


function AddLike(itemId) {
    $.ajax({
        url: '/Like/AddLike/' + itemId,
        type: 'POST',
        success: function (response) {
            debugger;
            if (response.success) {
                $("#btnRemoveLike").show();
                $("#btnLike").hide();
            } else {
                toastr.error("Server error!");
            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });

}
function RemoveLike(itemId) {
    $.ajax({
        url: '/Like/RemoveLike/' + itemId,
        type: 'DELETE',
        success: function (response) {
            debugger;
            if (response.success) {
                $("#btnLike").show();
                $("#btnRemoveLike").hide();
            } else {
                toastr.error("Server error!");
            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
}

function AddComment(itemId) {
    var commentText = $("#commentInput").val();
    if (commentText == "") {
        toastr.error("Comment can not be empty!");
        return;
    }
    $.ajax({
        url: '/Comment/AddComment/',
        type: 'POST',
        data: { itemId: itemId, commentText: commentText },
        success: function (response) {
            debugger;
            if (response.success) {
                $("#commentInput").val("");
                //toastr.success("Comment added successfully!");

            } else {
                toastr.error("Server error!");
            }
        },
        error: function (error) {
            debugger;
            alert("Internal server error!");
        }
    });
}

function GetComments(itemId) {
    $.ajax({
        url: '/Comment/GetAllComments/' + itemId,
        type: 'GET',
        success: function (response) {
            debugger;
            if (response.success) {
                $('#noCommentsText').show();
                $('#commentInputSpan').show();
                var commentsHtml = '';
                if (response.data && response.data.length > 0) {
                    $('#noCommentsText').hide();
                    response.data.forEach(comment => {
                        $('#noCommentsText').hide();
                        commentsHtml +=
                            `<div class="card w-50 mb-2">
                                <div class="card-body">
                                    <p>
                                        <span class="text-muted fw-bold fs-6 lh-sm"><a href="/collection/user/${comment.user.userId}" style="text-decoration: none"><i class="bi bi-file-person"></i> ${comment.user.name}</a></span><br />
                                        <span class="lh-sm">${comment.commentText}</span>
                                    </p>
                                </div>
                            </div>`;
                    });
                }
                $('#commentsArea').append(commentsHtml);
                startCommentConnection(itemId);
            } else {
                console.log("get comments returned false for user access");
            }
        },
        error: function (error) {
            debugger;
            //alert("Internal server error!");
        }
    });
}

function startCommentConnection(itemId) {
    var connectionLoadComment = new signalR.HubConnectionBuilder().withUrl("/hub/commentHub").build();

    connectionLoadComment.on("ReceiveComment", (comment) => {
        console.log(`New comment received.`);
        var newCommentHtml =
            `<div class="card w-50 mb-2">
                        <div class="card-body">
                            <p>
                                <span class="text-muted fw-bold fs-6 lh-sm"><a href="/collection/user/${comment.user.userId}" style="text-decoration: none"><i class="bi bi-file-person"></i> ${comment.user.name}</a></span><br />
                                <span class="lh-sm">${comment.commentText}</span>
                            </p>
                        </div>
                    </div>`;

        $('#noCommentsText').hide();
        $('#commentsArea').append(newCommentHtml);
    });

    connectionLoadComment.start().then(() => {
        connectionLoadComment.invoke("JoinGroup", itemId.toString()).then((msg) =>
            console.log(msg))
    }).catch(err => console.error(err.toString()));
}