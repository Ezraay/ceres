"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/LobbyHub").build();

// registering function that gets called by server (to report total number of clients on server)
connection.on("ClientsOnServer",(value) => {
    var clientsConnected = document.getElementById("clientsConnected");
    clientsConnected.innerText = value.toString();
})

connection.on("ClientsList",(value) => {
    var clientList = document.getElementById("clientsList");
    clientList.innerHTML = "";
    Object.entries(value).forEach(([connectionId,hubUser]) => {
        var li = document.createElement("li");
        clientList.appendChild(li);
        li.innerText = `ID: ${connectionId}`;
        if (hubUser && hubUser.userName){
            li.innerText += `   (${hubUser.userName})` 
        }
        if (hubUser && hubUser.readyToPlay){
            li.innerText += " == Ready To Play =="
        }
    });
})




connection.start().then(function () {
    // ClientConnected();
}).catch(function (err) {
    return console.error(err.toString());
});


