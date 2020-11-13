function tagLinkClicked(e) {
    window.hashtagClick.zone.run(() => {
        window.hashtagClick.hashtagClickedFunction(e.target.innerHTML);
    });
}

function addEventListenertoHashtagList() {
    var elems = document.getElementsByClassName("post-meta");
    var hashtags = [];
    for (let i = 0; i < elems.length; i++) {
        if ($(elems[i]).find("a").hasClass("link-hashtag")) {
            var allAnchors = $(elems[i]).children().find("a.link-hashtag");
            for (let k = 0; k < allAnchors.length; k++) {
                hashtags.push(allAnchors[k]);
            }
        }
    }
    for (let j = 0; j < hashtags.length; j++) {
        hashtags[j].setAttribute("onclick", "tagLinkClicked(event)")
    }
}

function addEventListenertoHashtagListFromAngular() {
    setTimeout(function () { addEventListenertoHashtagList() }, 2000);
}

function notifyComment(element) {
    document.getElementById("tone").play();
    $.notify(
        element.innerHTML, "success",
        { position: "right bottom" } ,

    );
}

function notifyFriendRequest(element) {
    document.getElementById("tone").play();

    $.notify(
        element.innerHTML, "success",
        { position: "right bottom" } ,

    );
}

function openPostMenu(e) {
    var parent = e.parentNode;
    if ($(e).hasClass("fa fa-ellipsis-h")) {
        $(e).removeClass("fa fa-ellipsis-h");
        $(e).addClass("fa fa-caret-square-o-left");
    } else {
        $(e).removeClass("fa fa-caret-square-o-left");
        $(e).addClass("fa fa-ellipsis-h");

    }
    var showState = $(parent).find(".post-menu").css("display");
    if (showState === "block") {
        $(parent).find(".post-menu").slideUp();
    } else {
        $(parent).find(".post-menu").slideDown();
    }
}

function editPost(event) {
    var parent = event.target.parentNode.parentNode.parentNode;
    var menu = event.target.parentNode.parentNode;
    var editSec = $(menu).slideUp();

    var editSec = $(parent).find(".editPostSection")[0];
    $(editSec).slideDown();
}

function closeEditSec(btn) {
    var parent = btn.parentNode.parentNode;
    $(parent).slideUp();
}

function signalFriendAcc(event, friendId) {

    window.signalFriendRequestAccept.zone.run(() => {
        window.signalFriendRequestAccept.acceptRequestFunction(friendId)
    });

    var count = document.getElementById("notification-count").innerHTML;

    if (count > 0) {

        count = parseInt(count) - 1;

        document.getElementById("notification-count").innerHTML = count;
    }


    $(event.target).remove();

}

function viewProfile(userId) {
    window.signalViewProfile.zone.run(() => {
        window.signalViewProfile.viewProfileFunction(userId)
    });
}

function viewSinglePost(postId){
    window.viewPost.zone.run(() => {
        window.viewPost.viewPostFunction(postId);
    });
}

function replyBtnClicked(e) {
    var replyForm = $(e.parentNode.parentNode).find("form.reply-form");
    if (replyForm.hasClass("hidden")) {
        $(replyForm).slideDown();
        $(replyForm).removeClass("hidden");
        $(replyForm).find("textarea.reply-text")[0].focus();
    } else {
        $(replyForm).slideUp()
        $(replyForm).addClass("hidden");
    }
}
