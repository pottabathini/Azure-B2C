/// <reference path="oidc-client.js" />

///////////////////////////////
// config
///////////////////////////////
Oidc.Log.logger = console;
Oidc.Log.level = Oidc.Log.NONE;

var settings = {
    authority: 'https://login.microsoftonline.com/tenantazureb2c.onmicrosoft.com',
    client_id: '2048095b-a953-4150-9ab4-5a2f6334d99f',
    redirect_uri: window.location.protocol + "//" + window.location.host + "/index.html",
    post_logout_redirect_uri: window.location.protocol + "//" + window.location.host + "/index.html",

    // these two will be done dynamically from the buttons clicked, but are
    // needed if you want to use the silent_renew
    response_type: 'token id_token',
    scope: 'openid 2048095b-a953-4150-9ab4-5a2f6334d99f',

    // silent renew will get a new access_token via an iframe 
    // just prior to the old access_token expiring (60 seconds prior)
    silent_redirect_uri: "http://localhost:21575/silent_renew.html",
    automaticSilentRenew: true,

    // will raise events for when user has performed a logout at IdentityServer
    monitorSession : true,

    // this will allow all the OIDC protocol claims to vbe visible in the window. normally a client app 
    // wouldn't care about them or want them taking up space
    filterProtocolClaims: true,

    // this will use the user info endpoint if it's an OIDC request and there's an access_token
    loadUserInfo: true
};
var mgr = new Oidc.UserManager(settings);

mgr.settings.metadata = {
    issuer: 'https://login.microsoftonline.com/tfp/b9545c46-ac5b-4245-b947-636bce5385ea/b2c_1_sign_in/v2.0/',
    authorization_endpoint: 'https://login.microsoftonline.com/tenantazureb2c.onmicrosoft.com/oauth2/v2.0/authorize?p=b2c_1_sign_in',
    end_session_endpoint: 'https://login.microsoftonline.com/tenantazureb2c.onmicrosoft.com/oauth2/v2.0/logout?p=b2c_1_sign_in',
    jwks_uri: 'https://login.microsoftonline.com/tenantazureb2c.onmicrosoft.com/discovery/v2.0/keys?p=b2c_1_sign_in'
};

mgr.settings.signingKeys = [
        {
            "kid": "IdTokenSigningKeyContainer",
            "use": "sig",
            "kty": "RSA",
            "e": "AQAB",
            "n": "tLDZVZ2Eq_DFwNp24yeSq_Ha0MYbYOJs_WXIgVxQGabu5cZ9561OUtYWdB6xXXZLaZxFG02P5U2rC_CT1r0lPfC_KHYrviJ5Y_Ekif7iFV_1omLAiRksQziwA1i-hND32N5kxwEGNmZViVjWMBZ43wbIdWss4IMhrJy1WNQ07Fqp1Ee6o7QM1hTBve7bbkJkUAfjtC7mwIWqZdWoYIWBTZRXvhMgs_Aeb_pnDekosqDoWQ5aMklk3NvaaBBESqlRAJZUUf5WDFoJh7yRELOFF4lWJxtArTEiQPWVTX6PCs0klVPU6SRQqrtc4kKLCp1AC5EJqPYRGiEJpSz2nUhmAQ"
        },
        {
            "kid": "IdTokenSigningKeyContainer.v2",
            "nbf": 1459289287,
            "use": "sig",
            "kty": "RSA",
            "e": "AQAB",
            "n": "s4W7xjkQZP3OwG7PfRgcYKn8eRYXHiz1iK503fS-K2FZo-Ublwwa2xFZWpsUU_jtoVCwIkaqZuo6xoKtlMYXXvfVHGuKBHEBVn8b8x_57BQWz1d0KdrNXxuMvtFe6RzMqiMqzqZrzae4UqVCkYqcR9gQx66Ehq7hPmCxJCkg7ajo7fu6E7dPd34KH2HSYRsaaEA_BcKTeb9H1XE_qEKjog68wUU9Ekfl3FBIRN-1Ah_BoktGFoXyi_jt0-L0-gKcL1BLmUlGzMusvRbjI_0-qj-mc0utGdRjY-xIN2yBj8vl4DODO-wMwfp-cqZbCd9TENyHaTb8iA27s-73L3ExOQ"
        }
];

///////////////////////////////
// events
///////////////////////////////
var user;
mgr.events.addUserLoaded(function (u) {
    user = u;
    console.log("user loaded");
    log("user loaded");
    showUser(user);    
});

mgr.events.addUserUnloaded(function () {
    user = null;
    console.log("user unloaded");
    showUser();
});

mgr.events.addAccessTokenExpiring(function () {
    console.log("token expiring");
    log("token expiring");    
    showUser(user);
});

mgr.events.addAccessTokenExpired(function () {
    console.log("token expired");
    log("token expired");
    showUser(user);
});

mgr.events.addSilentRenewError(function (e) {
    console.log("silent renew error", e.message);
    log("silent renew error", e.message);
});

mgr.events.addUserSignedOut(function () {
    console.log("user signed out");
    log("user signed out");
});

///////////////////////////////
// UI event handlers
///////////////////////////////
[].forEach.call(document.querySelectorAll(".request"), function (button) {
    button.addEventListener("click", function () {
        signIn(this.dataset["scope"], this.dataset["type"]);
    });
});
document.querySelector('.call').addEventListener("click", callApi, false);
document.querySelector(".logout").addEventListener("click", signOut, false);
//document.querySelector(".sign_up").addEventListener("click", signUp, false);
//document.querySelector(".editProfile").addEventListener("click", editProfile, false);

///////////////////////////////
// functions for UI elements
///////////////////////////////
function signIn(scope, response_type) {
    mgr.signinRedirect({ scope: scope, response_type: response_type }).then(null, function (e) {        
        log(e);
    });
}

function signInCallback() {
    mgr.signinRedirectCallback().then(function (user) {        
        var hash = window.location.hash.substr(1);
        var result = hash.split('&').reduce(function (result, item) {
            var parts = item.split('=');
            result[parts[0]] = parts[1];
            return result;
        }, {});
        log(result);
    }).catch(function (error) {        
        log(error);
    });
}

function signOut() {
    mgr.signoutRedirect();
}

function callApi() {
    var xhr = new XMLHttpRequest();
    xhr.onload = function (e) {
        if (xhr.status >= 400) {
            logAjaxResult({
                status: xhr.status,
                statusText: xhr.statusText,
                wwwAuthenticate: xhr.getResponseHeader("WWW-Authenticate")
            });
        }
        else {
            logAjaxResult(JSON.parse(xhr.responseText));
        }
    };
    xhr.onerror = function () {
        if (xhr.status === 401) {
            mgr.removeUser();
        }

        logAjaxResult({
            status: xhr.status,
            statusText: xhr.statusText,
            wwwAuthenticate: xhr.getResponseHeader("WWW-Authenticate")
        });
    };
    xhr.open("GET", "http://localhost:63071/api/feed/SportFeeds", true);
    if (user) {
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
    }
    xhr.send();
}

//function checkSessionState(user) {
//    mgr.metadataService.getCheckSessionIframe().then(function (url) {
//        if (url && user && user.session_state) {
//            console.log("setting up check session iframe for session state", user.session_state);
//            document.getElementById("rp").src = "check_session.html#" +
//                "session_state=" + user.session_state +
//                "&check_session_iframe=" + url +
//                "&client_id=" + mgr.settings.client_id
//            ;
//        }
//        else {
//            console.log("no check session url, user, or session state: not setting up check session iframe");
//            document.getElementById("rp").src = "about:blank";
//        }
//    });
//}

//window.onmessage = function (e) {
//    if (e.origin === window.location.protocol + "//" + window.location.host && e.data === "changed") {
//        console.log("user session has changed");
//        mgr.removeUser();
//        mgr.signinSilent().then(function () {
//            // Session state changed but we managed to silently get a new identity token, everything's fine
//            console.log('renewTokenSilentAsync success');
//        }).catch(function (err) {
//            // Here we couldn't get a new identity token, we have to ask the user to log in again
//            console.log('renewTokenSilentAsync failed', err.message);
//        });
//    }
//}

///////////////////////////////
// init
///////////////////////////////

// clears any old stale requests from storage
mgr.clearStaleState().then(function () {
    console.log("Finished clearing old state");
}).catch(function (e) {
    console.error("Error clearing state:", e.message);
});

// checks to see if we already have a logged in user
mgr.getUser().then(function (user) {
    showUser(user);
}).catch(function (e) {
    log(e);
});

// checks to see if the page being loaded looks like a login callback
if (window.location.hash) {
    signInCallback();
}

///////////////////////////////
// debugging helpers
///////////////////////////////
function log(msg) {
    display("#response", msg);
}
function logIdToken(msg) {
    display("#id-token", msg);
}
function logAccessToken(msg) {
    display("#access-token", msg);
}
function logAjaxResult(msg) {
    display("#ajax-result", msg);
}
function display(selector, msg) {
    document.querySelector(selector).innerText = '';

    if (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.querySelector(selector).innerHTML += msg + '\r\n';
    }
}

function showUser(user) {
    if (!user) {
        log("user not signed in");
        logIdToken();
        logAccessToken();
        logAjaxResult();
    }
    else {
        if (user.profile) {
            logIdToken({ profile: user.profile, session_state: user.session_state });
        }
        else {
            logIdToken();
        }
        if (user.access_token) {
            logAccessToken({ access_token: user.access_token, expires_in: user.expires_in, scope: user.scope });
        }
        else {
            logAccessToken();
        }
    }
}
