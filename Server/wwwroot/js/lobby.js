import * as ui from "./uifunctions.js";

"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/LobbyHub")
    // .withHubProtocol(new signalR.JsonHubProtocol({
    //     transferFormat: signalR.TransferFormat.Text,
    //     typeNameHandling: 2  // Equivalent to TypeNameHandling.All
    // }))
    .build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
document.getElementById("readytToPlayButton").disabled = true;

var readyToPlay = false;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    var message_list_ul = document.getElementById("messagesList")
    message_list_ul.appendChild(li).classList.add("list-group-item");
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user}: ${message}`;

    $('#messagesList').animate({
        scrollTop: $("#messagesList li").last().offset().top
      }, 'slow');

});

function GuidIsValid(guid) {
    const pattern = /^([0-9a-f]{8}-?[0-9a-f]{4}-?[1-5][0-9a-f]{3}-?[89ab][0-9a-f]{3}-?[0-9a-f]{12})$/i;
    let result = guid.match(pattern);
    return result;
}

connection.on("GamesList",(value) => {
    var GamesCount = Object.entries(value).length-1;
    document.getElementById("gamesListHeader").innerHTML = `Games (${GamesCount})`;

    var gamesList = document.getElementById("gamesList");
    gamesList.innerText = "";
    Object.entries(value).slice(1).forEach(([gameId]) => {
        var li = document.createElement("li");
        gamesList.appendChild(li);
        var a = document.createElement("a");
        a.textContent = gameId;
        a.setAttribute('href', "/games?gameid="+gameId);
        li.appendChild(a);
    })

    $('#gamesList').animate({
        scrollTop: $("#gamesList li").last().offset().top
      }, 'slow');
})

connection.on("GoToGame",(gameId, userId) => {
    console.log("/games?gameid="+gameId.toString())
    sessionStorage.setItem("gameId", gameId);
    sessionStorage.setItem("userId", userId);
    window.location = "/games?gameid="+gameId.toString();
})

connection.on("ClientsList",(value) => {
    var lobbyClientsCount = Object.entries(value).length-1;
    document.getElementById("clientsListHeader").innerHTML = `Players In Lobby (${lobbyClientsCount})`;

    var clientList = document.getElementById("clientsList");
    clientList.innerHTML = "";
    for (const [,hubUser] of Object.entries(value).slice(1)) {
        // if (GuidIsValid(hubUser.gameId)){
        //     continue;
        // }
        var li = document.createElement("li");
        clientList.appendChild(li);
        // li.innerText = `ID: ${hubUser.userId}`;
        if (hubUser && hubUser.userName){
            li.innerText += `  ${hubUser.userName}` 
        }
        if (hubUser && hubUser.readyToPlay){
            li.innerText += " == Ready To Play =="
        }
        if (hubUser && GuidIsValid(hubUser.gameId)){
            li.innerText += ` GameId: ${hubUser.gameId}`
        }
    }

    // $('#clientsList').animate({
    //     scrollTop: $("#clientsList li").last().offset().top
    //   }, 'slow');
})

connection.start().then(function () {
    document.getElementById("readytToPlayButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.onreconnecting(error => {
    console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
    ui.notifyUserOfTryingToReconnect(); // Your function to notify user.
});

connection.onreconnected(() => {
    ui.hideOverlay();
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("readytToPlayButton").addEventListener("click", function (event) {
    readyToPlay = !readyToPlay;
    var userName = document.getElementById("userInput").value;
    connection.send("UserIsReadyToPlay", userName, readyToPlay).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


function DisableSendButton(disable) {
    document.getElementById("sendButton").disabled = disable;    
}

document.getElementById("userInput").addEventListener('input', function (evt) {
    
    if(this.value && connection._connectionState === "Connected" && document.getElementById("messageInput").value){
        DisableSendButton(false);
    } else {
        DisableSendButton(true);
    }
});

document.getElementById("messageInput").addEventListener('input', function (evt) {
    
    if(this.value && connection._connectionState === "Connected" && document.getElementById("userInput").value){
        DisableSendButton(false);
    } else {
        DisableSendButton(true);
    }
});