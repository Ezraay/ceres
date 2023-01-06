"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/LobbyHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
document.getElementById("readytToPlayButton").disabled = true;

var readyToPlay = false;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

function GuidIsValid(guid) {
    const pattern = /^([0-9a-f]{8}-?[0-9a-f]{4}-?[1-5][0-9a-f]{3}-?[89ab][0-9a-f]{3}-?[0-9a-f]{12})$/i;
    let result = guid.match(pattern);
    return result;
}

connection.on("GamesList",(value) => {
    var gamesList = document.getElementById("gamesList");
    gamesList.innerText = "";
    Object.entries(value).forEach(([gameId]) => {
        var li = document.createElement("li");
        gamesList.appendChild(li);
        li.innerText = gameId;
    })
})

connection.on("GoToGame",(gameId) => {
    console.log("/games?gameid="+gameId.toString())
    window.location = "/games?gameid="+gameId.toString();
})

connection.on("ClientsList",(value) => {
    var clientList = document.getElementById("clientsList");
    clientList.innerHTML = "";
    for (const [,hubUser] of Object.entries(value)) {
        // if (GuidIsValid(hubUser.gameId)){
        //     continue;
        // }
        var li = document.createElement("li");
        clientList.appendChild(li);
        li.innerText = `ID: ${hubUser.lobbyConnectionId}`;
        if (hubUser && hubUser.userName){
            li.innerText += `   (${hubUser.userName})` 
        }
        if (hubUser && hubUser.readyToPlay){
            li.innerText += " == Ready To Play =="
        }
        if (hubUser && GuidIsValid(hubUser.gameId)){
            li.innerText += ` GameId: ${hubUser.gameId}`
        }
        
    }

    // Object.entries(value).forEach(([connectionId,hubUser]) => {
    // });
})

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    document.getElementById("readytToPlayButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
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
    var user = document.getElementById("userInput").value;
    connection.send("UserIsReadyToPlay", user, readyToPlay).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});