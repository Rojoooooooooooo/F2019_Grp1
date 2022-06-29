// SUBJECTED TO CLEANUP

// Check access token on load
//function checkpoint() {
//    window.addEventListener('load', () => {
//        // if access_token doesnt exist
//        const accessToken = getCookieToken();

//        if (!accessToken) {
//            console.log("no access token, getting u some")
//            const sessionToken = getSessionToken();
//            if (!sessionToken) return console.log("invalid request, you need to login.");

//            // if sessionToken exists, request access_token from auth server
//            getAccessToken(sessionToken);  
//        }
        
//    });
//}

//function autopilotAccessGenerator(interval) {
//    const margin = 1000*10 //10s
//    const i = interval || 60 * 3 * 1000; // 3 mins
//    const sessionToken = getSessionToken();
//    setInterval(() => {
//        getAccessToken(sessionToken)
//    }, i-margin)
//}

// https://stackoverflow.com/a/15724300
//function getCookieToken() {
//    const value = `; ${document.cookie}`;
//    const parts = value.split(`; session_token=`);
//    if (parts.length === 2) return parts.pop().split(';').shift();
//}

function getSessionToken(){
    const encToken = localStorage.getItem("session_token");
    return decToken = window.atob(encToken);
}

function setSessionToken(token) {
    const encToken = window.btoa(token);
    localStorage.setItem("session_token", encToken);
}

// generate access token
//function getAccessToken(sessionToken, tries) {
//    if (tries === 0) return console.log("failed to fetch");
//    if (!sessionToken) return;

//    tries = tries || 3; // default tries 3
//    fetch('/auth/refresh', {
//        method: "POST",
//        headers: {
//            "Content-Type": "application/json",
//            "Session-Token": sessionToken
//        }
//    })
//    .then(res=> {
//        if (res.ok) {
//            console.log("access token loaded!");
//        }
//    })
//    .catch(e=> {
//        return getAccessToken(sessionStorage, tries -= 1);
//    });
//}
