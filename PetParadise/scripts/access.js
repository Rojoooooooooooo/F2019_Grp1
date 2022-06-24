// Check access token on load
function checkpoint() {
    window.addEventListener('load', () => {
        // if access_token doesnt exist
        const accessToken = getCookieToken();

        if (!accessToken) {
            console.log("no access token, getting u some")
            const sessionToken = getSessionToken();
            if (!sessionToken) return console.log("invalid request");

            // if sessionToken exists, request access_token from auth server
            getAccessToken(sessionToken);  
        }
        
    });
}

function autopilotAccessGenerator(interval) {
    const margin = 1000*10 //10s
    const i = interval || 60 * 3 * 1000; // 3 mins
    const sessionToken = getSessionToken();
    setInterval(() => {
        getAccessToken(sessionToken)
    }, i-margin)
}

// https://stackoverflow.com/a/15724300
function getCookieToken() {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; access_token=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

function getSessionToken(){
    const encToken = localStorage.getItem("session_token");
    return decToken = window.atob(encToken);
}

// generate access token
function getAccessToken(sessionToken) {
    if (!sessionToken) return;

    fetch('/auth/refresh', {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Session-Token": sessionToken
        }
    })

    .catch(e=>console.log(e));
}