class PostBox {
    
    static createFeedbackBoxTemplate(name, rating, content, date) {
        date = magicDate(date);
        if (rating < 1) rating = 1;
        switch (rating) {
            case 1: {
                return (`<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"><div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src=https://ui-avatars.com/api/?background=random&bold=true&name=${name} style="height:36px;width:36px"><h6 class="fw-bold">${name}</h6></div><div class="w-100 d-flex flex-column justify-content-start align-items-start"><div class="w-100"><div class="d-flex flex-row stext-darker gap-2"><p class="fw-bold"><span id="loaded-rating">${rating}</span>/5</p><div class="mb-3"><i class="fa-solid fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i></div></div><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div></div>`);
            }
            case 2: {
                return(`<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"><div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src=https://ui-avatars.com/api/?background=random&bold=true&name=${name} style="height:36px;width:36px"><h6 class="fw-bold">${name}</h6></div><div class="w-100 d-flex flex-column justify-content-start align-items-start"><div class="w-100"><div class="d-flex flex-row stext-darker gap-2"><p class="fw-bold"><span id="loaded-rating">${rating}</span>/5</p><div class="mb-3"><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i></div></div><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div></div>`);
            }
            case 3: {
                return (`<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"><div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src=https://ui-avatars.com/api/?background=random&bold=true&name=${name} style="height:36px;width:36px"><h6 class="fw-bold">${name}</h6></div><div class="w-100 d-flex flex-column justify-content-start align-items-start"><div class="w-100"><div class="d-flex flex-row stext-darker gap-2"><p class="fw-bold"><span id="loaded-rating">${rating}</span>/5</p><div class="mb-3"><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i></div></div><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div></div>`);
            }
            case 4: {
                return (`<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"><div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src=https://ui-avatars.com/api/?background=random&bold=true&name=${name} style="height:36px;width:36px"><h6 class="fw-bold">${name}</h6></div><div class="w-100 d-flex flex-column justify-content-start align-items-start"><div class="w-100"><div class="d-flex flex-row stext-darker gap-2"><p class="fw-bold"><span id="loaded-rating">${rating}</span>/5</p><div class="mb-3"><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-regular fa-star"></i></div></div><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div></div>`);
            }
            default: {
                return (`<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"><div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src=https://ui-avatars.com/api/?background=random&bold=true&name=${name} style="height:36px;width:36px"><h6 class="fw-bold">${name}</h6></div><div class="w-100 d-flex flex-column justify-content-start align-items-start"><div class="w-100"><div class="d-flex flex-row stext-darker gap-2"><p class="fw-bold"><span id="loaded-rating">${rating}</span>/5</p><div class="mb-3"><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i></div></div><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div></div>`);
            }
        }
    }

    static addFeedbackBox(name, rating, content, date) {
        date = magicDate(date);
        if (rating < 1) rating = 1;
        switch (rating) {
            case 1: {
                $('#post-container').append(`<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"><div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src="https://ui-avatars.com/api/?background=random&bold=true&name=${name}" style="height:36px;width:36px"><h6 class="fw-bold">${name}</h6></div><div class="w-100 d-flex flex-column justify-content-start align-items-start"><div class="w-100"><div class="d-flex flex-row stext-darker gap-2"><p class="fw-bold"><span id="loaded-rating">${rating}</span>/5</p><div class="mb-3"><i class="fa-solid fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i></div></div><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div></div>`);
                break;
            }
            case 2: {
                $('#post-container').append(`<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"><div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src="https://ui-avatars.com/api/?background=random&bold=true&name=${name}" style="height:36px;width:36px"><h6 class="fw-bold">${name}</h6></div><div class="w-100 d-flex flex-column justify-content-start align-items-start"><div class="w-100"><div class="d-flex flex-row stext-darker gap-2"><p class="fw-bold"><span id="loaded-rating">${rating}</span>/5</p><div class="mb-3"><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i></div></div><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div></div>`);
                break;
            }
            case 3: {
                $('#post-container').append(`<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"><div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src="https://ui-avatars.com/api/?background=random&bold=true&name=${name}" style="height:36px;width:36px"><h6 class="fw-bold">${name}</h6></div><div class="w-100 d-flex flex-column justify-content-start align-items-start"><div class="w-100"><div class="d-flex flex-row stext-darker gap-2"><p class="fw-bold"><span id="loaded-rating">${rating}</span>/5</p><div class="mb-3"><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-regular fa-star"></i><i class="fa-regular fa-star"></i></div></div><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div></div>`);
                break;
            }
            case 4: {
                $('#post-container').append(`<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"><div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src="https://ui-avatars.com/api/?background=random&bold=true&name=${name}"style="height:36px;width:36px"><h6 class="fw-bold">${name}</h6></div><div class="w-100 d-flex flex-column justify-content-start align-items-start"><div class="w-100"><div class="d-flex flex-row stext-darker gap-2"><p class="fw-bold"><span id="loaded-rating">${rating}</span>/5</p><div class="mb-3"><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-regular fa-star"></i></div></div><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div></div>`);
                break;
            }
            default: {
                $('#post-container').append(`<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"><div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src="https://ui-avatars.com/api/?background=random&bold=true&name=${name}" style="height:36px;width:36px"><h6 class="fw-bold">${name}</h6></div><div class="w-100 d-flex flex-column justify-content-start align-items-start"><div class="w-100"><div class="d-flex flex-row stext-darker gap-2"><p class="fw-bold"><span id="loaded-rating">${rating}</span>/5</p><div class="mb-3"><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i><i class="fa-solid fa-star"></i></div></div><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div></div>`);
            }
        }
    }
    static postBoxTemplate(petId, postId, name, content, date, likeCount, commentCount, liked) {
        return `<div class="w-75 shadow rounded-2 p-4 d-flex flex-column align-items-start justify-content-start gap-2"> <div class="w-100 d-flex flex-row justify-content-start align-items-center gap-2"> <img id="profile-photo" class="rounded-circle" alt="user" src="https://ui-avatars.com/api/?background=random&bold=true&name=${name}" style="height:36px;width:36px;"/> <a href='/profile/${petId}' class="fw-bold fs-6 stext-darker text-decoration-none">${name}</a> </div><div class="w-100 d-flex flex-column justify-content-start align-items-start"> <div class="w-100"> <p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p class="fst-italic fs-6">${date}</p></div></div><div class="w-100 d-flex flex-column"> <div class="w-100 d-flex flex-row justify-content-evenly align-items-center gap-2"> <div id="like-${postId}" data-likescount=${likeCount} data-postliked=${liked} data-postid=${postId} class="text-center flex-grow-1 fw-bold rounded-pill sbg-light px-4 py-2" role="button"> <span data-postid=${postId}>${likeCount} <i data-postid=${postId} class="fa-solid fa-cookie"></i></span> </div><div id=comment-${postId} data-postid=${postId} class="text-center flex-grow-1 fw-bold rounded-pill sbg-light px-4 py-2" role="button"> <span data-postid=${postId}>${commentCount} <i data-postid=${postId} class="fa-solid fa-message"></i></span> </div></div></div></div>`;
    }

    static createPostBox(petId, postId, name, content, date, likeCount, commentCount, liked) {
        date = magicDate(date);
        $("#post-container").append(PostBox.postBoxTemplate(petId, postId, name, content, date, likeCount, commentCount, liked));
        $("#like-" + postId).click(giveTreat);
        $("#comment-" + postId).click(seeFullPost);
        if (liked)             
            $("#like-" + postId).addClass("sbg-base").removeClass("sbg-light");
        
    }
    static commentBoxTemplate(petId, commentId, name, content, date) {
        date = prettyDate(date);
        return `<div class="w-100 px-4 py-2 sbg-light rounded-2"><div class="d-flex flex-row justify-content-start align-items-center gap-2"><img id="profile-photo" class="rounded-circle" alt="user" src="https://ui-avatars.com/api/?background=random&bold=true&name=${name}" style="height:36px;width:36px"><a href='/profile/${petId}' class="fw-bold fs-6 stext-darker text-decoration-none">${name}</a></div><div class="w-100 d-flex flex-column justify-content-start align-items-start ps-4"><div class="w-100 ps-4"><p class="w-100 text-wrap text-break fs-5 lh-sm">${content}</p><p id="comment-${commentId}" class="fst-italic fs-6">${date}</p></div></div></div>`;
    }
    static openPostBox() {
        $("#newPostBox").attr("style", "display: block !important");
    }

    static closePostBox() {
        $("#newPostBox").attr("style", "display: none !important");
    }

    static addPost(e, callback) {
        e.preventDefault();
        const content = $("#postContent").val();
        const petid = window.atob(localStorage["current_pet"]);
        const body = {
            profileId: petid, 
            content
        }

        fetch("/post/add", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer "+getSessionToken()
            },
            body: JSON.stringify(body)
        })
        .then(res=>res.json())
        .then(data=> {
            if (data.stack) {
                // something should go here but now we are in rush
            }
            else if (data.error) {
                // something should go here but now we are in rush
            }
            callback(data); 
        })
    }

    static addFeedback(e, callback) {
        e.preventDefault();
        const content = $("#postContent").val();
        const rating = $(".star-rating").val();
        if(rating == 0) return;
        const petid = window.atob(localStorage["current_pet"]);
        const clinicId = window.location.pathname.split("/")[2];
        const body = {
            clinicId,
            content,
            rating
        }

        fetch(`/pet/${petid}/feedback`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + getSessionToken()
            },
            body: JSON.stringify(body)
        })
        .then(_=> {
            //if (data.stack) {
            //    // something should go here but now we are in rush
            //}
            //else if (data.error) {
            //    // something should go here but now we are in rush
            //}
            body.createdAt = new Date();
            callback(body);
        })
    }
}

function prettyDate(date) {
    var seconds = Math.floor((new Date() - date) / 1000);
    var interval = Math.floor(seconds / 31536000);
    if (interval > 1) {
        return interval + " years ago";
    }
    interval = Math.floor(seconds / 2592000);
    if (interval > 1) {
        return interval + " months ago";
    }
    interval = Math.floor(seconds / 604800);
    if (interval > 1) {
        return interval + " weeks ago";
    }
    interval = Math.floor(seconds / 86400);
    if (interval > 1) {
        return interval + " days ago";
    }
    interval = Math.floor(seconds / 3600);
    if (interval > 1) {
        return interval + " hours ago";
    }
    interval = Math.floor(seconds / 60);
    if (interval > 1) {
        return interval + " minutes ago";
    }
    return Math.floor(seconds) + " seconds ago";
}


function UTCToLocal(utcDate) {
    const date = new Date(utcDate); // utcdate string to date
    const gmt = new Date().getTimezoneOffset() / 60; // get offset in hour

    date.setHours(date.getHours() - gmt);
    return new Date(date);
}

function magicDate(utcdatestring) {
    const d = UTCToLocal(utcdatestring).toLocaleString("en");
    const pd = prettyDate(new Date(d));
    return pd;
}

// If jQuery is included in the page, adds a jQuery plugin to handle it as well
//if (typeof jQuery != "undefined")
//    jQuery.fn.prettyDate = function () {
//        return this.each(function () {
//            var date = prettyDate(this.title);
//            if (date)
//                jQuery(this).text(date);
//        });
//    };